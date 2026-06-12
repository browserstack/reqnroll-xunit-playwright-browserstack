# Reqnroll (xUnit + Playwright) with BrowserStack

Sample repository that runs [Reqnroll](https://reqnroll.net/) BDD tests on the **xUnit**
runner, driving the browser with **Microsoft.Playwright**, on
[BrowserStack Automate](https://www.browserstack.com/automate) via the
[BrowserStack C# SDK](https://www.browserstack.com/docs/automate/selenium/sdk-csharp).

The BrowserStack SDK requires **no test-code changes** — it instruments the xUnit runner and
patches Playwright at `dotnet test` time, reading all platform/credential configuration from
[`browserstack.yml`](browserstack.yml).

This repo contains exactly two tests:

- **Sample test** (`Features/SampleTest.feature`) — the add-to-cart flow on
  [bstackdemo.com](https://bstackdemo.com/): add the first product to the cart and assert the
  product shown in the cart matches the product added.
- **Local test** (`Features/LocalTest.feature`) — opens `http://bs-local.com:45454` and asserts
  the page title is `BrowserStack Local`, proving the BrowserStack Local tunnel is connected.

---

## Prerequisites

- A [BrowserStack account](https://www.browserstack.com/users/sign_up) (username + access key).
- [.NET SDK](https://dotnet.microsoft.com/download) **6.0+** (8.0 recommended). On Apple Silicon
  use the arm64 installer.
- Git.

---

## Setup

1. Clone the repository:

   ```bash
   git clone https://github.com/browserstack/reqnroll-xunit-playwright-browserstack.git
   cd reqnroll-xunit-playwright-browserstack
   ```

2. Configure your BrowserStack credentials. Either edit `browserstack.yml`:

   ```yaml
   userName: YOUR_USERNAME
   accessKey: YOUR_ACCESS_KEY
   ```

   or export them as environment variables (recommended):

   ```bash
   export BROWSERSTACK_USERNAME="YOUR_USERNAME"
   export BROWSERSTACK_ACCESS_KEY="YOUR_ACCESS_KEY"
   ```

3. Restore the BrowserStack SDK CLI tool and the project dependencies:

   ```bash
   dotnet tool restore
   dotnet restore
   ```

---

## Run Sample Test

Run the add-to-cart sample on the platforms defined in `browserstack.yml`:

```bash
dotnet test --filter "Category=sample-test"
```

To run the entire suite (both tests across the configured platforms):

```bash
dotnet test
```

`dotnet test` launches the BrowserStack SDK, which starts each session on BrowserStack Automate,
runs the Reqnroll scenarios on the cloud browser, and reports results back to BrowserStack.

---

## Run Local Test

The local test confirms the BrowserStack Local tunnel. `browserstackLocal: true` in
`browserstack.yml` makes the SDK start the Local tunnel automatically before the session — no
separate binary to launch.

```bash
dotnet test --filter "Category=sample-local-test"
```

---

## Notes / Dashboard

- View runs, video, logs, and network traffic on the
  [BrowserStack Automate dashboard](https://automate.browserstack.com/).
- Platforms, parallelism, BrowserStack Local, Test Observability, and debugging options are all
  configured in [`browserstack.yml`](browserstack.yml) — no code changes needed to add a browser
  or device.
- Test Observability (`testObservability: true`) reports test runs to
  [observability.browserstack.com](https://observability.browserstack.com/).
- This combination runs through the SDK's **Direct (HTTP) flow** — Reqnroll on the xUnit runner.
