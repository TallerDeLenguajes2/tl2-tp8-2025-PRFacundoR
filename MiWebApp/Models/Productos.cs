namespace MiWebApp.Models;

public class Productos
{
    private int idProducto;
    private string descripcion;
    private int precio;

    public int IdProducto { get => idProducto; set => idProducto = value; }
    public string Descripcion { get => descripcion; set => descripcion = value; }
    public int Precio { get => precio; set => precio = value; }

    public Productos()
    {
        
    }
    public Productos(string desc, int precio)
    {
        this.Descripcion = desc;
        this.precio = precio;
    }
}