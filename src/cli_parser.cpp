#include <iostream>
#include <unordered_map>
#include <string>
#include "../include/cli_parser.h"

CliParser::CliParser() {}

CliParser::~CliParser() {}

bool CliParser::addFlag(std::string flag, int min, int max)
{
    flag = normalizeFlag(flag);

    if(min < 0 || max < -1 || (max != -1 && min > max) ){
        setError("Invalid flag configuration", flag);
        return false;
    }

    if(flagExists(flag)){
        setError("Flag already defined", flag);
        return false;
    }
        
    flagSpecs[flag] = FlagSpec{min, max};
    return true;
}

bool CliParser::parse(int argc, char* argv[])
{   
    std::vector<std::string> argVector (argv + 1, argv + argc);
    std::string actualFlagName;

    for(const auto& arg : argVector){
        if(isFlag(arg)){
            std::string flag = normalizeFlag(arg);

            if(!flagExists(flag)){
                setError("Undefined flag", flag);
                return false;
            }

            parseResult[flag].present = true;

            // A flag não precisa de argumentos
            if(flagSpecs[flag].min == 0)
            {
                actualFlagName.clear();
                continue;
            }

            actualFlagName = flag;
            continue;
        }

        if(actualFlagName.empty() && !isFlag(arg))
            parseResult["posicional"].values.push_back(arg);
        else
            addFlagArgument(actualFlagName, arg);
    }
    
    return validateParse();
}

bool CliParser::has(std::string flag) const
{
    flag = normalizeFlag(flag);

    auto it = parseResult.find(flag);

    if(it == parseResult.end()) return false;

    return it->second.present;
}

const std::vector<std::string>& CliParser::getArgs(std::string flag) const
{
    flag = normalizeFlag(flag);

    static std::vector<std::string> empty {};
    auto it = parseResult.find(flag);

    if(it == parseResult.end()) return empty;

    return it->second.values;
}

bool CliParser::validateParse() 
{
    for(auto& [flag, result]: parseResult){
        auto spec = flagSpecs.at(flag);

        const int count = result.values.size();

        if(count < spec.min)
        {
            setError("Unreached minimun arguments", flag);
            return false;
        }

        if(spec.max != -1 && count > spec.max)
        {
            setError("Exceeded maximum arguments", flag);
            return false;
        }
    }
    return true;
}

void CliParser::addFlagArgument(const std::string& flag, const std::string& arg)
{
    FlagParseResult& flagArgs = parseResult[flag];

    flagArgs.values.push_back(arg);
}

// O argumento/flag pode vir com "-" ou com "--"
// Por isso a necessidade de normalizar
std::string CliParser::normalizeFlag(std::string flag) const
{
    if(flag.starts_with("--"))
        return flag.substr(2);

    if(flag.starts_with("-"))
        return flag.substr(1);

    return flag;
}

bool CliParser::flagExists(const std::string& flag) const
{
    return flagSpecs.contains(flag);
}

bool CliParser::isFlag(const std::string& flag) const
{
    return flag.starts_with("--") || flag.starts_with("-");
}

void CliParser::setError(std::string message, std::string flag)
{
    if(flag.empty())
        lastError = message + "\n";
    else 
        lastError = message + ": " + flag + "\n";
}

const std::string& CliParser::getLastErrorMessage(){
    return lastError;
}