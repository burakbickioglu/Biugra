
using Biugra.Infrastructure.Interfaces.Repositories;
using Biugra.Infrastructure.Persistance.Repositories;

namespace Biugra.Service.Utilities.DependencyResolvers;

public static class DependencyExtension
{
    public static void AddDependencies(this IServiceCollection services, IConfiguration Configuration)
    {
        services.AddScoped(typeof(IGeneralContentRepository<>), typeof(GeneralContentRepository<>));
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddScoped<IBlobService, BlobService>();

        services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(RequestExceptionHandlerBehaviour<,,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestAuthorizationBehaviour<,>));
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IGoogleLoginHelper, GoogleLoginHelper>();
        services.AddScoped<IMailService, MailService>();
        services.AddHttpContextAccessor();
    }
}
