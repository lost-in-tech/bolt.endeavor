namespace Bolt.MaySucceed.Tests;

public class ErrorTests
{
    [Fact]
    public void error_should_implicitly_convert_to_may_succeed()
    {
        MaySucceed got = new Error("");
        got.IsSucceed.ShouldBeFalse();
        got.StatusCode.ShouldBe(400);
        got.Failure.ShouldNotBeNull();
        got.Failure.Errors!.Length.ShouldBe(1);
    }
    
    [Fact]
    public void errors_should_implicitly_convert_to_may_succeed()
    {
        MaySucceed got = new[]{ new Error("") };
        got.IsSucceed.ShouldBeFalse();
        got.StatusCode.ShouldBe(400);
        got.Failure.ShouldNotBeNull();
        got.Failure.Errors!.Length.ShouldBe(1);
    }
    
    [Fact]
    public void error_should_implicitly_convert_to_may_succeed_of_T()
    {
        MaySucceed<string> got = new Error("");
        got.IsSucceed.ShouldBeFalse();
        got.StatusCode.ShouldBe(400);
        got.Failure.ShouldNotBeNull();
        got.Failure.Errors!.Length.ShouldBe(1);
    }
    
    [Fact]
    public void errors_should_implicitly_convert_to_may_succeed_of_T()
    {
        MaySucceed<string> got = new[]{ new Error("") };
        got.IsSucceed.ShouldBeFalse();
        got.StatusCode.ShouldBe(400);
        got.Failure.ShouldNotBeNull();
        got.Failure.Errors!.Length.ShouldBe(1);
    }
    
    [Fact]
    public void ToMaySucceed_should_convert_error_MaySucceed()
    {
        var got = new Error("").ToMaySucceed();
        got.IsSucceed.ShouldBeFalse();
        got.StatusCode.ShouldBe(400);
        got.Failure.ShouldNotBeNull();
        got.Failure.StatusCode.ShouldBe(400);
        got.Failure.Errors.ShouldNotBeNull();
        got.Failure.Errors.Length.ShouldBe(1);
    }
    
    [Fact]
    public async Task ToMaySucceed_should_convert_error_MaySucceedTask()
    {
        var got = await new Error("").ToMaySucceedTask();
        got.IsSucceed.ShouldBeFalse();
        got.StatusCode.ShouldBe(400);
        got.Failure.ShouldNotBeNull();
        got.Failure.StatusCode.ShouldBe(400);
        got.Failure.Errors.ShouldNotBeNull();
        got.Failure.Errors.Length.ShouldBe(1);
    }
    
    [Fact]
    public void ToMaySucceed_should_convert_errors_MaySucceed()
    {
        var got = new[]{ new Error("1"), new Error("2")}.ToMaySucceed();
        got.IsSucceed.ShouldBeFalse();
        got.StatusCode.ShouldBe(400);
        got.Failure.ShouldNotBeNull();
        got.Failure.StatusCode.ShouldBe(400);
        got.Failure.Errors.ShouldNotBeNull();
        got.Failure.Errors.Length.ShouldBe(2);
    }
    
    [Fact]
    public async Task ToMaySucceed_should_convert_errors_MaySucceedTask()
    {
        var got = await new[]{ new Error("1"), new Error("2")}.ToMaySucceedTask();
        got.IsSucceed.ShouldBeFalse();
        got.StatusCode.ShouldBe(400);
        got.Failure.ShouldNotBeNull();
        got.Failure.StatusCode.ShouldBe(400);
        got.Failure.Errors.ShouldNotBeNull();
        got.Failure.Errors.Length.ShouldBe(2);
    }
}