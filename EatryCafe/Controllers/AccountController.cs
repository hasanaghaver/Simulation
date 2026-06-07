using EatryCafe.Models;
using EatryCafe.Utilities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EatryCafe.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register register)
        {
            if (!ModelState.IsValid) return View(register);

            AppUser user = new()
            {
                Name = register.Name,
                Surname = register.Surname,
                UserName = register.Username,
                Email = register.Email
            };

            IdentityResult result = await userManager.CreateAsync(user, register.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                    return View(register);
                }
            }

            await userManager.AddToRoleAsync(user, UserRole.User.ToString());

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            if (!ModelState.IsValid) return View(login);

            AppUser? user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == login.UsernameOrEmail || u.Email == login.UsernameOrEmail);

            if (user is null)
            {
                ModelState.AddModelError(String.Empty, "Username,Email or password is incorrect");
                return View(login);
            }

            var result = await signInManager.PasswordSignInAsync(user, login.Password, login.IsPersistent, true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(String.Empty, "Your account is blocked!");
                return View(login);
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError(String.Empty, "Username,Email or password is incorrect");
                return View(login);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> CreateRole()
        {
            foreach (var role in Enum.GetNames(typeof(UserRole)))
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = role });
                }
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
