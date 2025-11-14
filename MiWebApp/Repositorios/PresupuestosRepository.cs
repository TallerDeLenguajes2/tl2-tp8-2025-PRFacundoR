using Microsoft.Data.Sqlite;
using MiWebApp.interfaces;
using MiWebApp.Models;
namespace MiWebApp.Repositorios;
public class PresupuestosRepository:IPresupuestoRepository
{
    private string _coneccionADB = "Data Source=DB/nueva.db";
    //Listar todos los Presupuestos registrados. (devuelve un List de Presupuestos)
    public List<Presupuestos> GetAllPresupuestos()
    {
        string sql_query = "SELECT * FROM presupuestos LEFT JOIN presupuestoDetalle using(idPresupuesto) LEFT JOIN productos ON id_prod=idProducto";
        List<Presupuestos> presupuestos = new List<Presupuestos>();

        using var conecttion = new SqliteConnection(_coneccionADB);
        conecttion.Open();

        var comando = new SqliteCommand(sql_query, conecttion);

        using (SqliteDataReader reader = comando.ExecuteReader())
        {


            while (reader.Read())
            {
                int idPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                var presupuesto = presupuestos.FirstOrDefault(p => p.IdPresupuesto == idPresupuesto);

                if (presupuesto == null)
                {
                    presupuesto = new Presupuestos
                    {
                        IdPresupuesto = idPresupuesto,
                        NombreDestinatario = reader["nombreDestinatario"].ToString(),
                        FechaCreacion = Convert.ToDateTime(reader["fechaCreacion"]),
                        Detalle = new List<PresupuestoDetalle>()
                    };
                    presupuestos.Add(presupuesto);
                }

                if (reader["id_prod"] != DBNull.Value)
            {
                var producto = new Productos
                {
                    IdProducto = Convert.ToInt32(reader["id_prod"]),
                    Descripcion = reader["descripcion"]?.ToString(),
                    Precio = reader["precio"] != DBNull.Value ? Convert.ToInt32(reader["precio"]) : 0
                };

                var detalle = new PresupuestoDetalle
                {
                    Producto = producto,
                    Cantidad = reader["cantidad"] != DBNull.Value ? Convert.ToInt32(reader["cantidad"]) : 0
                };

                presupuesto.Detalle.Add(detalle);
            }
            }


            //conecttion.Close(); no haria falta ya que usamos using
            return presupuestos;
        }
    }


    //Crear un nuevo Presupuesto. (recibe un objeto Presupuesto)
    public int CrearPresupuesto(Presupuestos presupuesto)
    {
        int nuevoID = 0;

        using (var conexion = new SqliteConnection(_coneccionADB))
        {
            conexion.Open();
            string sql = @"
            INSERT INTO presupuestos (nombreDestinatario, fechaCreacion)
            VALUES (@dest, @fecha);
            SELECT last_insert_rowid();
        ";

            using (var comando = new SqliteCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@dest", presupuesto.NombreDestinatario);
                comando.Parameters.AddWithValue("@fecha", presupuesto.FechaCreacion);

                // Devuelve el ID autogenerado del presupuesto
                nuevoID = Convert.ToInt32(comando.ExecuteScalar());
            }
        }

        return nuevoID;
    }





    //Eliminar un Presupuesto por ID

    public void borrarPresupuesto(int id)
    {
        using (var conexion = new SqliteConnection(_coneccionADB))
        {
            conexion.Open();

            string sql = "DELETE FROM presupuestos WHERE idPresupuesto = @id";
            using (var comando = new SqliteCommand(sql, conexion))
            {
                comando.Parameters.Add(new SqliteParameter("@id", id));
                comando.ExecuteNonQuery();
            }
        }
    }

    //Obtener detalles de un Presupuesto por su ID. (recibe un Id y devuelve un
    //Presupuesto con sus productos y cantidades)

    public Presupuestos obtenerPresupuestoPorId(int id)
    {

        string sql_query = "SELECT * FROM presupuestos INNER JOIN presupuestoDetalle using(idPresupuesto) JOIN productos ON id_prod=idProducto WHERE idPresupuesto=@id";


        using var conecttion = new SqliteConnection(_coneccionADB);
        conecttion.Open();

        var comando = new SqliteCommand(sql_query, conecttion);
        comando.Parameters.AddWithValue("@id", id);
        using (SqliteDataReader reader = comando.ExecuteReader())
        {
            Presupuestos presupuesto = null;
            while (reader.Read())
            {

                if (presupuesto == null)
                {
                    presupuesto = new Presupuestos
                    {
                        IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]),
                        NombreDestinatario = reader["nombreDestinatario"].ToString(),
                        FechaCreacion = Convert.ToDateTime(reader["fechaCreacion"]),
                        Detalle = new List<PresupuestoDetalle>()
                    };

                }

                var producto = new Productos
                {
                    IdProducto = Convert.ToInt32(reader["id_prod"]),
                    Descripcion = reader["descripcion"].ToString(),
                    Precio = Convert.ToInt32(reader["precio"])
                };

                var detalle = new PresupuestoDetalle
                {
                    Producto = producto,
                    Cantidad = Convert.ToInt32(reader["cantidad"])
                };

                presupuesto.Detalle.Add(detalle);

            }

