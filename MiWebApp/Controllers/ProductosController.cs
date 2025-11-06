using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
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



    [HttpGet]

    public IActionResult Details()
    {
        return View(new Productos());
    }


    [HttpPost]

    public IActionResult Details(string descripcion)
    {
        var aux = producto.ObtenerProductoPorNombre(descripcion);
        return View(aux);

    }

    //razor no distingue entre mayus y  minus

    [HttpGet]

    public IActionResult Create()
    {
        return View(new Productos());
    }


    [HttpPost]

    public IActionResult Create(Productos produc)
    {
        producto.InsertarProducto(produc);
        return RedirectToAction("Index");

    }




    [HttpGet]
    public IActionResult Edit()
    {

        return View(new Productos());
    }



    [HttpPost]
    public IActionResult Edit(Productos produc)
    {
        producto.ActualizarProducto(produc.IdProducto, produc);
        return RedirectToAction("Index");
    }

    
    [HttpGet]
    public IActionResult Delete()
    {

        return View(new Productos());
    }



    [HttpPost]
    public IActionResult Delete(Productos produc)
    {
        producto.borrarProducto(produc.IdProducto);
        return RedirectToAction("Index");
    }







    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}