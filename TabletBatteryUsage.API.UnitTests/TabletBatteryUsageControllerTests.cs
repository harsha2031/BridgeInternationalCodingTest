using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using TabletBatteryUsage.API.Controllers;
using TabletBatteryUsage.Data;
using Xunit;

namespace TabletBatteryUsage.API.UnitTests
{
    public class TabletBatteryUsageControllerTests
    {
        readonly TabletBatteryUsageController tabletBatteryUsageController;
        private readonly ITabletsData tabletsData;
        public TabletBatteryUsageControllerTests()
        {
            tabletsData = new TabletDataFromJsonFile();
            tabletBatteryUsageController = new TabletBatteryUsageController(new NullLogger<TabletBatteryUsageController>(),tabletsData);
        }

        [Fact]
        public void TestGetAllDevicesData()
        {
            //Act
            var data = tabletBatteryUsageController.GetAllTabletsData();

            //Assert
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public void TestGetAllDeviceDataByDeviceNotFound()
        {
            //Arange
            var serialNumber = "-1";

            //Act
            var data = tabletBatteryUsageController.GetAllTabletsDataByDevice(serialNumber);

            //Assert
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public void TestGetDeviceBatteryDataByAcademy()
        {
            //Arrange
            var academy = 0;

            //Act
            var data = tabletBatteryUsageController.GetAllTabletsDataByAcademy(academy);

            //Assert
            Assert.IsType<OkObjectResult>(data);

        }
        [Fact]
        public void TestGetDeviceBatteryDataByAcademyNotFound()
        {
            //Arrange
            var academy = -1;

            //Act
            var data = tabletBatteryUsageController.GetAllTabletsDataByAcademy(academy);

            //Assert
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public void TestGetDeviceBatteryDataByEmployeeNotFound()
        {
            //Arrange
            var employeeId = "0";

            //Act
            var data = tabletBatteryUsageController.GetAllTabletsDataByEmployee(employeeId);

            //Assert
            Assert.IsType<NotFoundResult>(data);
        }
        [Fact]
        public void TestGetDeviceBatteryDataByEmployee()
        {
            //Arrange
            var employeeId = "T1007384";

            //Act
            var data = tabletBatteryUsageController.GetAllTabletsDataByEmployee(employeeId);

            //Assert
            Assert.IsType<OkObjectResult>(data);
        }

    }
}
