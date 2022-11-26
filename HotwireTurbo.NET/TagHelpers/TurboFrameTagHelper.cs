using System.ComponentModel;
using System.Threading.Tasks;
using HotwireTurbo.NET.Interfaces;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HotwireTurbo.NET.TagHelpers;

public class TurboFrameTagHelper : TagHelper
{
    public object Id { get; set; }
    public string Target { get; set; }
    public string Src { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var id = convertToDomId(Id);
        output.Attributes.SetAttribute("id", id);
        if (!string.IsNullOrEmpty(Target)) output.Attributes.SetAttribute("target", Target);
        if (!string.IsNullOrEmpty(Src)) output.Attributes.SetAttribute("src", Src);
        output.Content.SetHtmlContent((await output.GetChildContentAsync()).GetContent());
    }

    private string convertToDomId(object target)
    {
        return target switch
        {
            IHasDomId hasDomId => hasDomId.ToDomId(),
            string @string => @string,
            _ => throw new InvalidEnumArgumentException("target must be of type IHasDomId or string")
        };
    }
}