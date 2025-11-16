using System.ComponentModel.DataAnnotations;
using MiWebApp.Models;

namespace MiWebApp.ViewModels;

public class ProductoViewModel
{
    public int IdProducto{get; set;}

    [Required(ErrorMessage ="Descripcion requerida")]
    [StringLength(250, ErrorMessage ="La descripcion no puede superar los 250 caracteres")]
    public string Descripcion {get;set;}

    [Required]
    [Range(0.1,1000000000.0,ErrorMessage ="El valor debe mayor a 0")]
    public float Precio{get;set;}

    public ProductoViewModel(int Id)
    {

        IdProducto=Id;

    }

    public ProductoViewModel(Productos pr)
    {
        Descripcion=pr.Descripcion;
        IdProducto=pr.IdProducto;
        Precio=pr.Precio;
    }


    public ProductoViewModel(){}
    
}