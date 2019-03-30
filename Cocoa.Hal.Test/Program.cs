using Cocoa.Data.Models;
using System;

namespace Cocoa.Hal.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var driver = new Driver("COM10", 115200);
            var poseEngine = new PoseEngine(driver);
            var layPose = new Pose("<P,50,-1,0,3.1048,1000,1.5647,1000,0.0000,1000,3.1048,1000,4.7185,1000,6.2770,1000,3.0680,1000,4.7124,1000,6.2770,1000,3.1845,1000,1.5401,1000,0.0000,1000>");
            var standPose = new Pose("<P,30,-1,0,3.1415,1000,2.0000,1000,1.2000,1000,3.1415,1000,4.1415,1000,5.0000,1000,3.1415,1000,4.1415,1000,4.9000,1000,3.1415,1000,2.000,1000,1.3000,1000>");

            driver.Open();

            while (true)
            {
                Pose pose;
                Console.WriteLine("Lay");
                pose = poseEngine.PlayPose(layPose).Result;
                PrintPose(pose);
                Console.WriteLine("Stand");
                pose = poseEngine.PlayPose(standPose).Result;
                PrintPose(pose);
            }

        }

        static void PrintPose(Pose pose)
        {
            if (pose != null)
            {
                Console.WriteLine("Pose " + pose.PoseId + " Done: " + pose.TotalTime);
            }
            else
            {
                Console.WriteLine("Pose timeout!");
            }
        }
    }
}
