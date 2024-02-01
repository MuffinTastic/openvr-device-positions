using System.Numerics;
using Valve.VR;

namespace OVRDP;

// Adapted from openvr_api.cs 
public static class HmdMatrix34_tExtensions
{
    public static Vector3 GetPosition( this HmdMatrix34_t m )
    {
        return new Vector3( m.m3, m.m7, -m.m11 );
    }

    public static bool IsRotationValid( this HmdMatrix34_t m )
    {
        return ( ( m.m2 != 0 || m.m6 != 0 || m.m10 != 0 ) && ( m.m1 != 0 || m.m5 != 0 || m.m9 != 0 ) );
    }

    public static Quaternion GetRotation( this HmdMatrix34_t m )
    {
        if ( m.IsRotationValid() )
        {
            float w = MathF.Sqrt( MathF.Max( 0, 1 + m.m0 + m.m5 + m.m10 ) ) / 2;
            float x = MathF.Sqrt( MathF.Max( 0, 1 + m.m0 - m.m5 - m.m10 ) ) / 2;
            float y = MathF.Sqrt( MathF.Max( 0, 1 - m.m0 + m.m5 - m.m10 ) ) / 2;
            float z = MathF.Sqrt( MathF.Max( 0, 1 - m.m0 - m.m5 + m.m10 ) ) / 2;

            CopySign( ref x, -m.m9 - -m.m6 );
            CopySign( ref y, -m.m2 - -m.m8 );
            CopySign( ref z, m.m4 - m.m1 );

            return new Quaternion( x, y, z, w );
        }

        return Quaternion.Identity;
    }

    private static void CopySign( ref float sizeval, float signval )
    {
        if ( signval > 0 != sizeval > 0 )
            sizeval = -sizeval;
    }
}
