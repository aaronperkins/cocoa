using System;
using System.Collections.Generic;
using System.Text;

namespace Cocoa.Hal.Models
{
    public class Joint
    {
        public int Id { get; set; }
        public double Angle;
        public int Duration;
    }
}
