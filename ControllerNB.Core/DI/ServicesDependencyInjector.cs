using ControllerNB.Services;
using ControllerNB.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ControllerNB.Core.DI;

public static class ServicesDependencyInjector
{
    public static void InjectServices(this IServiceCollection services)
    {
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<ITeacherService, TeacherService>();
    }
}
