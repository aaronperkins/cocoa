namespace Cocoa.Data.Models
{
    public class SequencePose
    {
        public int Order { get; set; }
        public int PoseId { get; set; }
        public Pose Pose { get; set; }
        public int SequenceId { get; set; }
        public Sequence Sequence { get; set; }
        public int SequencePoseId { get; set; }
    }
}