using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HotwireTurbo.NET.TagHelpers;

public class TurboFrameTagHelper : TurboTagHelper
{
    public object Id { get; set; }
    public string Target { get; set; }
    public string Src { get; set; }

    /// <inheritdoc />
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var id = ConvertToDomId(Id);
        output.Attributes.SetAttribute("id", id);
        if (!string.IsNullOrEmpty(Target)) output.Attributes.SetAttribute("target", Target);
        if (!string.IsNullOrEmpty(Src)) output.Attributes.SetAttribute("src", Src);
        output.Content.SetHtmlContent((await output.GetChildContentAsync()).GetContent());
    }
}