using System.ComponentModel;
using System.Threading.Tasks;
using HotwireTurbo.NET.Interfaces;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HotwireTurbo.NET.TagHelpers;

public abstract class TurboStreamTagHelper : TagHelper
{
    public object Target { get; set; }
    private string action;

    public TurboStreamTagHelper(string action)
    {
        this.action = action;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var target = convertToDomId(Target);
        output.Attributes.SetAttribute("action", action);
        output.Attributes.SetAttribute("target", target);
        var pre = action == "remove"
            ? ""
            : "<template>";
        output.PreContent.SetHtmlContent(pre);
        var post = action == "remove"
            ? ""
            : "</template>";
        output.PostContent.SetHtmlContent(post);
        if (action != "remove")
        {
            var content = await output.GetChildContentAsync();
            output.Content.SetHtmlContent(content.GetContent());
        }
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
