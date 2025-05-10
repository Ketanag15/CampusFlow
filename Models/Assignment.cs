namespace CampusFlow.Models
{
    public class Assignment
    {
        public int AssignmentId { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? UploadDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string? AttachmentPath { get; set; }

        // Navigation
        public Subject? Subject { get; set; }
        public Teacher? Teacher { get; set; }
        public ICollection<StudentAssignment>? StudentAssignment { get; set; }
    }
}
