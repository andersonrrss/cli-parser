namespace cli_parser.Interfaces;

public interface ICliParser
{
    /// <summary>
    /// Analisa os argumentos da linha de comando, separando flags e argumentos posicionais.
    /// Deve ser chamado antes de qualquer método de consulta.
    /// </summary>
    /// <param name="args"> Argumentos recebidos no método <c>Main</c> </param>
    /// <returns>
    /// <c>true</> se o parsing for concluído com sucesso; Caso contrário <c>false</c>
    /// </returns>
    bool TryParse(string[] args);

    /// <summary>
    /// Tenta obter os argumentos posicionais(não associados a nenhuma flag).
    /// </summary>
    /// <param name="posicionals">
    /// Contém a lista de argumentos posicionais
    /// </param>
    /// <returns>
    /// Retorna <c>true</c> se o existirem argumentos posicionais; Caso contrário <c>false</c>
    /// </returns>
    bool TryGetPosicionals(out IReadOnlyList<string> posicionals);

    /// <summary>
    /// Obtém os argumentos associados a uma flag já registrada
    /// </summary>
    /// <param name="flagName">
    /// Nome da flag cujo os nomes serão obtidos
    /// </param>
    /// <param name="arguments">
    /// Recebe a lista de argumentos associados à flag quando o método retorna <c>true</c>. 
    /// Caso contrário, será uma lista vazia
    /// </param>
    /// <returns>
    /// <c>true</c> se a flag for encontrada e contém argumentos; caso contrário <c>false</c>
    /// </returns>
    bool TryGetArgs(string flagName, out IReadOnlyList<string> arguments);

    /// <summary>
    /// Verifica se uma flag foi utilizada na linha de comando
    /// </summary>
    /// <param name="flagName">Nome da flag</param>
    /// <returns>
    /// <c>true</c> se a flag estiver presente; Caso contrário <c>false</c>
    /// </returns>
    bool Has(string flagName);
    
    /// <summary>
    /// Registra uma flag para o parser
    /// </summary>
    /// <param name="flagName">O nome da flag a ser registrada</param>
    /// <param name="minArguments">Quantidade mínima de argumentos exigida</param>
    /// <param name="maxArguments">Quantidade máxima de argumento. Use <c>-1</c> para ilimitado</param>
    void RegisterFlag(string flagName, int minArguments = 0, int maxArguments = 0);
}