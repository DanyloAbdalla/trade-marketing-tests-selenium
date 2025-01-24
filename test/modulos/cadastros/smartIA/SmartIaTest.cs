using NUnit.Framework.Interfaces;
using OpenQA.Selenium;

namespace MeuClienteWebTestProject;

/// <summary>
/// Classe com os testes do cadastro de campanhas SmartIA
/// </summary>
[TestFixture("SemPlantaLoja", Category = "PlanosSemPlantaDeLoja")]
[TestFixture("ComPlantaLoja", Category = "PlanosComPlantaDeLoja")]
[Parallelizable(ParallelScope.Fixtures)]
public class SmartIaTest
{
    private RunSettings runSettings;
    private IWebDriver webDriver;
    private readonly BrowserType browserType = BrowserType.Chrome;
    private bool previousTestFalied;
    private readonly string testContext;
    private readonly string className;
    private readonly string nomeCampanha = "CampanhaMassaAutomatizada";
    private readonly string whatsAppResponsavel = "15988086091";
    private readonly string nomeResponsavel = "Usuário Homologacao";
    private readonly string mensagemCabecalho = "Campanha Massa Automatizada";

    public SmartIaTest(string testContext)
    {
        this.testContext = testContext;
        className = TestContext.CurrentContext.Test.ClassName.Split('.').Last();
    }

    /// <summary>
    /// Método que será executado antes de cada teste
    /// </summary>
    [SetUp]
    public void Setup()
    {
        runSettings = RunSettings.LoadSettings();
        webDriver = DriverFactory.CreateDriver(browserType);
        var testName = TestContext.CurrentContext.Test.MethodName;

        if (previousTestFalied)
            Assert.Ignore("Pular o próximo teste, pois o teste anterior falhou");
        else if (runSettings.ToSkip(className, testContext, testName))
            Assert.Ignore("Teste ignorado pelas configurações de execução");

        new LoginPage(webDriver)
        .RealizarLogin(GlobalVariables.emailUsuarioSemPlanta, GlobalVariables.senhaUsuarioSemPlanta);

        new HomePage(webDriver)
        .AcessarCadastroSmartIa();
    }

    /// <summary>
    /// Testar a criação de uma campanha
    /// 
    /// Como comercial de trade marketing
    /// Eu quero criar uma campanha com pacotes de ativos disponíveis no inventário da loja para alocação
    /// Para comunicar meus clientes sobre vendas destes pacotes na campanha
    /// 
    /// Dado que eu não tenho permissão de planta de loja
    /// E que eu tenho disponibilidade de inventário
    /// Quando eu iniciar uma nova campanha, preenchendo os campos de cabeçalho
    /// E reservar o ativo, colocando as quantidades para as lojas, clicando no botão “Salvar”
    /// E clicar no botão “Salvar” da campanha
    /// Então a campanha será criada com sucesso com Status = Criando
    /// </summary>
    [Test, Order(1)]
    public void TestCriarCampanhaSmartIA()
    {
        var contextoDeExecucao = "NovaCampanha";
        var statusCampanhaEsperado = "Criando";

        new SmartIaPage(webDriver)
        .NovaCampanhaSmartIA()
        .PreencherCamposCampanha(nomeCampanha, whatsAppResponsavel, nomeResponsavel, mensagemCabecalho)
        .ValidarVarejoSelecionado()
        .RealizarVarredura()
        .SelecionarEReservarAtivos()
        .SalvarAtivosReservados()
        .SalvarCampanha(contextoDeExecucao)
        .FecharCampanha()
        .BuscarCampanhas(nomeCampanha)
        .ValidarStatusDaCampanha(statusCampanhaEsperado);
    }

    /// <summary>
    /// Testar a edição das quantidades dos ativos reservados na campanha
    /// 
    /// Como comercial de trade marketing
    /// Eu quero alterar as quantidades dos ativos reservados
    /// Para atualizar minha campanha
    /// 
    /// Dado que eu não tenho permissão de planta de loja
    /// E que eu tenho uma campanha criada, contendo um ativo com disponibilidade de inventário
    /// Quando eu acessar a tela de edição da campanha
    /// E alterar a quantidade do ativo, clicando no botão "Salvar"
    /// E clicar no botão “Salvar” da campanha
    /// Então a campanha será salva com a nova quantidade
    /// </summary>
    [Test, Order(2)]
    public void TestEditarAtivosReservadosNaCampanhaExistente()
    {
        var contexto = "EditarCampanha";

        new SmartIaPage(webDriver)
        .BuscarCampanhas()
        .AbrirEdicaoDaCampanha()
        .AbrirMenuSuspensoVarejos()
        .EditarQuantidadesDosAtivosReservados()
        .SalvarAtivosReservados()
        .SalvarCampanha(contexto);
    }

    /// <summary>
    /// Testar a reserva de um novo ativo em uma campanha existente
    /// 
    /// Como comercial de trade marketing
    /// Eu quero reservar um novo ativo para as lojas
    /// Para os valores da minha campanha, com o novo ativo
    /// 
    /// Dado que eu não tenho permissão de planta de loja
    /// E que eu tenho uma campanha criada no SmartIA
    /// Quando acessar a tela de edição
    /// E reservar um novo ativo para as lojas com disponibilidade de inventário, clicando no botão "Salvar"
    /// E clicar no botão "Salvar" da campanha
    /// Então a campanha será salva com o novo ativo
    /// </summary>
    [Test, Order(3)]
    public void TestReservarNovoAtivoNaCampanhaExistente()
    {
        var contexto = "EditarCampanha";
        var nomeAtivo = "Aplicativo";

        new SmartIaPage(webDriver)
        .BuscarCampanhas()
        .AbrirEdicaoDaCampanha()
        .AbrirMenuSuspensoVarejos()
        .ReservarNovosAtivosPorLoja(nomeAtivo)
        .SalvarAtivosReservados()
        .SalvarCampanha(contexto);
    }

    /// <summary>
    /// Método que será executado ao final de cada teste
    /// </summary>
    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Skipped)
        {
            webDriver.Close();
        }
        else if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
        {
            previousTestFalied = true;
            webDriver.Close();
        }
        else
        {
            //Retorna para o Dashboard de Operações no final de cada teste, realizando logout
            new HomePage(webDriver).AcessarDashboardOperacoes();
            new HomePage(webDriver).RealizarLogout();

            webDriver.Close();
        }
    }
}