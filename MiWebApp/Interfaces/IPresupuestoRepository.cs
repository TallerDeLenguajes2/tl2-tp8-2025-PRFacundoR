namespace MiWebApp.Interfaces;

using MiWebApp.Models;

public interface IPresupuestoRepository
{
    List<Presupuestos> GetAllPresupuestos();
    int CrearPresupuesto(Presupuestos presupuesto);
    void borrarPresupuesto(int id);
    Presupuestos obtenerPresupuestoPorId(int id);
    void AgregarProductoAPresupuesto(int idPresupuesto, int idProducto, int cantidad);
    void ActualizarPresupuesto(int idPresu, Presupuestos presu);
    void actualizarCantidad(int idPresupuesto, int idProducto, int cantidad);

} 


