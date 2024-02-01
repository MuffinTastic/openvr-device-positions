using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OVRDP;

public static class OverlayConstants
{
    public const string ProgramNameReadable = "OpenVR Device Positions";
    public const string ProgramNameInternal = "openvr-device-positions";
    public const string OverlayKeyName = "ovrdp-overlay";

    public const int RenderWidth = 320;
    public const int RenderHeight = 320;
    public const float VRWidth = 0.3f; // meters
}
