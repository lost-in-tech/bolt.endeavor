using System.Diagnostics.CodeAnalysis;

namespace Bolt.Endeavor.Extensions.Mvc;

internal static class StringExtensions
{
    public static bool HasValue([NotNullWhen(true)]this string? value) => string.IsNullOrWhiteSpace(value);
}