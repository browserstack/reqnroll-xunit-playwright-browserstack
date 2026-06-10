using Microsoft.Playwright;
using Reqnroll;

namespace ReqnrollXunitPlaywrightBrowserStack.StepDefinitions;

[Binding]
public class LocalTestSteps
{
    private readonly ScenarioContext _scenarioContext;

    public LocalTestSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    private IPage GetPage()
    {
        return _scenarioContext.TryGetValue(PlaywrightHooks.PageKey, out IPage? page) && page is not null
            ? page
            : throw new InvalidOperationException("Playwright page is not initialized. Ensure the [BeforeScenario] hook ran successfully.");
    }

    // Verifies the BrowserStack Local tunnel is connected: bs-local.com:45454 only
    // resolves when the SDK has started the Local tunnel (browserstackLocal: true).
    // The "I should see title BrowserStack Local" assertion is handled by the shared
    // [Then(@"I should see title (.*)")] step in SampleTestSteps.
    [Given(@"I navigate to local website")]
    public async Task GivenINavigateToLocalWebsite()
    {
        var page = GetPage();
        await page.GotoAsync("http://bs-local.com:45454/");
        await page.WaitForTimeoutAsync(2000);
    }
}
