using System.ComponentModel;
using HotwireTurbo.NET.Interfaces;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HotwireTurbo.NET.TagHelpers;

public abstract class TurboTagHelper : TagHelper
{
    protected static string ConvertToDomId(object target)
    {
        return target switch {
            IHasDomId hasDomId => hasDomId.ToDomId(),
            string @string => @string,
            _ => throw new InvalidEnumArgumentException("target must be of type IHasDomId or string")
        };
    }
}