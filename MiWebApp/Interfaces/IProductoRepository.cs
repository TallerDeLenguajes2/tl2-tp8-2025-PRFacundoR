namespace MiWebApp.interfaces;

using MiWebApp.Models;

public interface IProductoRepository
{
    public List<Productos> GetAll();
    public int InsertarProducto(Productos Produc);
    public int ActualizarProducto(int idProduc, Productos produc);
    public void borrarProducto(int id);
    public Productos ObtenerProductoPorNombre(int id);
}