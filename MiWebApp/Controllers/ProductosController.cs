using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;


namespace MiWebApp.Controllers;


using MiWebApp.Interfaces;
using MiWebApp.Models;
using MiWebApp.Repositorios;
using MiWebApp.ViewModels;


public class ProductosController : Controller
{


    private IProductoRepository producto;

    private IAutentificarService autorizacon;
    public ProductosController(IProductoRepository prod,IAutentificarService auto )
    {
       // producto = new ProductoRepositorio();


       producto=prod;
       autorizacon=auto;
    }


    [HttpGet]
    public IActionResult Index()
    {
        List<Productos> productos = producto.GetAll();
        List<ProductoViewModel> prodVM = productos.Select(p => new ProductoViewModel(p)).ToList();
        return View(prodVM);

    }



    [HttpGet]

    public IActionResult Details(int id)
    {

        var aux = producto.GetAll();
        var aux1 = aux.FirstOrDefault(p => p.IdProducto == id);

        return View(new ProductoViewModel(aux1));
    }


    [HttpPost]

    public IActionResult Details(ProductoViewModel prodVM)
    {
        /*List<Productos> productos = producto.GetAll();
        var produc=productos.FirstOrDefault(p=>p.IdProducto==prodVM.IdProducto);*/



        var aux = producto.ObtenerProductoPorNombre(prodVM.IdProducto);
        var aux1 = new ProductoViewModel(aux);

        return View(aux1);

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

        var produ = new Productos(producVM);
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
    {
        List<Productos> productos = producto.GetAll();
        var productoEncontrado = productos.FirstOrDefault(p => p.IdProducto == id);

        if (productoEncontrado == null)
            return NotFound();

        var vm = new ProductoViewModel(productoEncontrado);
        return View(vm);
    }


    [HttpPost]
    public IActionResult Edit(ProductoViewModel producVM)
    {
        if (!ModelState.IsValid)
        {
            // vuelve a la vista, mostrando los errores
            return View(producVM);
        }

        
        var produ = new Productos(producVM);

        producto.ActualizarProducto(produ.IdProducto, produ);
        return RedirectToAction("Index");
    }


    [HttpGet]
    public IActionResult Delete(int id)
    {

        return View(new ProductoViewModel(id));
    }



    [HttpPost]
    public IActionResult Delete(ProductoViewModel producVM)
    {
        
        producto.borrarProducto(producVM.IdProducto);
        return RedirectToAction("Index");
    }







    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}