using PruebaTecnica.Models;

namespace PruebaTecnica.Interface
{
    public interface IMovimientoRepoAsync
    {

        public Task<List<Movimiento>> ListMovimiento(string fechaInicio, string fechaFin, string tipoMovimiento, string NroDoc);

        public  Task<bool> RegistrarMovimiento(CabMovimiento modelo);

    }
}
