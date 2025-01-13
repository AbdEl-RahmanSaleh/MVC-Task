using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Task.Models;

namespace Task.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel signUpVM)
        {
            if (ModelState.IsValid) 
            {
                var user = new ApplicationUser
                {
                    Email = signUpVM.Email,
                    UserName = signUpVM.Email.Split('@')[0],
                    FirstName = signUpVM.FirstName,
                    LastName = signUpVM.LastName,
                };

                var result = await _userManager.CreateAsync(user, signUpVM.Password);
                if (result.Succeeded)
                    return RedirectToAction("LogIn");

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

            }
            return View(signUpVM);
        }

        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LogInViewModel logInVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(logInVM.Email);
                if (user == null)
                    ModelState.AddModelError(string.Empty, "Email Does't Exist ,Please Register First and try again");

                var isCorrectPassword = await _userManager.CheckPasswordAsync(user, logInVM.Password);
                
                if (isCorrectPassword)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, logInVM.Password, false,false);
                    if (result.Succeeded)
                        return RedirectToAction("Index", "Home");

                }
                ModelState.AddModelError(string.Empty, "The password that you've entered is incorrect.");
            }
            return View(logInVM);
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(LogIn));
        }

    }

}
