# Cli Parser

Um pequeno parser de linha de comando. O objetivo é definir uma forma de trabalhar com CLI sem precisar se dependências externas

## Exemplo

```csharp
using cli_parser;

public class Program{
    static void Main(string[] args){
        CliParser parser = new();               // Instância do parser

        parser.RegisterFlag("example", 1 , -1); // Registro de flags com argumentos infinitos

        if(!parser.TryParse())                  // Tentativa de parse
        {                 
            throw new Exception("Erro no parse");
        }

        if(parser.TryGetArgs("example", out var unlimitedArguments))
        {
            foreach(var argument in unlimitedArguments)
                Console.WriteLine(argument);
        }
    }
}
```

## API

- `RegisterFlag(string flagName, int minArguments = 0, int maxArguments = 0)`
- `TryParse(string[] args)`
- `Has(string flagName)`
- `TryGetArgs(string flagName, out IReadOnlyList<string> arguments)`
- `TryGetPosicionals(out IReadOnlyList<string> posicionals)`

## Licença

Este projeto utiliza a Licença MIT. Veja o arquivo LICENSE para mais informações.