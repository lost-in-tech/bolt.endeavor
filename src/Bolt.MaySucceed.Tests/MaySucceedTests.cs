namespace Bolt.MaySucceed.Tests;

public partial class MaySucceedTests
{
    [Fact]
    public void true_should_be_converted_to_may_succeed()
    {
        MaySucceed result = true;
        result.IsSucceed.ShouldBeTrue();
        result.Failure.ShouldBeNull();
        result.StatusCode.ShouldBe(200);
    }
    
    [Fact]
    public void false_should_be_converted_to_may_succeed()
    {
        MaySucceed result = false;
        result.IsSucceed.ShouldBeFalse();
        result.Failure.ShouldNotBeNull();
        result.Failure.Reason.ShouldBe("Undefined server error");
        result.Failure.StatusCode.ShouldBe(500);
    }

    [Fact]
    public void failure_should_be_converted_to_may_succeed()
    {
        MaySucceed result = new Failure("Internal server error");
        result.IsSucceed.ShouldBeFalse();
        result.IsFailed.ShouldBeTrue();
        result.Failure.ShouldNotBeNull();
        result.Failure.Reason.ShouldBe("Internal server error");
        result.Failure.StatusCode.ShouldBe(500);
    }

    [Fact]
    public void error_collection_should_be_converted_to_may_succeed()
    {
        MaySucceed result = new Error[]{ new("Title is required","Title","TitleRequired")};
        result.IsSucceed.ShouldBeFalse();
        result.IsFailed.ShouldBeTrue();
        result.Failure.ShouldNotBeNull();
        result.Failure.Reason.ShouldBe("Please check the error(s)");
        result.Failure.StatusCode.ShouldBe(400);
    }

    [Fact]
    public void error_should_be_converted_to_may_succeed()
    {
        MaySucceed result = new Error("Title is required","Title","TitleRequired");
        result.IsSucceed.ShouldBeFalse();
        result.IsFailed.ShouldBeTrue();
        result.Failure.ShouldNotBeNull();
        result.Failure.Reason.ShouldBe("Please check the error(s)");
        result.Failure.StatusCode.ShouldBe(400);
    }
}