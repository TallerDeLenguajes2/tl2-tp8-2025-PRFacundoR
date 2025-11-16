namespace MiWebApp.Repositorios;
using Microsoft.Data.Sqlite;
using MiWebApp.Interfaces;
using MiWebApp.Models;

public class AuntentificarService: IAutentificarService
{
    public bool Login(string username, string password){return true;}
    public  void Logout(){}
    public  bool IsAuthenticated(){return true;}
    public bool HasAccesLevel(string requiredAccessLevel)
    {
        return true;
    }
}