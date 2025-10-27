using Reqnroll;
using ReqnrollCucumberTests.Support;

namespace ReqnrollCucumberTests.Support
{
    [Binding]
    public class TestHooks
    {
        private readonly TestContext _testContext;

        public TestHooks(TestContext testContext)
        {
            _testContext = testContext;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _testContext.Initialize();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _testContext.Cleanup();
        }
    }
}
