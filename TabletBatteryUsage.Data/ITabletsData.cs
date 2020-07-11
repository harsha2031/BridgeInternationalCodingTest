using System;
using System.Collections.Generic;
using System.Text;
using TabletBatteryUsage.Core;

namespace TabletBatteryUsage.Data
{
    public interface ITabletsData
    {
        IEnumerable<TabletDetails> GetAllTabletsDetails();
        IEnumerable<TabletDetails> GetTabletDetailsByDeviceId(string serialNumber);
        IEnumerable<TabletDetails> GetTabletDetailsByAcademyId(int academyId);
        IEnumerable<TabletDetails> GetTabletDetailsByEmployeeId(string employeeId);
        IEnumerable<TabletBatteryData> CalculateTabletBatteryUsage(List<TabletDetails> tabletDetails);
    }
}
