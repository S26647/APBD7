namespace APBD7.Ex;

public class NoSuchOrderException : Exception
{
    public NoSuchOrderException ()
    {}

    public NoSuchOrderException (string message) 
        : base(message)
    {}

    public NoSuchOrderException (string message, Exception innerException)
        : base (message, innerException)
    {}  
}