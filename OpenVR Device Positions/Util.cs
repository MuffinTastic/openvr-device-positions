using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OVRDP;

public static class Util
{
    public static string AssetDirectory => Path.Join( GetExecutableDirectory(), "Assets" );
    public static string OutputDirectory => Path.Join( GetExecutableDirectory(), "Output" );

    public static void EnsureOutputDirectoryExists()
    {
        var outputDirectory = OutputDirectory;

        if ( !Directory.Exists( outputDirectory ) )
            Directory.CreateDirectory( outputDirectory );
    }

    public static void OpenOutputDirectory()
    {
        Log.Text( "Opening output folder" );
        Process.Start( "explorer.exe", OutputDirectory );
    }

    private static string GetExecutableDirectory()
    {
        var exePath = Assembly.GetExecutingAssembly().Location;
        return Path.GetDirectoryName( exePath )!;
    }

    public static void CopyUShorts( IntPtr source, ushort[] destination, int startIndex, int length )
    {
        unsafe
        {
            var sourcePtr = (ushort*) source;
            for ( int i = startIndex; i < startIndex + length; ++i )
            {
                destination[i] = *sourcePtr++;
            }
        }
    }
}
