namespace MiWebApp.Interfaces;


public interface IAutentificarService
{
    bool Login(string username, string password);
    void Logout();
    bool HasAccesLevel(string requiredAccessLevel);
    bool IsAuthenticated();
}
