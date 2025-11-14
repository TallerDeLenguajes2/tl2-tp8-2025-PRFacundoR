namespace MiWebApp.interfaces;

public interface IUserRepository
{
    //Retorna el objeto Usuario di las credenciales son validas, sino null
    Usuario GetUser(string username, string password);
} 