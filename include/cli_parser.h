#include <filesystem>
#include <unordered_map>
#include <vector>
#include <string>


struct FlagParseResult
{
    bool present = false;
    std::vector<std::string> values;
};

struct FlagSpec
{
    int min = 0;
    int max = -1; // Infinito por padrão
};

using ParsedFlagsMap = std::unordered_map<std::string, FlagParseResult>;
using FlagSpecMap = std::unordered_map<std::string, FlagSpec>;

class CliParser
{
public:

    CliParser();
    ~CliParser();

    /// @brief Registra uma flag
    /// @param flag Nome da flag (com ou sem '--', '-')
    /// @param min Número mínimo de argumentos que a flag deve receber
    /// @param max Número máximo(-1 = infinitos)
    /// @return `true` se a flag for adicionada com sucesso
    bool addFlag(std::string flag, int min = 0, int max = 0);

    /// @brief Executa o parsing da linha de comando
    /// @param argc Quantidade de argumentos
    /// @param argv Vetor de argumentos
    /// @return `true` se o parse for completado corretamente
    bool parse(int argc, char* argv[]);

    /// @brief Verifica se uma flag foi digitada na linha de comando
    /// @param flag A flag em si (com ou sem '-', '--')
    /// @return `true` se a flag estiver presente na cli
    bool has(std::string flag) const;

    /// @brief Resgata os argumentos de uma flag específica
    /// @param flag 
    /// @note Sempre verifique a presença da flag com `has` antes de resgatar os argumentos
    const std::vector<std::string>& getArgs(std::string flag) const; 

    /// @brief Retorna o ultimo erro ocorrido no parsing 
    const std::string& getLastErrorMessage();
    
private:

    FlagSpecMap flagSpecs;
    ParsedFlagsMap parseResult;
    std::string lastError;

    /// @brief Remove '-' ou '--' da flag se for preciso
    /// @param flag A flag a ser normalizada
    std::string normalizeFlag(std::string flag) const;

    /// @brief Verifica se a flag foi registrada com `addFlag()`
    /// @param flag 
    /// @return `true` se a flag já estiver registrada
    bool flagExists(const std::string& flag) const;
    
    bool isFlag(const std::string& flag) const;

    /// @brief Vincula um argumento a uma flag no map
    /// @param flag A flag alvo
    /// @param arg O argumento
    void addFlagArgument(const std::string& flag, const std::string& arg);

    /// @brief Valida se o parsing foi bem sucedido
    /// @return `true` se não houver nenhum erro
    bool validateParse();

    void setError(std::string error, std::string flagName);
};