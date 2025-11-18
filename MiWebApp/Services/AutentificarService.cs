namespace MiWebApp.Services;

using MiWebApp.Interfaces;
public class AutentificarService : IAutentificarService
{
    private readonly IUserRepository usuario;
    private readonly IHttpContextAccessor acceso;



    public AutentificarService(IUserRepository usuario, IHttpContextAccessor acceso)
    {
        this.usuario = usuario;
        this.acceso = acceso;

    }
    public bool Login(string username, string password)
    {
        var context = acceso.HttpContext;
        var user = usuario.GetUser(username, password);

        if (user != null)
        {

            if (context == null)
            {
                throw new InvalidOperationException("HttpContext no está disponible.");
            }
            context.Session.SetString("IsAuthenticated", "true");
            context.Session.SetString("User", user.User);
            context.Session.SetString("UserNombre", user.Nombre);
            context.Session.SetString("Rol", user.Rol);
            //es el tipo de acceso/rol admin o cliente
            return true;
        }
        return false;
    }
    public void Logout()
    {
        var context = acceso.HttpContext;
        if (context == null)
        {
            throw new InvalidOperationException("HttpContext no está disponible.");

        }
        context.Session.Clear();

    }
    public bool IsAuthenticated()
    {
        var context = acceso.HttpContext;
        if (context == null)
        {
            throw new InvalidOperationException("HttpContext noestá disponible.");
        }


        return context.Session.GetString("IsAuthenticated") == "true";

    }
    public bool HasAccesLevel(string requiredAccessLevel)
    {
        var context = acceso.HttpContext;
        if (context == null)
        {
            throw new InvalidOperationException("HttpContext noestá disponible.");
        }
        return context.Session.GetString("Rol") == requiredAccessLevel;


    }


}