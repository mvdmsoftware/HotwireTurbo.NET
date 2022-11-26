using System.ComponentModel;
using HotwireTurbo.NET.Interfaces;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HotwireTurbo.NET.TagHelpers;

/// <summary>
/// Base class for Turbo tag helpers.
/// </summary>
public abstract class TurboTagHelper : TagHelper
{
    protected static string? ConvertToDomId(object? target)
    {
        if(target is null)
            return null;

        return target switch {
            IHasDomId hasDomId => hasDomId.ToDomId(),
            string @string => @string,
            _ => null
        };
    }
}