namespace CampusFlow.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public string? Major { get; set; }

        //Navigation
        public User? User { get; set; }
        public ICollection<StudentSubject>? StudentSubject { get; set; }
        public ICollection<StudentAssignment>? StudentAssignment { get; set; }
    }
}
