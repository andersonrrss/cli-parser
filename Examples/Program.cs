using cli_parser;

class Program
{
    static void Main(string[] args)
    {
        CliParser parser = new();

        // Flag com argumentos
        parser.RegisterFlag("unlimited", 1 , -1); // Argumentos infinitos
        parser.RegisterFlag("limited", 1, 3); // Argumentos limitados
        // Sem argumentos
        parser.RegisterFlag("--present");

        if (!parser.TryParse(args))
        {
            Console.WriteLine("Falha ao organizar os argumentos da linha de comando");
            return;
        }

        if(parser.TryGetArgs("unlimited", out var unlimitedArguments))
        {
            Console.Write("unlimited: ");
            Console.WriteLine(string.Join(", ", unlimitedArguments));
        }

        if(parser.TryGetArgs("limited", out var limitedArguments))
        {
            Console.Write("limited: ");
            Console.WriteLine(string.Join(", ", limitedArguments));
        }

        Console.WriteLine($"present: {parser.Has("present")}");

        if(parser.TryGetPosicionals(out var posicionals))
        {
            Console.WriteLine($"posicionais: {string.Join(", ", posicionals)}");    
        }
    }
}