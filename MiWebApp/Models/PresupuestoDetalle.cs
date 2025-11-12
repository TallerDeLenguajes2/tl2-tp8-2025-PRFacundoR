namespace MiWebApp.Models;

public class PresupuestoDetalle
{
    private Productos producto;
    private int cantidad;

    public Productos Producto { get => producto; set => producto = value; }
    public int Cantidad { get => cantidad; set => cantidad = value; }
}