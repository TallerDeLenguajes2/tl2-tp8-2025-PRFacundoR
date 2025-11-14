using Microsoft.Data.Sqlite;
using MiWebApp.interfaces;
using MiWebApp.Models;


namespace MiWebApp.Repositorios;

class UsuarioRepository : IUserRepository
{

    private string _coneccionADB = "Data Source=DB/nueva.db";
    public Usuario GetUser(string usuario, string contrasena)
    {
        Usuario user = null;
        //Consulta SQL que busca por Usuario Y Contrasena
        const string sql = @"
 SELECT Id, Nombre, User, Pass, Rol
 FROM Usuarios
 WHERE User = @Usuario AND Pass = @Contrasena";
        using var conexion = new SqliteConnection(_coneccionADB);
        conexion.Open();
        using var comando = new SqliteCommand(sql, conexion);

        // Se usan parámetros para prevenir inyección SQ

        comando.Parameters.AddWithValue("@Usuario", usuario);
        comando.Parameters.AddWithValue("@Contrasena",contrasena);

        using var reader = comando.ExecuteReader();
        
        if (reader.Read())
        {
            // Si el lector encuentra una fila, el usuario existe y las credenciales son correctas
            user = new Usuario
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                User = reader.GetString(2),
                Pass = reader.GetString(3),
                Rol = reader.GetString(4)
            };
        }
        return user;
    }
}