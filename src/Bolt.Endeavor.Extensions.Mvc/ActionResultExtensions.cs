using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bolt.Endeavor.Extensions.Mvc;

public static class ActionResultExtensions
{
    public static async Task<IResult> ToResult(this Task<MaySucceed> source)
    {
        var rsp = await source;
        return new MaySucceedResult(rsp);
    }
    
    public static IResult ToResult(this MaySucceed source)
    {
        return new MaySucceedResult(source);
    }
    
    public static async Task<IResult> ToResult<T>(this Task<MaySucceed<T>> source)
    {
        var rsp = await source;
        return new MaySucceedResult<T>(rsp);
    }
    
    public static IResult ToResult<T>(this MaySucceed<T> source)
    {
        return new MaySucceedResult<T>(source);
    }
    
    public static async Task<IActionResult> ToActionResult(this Task<MaySucceed> source)
    {
        var rsp = await source;
        return new MaySucceedResult(rsp);
    }
    
    public static IActionResult ToActionResult(this MaySucceed source)
    {
        return new MaySucceedResult(source);
    }
    
    public static async Task<IActionResult> ToActionResult<T>(this Task<MaySucceed<T>> source)
    {
        var rsp = await source;
        return new MaySucceedResult<T>(rsp);
    }
    
    public static IActionResult ToActionResult<T>(this MaySucceed<T> source)
    {
        return new MaySucceedResult<T>(source);
    }
}