using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentInfoSystem.Api.Data;
using StudentInfoSystem.Api.Data.Entities;
using StudentInfoSystem.Api.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentInfoSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private AppDbContext _dbContext;

        public StudentsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/values
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            List<StudentModel> students = new();

            var dbStudents = await _dbContext.Students.ToListAsync();

            foreach (var item in dbStudents)
            {
                students.Add(new StudentModel
                {
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Number = item.Number,
                    Branch = item.Branch
                });

            }

            return Ok(students);
        }

        // GET api/values/5
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var dbStudent = await _dbContext.Students.FindAsync(id);

            if (dbStudent is null)
            {
                return NotFound("Student is not found.");
            }

            return Ok(dbStudent);
        }

        // POST api/values
        [HttpPost("register")]
        public async Task<IActionResult> Post([FromForm]StudentEntity student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _dbContext.Students.Add(student);

                await _dbContext.SaveChangesAsync();

                return Ok("Student added successfully.");

            }
            catch (DbUpdateException ex)
            {
                // SQL Unique constraint hatasını yakalamak
                if (ex.InnerException != null && ex.InnerException.Message.Contains("IX_Students_Number"))
                {
                    return BadRequest("Number must be unique. This student number already exists.");
                }

                return StatusCode(500, "An error occurred while saving the student.");
            }



        }

        // PUT api/values/5
        [HttpPut("update/{number}")]
        public async Task<IActionResult> Put(string number, [FromForm] StudentEntity student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbStudent = await _dbContext.Students.FirstOrDefaultAsync(s => s.Number == number);

            if (dbStudent is null)
            {
                return NotFound("Student is not found.");
            }

            //_dbContext.Entry(student).State = EntityState.Modified;
            dbStudent.FirstName = student.FirstName;
            dbStudent.LastName = student.LastName;
            dbStudent.Branch = student.Branch;

            await _dbContext.SaveChangesAsync();

            return Ok("Student updated successfully.");

        }

        // DELETE api/values/5
        [HttpDelete("delete/{number}")]
        public async Task<IActionResult> Delete(string number)
        {
            var dbStudent = await _dbContext.Students.FirstOrDefaultAsync(s => s.Number == number);

            if (dbStudent is null)
            {
                return NotFound();
            }

            _dbContext.Students.Remove(dbStudent);

            await _dbContext.SaveChangesAsync();

            return Ok("Student deleted successfully.");
        }
    }
}

