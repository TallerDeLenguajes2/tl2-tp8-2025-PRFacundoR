using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiWebApp.Models;

namespace MiWebApp.Controllers;

public class ProductosController : Controller
{


    private ProductoRepositorio producto;
    public ProductosController()
    {
        producto = new ProductoRepositorio();
    }


    [HttpGet]
    public IActionResult Index()
    {
        List<Productos> productos = producto.GetAll();
        return View(productos);

    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}