namespace Bolt.Endeavor.Tests;

public partial class MaySucceedTests
{
    [Fact]
    public void T_should_be_converted_to_may_succeed_of_T()
    {
        MaySucceed<string> result = "Hello world";
        result.IsSucceed.ShouldBeTrue();
        result.Failure.ShouldBeNull();
        result.Value.ShouldBe("Hello world");
        result.StatusCode.ShouldBe(200);
    }
}