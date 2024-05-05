namespace APBD7.Ex;

public class NoSuchProductException : Exception
{
    public NoSuchProductException ()
    {}

    public NoSuchProductException (string message) 
        : base(message)
    {}

    public NoSuchProductException (string message, Exception innerException)
        : base (message, innerException)
    {}  
}