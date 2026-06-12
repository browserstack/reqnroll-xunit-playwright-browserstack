using Microsoft.Playwright;
using Reqnroll;
using Xunit;

namespace ReqnrollXunitPlaywrightBrowserStack.StepDefinitions;

[Binding]
public class SampleTestSteps
{
    private const float DemoWaitMs = 1200;

    private readonly ScenarioContext _scenarioContext;

    private string? _productOnPageText;
    private string? _productOnCartText;

    public SampleTestSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    private IPage GetPage()
    {
        return _scenarioContext.TryGetValue(PlaywrightHooks.PageKey, out IPage? page) && page is not null
            ? page
            : throw new InvalidOperationException("Playwright page is not initialized. Ensure the [BeforeScenario] hook ran successfully.");
    }

    [Given(@"I navigate to website")]
    public async Task GivenINavigateToWebsite()
    {
        var page = GetPage();
        await page.GotoAsync("https://bstackdemo.com/");
        await page.WaitForTimeoutAsync(DemoWaitMs);
    }

    [Then(@"I should see title (.*)")]
    public async Task ThenIShouldSeeTitle(string title)
    {
        var page = GetPage();
        Assert.Equal(title, await page.TitleAsync());
        await page.WaitForTimeoutAsync(DemoWaitMs);
    }

    [Then(@"I add product to cart")]
    public async Task ThenIAddProductToCart()
    {
        var page = GetPage();
        _productOnPageText = await page.Locator("//*[@id='1']/p").TextContentAsync();
        await page.WaitForTimeoutAsync(DemoWaitMs);
        await page.Locator("//*[@id='1']/div[4]").ClickAsync();
        await page.WaitForTimeoutAsync(DemoWaitMs);
    }

    [When(@"I check if cart is opened")]
    public async Task WhenICheckIfCartIsOpened()
    {
        var page = GetPage();
        bool cartOpened = await page.Locator(".float-cart__content").IsVisibleAsync();
        Assert.True(cartOpened);
        await page.WaitForTimeoutAsync(DemoWaitMs);
    }

    [Then(@"I should see same product in cart")]
    public async Task ThenIShouldSeeSameProductInCart()
    {
        var page = GetPage();
        _productOnCartText = await page
            .Locator("//*[@id='__next']/div/div/div[2]/div[2]/div[2]/div/div[3]/p[1]")
            .TextContentAsync();
        Assert.Equal(_productOnPageText, _productOnCartText);
    }
}
