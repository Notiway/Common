using Notiway.Common.Core.Models;

namespace Notiway.Common.Plugins.Interfaces;

public interface IServiceConfig
{
    Task<Result<string>> GetServiceIdAsync();

    Task<Result<(int cpu, int memory)>> GetResourceAllocationAsync();
}
