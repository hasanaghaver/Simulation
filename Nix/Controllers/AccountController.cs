using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nix.Models;
using Nix.Utilities.Enums;
using Nix.ViewModels;
using System.Threading.Tasks;

namespace Nix.Controllers
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
        public async Task<IActionResult> Register(RegisterVm registerVm)
        {
            if (!ModelState.IsValid) return View(registerVm);

            AppUser user = new()
            {
                Name = registerVm.Name,
                Surname = registerVm.Surname,
                UserName = registerVm.Username,
                Email = registerVm.Email
            };

            IdentityResult result = await userManager.CreateAsync(user, registerVm.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);

                }
                return View();
            }

            await userManager.AddToRoleAsync(user, UserRole.User.ToString());

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm)
        {
            if (!ModelState.IsValid) return View(loginVm);

            AppUser? user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginVm.UsernameOrEmail || u.Email == loginVm.UsernameOrEmail);

            if (user is null)
            {
                ModelState.AddModelError(String.Empty, "Username,Email or Password is incorrect!");
                return View();
            }

            var result = await signInManager.PasswordSignInAsync(user, loginVm.Password, loginVm.IsPersistent, true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(String.Empty, "Your Account is blocked!");
                return View();
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError(String.Empty, "Username,Email or Password is incorrect!");
                return View();
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> CreateRoles()
        {
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
