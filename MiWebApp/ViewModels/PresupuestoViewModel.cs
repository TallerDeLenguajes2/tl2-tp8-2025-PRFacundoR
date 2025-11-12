using System.ComponentModel.DataAnnotations;
using MiWebApp.Models;

namespace MiWebApp.ViewModels;

public class PresupuestoViewModel
{

    public int IdPresupuesto {get; set;}

    [Required(ErrorMessage ="Ingrese un nombre")]
    public string NombreDestinatario{get; set;}
    
    [Required(ErrorMessage ="Ingrese una fecha")]

    public DateTime FechaCreacion{get; set;}


    public PresupuestoViewModel(int Id)
    {

        IdPresupuesto=Id;

    }

    public PresupuestoViewModel(){}
}