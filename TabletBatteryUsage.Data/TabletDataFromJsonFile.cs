using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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

    //Implementation of Data layer 
    public class TabletDataFromJsonFile : ITabletsData
    {
        readonly List<TabletDetails> tabletDetails;
        readonly string filePath;
        //Constructor
        public TabletDataFromJsonFile()
        {
            this.filePath = "C:\\Users\\harsh\\Desktop\\BridgeInternational\\TabletBatteryUsage.API\\TabletBatteryUsage.Data\\Resources\\battery.json";
            this.tabletDetails = ParseJsonDataForTabletsBatteryDetails(filePath);
        }


        //Method to parse json file from resources folder takes filePath as input in string format
        //Returns list of Tablet details.
        public List<TabletDetails> ParseJsonDataForTabletsBatteryDetails(string filePath)
        {
            List<TabletDetails> data = new List<TabletDetails>();

            try
            {
                using (StreamReader r = new StreamReader(filePath))
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

        //Get all devices battery usage percentage method.
        //Returns list of battery usage data for each device.
        public IEnumerable<TabletBatteryData> GetAllTabletsDetails()
        {
            return CalculateTabletBatteryUsage(tabletDetails);
        }

        //Get all devices battery usage percentage after applying academy filter takes academyId as input parameter in int format.
        //Returns list of battery usage data for each device.
        public IEnumerable<TabletBatteryData> GetTabletDetailsByAcademyId(int academyId)
        {
            List<TabletDetails> tabDetails = tabletDetails.Where(detail => detail.AcadamyId == academyId).ToList();
            return CalculateTabletBatteryUsage(tabDetails);
        }

        //Get all devices battery usage percentage after applying serialnumber filter takes serialnumber as input parameter in string format
        //Returns list of battery usage data for each device.
        public IEnumerable<TabletBatteryData> GetTabletDetailsByDeviceId(string serialNumber)
        {
            List<TabletDetails> tabDetails = tabletDetails.Where(detail => detail.SerialNumber == serialNumber).ToList();
            return CalculateTabletBatteryUsage(tabDetails);
        }

        //Get all devices battery usage percentage after applying employee filter takes employeeId as input parameter in string format
        //Returns list of battery usage data for each device.
        public IEnumerable<TabletBatteryData> GetTabletDetailsByEmployeeId(string employeeId)
        {
            List<TabletDetails> tabDetails = tabletDetails.Where(detail => detail.EmployeeId == employeeId).ToList();
            return CalculateTabletBatteryUsage(tabDetails);
        }

        //Method to calculate battery usage percentage takes list of TabletDetails as input parameter.
        //Returns list of battery usage data for each device.
        public IEnumerable<TabletBatteryData> CalculateTabletBatteryUsage(List<TabletDetails> devicesList)
        {
            var devicesGroupedBySerialNumber = devicesList.GroupBy(t => t.SerialNumber)
                        .ToDictionary(group => group.Key,group => group.ToList());
            List<TabletBatteryData> tabletBatteryPercentageData = new List<TabletBatteryData>();
            foreach(var data in devicesGroupedBySerialNumber)
            {
                var devicedata = data.Value.OrderBy(key => key.TimeStamp).ToList();
                var dropinpercentage = 0.0;
                double duration = 0.0;
                int i = 0;
                if (devicedata.Count > 1)
                {
                    while (i < devicedata.Count() - 1)
                    {
                        if (devicedata[i].BatteryLevel > devicedata[i + 1].BatteryLevel)
                        {
                            dropinpercentage += devicedata[i].BatteryLevel - devicedata[i + 1].BatteryLevel;
                            duration += (devicedata[i + 1].TimeStamp - devicedata[i].TimeStamp).TotalSeconds;
                        }
                        i++;
                    }

                    var dailypercentage = Math.Round((86400 * (dropinpercentage * 100)) / duration , 2);
                    TabletBatteryData tabletBatteryData = new TabletBatteryData { BatteryPercentage = dailypercentage, SerialNumber = data.Key, BatteryReplacementNeeded = dailypercentage > 30 ? BatteryReplacementRecommendation.ReplaceBattery.ToString() : BatteryReplacementRecommendation.BatteryIsGood.ToString() };
                    tabletBatteryPercentageData.Add(tabletBatteryData);
                }
                else
                {
                    tabletBatteryPercentageData.Add(new TabletBatteryData { BatteryPercentage = 0.0, SerialNumber = data.Key, BatteryReplacementNeeded = BatteryReplacementRecommendation.Unknown.ToString() });
                }
            }

            return tabletBatteryPercentageData;
        }

    }
}
