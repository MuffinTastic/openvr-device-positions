using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OVRDP;

public struct SaveSettings
{
    required public bool UseDeviceModels;
    required public bool CenterOnHMD;
    required public bool SaveBaseStations;
    required public bool SaveHMD;
    required public bool SaveControllers;
    required public bool SaveTrackers;
}
