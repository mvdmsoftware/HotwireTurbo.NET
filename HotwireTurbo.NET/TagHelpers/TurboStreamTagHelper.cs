using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HotwireTurbo.NET.TagHelpers;

public abstract class TurboStreamTagHelper : TurboTagHelper
{
    private readonly string _action;

    public object Target { get; set; }
    
    protected TurboStreamTagHelper(string action)
    {
        _action = action;
    }

    /// <inheritdoc />
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var target = ConvertToDomId(Target);
        output.Attributes.SetAttribute("action", _action);
        output.Attributes.SetAttribute("target", target);
        var pre = _action == "remove" ? "" : "<template>";
        output.PreContent.SetHtmlContent(pre);
        var post = _action == "remove" ? "" : "</template>";
        output.PostContent.SetHtmlContent(post);

        if (_action != "remove")
        {
            var content = await output.GetChildContentAsync();
            output.Content.SetHtmlContent(content.GetContent());
        }
    }
}
