using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamVR_Device_Positions;

internal struct SaveSettings
{
    public bool UseModels;
    public bool CenterOnHMD;

    public bool SaveBaseStations;
    public bool SaveHMD;
    public bool SaveControllers;
    public bool SaveTrackers;
}
