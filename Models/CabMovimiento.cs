namespace PruebaTecnica.Models
{
    public class CabMovimiento
    {

        public string fechaInicio { get; set; }
        public string fechaFinal { get; set; }

        public string tipoMov { get; set; }

        public string NroDoc { get; set; }

        public List<Movimiento> movimientos { get; set; }

    }
}
