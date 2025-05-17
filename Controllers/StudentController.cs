using CampusFlow.Data;
using CampusFlow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using System.Security.Claims;

namespace CampusFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly CampusFlowDbContext _context;
        private readonly IConfiguration _configuration;

        public StudentController(CampusFlowDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        //Get All Students
        [HttpGet]
        [Authorize(Roles ="admin, teacher")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = _context.Student.Include(s => s.User).ToListAsync();
            return Ok(students);
        }

        //Get Student By Id
        [HttpGet("{id}")]
        [Authorize(Roles ="admin, teacher, student")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var username = User.FindFirstValue(ClaimTypes.Name);

            var student = await _context.Student.Include(s => s.User).FirstOrDefaultAsync(s => s.StudentId == id);
            if (student == null)
            {
                return NotFound("Student Not Found");
            }

            //If Student is accessing , ensure that they can access only their data.
            if(role == "student" && student.User.UserName != username)
            {
                return Forbid();
            }

            return Ok(student);
        }

        //Create Student
        [HttpPost("{add-student}")]
        [Authorize(Roles = "admin, teacher")]
        public async Task<IActionResult> CreateStudent(Student student)
        {
            _context.Student.Add(student);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStudentById), new { id = student.StudentId }, student);
        }

        //Update Student
        [HttpPut("{id}")]
        [Authorize(Roles = "admin, teacher, student")]
        public async Task<IActionResult> UpdateStudent(int id, Student updatedStudent)
        {
            var existingdata = await _context.Student.FindAsync(id);
            if (existingdata == null)
            {
                return NotFound();
            }

            var role = User.FindFirstValue(ClaimTypes.Role);
            var username = User.FindFirstValue(ClaimTypes.Name);

            if(role == "student" && existingdata.User.UserName != username)
            {
                return Forbid();
            }

            existingdata.Name = updatedStudent.Name;
            existingdata.EnrollmentDate = updatedStudent.EnrollmentDate;
            existingdata.Major = updatedStudent.Major;
            await _context.SaveChangesAsync();

            return Ok(existingdata);
        }

        //Delete a student
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin, teacher")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return Ok(student);
        }

        //Allows a student to fetch their own data without needing to know their ID.
        [HttpGet]
        [Authorize(Roles ="student")]
        public async Task<IActionResult> GetMyStudentDetails()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var student = await _context.Student.Include(s=> s.User)
                                                .FirstOrDefaultAsync(s => s.User.UserName == username);

            if(student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }


    }
}