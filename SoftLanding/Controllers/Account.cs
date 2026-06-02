using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftLanding.Models;
using SoftLanding.Utilities.Enums;
using SoftLanding.ViewModel;
using System.Threading.Tasks;

namespace SoftLanding.Controllers
{
    public class Account : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public Account(UserManager<AppUser> userManager
            , RoleManager<IdentityRole> roleManager
            , SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            AppUser user = new()
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                UserName = registerVM.Username,
                Email = registerVM.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
                return View();
            }

            await _userManager.AddToRoleAsync(user,UserRole.USer.ToString());

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

            AppUser? user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.UserName == loginVm.UsernameOrEmail || u.Email == loginVm.UsernameOrEmail);

            if (user is null)
            {
                ModelState.AddModelError(String.Empty, "Username,Email or password is incorrect!");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginVm.Password, loginVm.IsPersistent, true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(String.Empty, "Your Account is blocked!");
                return View();
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError(String.Empty, "Username,Email or password is incorrect!");
                return View();
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        //public async Task<IActionResult> CreateRoles()
        //{
        //    foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
        //    }

        //    return RedirectToAction(nameof(HomeController.Index), "Home");
        //}
    }

}
