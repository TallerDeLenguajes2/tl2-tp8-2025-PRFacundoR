using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;


namespace MiWebApp.Controllers;


using MiWebApp.Models;
using MiWebApp.Repositorios;
using MiWebApp.ViewModels;


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

    public IActionResult Details(int id)
    {
        var aux = producto.ObtenerProductoPorNombre(id);
        return View(aux);

    }

    //razor no distingue entre mayus y  minus

    [HttpGet]

    public IActionResult Create()
    {
        List<Productos> productos = producto.GetAll();
        int idDeUltimo = productos.LastOrDefault().IdProducto;
        return View(new ProductoViewModel(idDeUltimo + 1));
    }


    [HttpPost]

    public IActionResult Create(ProductoViewModel producVM)
    {



        if (!ModelState.IsValid)
        {
            // vuelve a la vista, mostrando los errores
            return View(producVM);
        }

        var produ=new Productos(producVM);
        producto.InsertarProducto(produ);
        return RedirectToAction("Index");

    }




/*    [HttpGet]
    public IActionResult Edit(int id)
    {
        
        return View(new ProductoViewModel(id));
    }
    
sin data en el form
*/
[HttpGet]
public IActionResult Edit(int id)
{   List<Productos> productos = producto.GetAll();
    var productoEncontrado = productos.FirstOrDefault(p=>p.IdProducto==id);

    if (productoEncontrado == null)
        return NotFound();

    var vm = new ProductoViewModel(productoEncontrado);
    return View(vm);
}


    [HttpPost]
    public IActionResult Edit(ProductoViewModel producVM)
    {

 
        var produ=new Productos(producVM);

        producto.ActualizarProducto(produ.IdProducto, produ);
        return RedirectToAction("Index");
    }

    
    [HttpGet]
    public IActionResult Delete()
    {

        return View(new ProductoViewModel());
    }



    [HttpPost]
    public IActionResult Delete(ProductoViewModel producVM)
    { var produ=new Productos(producVM);
        producto.borrarProducto(produ.IdProducto);
        return RedirectToAction("Index");
    }







    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}