using System.Numerics;
using Valve.VR;

namespace OVRDP;

// Adapted from openvr_api.cs 
public static class HmdMatrix34_tExtensions
{
    public static Vector3 GetPosition( this HmdMatrix34_t m )
    {
        return new Vector3( m.m3, m.m7, m.m11 );
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

            CopySign( ref x, m.m9 - m.m6 );
            CopySign( ref y, m.m2 - m.m8 );
            CopySign( ref z, m.m4 - m.m1 );

            return new Quaternion( x, y, z, w );
        }

        return Quaternion.Identity;
    }

    public static Matrix4x4 GetMatrix4x4( this HmdMatrix34_t m )
    {
        var r = new Matrix4x4(
            m.m0,  m.m1,  m.m2,  m.m3,
            m.m4,  m.m5,  m.m6,  m.m7,
            m.m8,  m.m9,  m.m10, m.m11,
            0.0f,  0.0f,  0.0f,  1.0f
        );

        r = Matrix4x4.Transpose( r );

        return r;
    }

    private static void CopySign( ref float sizeval, float signval )
    {
        if ( signval > 0 != sizeval > 0 )
            sizeval = -sizeval;
    }
}

public static class AsposeExtensions
{
    public static AsposeVector3 ToAspose( this Vector3 v )
    {
        return new AsposeVector3( v.X, v.Y, v.Z );
    }

    public static AsposeVector4 ToAspose( this Vector4 v )
    {
        return new AsposeVector4( v.X, v.Y, v.Z, v.W );
    }

    public static AsposeQuaternion ToAspose( this Quaternion q )
    {
        return new AsposeQuaternion( q.W, q.X, q.Y, q.Z );
    }

    public static AsposeMatrix4 ToAspose( this Matrix4x4 m )
    {
        //m = Matrix4x4.Transpose( m );
        return new AsposeMatrix4(
            m.M11, m.M12, m.M13, m.M14,
            m.M21, m.M22, m.M23, m.M24,
            m.M31, m.M32, m.M33, m.M34,
            m.M41, m.M42, m.M43, m.M44
        );
    }
}