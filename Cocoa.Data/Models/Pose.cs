using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocoa.Data.Models
{
    public class Pose
    {
        public Pose()
        {
            Joints = new List<Joint>();
        }

        public Pose(string pose) : this()
        {
            var poseParse = pose.Substring(0, pose.Length - 1).Split(",");

            PoseId = Convert.ToInt32(poseParse[1]);
            int i = 0;
            for (int j = 2; j < poseParse.Length; j += 2)
            {
                var angle = Convert.ToDouble(poseParse[j]);

                if (angle >= 0)
                {
                    var joint = new Joint()
                    {
                        ServoId = i,
                        Angle = angle,
                        Duration = Convert.ToInt32((poseParse[j + 1]))
                    };
                    Joints.Add(joint);
                }

                i++;
            }
        }

        public int Delay { get; set; }

        public List<Joint> Joints { get; set; }

        public int PoseId { get; set; }

        public string Name { get; set; }

        public int TotalTime
        {
            get
            {
                if (Joints.Count > 0)
                    return Joints.Max(j => j.Duration);
                else
                    return 0;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<P,");
            builder.Append(this.PoseId);
            int numberOfJoints = Joints.Max(j => j.ServoId) + 1;
            for (int i = 0; i < numberOfJoints; i++)
            {
                var joint = this.Joints.Where(j => j.ServoId == i).FirstOrDefault();

                if (joint == null)
                {
                    builder.Append(",-1,0");
                }
                else
                {
                    builder.AppendFormat(",{0:F4},{1}", joint.Angle, joint.Duration);
                }
            }
            builder.Append(">");

            return builder.ToString();
        }
    }
}