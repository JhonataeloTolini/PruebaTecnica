using PruebaTecnica.Interface;

namespace PruebaTecnica.Repository
{
    public static class ServiceExtensions
    {
        public static void AddPersistenceInfraestructure(this IServiceCollection service, IConfiguration configuration)
        {

            service.AddTransient(typeof(IMovimientoRepoAsync), typeof(MovimientoRepoAsync));

        }
    }
}
