using JWT_Identity_Secrets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JWT_Identity_Secrets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly DevDbContext _devDbContext;

        public EmployeeController(DevDbContext devDbContext)
        {
            _devDbContext = devDbContext;
        }

        [HttpGet]
        [Route("Employees")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _devDbContext.Employees.ToListAsync();
            return Ok(employees);
        }

        [HttpGet]
        [Route("AdminDashboard")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdminDashBoard()
        {
            return Ok("This is Admin Dashboard");
        }

        [HttpGet]
        [Route("UserDashBoard")]
        [Authorize(Roles = "User")]
        public IActionResult GetUserDashBoard()
        {
            return Ok("This is User Dashboard");
        }

        [HttpGet]
        [Route("AuthorizedDashBoard")]
        [Authorize]
        public IActionResult GetAuthorizedDashBoard()
        {
            return Ok("This is Authorized Dashboard");
        }

        [HttpGet]
        [Route("AnyDashBoard")]
        [Authorize(Roles = "Admin, User")]
        public IActionResult GetAnyDashBoard()
        {
            return Ok("This is Any Dashboard");
        }
    }
}
