using DisprzTraining.Business;
using DisprzTraining.DataAccess;

namespace DisprzTraining.Utils
{
    public static class ConfigureDependenciesExtension
    {
        public static void ConfigureDependencyInjections(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<IAppointmentBL,AppointmentBL>();
            services.AddSingleton<IAppointmentDAL,AppointmentDAL>();
            
        }
    }
}
