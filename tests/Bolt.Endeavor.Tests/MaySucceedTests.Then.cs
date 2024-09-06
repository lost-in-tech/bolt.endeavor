namespace Bolt.Endeavor.Tests;

public partial class MaySucceedTests
{
    [Fact]
    public void Then_should_return_failure_when_may_succeed_is_failed()
    {
        MaySucceed got = false;
        var rsp = got.Then<string>(() => "Hello");
        rsp.IsSucceed.ShouldBe(false);
        rsp.HasValue.ShouldBeFalse();
    }
    
    [Fact]
    public void Then_should_return_response_when_may_succeed_is_succeed()
    {
        MaySucceed got = true;
        var rsp = got.Then(() => MaySucceed<string>.Ok("Hello"));
        rsp.IsSucceed.ShouldBeTrue();
        rsp.Value.ShouldBe("Hello");
    }
    
    [Fact]
    public void Then_should_return_maysucceed_rsp_when_succeed()
    {
        MaySucceed got = true;
        var rsp = got.Then(() => new Error("hello"));
        rsp.IsSucceed.ShouldBeFalse();
    }
}