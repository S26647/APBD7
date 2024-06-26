namespace APBD7.Ex;

public class OrderAlreadyFulfilledException : Exception
{
    public OrderAlreadyFulfilledException ()
    {}

    public OrderAlreadyFulfilledException (string message) 
        : base(message)
    {}

    public OrderAlreadyFulfilledException (string message, Exception innerException)
        : base (message, innerException)
    {}  
}