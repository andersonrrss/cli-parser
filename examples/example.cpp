#include <iostream>
#include "../include/cli_parser.h"

int main(int argc, char* argv[])
{
    CliParser parser;

    parser.addFlag("noArg");
    parser.addFlag("threeArgs", 1, 3);
    // Também aceita com --
    parser.addFlag("--unlimitedArgs", 1, -1);

    if(!parser.parse(argc, argv))
    {
        std::cerr << parser.getLastErrorMessage();
        return 1;
    }

    if(parser.has("noArg"))
        std::cout << "--noArg\n";

    if(parser.has("threeArgs"))
    {
        std::cout << "--threeArgs [ ";
        for(auto& arg : parser.getArgs("threeArgs"))
            std::cout << arg << " / ";

        std::cout << "]\n";
    }

    if(parser.has("unlimitedArgs")){
        std::cout << "--unlimitedArgs [ ";
        for(auto& arg : parser.getArgs("unlimitedArgs"))
            std::cout << arg << " / x";

        std::cout << "]\n";
    }
}