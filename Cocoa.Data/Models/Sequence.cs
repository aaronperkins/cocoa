using System.Collections.Generic;

namespace Cocoa.Data.Models
{
    public class Sequence
    {
        public Sequence()
        {
            SequencePoses = new List<SequencePose>();
        }

        public string Name { get; set; }
        public int SequenceId { get; set; }
        public List<SequencePose> SequencePoses { get; set; }
    }
}