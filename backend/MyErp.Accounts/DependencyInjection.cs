using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyErp.Accounts;

/// <summary>
/// Composition entry point for the Accounts module. The host calls this once at
/// startup; nothing outside this file registers Accounts services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the module's services and makes its controllers visible to MVC.
    /// </summary>
    public static IMvcBuilder AddAccountsModule(this IMvcBuilder mvc, IConfiguration configuration)
    {
        // Controllers live in this assembly, so MVC has to be told about it.
        mvc.AddApplicationPart(typeof(DependencyInjection).Assembly);

        // Register feature services here as features are added, for example:
        // mvc.Services.AddScoped<IItemGroupService, ItemGroupService>();

        return mvc;
    }
}
