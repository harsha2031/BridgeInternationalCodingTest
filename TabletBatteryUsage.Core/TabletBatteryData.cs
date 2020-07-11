using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TabletBatteryUsage.Core
{
    public class TabletBatteryData
    {
        public string SerialNumber { get; set; }
        public double BatteryPercentage { get; set; }       
        public string BatteryReplacementNeeded { get; set; }

    }

    public enum BatteryReplacementRecommendation
    {
        Unknown,
        ReplaceBattery,
        BatteryIsGood
    }

    
}
