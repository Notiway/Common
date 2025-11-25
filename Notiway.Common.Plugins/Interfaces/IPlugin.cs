using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Notiway.Common.Core.Enums;

namespace Notiway.Common.Plugins.Interfaces;
public interface IPlugin
{
    /// <summary>
    /// Name of the plugin
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Midlewares priority order. Lower number means it will be executed first.
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// Infrastructure provider the plugin is built for
    /// </summary>
    InfrastructureProviders Provider { get; }

    /// <summary>
    /// Service registration for this plugin
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    void Register(IServiceCollection services, IConfiguration configuration);

    /// <summary>
    /// Middlewares of this plugin. Executed based on priority.
    /// Default middleware priorities
    /// HttpRedirection: 100
    /// Hsts: 200
    /// Cors: 300
    /// </summary>
    /// <param name="app"></param>
    void Use(IApplicationBuilder app);
}