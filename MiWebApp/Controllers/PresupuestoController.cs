using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;


namespace MiWebApp.Controllers;

using MiWebApp.Repositorios;
using MiWebApp.ViewModels;
using MiWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiWebApp.Interfaces;

public class PresupuestoController : Controller
{
    private readonly IPresupuestoRepository presu;

    private readonly IAutentificarService autorizacon;
    private readonly IProductoRepository producto;


    public PresupuestoController(IPresupuestoRepository prR, IAutentificarService auto, IProductoRepository prod)
    {//porque ya no se usa new presupuesto repo? porque lo hace el builder
        presu = prR;
        producto = prod;
        autorizacon = auto;
    }

    private IActionResult CheckAdminPermissions()
    {
        // 1. No logueado? -> vuelve al login
        if (!autorizacon.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        // 2. No es Administrador? -> Da Error
        if (!autorizacon.HasAccesLevel("Administrador"))
        {
            // Llamamos a AccesoDenegado (llama a la vista correspondiente de Productos)
            return RedirectToAction(nameof(AccesoDenegado));
        }
        return null; // Permiso concedido
    }




    [HttpGet]
    public IActionResult Index()
    {
        if (!autorizacon.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        if (autorizacon.HasAccesLevel("Administrador")||autorizacon.HasAccesLevel("Cliente"))
        {
               List<Presupuestos> presupuestos = presu.GetAllPresupuestos();
        List<PresupuestoViewModel> presupuestosVM = presupuestos.Select(p => new PresupuestoViewModel(p)).ToList();
        return View(presupuestosVM);
        }
        else
        {
            return RedirectToAction("Index", "Login");
        }
     

    }


    /*<input type="hidden" asp-for="IdPresupuesto" />  agregar luego*/


    /*public IActionResult Details(int id)
    {


        return View(new PresupuestoViewModel(id));
    }asi si funciona*/


    [HttpGet]
    public IActionResult Details(int id)
    {
        if (!autorizacon.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        if (autorizacon.HasAccesLevel("Administrador")||autorizacon.HasAccesLevel("Cliente"))
        {
        List<Presupuestos> presupuestos = presu.GetAllPresupuestos();
        var aux = presupuestos.FirstOrDefault(p => p.IdPresupuesto == id);
        return View(new PresupuestoViewModel(aux));
        }
        else
        {
            return RedirectToAction("Index", "Login");
        }

    }

    [HttpPost]

    public IActionResult Details(PresupuestoViewModel pr)
    {
        if (!autorizacon.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        if (autorizacon.HasAccesLevel("Administrador")||autorizacon.HasAccesLevel("Cliente"))
        {
        var aux = presu.obtenerPresupuestoPorId(pr.IdPresupuesto);
        var aux1 = new PresupuestoViewModel(aux);
        return View(aux1);
        }
        else
        {
            return RedirectToAction("Index", "Login");
        }

    }

    //razor no distingue entre mayus y  minus

    [HttpGet]

    public IActionResult Create()
    {

        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;

        List<Presupuestos> presupuestos = presu.GetAllPresupuestos();


        int siguienteId = 1;
        if (presupuestos != null && presupuestos.Any())// la lista que trajimos
    {
        // 3. Si hay productos, obtén el Id del último y suma 1.
        int idDeUltimo = presupuestos.LastOrDefault().IdPresupuesto;
        siguienteId = idDeUltimo + 1;
        }
        var pvm = new PresupuestoViewModel(siguienteId+ 1);
        pvm.FechaCreacion = DateTime.Now;
        return View(pvm);
    }


    [HttpPost]

    public IActionResult Create(PresupuestoViewModel presupuestoVM)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;


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
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;

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
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;

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
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;

        return View(new PresupuestoViewModel(id));
    }



    [HttpPost]
    public IActionResult Delete(PresupuestoViewModel presupuesto)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;

        presu.borrarPresupuesto(presupuesto.IdPresupuesto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult AgregarProducto(int id)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;

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
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;

        if (!ModelState.IsValid)
        {
            List<Productos> productos = producto.GetAll();
            model.ListaProductos = new SelectList(productos, "IdProducto", "Descripcion");
            return View(model);

        }

        if (presu.GetAllPresupuestos().Exists(p => p.IdPresupuesto == model.IdPresupuesto && p.Detalle.Exists(p1 => p1.Producto.IdProducto == model.IdProducto)))
        {
            var auxPresu = presu.GetAllPresupuestos().FirstOrDefault(p => p.IdPresupuesto == model.IdPresupuesto);

            var cant = auxPresu.cantidadDeUnProducto(model.IdProducto) + model.Cantidad;
            presu.actualizarCantidad(model.IdPresupuesto, model.IdProducto, cant);
        }
        else
        {
            presu.AgregarProductoAPresupuesto(model.IdPresupuesto, model.IdProducto, model.Cantidad);
        }

        return RedirectToAction("Details", new { id = model.IdPresupuesto });
    }



    [HttpGet]

    public IActionResult EditCantidad(int idProducto, int idPresupuesto)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;

        List<Productos> productos = producto.GetAll();

        var model = new AgregarProductoViewModel(idProducto, idPresupuesto);

        return View(model);

    }

    [HttpPost]
    public IActionResult EditCantidad(AgregarProductoViewModel model)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("IdProducto", "Falta producto");
            ModelState.AddModelError("IdPresupuesto", "Falta presupuesto");
            return View(model);
        }

        presu.actualizarCantidad(model.IdPresupuesto, model.IdProducto, model.Cantidad);
        return RedirectToAction("Details", new { id = model.IdPresupuesto });
    }


    public IActionResult AccesoDenegado()
    {
        return View();
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