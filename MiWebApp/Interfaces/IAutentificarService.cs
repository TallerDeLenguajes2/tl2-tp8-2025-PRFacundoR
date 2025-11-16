namespace MiWebApp.Interfaces;


public interface IAutentificarService
{
    bool Login(string username, string password);
    void Logout();
    bool IsAuthenticated();
    bool HasAccesLevel(string requiredAccessLevel);
}
