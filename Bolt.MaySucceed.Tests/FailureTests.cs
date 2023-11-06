namespace Bolt.MaySucceed.Tests;

public class FailureTests
{
    [Fact]
    public void failure_should_implicitly_convert_to_may_succeed()
    {
        MaySucceed got = new Failure("Failed");
        got.IsSucceed.ShouldBeFalse();
        got.StatusCode.ShouldBe(500);
        got.Failure.ShouldNotBeNull();
        got.Failure.Reason.ShouldBe("Failed");
    }
    
    [Fact]
    public void failure_should_implicitly_convert_to_may_succeed_of_T()
    {
        MaySucceed<string> got = new Failure("Failed");
        got.IsSucceed.ShouldBeFalse();
        got.StatusCode.ShouldBe(500);
        got.Failure.ShouldNotBeNull();
        got.Failure.Reason.ShouldBe("Failed");
    }
}