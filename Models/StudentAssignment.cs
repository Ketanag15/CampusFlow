namespace CampusFlow.Models
{
    public class StudentAssignment
    {
        public int StudentAssignmentId { get; set; }
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public string? AttachmentPath { get; set; }
        public string? Grade { get; set; }

        // Navigation
        public Assignment? Assignment { get; set; }
        public Student? Student { get; set; }
    }
}
