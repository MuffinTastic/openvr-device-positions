using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OVRDP;

/// <summary>
/// Snapshot of the user's settings for saving FBXs
/// </summary>
public struct SaveSettings
{
    required public bool UseDeviceModels;
    required public bool CenterOnHMD;
    required public bool SaveInvalidPoses;
    required public bool SaveBaseStations;
    required public bool SaveHMD;
    required public bool SaveControllers;
    required public bool SaveTrackers;
}
