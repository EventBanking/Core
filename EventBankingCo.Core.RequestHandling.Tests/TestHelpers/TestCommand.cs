using EventBankingCo.Core.RequestHandling.Abstraction;

namespace EventBankingCo.Core.RequestHandling.Tests.TestHelpers
{
    public class TestCommand : ICommand
    {
        public string ValueToTrace { get; set; }

        public TestCommand(string valueToTrace = "Log This Value As Trace")
        {
            ValueToTrace = valueToTrace;
        }
    }
}
