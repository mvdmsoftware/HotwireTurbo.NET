using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HotwireTurbo.NET.TagHelpers;

/// <summary>
/// Tag helper for turbo-frame tags.
/// </summary>
[HtmlTargetElement("turbo-frame", Attributes = "id, target, src")]
public class TurboFrameTagHelper : TurboTagHelper
{
    /// <inheritdoc />
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var id = ConvertToDomId(context.AllAttributes["id"].Value);
        var target = context.AllAttributes["target"].ToString();
        var src = context.AllAttributes["src"].ToString();

        if(id is not null)
            output.Attributes.SetAttribute("id", id);

        if (!string.IsNullOrEmpty(target)) 
            output.Attributes.SetAttribute("target", target);
        if (!string.IsNullOrEmpty(src)) 
            output.Attributes.SetAttribute("src", src);

        output.Content.SetHtmlContent((await output.GetChildContentAsync()).GetContent());
    }
}