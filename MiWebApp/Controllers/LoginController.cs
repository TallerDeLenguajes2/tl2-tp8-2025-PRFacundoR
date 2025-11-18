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
        if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
        {
            return View("Index", model);
        }
        if (auntetificacion.Login(model.Username, model.Password))
        {
            return RedirectToAction("Index", "Home");
        }


        ModelState.AddModelError( "","Credenciales inválidas.");
        return View("Index", model);
    }
    // [HttpGet] Cierra sesión
    public IActionResult Logout()
    {
        auntetificacion.Logout();
        return RedirectToAction("Index");
    }
}