using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.Data.Models.Abstract;
using FaturaTakip.Models;
using FaturaTakip.Utils;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Net;
using WebApplication1.Controllers;
using System.Web;

namespace FaturaTakip.Controllers
{
    public class AuthController : Controller
    {
        private readonly InvoiceTrackContext _context;
        private bool _userLogined = false;

        public AuthController(InvoiceTrackContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            
            if (!Request.Cookies.ContainsKey("Status"))
            {
                return RedirectToAction(nameof(Login));
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            ViewData["NotFound"] = "";
            ViewData["PasswordError"] = "";

            if (!Request.Cookies.ContainsKey("Status"))
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Login([Bind("GovermentId, Password")]UserForLoginDTO user)
        {
            ViewData["NotFound"] = "";
            ViewData["PasswordError"] = "";

            if (ModelState.IsValid)
            {
                User userToLogin;
                userToLogin = _context.Landlords.FirstOrDefault(u => u.GovermentId == user.GovermentId);

                if (userToLogin == null)
                {
                    userToLogin = _context.Tenants.FirstOrDefault(t => t.GovermentId == user.GovermentId);
                    if (userToLogin == null)
                    {
                        ViewData["NotFound"] = "Kullanıcı Bulunamadı.";
                        return View(user);
                    }
                }

                if (!HashingHelper.VerifyPassowrd(user.Password, userToLogin.PasswordHash, userToLogin.PasswordSalt))
                {
                    ViewData["PasswordError"] = "Girdiğiniz şifre geçersiz.";
                    return View(user);
                }

                Response.Cookies.Append("Status", "Logined");
                

                // TODO : claims verilecek
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("Status");
            return RedirectToAction("Index", "Home");
        }
    }
}
