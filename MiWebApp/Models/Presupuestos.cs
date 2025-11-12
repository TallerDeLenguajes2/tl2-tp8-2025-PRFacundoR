namespace MiWebApp.Models;
using MiWebApp.ViewModels;

public class Presupuestos
{
    private int idPresupuesto;
    private string nombreDestinatario;
    private DateTime fechaCreacion;
    private List<PresupuestoDetalle> detalle;

    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
    public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
    public List<PresupuestoDetalle> Detalle { get => detalle; set => detalle = value; }



    public Presupuestos(PresupuestoViewModel presupuesto)
    {
        IdPresupuesto=presupuesto.IdPresupuesto;
        NombreDestinatario=presupuesto.NombreDestinatario;
        FechaCreacion=presupuesto.FechaCreacion;

    }

    public Presupuestos(){}

    //METODOS
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

        foreach (var d in detalle)
        {
            suma += d.Cantidad;
        }
        return suma;
    }


}