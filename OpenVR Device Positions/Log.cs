﻿using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace OVRDP;

/// <summary>
/// Program-wide logger
/// </summary>
public static class Log
{
    private static List<(bool, Action<string>)> LogSinks = new();

    /// <summary>
    /// Register a sink with the logger
    /// </summary>
    /// <param name="verbose">Print file name, line and caller function</param>
    /// <param name="action">Log sink callback</param>
    public static void RegisterSink( bool verbose, Action<string> action )
    {
        if ( action is null )
        {
            Debug.Write( "Tried to register a null log sink action" );
            return;
        }

        LogSinks.Add( (verbose, action) ); 
    }

    public static void Text( string text,
        [CallerFilePath] string? filePath = null,
        [CallerLineNumber] int lineNumber = 0,
        [CallerMemberName] string? caller = null)
    {
        lock ( LogSinks )
        {
            string shortTimestamp = DateTime.Now.ToString( "hh:mm:ss" );
            string longTimestamp = DateTime.Now.ToString( "yyyy/MM/dd hh:mm:ss" );

            string shortMessage = $"[{shortTimestamp}] {text}";
            string longMessage = $"[{Path.GetFileName( filePath )}:{lineNumber}] [{caller}] [{longTimestamp}] {text}";

            foreach ( (bool verbose, Action<string> action) in LogSinks )
            {
                if ( verbose )
                {
                    action( longMessage );
                }
                else
                {
                    action( shortMessage );
                }
            }
        }
    }
}
