using APIServer.Controllers;

namespace APIServer.StateType;

public enum EventType
{
    CreateAccount = 2,
    Login = 5,
    LoginAddRedis = 6,
    LogOut = 10,

    UpdateScore = 15,
}