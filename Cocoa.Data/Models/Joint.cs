using System;
using System.Collections.Generic;
using System.Text;

namespace Cocoa.Data.Models
{
    public class Joint
    {
        public int JointId { get; set; }
        public int ServoId { get; set; }
        public double Angle { get; set; }
        public int Duration { get; set; }
        public int PoseId { get; set; }
    }
}
