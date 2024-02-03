namespace OVRDP;

public class OverlayFatalException : Exception
{
    public OverlayFatalException()
    {
            
    }

    public OverlayFatalException( string message )
        : base( message )
    {
        
    }

    public OverlayFatalException( string message, Exception inner )
        : base(message, inner)
    {

    }
}