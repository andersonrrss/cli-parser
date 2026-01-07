# cli-parser

Um parser simples para argumentos de linha de comando em **c++20**. O objetivo é construir uma forma de definir e validar flags sem necessitar de dependências externas.

## Funcionalidades

- Definição de flags com:
    - Número mínimo
    - Número máximo de argumentos(ou ilimitados)
- Suporte somente a flags longas (por enquanto)
- Validação dos argumentos após o parser
- Argumentos posicionais

## Como compilar

Para rodar é necessário um compilador compatível com C++20.  
Esse seria o comando pra compilar o arquivo de exemplo com `g++`.
``` bash
g++ ./examples/example.cpp ./src/cli_parser.cpp -o build/example -std=c++20 -Wall -Wextra -Wpedantic
```

## Exemplo de uso
```cpp
#include "cli_parser.h"

int main(int argc, char* argv[])
{
    CliParser parser;

    parser.addFlag("--output", 1, 1);
    parser.addFlag("--verbose", 0, 0); // Não seria necessário definir o mínimo e máximo

    if(!parser.parse(argc, argv))
    {
        std::cerr << parser.getLastErrorMessage();
        return 1;
    }

    if(parser.has("verbose"))
        std::cout << "Verbose ativado\n";

    auto output = parser.getArgs("output")[0];
    std::cout << "Arquivo de saída: " << output << "\n";
}
```

## Comportamento das flags

- As flags podem ser registradas com ou sem `-`/`--` (ainda não implementei completamente o uso do `-`)
- Internamente os prefixos são normalizados
- `min = 0`(padrão) -> flag não aceita argumentos
- `max = -1`(padrão) -> número ilimitado de argumentos

## Tratamento de erros

## Licença

Este projeto utiliza a Licença MIT. Veja o arquivo LICENSE para mais informações.