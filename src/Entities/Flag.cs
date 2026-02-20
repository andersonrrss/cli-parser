using System.Collections.ObjectModel;

namespace cli_parser.Entities;

public class Flag (string name, int minArgs = 0, int maxArgs = 0)
{
    public string Name { get; private set; } = name;
    public int MinArgs { get; private set; } = minArgs;
    public int MaxArgs { get; private set; } = maxArgs;
    private bool Present { get; set; } = false;
    
    public IReadOnlyList<string> Arguments => _arguments;
    private List<string> _arguments { get; set; } = new List<string>();

    public void SetPresence()
    {
        Present = true;
    }

    public bool IsPresent()
    {
        return Present;
    }

    public void AddArgument(string argument)
    {
        if(!IsPresent())
            throw new Exception("A flag não está presente na linha de comando");
        
        if(String.IsNullOrEmpty(argument))
            throw new ArgumentException("Argumento nulo");

        _arguments.Add(argument);
    }
}
