using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TabletBatteryUsage.Core;
using TabletBatteryUsage.Data;

namespace TabletBatteryUsage.API.Controllers
{
    
    [ApiController]
    public class TabletBatteryUsageController : ControllerBase
    {
        private readonly ILogger<TabletBatteryUsageController> logger;
        private readonly ITabletsData tabletsData;

        //Constructor with dependancy injection. 
        public TabletBatteryUsageController(ILogger<TabletBatteryUsageController> logger, ITabletsData tabletsData)
        {
            this.logger = logger;
            this.tabletsData = tabletsData;
        }

        //Get all devices battery usage details 
        [HttpGet]
        [Route("api/TabletBatteryUsage")]
        public IActionResult GetAllTabletsData()
        {
            logger?.LogDebug("'{0}' has been invoked", nameof(GetAllTabletsData));
            IEnumerable<TabletBatteryData> response = new List<TabletBatteryData>();
            try
            {
                response = tabletsData.GetAllTabletsDetails();
                logger.LogInformation("Tablet battery usage data retrieved successfully.");
            }
            catch(Exception ex)
            {
                
                logger.LogCritical("There was an error on '{0}' invocation: {1}", nameof(GetAllTabletsData), ex);
                return BadRequest();
            }
            if(response.Count() == 0)
            {
                return NotFound();
            }
            return Ok(response);
        }

        //Get all devices battery usage details for given serialnumber
        [HttpGet]
        [Route("api/[controller]/tablets/{serialnumber}")]
        public IActionResult GetAllTabletsDataByDevice(string serialnumber)
        {
            logger?.LogDebug("'{0}' has been invoked", nameof(GetAllTabletsData));
            IEnumerable<TabletBatteryData> response = new List<TabletBatteryData>();
            try
            {
                response = tabletsData.GetTabletDetailsByDeviceId(serialnumber);
                logger.LogInformation("Tablet battery usage data retrieved successfully.");
            }
            catch (Exception ex)
            {
                logger.LogCritical("There was an error on '{0}' invocation: {1}", nameof(GetAllTabletsData), ex);
                return BadRequest();
            }

            if (response.Count() == 0)
            {
                return NotFound();
            }
            return Ok(response);
        }

        //Get all devices battery usage details for given EmployeeId
        [HttpGet]
        [Route("api/[controller]/employee/{employeeid}")]
        public IActionResult GetAllTabletsDataByEmployee(string employeeid)
        {
            logger?.LogDebug("'{0}' has been invoked", nameof(GetAllTabletsData));
            IEnumerable<TabletBatteryData> response = new List<TabletBatteryData>();

            try
            {
                response = tabletsData.GetTabletDetailsByEmployeeId(employeeid);
                logger.LogInformation("Tablet battery usage data retrieved successfully.");
            }
            catch (Exception ex)
            {
                logger.LogCritical("There was an error on '{0}' invocation: {1}", nameof(GetAllTabletsData), ex);
                return BadRequest();
            }
            if(response.Count() == 0)
            {
                return NotFound();
            }
            return Ok(response);
        }

        //Get all devices battery usage details for given academyId
        [HttpGet]
        [Route("api/[controller]/academy/{academyid}")]
        public IActionResult GetAllTabletsDataByAcademy(int academyid)
        {
            logger?.LogDebug("'{0}' has been invoked", nameof(GetAllTabletsData));
            IEnumerable<TabletBatteryData> response = new List<TabletBatteryData>();
            try
            {
                response = tabletsData.GetTabletDetailsByAcademyId(academyid);
                logger.LogInformation("Tablet battery usage data retrieved successfully.");
            }
            catch (Exception ex)
            {
                logger.LogCritical("There was an error on '{0}' invocation: {1}", nameof(GetAllTabletsData), ex);
                return BadRequest();
            }
            if (response.Count() == 0)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}
