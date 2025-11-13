namespace MiWebApp.Models;
using MiWebApp.ViewModels;

public class Productos
{
    private int idProducto;
    private string descripcion;
    private float precio;

    public int IdProducto { get => idProducto; set => idProducto = value; }
    public string Descripcion { get => descripcion; set => descripcion = value; }
    public float Precio { get => precio; set => precio = value; }

    public Productos()
    {
        
    }
    public Productos(ProductoViewModel pr)
    {   
        idProducto=pr.IdProducto;
        this.Descripcion = pr.Descripcion;
        this.Precio = pr.Precio;
    }
}