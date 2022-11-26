using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace HotwireTurbo.NET;

/// <summary>
/// Provides a way to wrap a View with pre- and post-content.
/// </summary>
internal class WrappedView : IView
{
    private readonly IView _innerView;
    private readonly string _preContent;
    private readonly string _postContent;

    /// <inheritdoc />
    public string Path => _innerView.Path;

    /// <summary>
    /// Creates a new <see cref="WrappedView"/> object.
    /// </summary>
    /// <param name="innerView">The view to wrap.</param>
    /// <param name="preContent">The pre-content.</param>
    /// <param name="postContent">The post-content.</param>
    public WrappedView(IView innerView, string preContent, string postContent)
    {
        _innerView = innerView;
        _preContent = preContent;
        _postContent = postContent;
    }

    /// <inheritdoc />
    public async Task RenderAsync(ViewContext context)
    {
        await context.Writer.WriteAsync(_preContent);
        await _innerView.RenderAsync(context);
        await context.Writer.WriteAsync(_postContent);
    }
}