using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using TabletBatteryUsage.Core;

namespace TabletBatteryUsage.Data
{
    public class TabletDataFromJsonFile : ITabletsData
    {
        readonly List<TabletDetails> tabletDetails;
        public TabletDataFromJsonFile()
        {
            this.tabletDetails = ParseJsonDataForTabletsBatteryDetails();
        }

        private List<TabletDetails> ParseJsonDataForTabletsBatteryDetails()
        {
            List<TabletDetails> data = new List<TabletDetails>();

            try
            {
                string jsonFilePath = "C:\\Users\\harsh\\Desktop\\BridgeInternational\\TabletBatteryUsage.API\\TabletBatteryUsage.Data\\Resources\\battery.json";
                using (StreamReader r = new StreamReader(jsonFilePath))
                {
                    string json = r.ReadToEnd();
                    data = JsonConvert.DeserializeObject<List<TabletDetails>>(json);
                }
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException(@"[battery.json not found in current directory]", e);
            }
            catch (DirectoryNotFoundException e)
            {
                throw new DirectoryNotFoundException(@"[The directory not found]", e);
            }
            catch (IOException e)
            {
                throw new IOException(@"[The file cannot be opened]", e);
            }
            CalculateTabletBatteryUsage(data);
            return data;
        }

        public IEnumerable<TabletDetails> GetAllTabletsDetails()
        {
            return tabletDetails;
        }

        public IEnumerable<TabletDetails> GetTabletDetailsByAcademyId(int academyId)
        {
            return tabletDetails.Where(detail => detail.AcadamyId == academyId);
        }

        public IEnumerable<TabletDetails> GetTabletDetailsByDeviceId(string serialNumber)
        {
            return tabletDetails.Where(detail => detail.SerialNumber == serialNumber);
        }

        public IEnumerable<TabletDetails> GetTabletDetailsByEmployeeId(string employeeId)
        {
            return tabletDetails.Where(detail => detail.EmployeeId == employeeId);
        }

        public IEnumerable<TabletBatteryData> CalculateTabletBatteryUsage(List<TabletDetails> devicesList)
        {
            var temp = devicesList.GroupBy(t => t.SerialNumber)
                        .ToDictionary(group => group.Key,group => group.ToList());
            List<TabletBatteryData> tabletBatteryPercentageData = new List<TabletBatteryData>();
            foreach(var data in temp)
            {
                var devicedata = data.Value.OrderBy(key => key.TimeStamp).ToList();
                var dropinpercentage = 0.0;
                double duration = 0.0;
                int i = 0;
                while(i < devicedata.Count() -1)
                {
                    if(devicedata[i].BatteryLevel > devicedata[i + 1].BatteryLevel)
                    {
                        dropinpercentage += devicedata[i].BatteryLevel - devicedata[i + 1].BatteryLevel;
                        duration += (devicedata[i + 1].TimeStamp - devicedata[i].TimeStamp).TotalSeconds;
                    }
                    i++;
                }
                var dailypercentage = (86400 * (dropinpercentage * 100)) / duration;
                TabletBatteryData tabletBatteryData = new TabletBatteryData { BatteryPercentage = dailypercentage, SerialNumber = data.Key, BatteryReplacementNeeded = dailypercentage > 30 ? true : false };
                tabletBatteryPercentageData.Add(tabletBatteryData);
            }

            return tabletBatteryPercentageData;
        }

    }
}
