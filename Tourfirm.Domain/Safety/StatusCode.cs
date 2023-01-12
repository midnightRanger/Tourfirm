namespace Tourfirm.Domain.Safety;

public enum StatusCode
{
    UserNotFound = 0,
    UserAlreadyExists = 1,
        
    ProductNotFound = 10,

    NoImages = 20,
    NoFormat = 30,

    OK = 200,
    InternalServerError = 500,
    KeyNotFound = 20
}