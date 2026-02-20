using cli_parser.Entities;
using cli_parser.Interfaces;

namespace cli_parser;

public class CliParser : ICliParser
{
    private readonly List<Flag> Flags = [];
    private readonly List<string> PosicionalArgs = [];

    public CliParser() {}

    public bool TryParse(string[] args)
    {
        Flag? actualFlag = null;

        foreach (var arg in args)
        {
            if(IsValidFlag(arg) && FlagIsRegistered(arg))
            {
                actualFlag = null;
                
                var flag = GetFlag(arg) 
                    ?? throw new Exception("Falha ao organizar linha de comando");

                flag.SetPresence();

                if(flag.MaxArgs != 0)
                    actualFlag = flag;

                continue;
            }

            if (actualFlag is null)
            {
                PosicionalArgs.Add(arg);
            }
            else
            {
                int argumentCount = actualFlag.Arguments.Count;
                int maxArgs = actualFlag.MaxArgs;

                if(maxArgs != -1 && argumentCount >= maxArgs )
                    throw new InvalidOperationException(
                        $"Flag {actualFlag.Name} aceita um máximo de {maxArgs} argumentos"
                    );

                actualFlag.AddArgument(arg);                    
                continue;
            }
        }

        return true;
    }

    public void RegisterFlag(string flagName, int minArguments = 0, int maxArguments = 0)
    {
        string flag = NormalizeFlag(flagName);

        if(maxArguments < minArguments && maxArguments != -1)
            throw new ArgumentException("O número mínimo de argumentos não pode ser maior que o número máximo");

        if(FlagIsRegistered(flag))
            throw new Exception("Flag já definida");

        Flag newFlag = new(flag, minArguments, maxArguments);
        Flags.Add(newFlag);
    }

    public bool TryGetPosicionals(out IReadOnlyList<string> posicionals)
    {
        posicionals = [];

        if(PosicionalArgs.Count < 1)
            return false;

        posicionals = PosicionalArgs;
        return true;
    }

    public bool TryGetArgs(string flagName, out IReadOnlyList<string> arguments)
    {   

        if(GetFlag(flagName) is not Flag flagQuery || flagQuery.Arguments.Count == 0)
        {
            arguments = [];
            return false;
        }
        
        arguments = flagQuery.Arguments;
        return true;
    }

    public bool Has(string flagName)
    {
        if(GetFlag(flagName) is not Flag flagQuery)
            return false;

        return flagQuery.IsPresent();
    }

    private Flag? GetFlag(string flagName)
    {
        return Flags.FirstOrDefault(f => 
            f.Name == NormalizeFlag(flagName));
    }

    private bool FlagIsRegistered(string flagName)
    {
        return GetFlag(flagName) is not null;
    }

    private static bool IsValidFlag(string flagName)
    {
        return flagName.StartsWith('-');
    }

    private static string NormalizeFlag(string flagName)
    {
        if (IsValidFlag(flagName))
            return flagName;

        return $"--{flagName}";
    }
}