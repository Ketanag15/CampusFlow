using CampusFlow.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusFlow.Data
{
    public class CampusFlowDbContext : DbContext
    {
        public CampusFlowDbContext(DbContextOptions<CampusFlowDbContext> options) : base(options)
        {

        }

        public DbSet<User> User  { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<StudentSubject> StudentSubject { get; set; }
        public DbSet<TeacherSubject> TeacherSubject { get; set; }
        public DbSet<Assignment> Assignment { get; set; }
        public DbSet<StudentAssignment> StudentAssignment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring one-to-one relationship between User and Student
            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)  // Each student has one user
                .WithOne(u => u.Student) // Each user corresponds to one student
                .HasForeignKey<Student>(s => s.UserId); // UserId is the foreign key in Student

            // Configuring one-to-one relationship between User and Teacher
            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.User)  // Each teacher has one user
                .WithOne(u => u.Teacher) // Each user corresponds to one teacher
                .HasForeignKey<Teacher>(t => t.UserId); // UserId is the foreign key in Teacher

            // Configuring many-to-many relationship between Student and Subject through StudentSubject
            modelBuilder.Entity<StudentSubject>()
                .HasKey(ss => new { ss.StudentId, ss.SubjectId });  // Composite key for the junction table
            modelBuilder.Entity<StudentSubject>()
                .HasOne(ss => ss.Student) // A StudentSubject belongs to a student
                .WithMany(s => s.StudentSubject) // A student can have many subjects
                .HasForeignKey(ss => ss.StudentId); // StudentId as the foreign key

            modelBuilder.Entity<StudentSubject>()
                .HasOne(ss => ss.Subject) // A StudentSubject belongs to a subject
                .WithMany(s => s.StudentSubject) // A subject can have many students
                .HasForeignKey(ss => ss.SubjectId); // SubjectId as the foreign key

            // Configuring many-to-many relationship between Teacher and Subject through TeacherSubject
            modelBuilder.Entity<TeacherSubject>()
                .HasKey(ts => new { ts.TeacherId, ts.SubjectId }); // Composite key for the junction table
            modelBuilder.Entity<TeacherSubject>()
                .HasOne(ts => ts.Teacher) // A TeacherSubject belongs to a teacher
                .WithMany(t => t.TeacherSubject) // A teacher can teach many subjects
                .HasForeignKey(ts => ts.TeacherId); // TeacherId as the foreign key

            modelBuilder.Entity<TeacherSubject>()
                .HasOne(ts => ts.Subject) // A TeacherSubject belongs to a subject
                .WithMany(s => s.TeacherSubject) // A subject can be taught by many teachers
                .HasForeignKey(ts => ts.SubjectId); // SubjectId as the foreign key

            // Configuring one-to-many relationship between Teacher and Assignment
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Teacher)  // An Assignment belongs to a teacher
                .WithMany(t => t.Assignment) // A teacher can have many assignments
                .HasForeignKey(a => a.TeacherId) // TeacherId as the foreign key in Assignment
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete if a teacher is deleted

            // Configuring one-to-many relationship between Subject and Assignment
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Subject)  // An Assignment belongs to a subject
                .WithMany(s => s.Assignment) // A subject can have many assignments
                .HasForeignKey(a => a.SubjectId) // SubjectId as the foreign key in Assignment
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete if a subject is deleted

            // Configuring one-to-many relationship between Assignment and StudentAssignment
            modelBuilder.Entity<StudentAssignment>()
                .HasOne(sa => sa.Assignment)  // A StudentAssignment belongs to an Assignment
                .WithMany(a => a.StudentAssignment) // An Assignment can have many StudentAssignments
                .HasForeignKey(sa => sa.AssignmentId) // AssignmentId as the foreign key in StudentAssignment
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete if an Assignment is deleted

            // Configuring one-to-many relationship between Student and StudentAssignment
            modelBuilder.Entity<StudentAssignment>()
                .HasOne(sa => sa.Student) // A StudentAssignment belongs to a Student
                .WithMany(s => s.StudentAssignment) // A student can have many StudentAssignments
                .HasForeignKey(sa => sa.StudentId) // StudentId as the foreign key in StudentAssignment
                .OnDelete(DeleteBehavior.Restrict); // Do not allow deletion of student if there are assignments

        }

    }
}
