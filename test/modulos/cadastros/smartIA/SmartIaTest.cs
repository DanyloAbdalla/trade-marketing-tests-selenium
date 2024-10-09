
using OpenQA.Selenium;

namespace MeuClienteWebTestProject;

/// <summary>
/// Classe com os testes do cadastro de campanhas SmartIA
/// </summary>
[TestFixture]
public class SmartIaTest
{
    private IWebDriver webDriver;
    private readonly BrowserType browserType = BrowserType.Chrome;

    /// <summary>
    /// Método que será executado antes de cada teste
    /// </summary>
    [SetUp]
    public void Setup()
    {
        webDriver = DriverFactory.CreateDriver(browserType);
        webDriver.Navigate().GoToUrl(GlobalVariables.urlPlataforma);

        new LoginPage(webDriver)
        .PreencherEmailUsuario(GlobalVariables.emailUsuario)
        .PreencherSenhaUsuario(GlobalVariables.senhaUsuario)
        .SubmeterLogin();

        new HomePage(webDriver)
        .AbrirCadastroSmartIa();
    }

    /// <summary>
    /// Testar a criação de uma campanha
    /// 
    /// Como comercial de trade marketing
    /// Eu quero criar uma campanha
    /// Para começar a montar meu pacote de ativos, com disponibiliade de alocação no inventário da loja
    /// 
    /// Dado que eu tenho espaços disponíveis para realização de uma nova campanha no SmartIA
    /// Quando eu criar a campanha
    /// E preencher os campos Nome da Campanha, Inicio/Fim Vigência, Email, WhatsApp, Data Limite e Responsável da Campanha
    /// E clicar no botão Salvar Campanha
    /// Então uma campanha será criada com Status = Criando
    /// </summary>
    [Test]
    public void TestCriarCampanhaSmartIA()
    {
        var contexto = "NovaCampanha";
        var statusCampanha = "Criando";

        new SmartIaPage(webDriver)
        .NovaCampanhaSmartIA()
        .PreencherCamposCampanha()
        .SalvarCampanha(contexto)
        .FecharCampanha()
        .BuscarCampanhas()
        .ValidarStatusDaCampanha(statusCampanha);
    }


    /// <summary>
    /// Testar a inclusão dos ativos na campanha
    /// 
    /// Como comercial de trade marketing
    /// Eu quero adicionar na campanha, o varejo e os ativos com disponibilidade no inventário da loja
    /// Para começar a montar meu pacote, que será enviado para os fornecedores
    /// 
    /// Dado que eu tenho uma campanha criada no SmartIA
    /// Quando eu editar a campanha
    /// E selecionar o varejo 
    /// E reservar os ativos para as lojas
    /// E clicar no botão Salvar Campanha
    /// Então o varejo E os ativos com disponibilidade de inventário serão reservados para a campanha, como um pacote de venda
    /// </summary>
    [Test]
    public void TestAdicionarVarejoEAtivosNaCampanha()
    {
        var contexto = "EditarCampanha";

        new SmartIaPage(webDriver)
        .BuscarCampanhas()
        .AbrirEditacaoDaCampanha()
        .AdicionarVarejo()
        .RealizarVarredura()
        .SelecionarEReservarAtivos()
        .SalvarAtivosReservados()
        .SalvarCampanha(contexto);

        Dsl.Esperar1Segundo();
    }


    /// <summary>
    /// Método que será executado ao final de cada teste
    /// </summary>
    [TearDown]
    public void TearDown()
    { webDriver.Close(); }
}