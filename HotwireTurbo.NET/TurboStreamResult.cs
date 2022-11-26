using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace HotwireTurbo.NET;

public class TurboStreamResult : ActionResult, IStatusCodeActionResult
{
    public string Action { get; }
    public string Target { get; }

    public TurboStreamResult(string action, string target)
    {
        Action = action;
        Target = target;
    }

    /// <summary>
    /// Gets or sets the HTTP status code.
    /// </summary>
    public int? StatusCode { get; set; }

    /// <summary>
    /// Gets or sets the name or path of the partial view that is rendered to the response.
    /// </summary>
    /// <remarks>
    /// When <c>null</c>, defaults to <see cref="ControllerActionDescriptor.ActionName"/>.
    /// </remarks>
    public string ViewName { get; set; }

    /// <summary>
    /// Gets the view data model.
    /// </summary>
    public object Model => ViewData.Model;

    /// <summary>
    /// Gets or sets the <see cref="ViewDataDictionary"/> used for rendering the view for this result.
    /// </summary>
    public ViewDataDictionary ViewData { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="ITempDataDictionary"/> used for rendering the view for this result.
    /// </summary>
    public ITempDataDictionary TempData { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="IViewEngine"/> used to locate views.
    /// </summary>
    /// <remarks>When <c>null</c>, an instance of <see cref="ICompositeViewEngine"/> from
    /// <c>ActionContext.HttpContext.RequestServices</c> is used.</remarks>
    public IViewEngine ViewEngine { get; set; }

    /// <inheritdoc />
    public override Task ExecuteResultAsync(ActionContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var services = context.HttpContext.RequestServices;
        var executor = services.GetService<IActionResultExecutor<TurboStreamResult>>();
        if (executor == null)
        { // TODO match framework exception format
            throw new InvalidOperationException("IActionResultExecutor<TurboStreamResult> not registered");
        }

        return executor.ExecuteAsync(context, this);
    }
}