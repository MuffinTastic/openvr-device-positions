using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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