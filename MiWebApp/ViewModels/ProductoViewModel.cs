using System.ComponentModel.DataAnnotations;

namespace MiWebApp.ViewModels;

public class ProductoViewModel
{
    [StringLength(250, ErrorMessage ="La descripcion no puede superar los 250 caracteres")]
    public string Descripcion {get;set;}

    [Required]
    [Range(0.1,1000000000.0,ErrorMessage ="El valor debe mayor a 0")]
    public float Precio{get;set;}


    
}