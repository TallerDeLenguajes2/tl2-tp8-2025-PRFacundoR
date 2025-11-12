using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;


namespace MiWebApp.Controllers;

using MiWebApp.Repositorios;
using MiWebApp.ViewModels;
using MiWebApp.Models;
public class PresupuestoController : Controller
{
    private PresupuestosRepository presu;
    public PresupuestoController()
    {
        presu = new PresupuestosRepository();
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Presupuestos> presupuestos = presu.GetAllPresupuestos();
        return View(presupuestos);

    }


    /*<input type="hidden" asp-for="IdPresupuesto" />  agregar luego*/
    [HttpGet]

    public IActionResult Details()
    {
        return View(new Presupuestos());
    }


    [HttpPost]

    public IActionResult Details(Presupuestos pr)
    {
        var aux = presu.obtenerPresupuestoPorId(pr.IdPresupuesto);
        return View(aux);

    }

    //razor no distingue entre mayus y  minus

    [HttpGet]

    public IActionResult Create()
    {
        List<Presupuestos> presupuestos = presu.GetAllPresupuestos();
        int idDeUltimo = presupuestos.LastOrDefault().IdPresupuesto;

        return View(new PresupuestoViewModel(idDeUltimo + 1));
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
    public IActionResult Edit()
    {

        return View(new PresupuestoViewModel());
    }



    [HttpPost]
    public IActionResult Edit(PresupuestoViewModel presupuesto)
    {   

        var presup=new Presupuestos(presupuesto);
        presu.ActualizarPresupuesto(presup.IdPresupuesto, presup);
        return RedirectToAction("Index");
    }


    [HttpGet]
    public IActionResult Delete()
    {

        return View(new PresupuestoViewModel());
    }



    [HttpPost]
    public IActionResult Delete(PresupuestoViewModel presupuesto)
    {
        var presup=new Presupuestos(presupuesto);
        presu.borrarPresupuesto(presup.IdPresupuesto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult AgregarProducto()
    {

        return View(new PresupuestoDetalle());
    }



    [HttpPost]
    public IActionResult AgregarProducto(Presupuestos presupuesto, PresupuestoDetalle detalle)
    {
        presu.AgregarProductoAPresupuesto(presupuesto.IdPresupuesto, detalle.Producto.IdProducto, detalle.Cantidad);
        return RedirectToAction("Index");
    }






    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}