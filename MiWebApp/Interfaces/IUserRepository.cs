using MiWebApp.Models;

namespace MiWebApp.Interfaces;

public interface IUserRepository
{
    //Retorna el objeto Usuario di las credenciales son validas, sino null
    Usuario GetUser(string username, string password);
} 