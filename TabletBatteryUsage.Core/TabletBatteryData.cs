using System;
using System.Collections.Generic;
using System.Text;

namespace TabletBatteryUsage.Core
{
    public class TabletBatteryData
    {
        public string SerialNumber { get; set; }
        public float BatteryPercentage { get; set; }
        public bool BatteryReplacementNeeded { get; set; }
    }
}
