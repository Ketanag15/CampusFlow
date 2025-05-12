namespace CampusFlow.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Role { get; set; }  //Roles- Admin, Teacher, Student

        //Relationship
        public Student? Student { get; set; }
        public Teacher? Teacher { get; set; }

    }
}
