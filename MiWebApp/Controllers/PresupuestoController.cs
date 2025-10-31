using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiWebApp.Models;

namespace MiWebApp.Controllers;

public class PresupuestoController: Controller
{
    
    public PresupuestoController()
    {
        
    }

    public IActionResult Index()
    {
        return View();
    }

   

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}