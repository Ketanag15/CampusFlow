namespace CampusFlow.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string? SubjectDescription { get; set; }

        // Navigation
        public ICollection<TeacherSubject>? TeacherSubject { get; set; }
        public ICollection<StudentSubject>? StudentSubject { get; set; }
        public ICollection<Assignment>? Assignment { get; set; }
    }
}
