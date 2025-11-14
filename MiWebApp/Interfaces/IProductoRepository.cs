namespace MiWebApp.interfaces;

using MiWebApp.Models;

public interface IProductoRepository
{
    public List<Productos> GetAll();
    
}