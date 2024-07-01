using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Interface;
using PruebaTecnica.Models;
using PruebaTecnica.Repository;
using System.Diagnostics;

namespace PruebaTecnica.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IMovimientoRepoAsync _movimientoRepoAsync;

        public HomeController(ILogger<HomeController> logger, IMovimientoRepoAsync movimientoRepoAsync)
        {
            _logger = logger;
            _movimientoRepoAsync = movimientoRepoAsync;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            List<Movimiento> lista = await _movimientoRepoAsync.ListMovimiento("2024-01-01","2024-12-31","*","*");

            CabMovimiento cab = new CabMovimiento();

            cab.movimientos = lista;

            return View(cab);

        }

        [HttpPost]
        public async Task<IActionResult> Consultar(CabMovimiento MovInput)
        {

            var fechaInicio =  MovInput.fechaInicio;
            var fechaFinal = MovInput.fechaFinal;
            var tipoMov = MovInput.tipoMov;
            var nroDoc = MovInput.NroDoc;

            tipoMov = (tipoMov == "" || tipoMov== null ? "*" : tipoMov);
            nroDoc = (nroDoc == "" || nroDoc == null ? "*" : nroDoc);

            List <Movimiento> lista = await _movimientoRepoAsync.ListMovimiento(fechaInicio, fechaFinal, tipoMov, nroDoc);

            CabMovimiento cab = new CabMovimiento();

            cab.movimientos = lista;

            return View("Index",cab);

        }

        [HttpPost]
        public async Task<IActionResult> Registrar(CabMovimiento MovInput)
        {

          
            var tipoMov = MovInput.tipoMov;
            var nroDoc = MovInput.NroDoc;

            bool flg = await _movimientoRepoAsync.RegistrarMovimiento(MovInput);

            List<Movimiento> lista = await _movimientoRepoAsync.ListMovimiento("2024-01-01", "2024-12-31", "*", "*");

            MovInput.movimientos = lista;

            MovInput.tipoMov = "";
            MovInput.NroDoc = "";

            return View("Index", MovInput);

        }

        //[HttpGet]
        //public async Task<IActionResult> Listar()
        //{

        //    List<Movimiento> lista = await _movimientoRepoAsync.ListMovimiento();

        //    return View(lista);
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
