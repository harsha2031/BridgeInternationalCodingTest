using System;
using System.Collections.Generic;
using System.Text;
using TabletBatteryUsage.Core;

namespace TabletBatteryUsage.Data
{
    //Interface for loose coupling, easier maintainability 
    public interface ITabletsData
    {
        IEnumerable<TabletBatteryData> GetAllTabletsDetails();
        IEnumerable<TabletBatteryData> GetTabletDetailsByDeviceId(string serialNumber);
        IEnumerable<TabletBatteryData> GetTabletDetailsByAcademyId(int academyId);
        IEnumerable<TabletBatteryData> GetTabletDetailsByEmployeeId(string employeeId);
        IEnumerable<TabletBatteryData> CalculateTabletBatteryUsage(List<TabletDetails> tabletDetails);
    }
}
