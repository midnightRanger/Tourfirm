namespace Tourfirm.Domain.Safety;
//список статус-кодов
public enum StatusCode
{
    UserNotFound = 0,
    UserAlreadyExists = 1,
    TourNotFound = 3,
    RouteNotFound = 4,
    CartNotFound = 5, 
        
    ProductNotFound = 10,

    NoImages = 20,
    NoFormat = 30,

    OK = 200,
    InternalServerError = 500,
    KeyNotFound = 20
}