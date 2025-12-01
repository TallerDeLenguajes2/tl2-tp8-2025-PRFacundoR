using Microsoft.AspNetCore.Mvc;
using MiWebApp.Interfaces;
using MiWebApp.ViewModels;
using MiWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MiWebApp.Controllers;


public class LoginController : Controller
{
    private readonly IAutentificarService auntetificacion;

    public LoginController(IAutentificarService auntetificacion)
    {

        this.auntetificacion = auntetificacion;
    }

    [HttpGet]
    public IActionResult Index()
    {

        return View(new LoginViewModel());
    }


    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {

        if (!ModelState.IsValid)
    {
        // Los mensajes de error de los Data Annotations viajan con el 'model'.
        return View("Index", model); 
    }

        if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
        {
           // model.ErrorMessage = "Debe ingresar usuario y contraseña.";

            return View("Index", model);
        }
        if (auntetificacion.Login(model.Username, model.Password))
        {
            return RedirectToAction("Index", "Home");
        }


        
        model.ErrorMessage = "Usuario o contraseña incorrecto.";
        
        return View("Index", model);
    }
    // [HttpGet] Cierra sesión
    public IActionResult Logout()
    {
        auntetificacion.Logout();
        return RedirectToAction("Index");
    }
}