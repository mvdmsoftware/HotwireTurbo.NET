using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace HotwireTurbo.NET.Extensions;

/// <summary>
/// Extension methods for Turbo in .NET.
/// </summary>
public static class TurboExtensions
{
    /// <summary>
    /// Check if the request is a turbo-frame request.
    /// </summary>
    /// <param name="request">The <see cref="HttpRequest"/> to check.</param>
    /// <returns>True if the turbo-frame header is set. False otherwise.</returns>
    public static bool IsTurboFrameRequest(this HttpRequest request)
    {
        return request.Headers.Any(h => h.Key == "turbo-frame");
    }

    /// <summary>
    /// Check if the request is a turbo-stream request.
    /// </summary>
    /// <param name="request">The <see cref="HttpRequest"/> to check.</param>
    /// <returns>True if the media type header is set to text/vnd.turbo-stream.html. False otherwise.</returns>
    public static bool IsTurboStreamRequest(this HttpRequest request)
    {
        return request.GetTypedHeaders().Accept.Any(h => h.MediaType == "text/vnd.turbo-stream.html");
    }

    /// <summary>
    /// Adds Turbo to the service pipeline.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add Turbo to.</param>
    /// <returns>The <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddHotwireTurbo(this IServiceCollection services)
    {
        return services.AddSingleton<IActionResultExecutor<TurboStreamResult>, TurboStreamResultExecutor>();
    }
}