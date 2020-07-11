using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TabletBatteryUsage.Core;
using TabletBatteryUsage.Data;
using Xunit;

namespace TabletBatteryUsage.API.UnitTests
{
    public class TabletBatteryUsageDataTests
    {
        readonly List<TabletDetails> inMemoryTabletData;
        readonly TabletDataFromJsonFile tabletDataFromJson;
        public TabletBatteryUsageDataTests()
        {
            inMemoryTabletData = new List<TabletDetails>
            {
                new TabletDetails {AcadamyId = 0, SerialNumber = "1",EmployeeId = "E1", BatteryLevel = 0.90f, TimeStamp = DateTime.Parse("2019-05-17T01:47:25.833-05:00")},
                new TabletDetails {AcadamyId = 0, SerialNumber = "1",EmployeeId = "E2", BatteryLevel = 0.80f, TimeStamp = DateTime.Parse("2019-05-17T03:15:17.478-05:00")},
                new TabletDetails {AcadamyId = 0, SerialNumber = "1",EmployeeId = "E2", BatteryLevel = 0.60f, TimeStamp = DateTime.Parse("2019-06-17T05:47:25.833-05:00")},
                new TabletDetails {AcadamyId = 0, SerialNumber = "2",EmployeeId = "E3", BatteryLevel = 0.80f, TimeStamp = DateTime.Parse("2019-07-17T04:47:25.833-05:00")}
            };

            tabletDataFromJson = new TabletDataFromJsonFile();

        }

        //Test for Json file error
        [Fact]
        public void IsJsonFileExists()
        {
            //Arrange
            string filePath = "C:\\Users\\harsh\\Desktop\\BridgeInternational\\TabletBatteryUsage.API\\TabletBatteryUsage.Data\\Resources\\battery.json";

            //Act
            bool fileExists = File.Exists(filePath);

            //Assert
            Assert.True(fileExists);
        }

        [Fact]
        public void ValidateForDataInJson()
        {
            //Arrange 
            string filePath = "C:\\Users\\harsh\\Desktop\\BridgeInternational\\TabletBatteryUsage.API\\TabletBatteryUsage.Data\\Resources\\battery.json";

            //Act
            string json;
            using (StreamReader r = new StreamReader(filePath))
            {
                json = r.ReadToEnd();
            }

            //Assert
            Assert.True(json.Length != 0);

        }

        [Fact]
        public void ValidateJsonParsing()
        {
            //Arrange
            string filePath = "C:\\Users\\harsh\\Desktop\\BridgeInternational\\TabletBatteryUsage.API\\TabletBatteryUsage.Data\\Resources\\battery.json";

            //Act
            List<TabletDetails> actualData = tabletDataFromJson.ParseJsonDataForTabletsBatteryDetails(filePath);


            //Assert
            Assert.True(actualData.Count() != 0);
        }

        //Test for Calculation
        [Fact]
        public void TestForValidCalculation()
        {
            //Arrange 
            IEnumerable<TabletBatteryData> expectedData = new List<TabletBatteryData> { new TabletBatteryData { BatteryPercentage = 0.0, BatteryReplacementNeeded = "Unknown", SerialNumber = "1805C67HD02332" } };

            //Act
            IEnumerable<TabletBatteryData> actualData = tabletDataFromJson.CalculateTabletBatteryUsage(new List<TabletDetails> { new TabletDetails {AcadamyId = 0, SerialNumber = "1805C67HD02332", EmployeeId = "E1", BatteryLevel = 0.90f, TimeStamp = DateTime.Parse("2019-05-17T01:47:25.833-05:00")} });

            //Assert


            IEnumerator<TabletBatteryData> e1 = expectedData.GetEnumerator();
            IEnumerator<TabletBatteryData> e2 = actualData.GetEnumerator();

            while (e1.MoveNext() && e2.MoveNext())
            {
                Assert.Equal(e1.Current.SerialNumber, e2.Current.SerialNumber);
                Assert.Equal(e1.Current.BatteryPercentage, e2.Current.BatteryPercentage);
                Assert.Equal(e1.Current.BatteryReplacementNeeded, e2.Current.BatteryReplacementNeeded);
            }
        }

        [Fact]
        public void TestForValidateBatteryPercentageCalculationOnJsonFile()
        {
            IEnumerable<TabletBatteryData> expectedData = new List<TabletBatteryData> { 
                new TabletBatteryData { BatteryPercentage = 0.96, BatteryReplacementNeeded = "BatteryIsGood", SerialNumber = "1" },
                new TabletBatteryData { BatteryPercentage = 0,BatteryReplacementNeeded="Unknown",SerialNumber="2"}
            };

            //Act
            IEnumerable<TabletBatteryData> actualData = tabletDataFromJson.CalculateTabletBatteryUsage(inMemoryTabletData);

            //Assert

            IEnumerator<TabletBatteryData> e1 = expectedData.GetEnumerator();
            IEnumerator<TabletBatteryData> e2 = actualData.GetEnumerator();

            while (e1.MoveNext() && e2.MoveNext())
            {
                Assert.Equal(e1.Current.SerialNumber, e2.Current.SerialNumber);
                Assert.Equal(e1.Current.BatteryPercentage, e2.Current.BatteryPercentage);
                Assert.Equal(e1.Current.BatteryReplacementNeeded, e2.Current.BatteryReplacementNeeded);
            }
        }

        [Fact]
        public void TestForValidateBatteryPercentageByDevice()
        {
            IEnumerable<TabletBatteryData> expectedData = new List<TabletBatteryData> { new TabletBatteryData { BatteryPercentage = 0.0, BatteryReplacementNeeded = "Unknown", SerialNumber = "1805C67HD02332" } };
            var serialNumber = "1";

            //Act
            IEnumerable<TabletBatteryData> actualData = tabletDataFromJson.GetTabletDetailsByDeviceId(serialNumber);


            IEnumerator<TabletBatteryData> e1 = expectedData.GetEnumerator();
            IEnumerator<TabletBatteryData> e2 = actualData.GetEnumerator();

            while (e1.MoveNext() && e2.MoveNext())
            {
                Assert.Equal(e1.Current.SerialNumber, e2.Current.SerialNumber);
                Assert.Equal(e1.Current.BatteryPercentage, e2.Current.BatteryPercentage);
                Assert.Equal(e1.Current.BatteryReplacementNeeded, e2.Current.BatteryReplacementNeeded);
            }

        }

        [Fact]
        public void TestForValidateBatteryPercentageByEmployee()
        {
            IEnumerable<TabletBatteryData> expectedData = new List<TabletBatteryData> { new TabletBatteryData { BatteryPercentage = 0.0, BatteryReplacementNeeded = "Unknown", SerialNumber = "1805C67HD02332" } };
            var serialNumber = "E1";

            //Act
            IEnumerable<TabletBatteryData> actualData = tabletDataFromJson.GetTabletDetailsByDeviceId(serialNumber);


            IEnumerator<TabletBatteryData> e1 = expectedData.GetEnumerator();
            IEnumerator<TabletBatteryData> e2 = actualData.GetEnumerator();

            while (e1.MoveNext() && e2.MoveNext())
            {
                Assert.Equal(e1.Current.SerialNumber, e2.Current.SerialNumber);
                Assert.Equal(e1.Current.BatteryPercentage, e2.Current.BatteryPercentage);
                Assert.Equal(e1.Current.BatteryReplacementNeeded, e2.Current.BatteryReplacementNeeded);
            }
        }

    }
}
