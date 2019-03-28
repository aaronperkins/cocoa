using Cocoa.Hal;
using Cocoa.Hal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Cocoa.ControlPanel.Controllers
{
    [Route("api/[controller]")]
    public class CommandController : Controller
    {
        private IConfiguration _configuration;
        private Driver _driver;

        public CommandController(IConfiguration Configuration, Driver driver)
        {
            _configuration = Configuration;
            _driver = driver;
        }

        [HttpPost("[action]")]
        public ActionResult Close()
        {
            _driver.Close();

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Lay()
        {
            if (!_driver.IsOpen)
                _driver.Open();

            var pose = new Pose("<P,50,-1,0,3.1048,1000,1.5647,1000,0.0000,1000,3.1048,1000,4.7185,1000,6.2770,1000,3.0680,1000,4.7124,1000,6.2770,1000,3.1845,1000,1.5401,1000,0.0000,1000>");
            await _driver.SetPose(pose);

            await _driver.Relax();

            return Ok();
        }

        [HttpPost("[action]")]
        public ActionResult Open()
        {
            _driver.Open();

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Pose([FromBody] Pose pose)
        {
            if (!_driver.IsOpen)
                _driver.Open();

            await _driver.SetPose(pose);

            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<Pose>> Pose()
        {
            if (!_driver.IsOpen)
            {
                _driver.Open();
                await Task.Delay(500); //wait for pose update
            }

            return Ok(_driver.CurrentPose);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Relax()
        {
            if (!_driver.IsOpen)
                _driver.Open();

            await _driver.Relax();

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Stand()
        {
            if (!_driver.IsOpen)
                _driver.Open();

            var pose = new Pose("<P,30,-1.0000,1000,3.1232,1000,2.0678,1000,1.3683,1000,3.1170,1000,4.1540,1000,4.9271,1000,3.1109,1000,4.4915,1000,4.9149,1000,3.1600,1000,1.6935,1000,1.3683,1000>");

            await _driver.SetPose(pose);

            return Ok();
        }
    }
}