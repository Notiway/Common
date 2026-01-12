using Notiway.Common.Core.Models;

namespace Notiway.Common.Plugins.Interfaces;

public interface IServiceConfig
{
    string GetEnvironmentName();

    Task<Result<string>> GetServiceIdAsync();
}
