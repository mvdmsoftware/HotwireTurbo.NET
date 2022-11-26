using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HotwireTurbo.NET;

/// <summary>
/// Finds and executes an <see cref="IView"/> for a <see cref="TurboStreamResult"/>.
/// </summary>
public class TurboStreamResultExecutor : ViewExecutor, IActionResultExecutor<TurboStreamResult>
{
    /// <summary>
    /// Creates a new <see cref="PartialViewResultExecutor"/>.
    /// </summary>
    /// <param name="viewOptions">The <see cref="IOptions{TOptions}"/>.</param>
    /// <param name="writerFactory">The <see cref="IHttpResponseStreamWriterFactory"/>.</param>
    /// <param name="viewEngine">The <see cref="ICompositeViewEngine"/>.</param>
    /// <param name="tempDataFactory">The <see cref="ITempDataDictionaryFactory"/>.</param>
    /// <param name="diagnosticListener">The <see cref="DiagnosticListener"/>.</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/>.</param>
    /// <param name="modelMetadataProvider">The <see cref="IModelMetadataProvider"/>.</param>
    public TurboStreamResultExecutor(
        IOptions<MvcViewOptions> viewOptions,
        IHttpResponseStreamWriterFactory writerFactory,
        ICompositeViewEngine viewEngine,
        ITempDataDictionaryFactory tempDataFactory,
        DiagnosticListener diagnosticListener,
        ILoggerFactory loggerFactory,
        IModelMetadataProvider modelMetadataProvider)
        : base(viewOptions, writerFactory, viewEngine, tempDataFactory, diagnosticListener, modelMetadataProvider)
    {
        if (loggerFactory == null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        Logger = loggerFactory.CreateLogger<TurboStreamResultExecutor>();
    }

    /// <summary>
    /// Gets the <see cref="ILogger"/>.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Attempts to find the <see cref="IView"/> associated with <paramref name="viewResult"/>.
    /// </summary>
    /// <param name="actionContext">The <see cref="ActionContext"/> associated with the current request.</param>
    /// <param name="viewResult">The <see cref="TurboStreamResultExecutor"/>.</param>
    /// <returns>A <see cref="ViewEngineResult"/>.</returns>
    public virtual ViewEngineResult FindView(ActionContext actionContext, TurboStreamResult viewResult)
    {
        if (actionContext == null)
        {
            throw new ArgumentNullException(nameof(actionContext));
        }

        if (viewResult == null)
        {
            throw new ArgumentNullException(nameof(viewResult));
        }

        var viewEngine = viewResult.ViewEngine;
        var viewName = viewResult.ViewName;

        var result = viewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: false);
        var originalResult = result;
        if (!result.Success)
        {
            result = viewEngine.FindView(actionContext, viewName, isMainPage: false);
        }

        if (result.Success)
            return result;

        if (!originalResult.SearchedLocations.Any())
            return result;

        if (result.SearchedLocations.Any())
        {
            // Return a new ViewEngineResult listing all searched locations.
            var locations = new List<string>(originalResult.SearchedLocations);
            locations.AddRange(result.SearchedLocations);
            result = ViewEngineResult.NotFound(viewName, locations);
        }
        else
        {
            // GetView() searched locations but FindView() did not. Use first ViewEngineResult.
            result = originalResult;
        }

        return result;
    }

    /// <summary>
    /// Executes the <see cref="IView"/> asynchronously.
    /// </summary>
    /// <param name="actionContext">The <see cref="ActionContext"/> associated with the current request.</param>
    /// <param name="view">The <see cref="IView"/>.</param>
    /// <param name="viewResult">The <see cref="TurboStreamResult"/>.</param>
    /// <returns>A <see cref="Task"/> which will complete when view execution is completed.</returns>
    public virtual async Task ExecuteAsync(ActionContext actionContext, IView view, TurboStreamResult viewResult)
    {
        if (actionContext == null)
        {
            throw new ArgumentNullException(nameof(actionContext));
        }

        if (view == null)
        {
            throw new ArgumentNullException(nameof(view));
        }

        if (viewResult == null)
        {
            throw new ArgumentNullException(nameof(viewResult));
        }

        var preContent = viewResult.Action == "remove"
            ? $"""<turbo-stream action="{viewResult.Action}">"""
            : $"""<turbo-stream action="{viewResult.Action}" target="{viewResult.Target}"><template>""";

        var postContent = viewResult.Action == "remove"
            ? "</turbo-stream>"
            : "</template></turbo-stream>";

        await ExecuteAsync(
            actionContext,
            new WrappedView(view, preContent, postContent),
            viewResult.ViewData,
            viewResult.TempData,
            "text/vnd.turbo-stream.html",
            viewResult.StatusCode);
    }

    /// <inheritdoc />
    public virtual async Task ExecuteAsync(ActionContext context, TurboStreamResult result)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        if (result == null)
            throw new ArgumentNullException(nameof(result));

        var viewEngineResult = FindView(context, result);
        viewEngineResult.EnsureSuccessful(originalLocations: null);

        await ExecuteAsync(context, viewEngineResult.View, result);
    }
}