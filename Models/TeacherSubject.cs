namespace CampusFlow.Models
{
    public class TeacherSubject
    {
        public int TeacherSubjectId { get; set; }
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }

        //Navigation
        public Teacher? Teacher { get; set; }
        public Subject? Subject { get; set; }
    }
}
