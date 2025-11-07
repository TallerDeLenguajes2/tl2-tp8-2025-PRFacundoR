using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiWebApp.Models;

namespace MiWebApp.Controllers;

public class PresupuestoController: Controller
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



    [HttpGet]

    public IActionResult Details()
    {
        return View(new Presupuestos());
    }


    [HttpPost]

    public IActionResult Details(int IdPresupuesto)
    {
        var aux = presu.obtenerPresupuestoPorId(IdPresupuesto);
        return View(aux);

    }

    //razor no distingue entre mayus y  minus

    [HttpGet]

    public IActionResult Create()
    {
        return View(new Presupuestos());
    }


    [HttpPost]

    public IActionResult Create(Presupuestos presupuesto)
    {
        presu.CrearPresupuesto(presupuesto);
        return RedirectToAction("Index");

    }




    [HttpGet]
    public IActionResult Edit()
    {

        return View(new Presupuestos());
    }



    [HttpPost]
    public IActionResult Edit(Presupuestos presupuesto)
    {
        presu.ActualizarPresupuesto(presupuesto.IdPresupuesto, presupuesto);
        return RedirectToAction("Index");
    }

    
    [HttpGet]
    public IActionResult Delete()
    {

        return View(new Presupuestos());
    }



    [HttpPost]
    public IActionResult Delete(Presupuestos presupuesto)
    {
        presu.borrarPresupuesto(presupuesto.IdPresupuesto);
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