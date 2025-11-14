using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using MiWebApp.Models;

namespace MiWebApp.ViewModels;

public class PresupuestoViewModel
{

    public int IdPresupuesto {get; set;}

    [Required(ErrorMessage ="Ingrese un nombre")]
    public string NombreDestinatario{get; set;}


    [DataType(DataType.Date)]
    [Required(ErrorMessage ="Ingrese una fecha")]
    public DateTime FechaCreacion{get; set;}

    public List<PresupuestoDetalle> Detalle{get; set;}

    public PresupuestoViewModel(int Id)
    {
        IdPresupuesto=Id;
    }


    public PresupuestoViewModel(Presupuestos pr)
    {
        
        IdPresupuesto=pr.IdPresupuesto;
        NombreDestinatario=pr.NombreDestinatario;
        FechaCreacion=pr.FechaCreacion;
        Detalle=pr.Detalle;
    }

    

    public PresupuestoViewModel(){}


    public float montoPresupuesto()
    {
        float monto = Detalle.Sum(d => d.Producto.Precio);
        return monto;
    }

    public double montoPresupuestoConIva()
    {
    
        return montoPresupuesto() * 1.21;
    }
    public int CantidadProductos()
    {
        int suma = 0;

        foreach (var d in Detalle)
        {
            suma += d.Cantidad;
        }
        return suma;
    }
}