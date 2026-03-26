namespace Notiway.Common.Core.Enums;
public enum Processing
{
    Success = 10,

    NotFound = 40,

    NotSupported = 41,

    WrongInput = 45,

    AlreadyExists = 30,
    
    Conflict = 31,

    RetryAttemptsReached = 32,

    Unauthorized = 33,

    Forbidden = 34,

    Error = 50,

    Timeout = 70,

    Throttled = 71,
}
