namespace CampusFlow.Models
{
    public class StudentSubject
    {
        public int StudentSubjectId { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }

        //Navigation
        public Student? Student { get; set; }
        public Subject? Subject { get; set; }
    }
}
