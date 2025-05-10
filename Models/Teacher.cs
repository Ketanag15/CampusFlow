namespace CampusFlow.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Department { get; set; }

        // Navigation
        public User? User { get; set; }
        public ICollection<TeacherSubject>? TeacherSubject { get; set; }
        public ICollection<Assignment>? Assignment { get; set; }
    }
}
