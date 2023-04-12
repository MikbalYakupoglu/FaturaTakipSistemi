using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FaturaTakip.Data;
using FaturaTakip.Business.Concrete;

namespace FaturaTakip.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<InvoiceTrackUser> _userManager;
        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userManager.GetAllUsersAsync();

            return Ok(users);
        }
    }
}
