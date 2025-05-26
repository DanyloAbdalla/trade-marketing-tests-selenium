using Newtonsoft.Json;

public class DataLoader
{
    private static dynamic massaDados;

    public static void CarregarArquivo()
    {
        string json = File.ReadAllText("datatest.json");
        massaDados = JsonConvert.DeserializeObject<dynamic>(json);
    }

    public static dynamic ObterDados(string suite, string teste, string data)
    {
        return massaDados[suite][teste][data];
    }
}