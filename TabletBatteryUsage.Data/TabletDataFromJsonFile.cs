using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public IEnumerable<TabletBatteryData> CalculateTabletBatteryUsage()
        {
            throw new NotImplementedException();
        }
    }
}
