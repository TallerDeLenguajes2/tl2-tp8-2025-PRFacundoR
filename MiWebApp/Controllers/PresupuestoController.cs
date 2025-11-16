using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;


namespace MiWebApp.Controllers;

using MiWebApp.Repositorios;
using MiWebApp.ViewModels;
using MiWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiWebApp.interfaces;
using Microsoft.AspNetCore.Authentication;

public class PresupuestoController : Controller
{
    private IPresupuestoRepository presu;

    private IAuthenticationService autorizacion;
    private ProductoRepositorio producto;
    public PresupuestoController(IPresupuestoRepository prR ,IAuthenticationService auto)
    {//porque ya no se usa new presupuesto repo?
        presu = prR;
        producto = new ProductoRepositorio();
        autorizacion=auto;
    }






    [HttpGet]
    public IActionResult Index()
    {
        List<Presupuestos> presupuestos = presu.GetAllPresupuestos();
        List<PresupuestoViewModel> presupuestosVM = presupuestos.Select(p => new PresupuestoViewModel(p)).ToList();
        return View(presupuestosVM);

    }


    /*<input type="hidden" asp-for="IdPresupuesto" />  agregar luego*/


    /*public IActionResult Details(int id)
    {


        return View(new PresupuestoViewModel(id));
    }asi si funciona*/


    [HttpGet]
    public IActionResult Details(int id)
    {
        List<Presupuestos> presupuestos = presu.GetAllPresupuestos();
        var aux = presupuestos.FirstOrDefault(p => p.IdPresupuesto == id);



        return View(new PresupuestoViewModel(aux));
    }

    [HttpPost]

    public IActionResult Details(PresupuestoViewModel pr)
    {
        var aux = presu.obtenerPresupuestoPorId(pr.IdPresupuesto);
        var aux1 = new PresupuestoViewModel(aux);
        return View(aux1);

    }

    //razor no distingue entre mayus y  minus

    [HttpGet]

    public IActionResult Create()
    {
        List<Presupuestos> presupuestos = presu.GetAllPresupuestos();
        int idDeUltimo = presupuestos.LastOrDefault().IdPresupuesto;
        var pvm = new PresupuestoViewModel(idDeUltimo + 1);
        pvm.FechaCreacion = DateTime.Now;
        return View(pvm);
    }


    [HttpPost]

    public IActionResult Create(PresupuestoViewModel presupuestoVM)
    {

        if (!(presupuestoVM.FechaCreacion <= DateTime.Now))
        {
            ModelState.AddModelError("FechaCreacion", "La fecha no puede ser posterior a hoy");


        }


        if (!ModelState.IsValid)
        {
            // vuelve a la vista, mostrando los errores
            return View(presupuestoVM);
        }


        Presupuestos presupuesto = new Presupuestos(presupuestoVM);

        presu.CrearPresupuesto(presupuesto);
        return RedirectToAction("Index");



    }




    [HttpGet]
    public IActionResult Edit(int id)
    {
        List<Presupuestos> presupuestos = presu.GetAllPresupuestos();
        var presuEncontrado = presupuestos.FirstOrDefault(p => p.IdPresupuesto == id);

        if (presuEncontrado == null)
            return NotFound();


        var vm = new PresupuestoViewModel(presuEncontrado);
        return View(vm);
    }



    [HttpPost]
    public IActionResult Edit(PresupuestoViewModel presupuesto)
    {

        if (!(presupuesto.FechaCreacion <= DateTime.Now))
        {
            ModelState.AddModelError("FechaCreacion", "La fecha no puede ser posterior a hoy");


        }
        if (!ModelState.IsValid)
        {
            return View(presupuesto);
        }

         
        var presup = new Presupuestos(presupuesto);
        presu.ActualizarPresupuesto(presup.IdPresupuesto, presup);
        return RedirectToAction("Index");
    }


    [HttpGet]
    public IActionResult Delete(int id)
    {

        return View(new PresupuestoViewModel(id));
    }



    [HttpPost]
    public IActionResult Delete(PresupuestoViewModel presupuesto)
    {
        
        presu.borrarPresupuesto(presupuesto.IdPresupuesto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult AgregarProducto(int id)
    {
        List<Productos> productos = producto.GetAll();

        AgregarProductoViewModel model = new AgregarProductoViewModel()
        {
            IdPresupuesto = id, // Pasamos el ID del presupuesto actual
                                // 3. Crear el SelectList
            ListaProductos = new SelectList(productos, "IdProducto", "Descripcion")
        };

        return View(model);
    }



    [HttpPost]
    public IActionResult AgregarProducto(AgregarProductoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            List<Productos> productos = producto.GetAll();
            model.ListaProductos = new SelectList(productos, "IdProducto", "Descripcion");
            return View(model);

        }

        if (presu.GetAllPresupuestos().Exists(p=>p.IdPresupuesto==model.IdPresupuesto && p.Detalle.Exists(p1=>p1.Producto.IdProducto==model.IdProducto)))
        {
           var auxPresu= presu.GetAllPresupuestos().FirstOrDefault(p=>p.IdPresupuesto==model.IdPresupuesto);
        
           var cant= auxPresu.cantidadDeUnProducto(model.IdProducto)+ model.Cantidad;
            presu.actualizarCantidad(model.IdPresupuesto, model.IdProducto, cant);
        }else
        {
            presu.AgregarProductoAPresupuesto(model.IdPresupuesto, model.IdProducto, model.Cantidad);
        }        
        
        return RedirectToAction("Details", new { id = model.IdPresupuesto });
    }



    [HttpGet]

    public IActionResult EditCantidad(int idProducto, int idPresupuesto)
    {
        List<Productos> productos = producto.GetAll();

        var model=new AgregarProductoViewModel(idProducto, idPresupuesto);

        return View(model);
        
    }

    [HttpPost]
    public IActionResult EditCantidad(AgregarProductoViewModel model)
    {

        if (!ModelState.IsValid)
        {   
            ModelState.AddModelError("IdProducto", "Falta producto");
            ModelState.AddModelError("IdPresupuesto", "Falta presupuesto");
            return View(model);
        }

        presu.actualizarCantidad(model.IdPresupuesto,model.IdProducto, model.Cantidad);
        return RedirectToAction("Details", new { id = model.IdPresupuesto });
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}//asp-route-idProducto
//osea porque route se llama idProducto entonces en el get debe llamarse asi?
//Sí, exactamente.
//En ASP.NET Core MVC, los nombres de las variables que vos pasás con asp-route-xxxx deben coincidir con los nombres de los parámetros del método GET. Si no coinciden, el valor no se enlaza y te llega 0, null, o el valor por defecto.