using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace HotwireTurbo;

public class WrapperView : IView
{
    private IView innerView;
    private string preContent;
    private string postContent;

    public string Path { get; }

    public WrapperView(IView innerView, string preContent, string postContent)
    {
        this.innerView = innerView;
        this.preContent = preContent;
        this.postContent = postContent;
    }

    public async Task RenderAsync(ViewContext context)
    {
        context.Writer.Write(preContent);
        await innerView.RenderAsync(context);
        context.Writer.Write(postContent);
    }
}