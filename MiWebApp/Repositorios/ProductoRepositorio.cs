//aca va la rutina para interactuar con la db
// select last_insert_aca va algo, devuelve el ultimo id de la consulta hecha?
using Microsoft.Data.Sqlite;

public class ProductoRepositorio
{
    private string _coneccionADB = "Data Source=DB/nueva.db";

    public List<Productos> GetAll()
    {
        string sql_query = "SELECT * FROM productos";
        List<Productos> productos = [];

        using var conecttion = new SqliteConnection(_coneccionADB);
        conecttion.Open();

        var comando = new SqliteCommand(sql_query, conecttion);

        using (SqliteDataReader reader = comando.ExecuteReader())
        {
            while (reader.Read())
            {
                var producto = new Productos();
                producto.IdProducto = Convert.ToInt32(reader["id_prod"]);
                producto.Descripcion = reader["descripcion"].ToString();
                producto.Precio = Convert.ToInt32(reader["precio"]);

                productos.Add(producto);
            }

        } 

        conecttion.Close();
        return productos;
    }

    public int InsertarProducto(Productos Produc)
    {
        int nuevoID = 0;
        using (SqliteConnection coneccion = new SqliteConnection(_coneccionADB))
        {
            coneccion.Open();
            string sql = "INSERT INTO productos (descripcion, precio) VALUES (@desc, @precio); SELECT last_insert_rowid();";

            using (var comando = new SqliteCommand(sql, coneccion)) {

                comando.Parameters.Add(new SqliteParameter("@desc", Produc.Descripcion));
                comando.Parameters.Add(new SqliteParameter("@precio", Produc.Precio));
                nuevoID = Convert.ToInt32(comando.ExecuteScalar());
            }
            coneccion.Close();
        }

        return nuevoID;
    }


public int ActualizarProducto(int idProduc, Productos produc)
{
    int filasAfectadas = 0;

    using (SqliteConnection conexion = new SqliteConnection(_coneccionADB))
    {
        conexion.Open();

        string sql = "UPDATE productos SET precio = @precio, descripcion=@desc WHERE id_prod = @idProduc;";

        using (var comando = new SqliteCommand(sql, conexion))
        {
            comando.Parameters.AddWithValue("@desc", produc.Descripcion);
            comando.Parameters.AddWithValue("@precio", produc.Precio);
            comando.Parameters.AddWithValue("@idProduc", idProduc);

            filasAfectadas = comando.ExecuteNonQuery();
        }
    }

    return filasAfectadas;
}

    public void borrarProducto(int id)
    {
        using (var conexion = new SqliteConnection(_coneccionADB))
        {
            conexion.Open();

            string sql = "DELETE FROM productos WHERE id_prod = @id";
            using (var comando = new SqliteCommand(sql, conexion))
            {
                comando.Parameters.Add(new SqliteParameter("@id", id));
                comando.ExecuteNonQuery();
            }
        }
    }

    public Productos ObtenerProductoPorNombre(string nom)
    {
        using (var conexion = new SqliteConnection(_coneccionADB))
        {
            conexion.Open();
            string sql = "SELECT * FROM productos WHERE descripcion LIKE '%' || @nom || '%'";
            using (var comando = new SqliteCommand(sql, conexion))
            {
                // Parámetro para evitar inyección SQL
                comando.Parameters.AddWithValue("@nom", nom);
                using (var lector = comando.ExecuteReader())
                {
                    if (lector.Read()) // si encontró el producto
                    {
                        var producto = new Productos
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
    }
}
