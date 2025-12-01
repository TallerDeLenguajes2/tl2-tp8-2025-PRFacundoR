using System.ComponentModel.DataAnnotations;

namespace MiWebApp.ViewModels;

public class LoginViewModel
{
    
    [Required(ErrorMessage = "El usuario es obligatorio.")]
    public string Username{get; set;}
    
[Required(ErrorMessage = "La contraseña es obligatoria.")]    public string  Password{get;set;}

 public string ErrorMessage{get;set;}
    
}