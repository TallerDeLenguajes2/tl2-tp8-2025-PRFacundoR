using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MiWebApp.ViewModels;

public class AgregarProductoViewModel
{
    public int IdPresupuesto{get; set;}
    [Display(Name = "Producto a agregar")]
    public int IdProducto{get; set;}

    [Display(Name = "Cantidad")]
    [Range(1,10000000000,ErrorMessage ="El valor debe mayor a 0")]
    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    public int  Cantidad{get; set;}

    public SelectList ListaProductos { get; set; }
    public AgregarProductoViewModel(){}
    public AgregarProductoViewModel(int idProd, int idPresu)
    {
        
        IdProducto=idProd;
        IdPresupuesto=idPresu;
    }

}