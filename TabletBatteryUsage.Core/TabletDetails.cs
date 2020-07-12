using System;
using System.Collections.Generic;
using System.Text;

namespace TabletBatteryUsage.Core
{

    //Class to parse Json file
    public class TabletDetails
    {
        public int AcadamyId { get; set; }

        public float BatteryLevel { get; set; }

        public string EmployeeId { get; set; }

        public string SerialNumber { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
