using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.CRUD.Web.Data;
using StudentPortal.CRUD.Web.Models;
using StudentPortal.CRUD.Web.Models.Entities;

namespace StudentPortal.CRUD.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public StudentsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddStudentModel viewModel)
        {
            var student = new Student()
            {
                Name =viewModel.Name,
                Email =viewModel.Email,
                Phone =viewModel.Phone,
                Subscribed=viewModel.Subscribed
            };
            await dbContext.Students.AddAsync(student);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("StudentList", "Students");
        }
        [HttpGet]
        public async Task<IActionResult> StudentList()
        {
            var students=await dbContext.Students.ToListAsync();
            return View(students);
        }
        [HttpGet]
        public async Task<IActionResult> StudentEdit(Guid id)
        {
            var students=await dbContext.Students.FindAsync(id);
            return View(students);
        }
        [HttpPost]
        public async Task<IActionResult> StudentEdit(Student viewModel)
        {
            var student= await dbContext.Students.FindAsync(viewModel.Id);
            if (student is not null)
            {
                student.Name = viewModel.Name;
                student.Email = viewModel.Email;
                student.Phone = viewModel.Phone;
                student.Subscribed = viewModel.Subscribed;

                await dbContext.SaveChangesAsync();
            }
            
            return RedirectToAction("StudentList", "Students");
        } 
        [HttpPost]
        public async Task<IActionResult> StudentDelete(Student viewModel)
        {
            var student= await dbContext.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(x=>x.Id==viewModel.Id);
            if (student is not null)
            {
                dbContext.Students.Remove(viewModel);
                await dbContext.SaveChangesAsync();
            }
            
            return RedirectToAction("StudentList", "Students");
        }
        [HttpPost]
        public IActionResult RedirectToStudList()
        {
            return RedirectToAction("StudentList", "Students");
        }
    }
}