            return presupuesto;
        }

    }


    // Agregar un producto y una cantidad a un presupuesto (recibe un Id)
    public void AgregarProductoAPresupuesto(int idPresupuesto, int idProducto, int cantidad)
    {
        using (var conexion = new SqliteConnection(_coneccionADB))
        {
            conexion.Open();
            string sql = @"
            INSERT INTO presupuestoDetalle (idPresupuesto, idProducto, cantidad)
            VALUES (@idPre, @idProd, @cant);
        ";

            using (var comando = new SqliteCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@idPre", idPresupuesto);
                comando.Parameters.AddWithValue("@idProd", idProducto);
                comando.Parameters.AddWithValue("@cant", cantidad);

                comando.ExecuteNonQuery();
            }
        }
    }


    public void ActualizarPresupuesto(int idPresu, Presupuestos presu)
    {


        using (var conexion = new SqliteConnection(_coneccionADB))
        {
            conexion.Open();
            string sql = "UPDATE presupuestos SET nombreDestinatario=@nomb, fechaCreacion=@fecha WHERE idPresupuesto=@idP";

            using(var comando=new SqliteCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@idP", idPresu);
                comando.Parameters.AddWithValue("@nomb", presu.NombreDestinatario);
                comando.Parameters.AddWithValue("@fecha", presu.FechaCreacion);
                
                comando.ExecuteNonQuery();
            }
        }
    }




























    /* public Presupuestos ObtenerProductoPorId(int id)
     {
         using (var conexion = new SqliteConnection(_coneccionADB))
         {
             conexion.Open();
             string sql = "SELECT * FROM producto WHERE id_prod=@id";
             using (var comando = new SqliteCommand(sql, conexion))
             {
                 // Parámetro para evitar inyección SQL
                 comando.Parameters.AddWithValue("@id", id);
                 using (var lector = comando.ExecuteReader())
                 {
                     if (lector.Read()) // si encontró el producto
                     {
                         var producto = new Presupuestos
                         {
                             IdProducto = lector.GetInt32(0),              // o lector["id_prod"]
                             Descripcion = lector.GetString(1),         // lector["nombre"]
                             Precio = lector.GetInt32(2),        // lector["precio"]

                         };

                         return producto;
                     }
                     else
                     {
                         return null; // no se encontró
                     }
                 }

             }

         }
     }*/



    // public int ActualizarProducto(int idProduc, Presupuestos presupuesto)
    // {
    //     int filasAfectadas = 0;

    //     using (SqliteConnection conexion = new SqliteConnection(_coneccionADB))
    //     {
    //         conexion.Open();

    //         string sql = "UPDATE presupuesto SET precio = @precio WHERE id_prod = @idProduc;";

    //         using (var comando = new SqliteCommand(sql, conexion))
    //         {
    //             comando.Parameters.AddWithValue("@precio", produc.Precio);
    //             comando.Parameters.AddWithValue("@idProduc", idProduc);

    //             filasAfectadas = comando.ExecuteNonQuery();
    //         }
    //     }

    //     return filasAfectadas;
    // }



    /*public List<Presupuestos> GetAllPresupuestos()
        {
            string sql_query = "SELECT * FROM presupuestos INNER JOIN presupuestoDetalle using(idPresupuesto) JOIN productos ON id_prod=idProducto";
            List<Presupuestos> presupuestos = new List<Presupuestos>();

            using var conecttion = new SqliteConnection(_coneccionADB);
            conecttion.Open();

            var comando = new SqliteCommand(sql_query, conecttion);

            using (SqliteDataReader reader = comando.ExecuteReader())
            {
                while (reader.Read())
                {
                    int idPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);

                    var presupuesto = presupuestos.FirstOrDefault(p => p.IdPresupuesto == idPresupuesto);
                    if (presupuesto == null)// Verifico si ya existe,Evitar duplicar presupuestos, luego ver sin if
                    {
                        var presupuest = new Presupuestos();
                        presupuest.IdPresupuesto = idPresupuesto;
                        presupuest.NombreDestinatario = reader["nombreDestinatario"].ToString();
                        presupuest.FechaCreacion = Convert.ToDateTime(reader["fechaCreacion"]);

                        var produc = new Productos();
                        produc.IdProducto = Convert.ToInt32(reader["id_prod"]);
                        produc.Descripcion = reader["descripcion"].ToString();
                        produc.Precio = Convert.ToInt32(reader["precio"]);

                        var presuDetalle = new PresupuestoDetalle();
                        presuDetalle.Producto = produc;
                        presuDetalle.Cantidad = Convert.ToInt32(reader["cantidad"]);

                        presupuest.Detalle = new List<PresupuestoDetalle>();
                        presupuest.Detalle.Add(presuDetalle);

                        presupuestos.Add(presupuest);

                    }

                }



                //conecttion.Close(); no haria falta ya que usamos using
                return presupuestos;
            }
        }*/

    /*public int InsertarPresupuesto(Presupuestos presupuesto)
    {
        int nuevoID = 0;
        using (SqliteConnection coneccion = new SqliteConnection(_coneccionADB))
        {
            coneccion.Open();
            string sql = "INSERT INTO presupuestos (nombreDestinario, fechaCreacion) VALUES (@dest, @fecha); SELECT last_insert_rowid();";
            string sql1 = " INSERT INTO presupuestoDetalle (idProducto, cantidad) VALUES (@idP, @cant);";

            using (var comando = new SqliteCommand(sql, coneccion))
            {

                comando.Parameters.Add(new SqliteParameter("@dest", presupuesto.NombreDestinatario));
                comando.Parameters.Add(new SqliteParameter("@fecha", presupuesto.FechaCreacion));
                nuevoID = Convert.ToInt32(comando.ExecuteScalar());


            }

            for (int i = 0; i < presupuesto.Detalle.Count(); i++) //o usar foreach
            {

                using (var comando = new SqliteCommand(sql1, coneccion))
                {


                    comando.Parameters.Add(new SqliteParameter("@idP", presupuesto.Detalle[i].Producto.IdProducto));
                    comando.Parameters.Add(new SqliteParameter("@cant", presupuesto.Detalle[i].Cantidad));
                    comando.ExecuteNonQuery();
                }



            }




        }

        return nuevoID;
    }*/

}






