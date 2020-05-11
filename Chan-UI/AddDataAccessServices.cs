using System;
using System.Net.Http;
using Framework.Interfaces.Services;
using Framework.Interfaces.Settings;
using Framework.Settings;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Chan_UI
{
    public static class ServiceExtensions
    {
        public static void AddDataAccessServices(this IServiceCollection services, DataAccessSettings settings)
        {
            services.AddSingleton<IDataAccessSettings, DataAccessSettings>(s => settings);

            services.AddHttpClient<IBoardService, BoardService>(client =>
            {
                client.BaseAddress = new Uri(settings.EndpointUrlSettings.BoardServiceEndPoint);
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
                return handler;
            });
            services.AddHttpClient<IImageService, ImageService>(client =>
            {
                client.BaseAddress = new Uri(settings.EndpointUrlSettings.ImageServiceEndPoint);
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
                return handler;
            });
        }
    }
}
