using Cocoa.Hal.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cocoa.Hal
{
    public class PoseEngine
    {
        protected static SemaphoreSlim _syncPose = new SemaphoreSlim(1, 1);
        private readonly Driver _driver;

        public PoseEngine(Driver driver)
        {
            _driver = driver;
        }

        public async Task<Pose> PlayPose(Pose pose)
        {
            await _syncPose.WaitAsync();

            try
            {
                int newPoseId = pose.Id;
                if (_driver.LastFinishedPose.Id == newPoseId)
                    newPoseId++;

                await _driver.SendPose(pose);

                var timer = new Stopwatch();
                timer.Start();
                while (newPoseId != _driver.LastFinishedPose.Id)
                {
                    if (timer.ElapsedMilliseconds > pose.TotalTime + 100) // allow a little buffer on the time out for servos to settle
                    {                       
                        return null;
                    }

                    await Task.Yield();
                }

                if (pose.Delay > 0)
                    await Task.Delay(pose.Delay);

                return _driver.LastFinishedPose;
            }
            finally
            {
                _syncPose.Release();
            }
        }

    }
}
