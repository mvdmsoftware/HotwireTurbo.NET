using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace HotwireTurbo {
    public static class TurboExtensions {
        public static bool IsTurboFrameRequest(this HttpRequest request) {
            return request.Headers.Any(h => h.Key == "turbo-frame");
        }
        
        public static bool IsTurboStreamRequest(this HttpRequest request) {
            return request.GetTypedHeaders().Accept.Any(h => h.MediaType == "text/vnd.turbo-stream.html");
        }

        public static IServiceCollection AddHotwireTurbo(this IServiceCollection services) {
            return services.AddSingleton<IActionResultExecutor<TurboStreamResult>, TurboStreamResultExecutor>();
        }
    }
}