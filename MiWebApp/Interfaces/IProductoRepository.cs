namespace MiWebApp.Interfaces;

using MiWebApp.Models;

public interface IProductoRepository
{
    List<Productos> GetAll();
    int InsertarProducto(Productos Produc);
    int ActualizarProducto(int idProduc, Productos produc);
    void borrarProducto(int id);
    Productos ObtenerProductoPorNombre(int id);
}