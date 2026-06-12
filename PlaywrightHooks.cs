using Microsoft.Playwright;
using Reqnroll;

namespace ReqnrollXunitPlaywrightBrowserStack;

// Reqnroll lifecycle hooks that create and dispose a Playwright page per scenario.
//
// The browser is launched normally with Microsoft.Playwright. When the tests run
// through the BrowserStack SDK (`dotnet test` after `dotnet tool restore`), the SDK
// patches the Playwright entry point and routes this browser to the BrowserStack cloud
// using the platforms declared in `browserstack.yml`. No BrowserStack-specific code is
// required in the test itself.
[Binding]
public sealed class PlaywrightHooks
{
    internal const string PlaywrightKey = "Playwright";
    internal const string BrowserKey = "Browser";
    internal const string ContextKey = "Context";
    internal const string PageKey = "Page";

    private readonly ScenarioContext _scenarioContext;

    public PlaywrightHooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeScenario]
    public async Task Initialize()
    {
        var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();

        _scenarioContext.Set(playwright, PlaywrightKey);
        _scenarioContext.Set(browser, BrowserKey);
        _scenarioContext.Set(context, ContextKey);
        _scenarioContext.Set(page, PageKey);
    }

    [AfterScenario]
    public async Task TearDown()
    {
        if (_scenarioContext.TryGetValue(PageKey, out IPage? page) && page is not null)
        {
            await page.CloseAsync();
        }

        if (_scenarioContext.TryGetValue(ContextKey, out IBrowserContext? context) && context is not null)
        {
            await context.CloseAsync();
        }

        if (_scenarioContext.TryGetValue(BrowserKey, out IBrowser? browser) && browser is not null)
        {
            await browser.CloseAsync();
        }

        if (_scenarioContext.TryGetValue(PlaywrightKey, out IPlaywright? playwright) && playwright is not null)
        {
            playwright.Dispose();
        }
    }
}
