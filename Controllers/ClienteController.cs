using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using tl2_tp6_2024_MaguiNavarro.Models;
namespace tl2_tp6_2024_MaguiNavarro.Controllers;

public class ClienteController: Controller
{
     private readonly ILogger<HomeController> _logger;
    private ClientesRepository cliRep;

    public ClienteController(ILogger<HomeController> logger)
    {
        _logger = logger;
        cliRep = new ClientesRepository();
    }

    public IActionResult Listar()
    {
        return View(cliRep.ObtenerClientes());
    }

    [HttpGet]
    public IActionResult Crear()
    {
        return View(new ClienteViewModel());
    }

    [HttpPost]
    public IActionResult Crear(ClienteViewModel clienteVM)
    {
        if(!ModelState.IsValid) return RedirectToAction("Listar");

        var cliente = new Cliente(clienteVM); 
        cliRep.CrearCliente(cliente);
        return RedirectToAction("Listar");
    } 

    [HttpGet]
    public IActionResult Modificar(int idCli)
    {
        var cliente = cliRep.ObtenerCliente(idCli);
        return View(new ClienteViewModel(cliente));
    }

    [HttpPost]
    public IActionResult Modificar(ClienteViewModel clienteView)
    {

        if(!ModelState.IsValid) return RedirectToAction("Listar");

        var cliente = new Cliente(clienteView);
        cliRep.ModificarCliente(cliente);

        return RedirectToAction("Listar");
    }

    [HttpGet]
    public IActionResult Eliminar(int idCli)
    {
        var producto = cliRep.ObtenerCliente(idCli);
        return View(producto);
    }

    [HttpPost]
    public IActionResult EliminarConfirm(int idCli)
    {    
        cliRep.EliminarCliente(idCli);
        return RedirectToAction("Listar");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}