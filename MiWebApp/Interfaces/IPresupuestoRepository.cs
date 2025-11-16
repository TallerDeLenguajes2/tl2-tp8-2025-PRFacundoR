namespace MiWebApp.Interfaces;

using MiWebApp.Models;

public interface IPresupuestoRepository
{
    public List<Presupuestos> GetAllPresupuestos();
    public int CrearPresupuesto(Presupuestos presupuesto);
    public void borrarPresupuesto(int id);
    public Presupuestos obtenerPresupuestoPorId(int id);
    public void AgregarProductoAPresupuesto(int idPresupuesto, int idProducto, int cantidad);
    public void ActualizarPresupuesto(int idPresu, Presupuestos presu);
    public void actualizarCantidad(int idPresupuesto, int idProducto, int cantidad);

} 


