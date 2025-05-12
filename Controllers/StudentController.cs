//using Microsoft.AspNetCore.Components;
using CampusFlow.Data;
using CampusFlow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusFlow.Controllers
{
    [Route("api/[controller")]
    [ApiController]

    public class StudentController : ControllerBase
    {
        private readonly CampusFlowDbContext _DbContext;

        public StudentController(CampusFlowDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        //GET api for all students accessed only by teachers and admins
        [Authorize(Roles = "admin, teacher")]
        [HttpGet("allStudents")]
        public IActionResult GetAllStudents()
        {
            var students = _DbContext.Student.ToList();
            return Ok(students);
        }

        [Authorize(Roles = "admin,teacher,student")]
        [HttpGet("student/{id}")]
        public IActionResult GetStudentById(int id)
        {
            var currentUser = User.Identity.Name;

            if(User.IsInRole("Student") && currentUser != id.ToString())
            {
                return Unauthorized("You can see only your record. Not of others.");
            }

            var student = _DbContext.Student.FirstOrDefault(s => s.StudentId == id);
            if(student == null)
            {
                return NotFound("Student not found.");
            }
            return Ok(student);
        }

        [Authorize(Roles ="admin, teacher")]
        [HttpPost("addStudent")]
        public IActionResult CreateStudent([FromBody] Student student)
        {
            if(student == null)
            {
                return BadRequest("Invalid Student Data.");
            }

            _DbContext.Student.Add(student);
            _DbContext.SaveChanges();
            return CreatedAtAction(nameof(GetStudentById), new { id = student.StudentId }, student);
        }

        [Authorize(Roles ="admin")]
        [HttpDelete("deleteStudent/{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var student = _DbContext.Student.FirstOrDefault(s => s.StudentId==id);
            if(student != null)
            {
                return NotFound("Student not found");
            }

            _DbContext.Student.Remove(student);
            _DbContext.SaveChanges();
            return NoContent();
        }

        // PUT: api/students/{id} (admin, teacher, or the student themselves)
        [Authorize(Roles = "admin,teacher")]
        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] Student student)
        {
            var currentUser = User.Identity.Name;

            // A student can only update their own record
            if (User.IsInRole("student") && currentUser != id.ToString())
            {
                return Unauthorized("You can only update your own record.");
            }

            var existingStudent = _DbContext.Student.FirstOrDefault(s => s.StudentId == id);
            if (existingStudent == null) return NotFound("Student not found.");

            // Update fields (you can add specific field updates here if needed)
            existingStudent.Name = student.Name;
            existingStudent.EnrollmentDate = student.EnrollmentDate;
            existingStudent.Major = student.Major;

            _DbContext.SaveChanges();
            return NoContent();
        }
    }
}
