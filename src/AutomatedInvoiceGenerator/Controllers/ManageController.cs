using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutomatedInvoiceGenerator.Models;
using AutomatedInvoiceGenerator.Models.ManageViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using AutomatedInvoiceGenerator.Filters;

namespace AutomatedInvoiceGenerator.Controllers
{
    [AssemblyVersionFilter]
    [Authorize]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;

        public ManageController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = loggerFactory.CreateLogger<ManageController>();
        }

        // GET: /Manage/Index
        [HttpGet]
        public async Task<IActionResult> Index(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.ChangePasswordSuccess ? "Twoje hasło zostało zmienione."
                : message == ManageMessageId.AddUserSuccess ? "Uzytkownik został dodany."
                : message == ManageMessageId.RemoveUserSuccess ? "Uzytkownik został usunięty."
                : message == ManageMessageId.ChangeUserPasswordSuccess ? "Hasło użytkownika zostało zmienione."
                : message == ManageMessageId.ChangeUserRolesSuccess ? "Uprawnienia użytkownika zostały zmienione."
                : message == ManageMessageId.LockUnlockUser ? "Blokdad użytkownika została zmieniona."
                : message == ManageMessageId.Error ? "Wystapił błąd."
                : "";

            var user = await GetCurrentUserAsync();

            if (user == null)
            {
                return View("Error");
            }

            List<UserViewModel> userViewModels = new List<UserViewModel>();

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                userViewModels = await _userManager.Users
                    .Include(u => u.Roles)
                    .Select(g => new UserViewModel
                    {
                        UserName = g.UserName,
                        Email = g.Email
                    })
                    .OrderBy(o =>o.Email)
                    .ToListAsync();


                foreach (var u in userViewModels)
                {
                    u.Roles = await _userManager.GetRolesAsync(await _userManager.FindByNameAsync(u.UserName));
                }
            }

            var indexViewModel = new IndexViewModel
            {
                Roles = _roleManager.Roles.Select(r => r.Name),
                ApplicationUsers = userViewModels
            };

            return View(indexViewModel);
        }

        // GET: /Manage/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                IdentityResult resultChangePassword = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                if (resultChangePassword.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "Użytkownik zmienił swoje hasło.");
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePasswordSuccess });
                }

                AddErrors(resultChangePassword);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }


        // GET: /Manage/AddUser
        [Authorize(Roles = "Admin")]
        [HttpGet("Manage/AddUser")]
        public async Task<IActionResult> AddUser()
        {
            List<SelectListItem> roles = new List<SelectListItem>();

            foreach (var role in await _roleManager.Roles.ToListAsync())
            {
                roles.Add(new SelectListItem()
                {
                    Text = role.Name,
                    Value = role.Name,
                });
            }

            var model = new AddUserViewModel()
            {
                Roles = roles
            };
            return View(model);
        }

        // POST: /Manage/AddUser
        [Authorize(Roles = "Admin")]
        [HttpPost("Manage/AddUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(AddUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                IdentityResult resultAddToRole = new IdentityResult();

                var newUser = new ApplicationUser { UserName = model.Email, Email = model.Email };
                IdentityResult resultAddUser = await _userManager.CreateAsync(newUser, model.Password);

                foreach (var role in model.UserRoles)
                {
                    if (await _roleManager.RoleExistsAsync(role))
                    {
                        resultAddToRole = await _userManager.AddToRoleAsync(newUser, role);
                    }
                }

                if (resultAddUser.Succeeded && resultAddToRole.Succeeded)
                {
                    _logger.LogInformation(3, "Użytkownik został dodany.");
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.AddUserSuccess });
                }
                AddErrors(resultAddUser);
                AddErrors(resultAddToRole);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }
        

        // GET: /Manage/ChangeUserPassword/{email}
        [Authorize(Roles = "Admin")]
        [HttpGet("Manage/ChangeUserPassword/{email}")]
        public async Task<IActionResult> ChangeUserPassword(string email)
        {
            if (await _userManager.FindByEmailAsync(email) == null)
            {
                return NotFound();
            }

            var model = new ChangeUserPasswordViewModel()
            {
                Email = email
            };

            return View(model);
        }

        // POST: /Manage/ChangeUserPassword/{email}
        [Authorize(Roles = "Admin")]
        [HttpPost("Manage/ChangeUserPassword/{email}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserPassword(ChangeUserPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                IdentityResult resultRemovePassoword = await _userManager.RemovePasswordAsync(user);
                IdentityResult resultAddPassword = await _userManager.AddPasswordAsync(user, model.NewPassword);

                if (resultRemovePassoword.Succeeded && resultAddPassword.Succeeded)
                {
                    _logger.LogInformation(3, "Hasło użytkownika zostało zmienione.");
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangeUserPasswordSuccess });
                }

                AddErrors(resultRemovePassoword);
                AddErrors(resultAddPassword);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }


        // GET: /Manage/ChangeUserRoles/{email}
        [Authorize(Roles = "Admin")]
        [HttpGet("Manage/ChangeUserRoles/{email}")]
        public async Task<IActionResult> ChangeUserRoles(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }

            List<SelectListItem> userRoles = new List<SelectListItem>();

            foreach (var role in await _roleManager.Roles.ToListAsync())
            {
                userRoles.Add(new SelectListItem()
                {
                    Text = role.Name,
                    Value = role.Name,
                    Selected = await _userManager.IsInRoleAsync(user, role.Name)
                });
            }

            var model = new ChangeUserRolesViewModel()
            {
                Email = email,
                Roles = userRoles
            };

            return View(model);
        }

        // POST: /Manage/ChangeUserRoles/{email}
        [Authorize(Roles = "Admin")]
        [HttpPost("Manage/ChangeUserRoles/{email}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserRoles(ChangeUserRolesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && model.UserRoles != null)
            {
                IdentityResult resultAddToRole = new IdentityResult();
                IdentityResult resultRemoveFromRoles = await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));

                foreach (var role in model.UserRoles)
                {
                    if (await _roleManager.RoleExistsAsync(role))
                    {
                        resultAddToRole = await _userManager.AddToRoleAsync(user, role);
                    }
                }

                if (resultRemoveFromRoles.Succeeded && resultAddToRole.Succeeded)
                {
                    _logger.LogInformation(3, "Uprawnienia użytkownika zostały zmienione.");
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangeUserRolesSuccess });
                }

                AddErrors(resultRemoveFromRoles);
                AddErrors(resultAddToRole);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }


        // GET: /Manage/LockUnlockUser/{email}
        [Authorize(Roles = "Admin")]
        [HttpGet("Manage/LockUnlockUser/{email}")]
        public async Task<IActionResult> LockUnlockUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }

            var model = new LockUnlockUserViewModel()
            {
                Email = email,
                IsLocked = await _userManager.IsLockedOutAsync(user)
            };

            return View(model);
        }

        // POST: /Manage/LockUnlockUser/{email}
        [Authorize(Roles = "Admin")]
        [HttpPost("Manage/LockUnlockUser/{email}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockUnlockUser(LockUnlockUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                if (model.IsLocked)
                {
                    IdentityResult resultSetUserLockoutEndDate = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);

                    if (resultSetUserLockoutEndDate.Succeeded)
                    {
                        _logger.LogInformation(3, "Blokada użytkownika została zmieniona.");
                        return RedirectToAction(nameof(Index), new { Message = ManageMessageId.LockUnlockUser });
                    }
                    AddErrors(resultSetUserLockoutEndDate);
                }
                else
                {
                    IdentityResult resultSetUserLockoutEndDate = await _userManager.SetLockoutEndDateAsync(user, null);
                    IdentityResult resultResetUserAccessFailedCount = await _userManager.ResetAccessFailedCountAsync(user);

                    if (resultSetUserLockoutEndDate.Succeeded && resultResetUserAccessFailedCount.Succeeded)
                    {
                        _logger.LogInformation(3, "Blokada użytkownika została zmieniona.");
                        return RedirectToAction(nameof(Index), new { Message = ManageMessageId.LockUnlockUser });
                    }
                    AddErrors(resultSetUserLockoutEndDate);
                    AddErrors(resultResetUserAccessFailedCount);
                }
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }


        // GET: /Manage/RemoveUser/{email}
        [Authorize(Roles = "Admin")]
        [HttpGet("Manage/RemoveUser/{email}")]
        public async Task<IActionResult> RemoveUser(string email)
        {
            if (await _userManager.FindByEmailAsync(email) == null)
            {
                return NotFound();
            }

            var model = new RemoveUserViewModel()
            {
                Email = email
            };

            return View(model);
        }

        // POST: /Manage/RemoveUser/{email}
        [Authorize(Roles = "Admin")]
        [HttpPost("Manage/RemoveUser/{email}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveUser(RemoveUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentUser = await GetCurrentUserAsync();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && user != currentUser)
            {
                IdentityResult resultRemoveUser = await _userManager.DeleteAsync(user);

                if (resultRemoveUser.Succeeded)
                {
                    _logger.LogInformation(3, "Użytkownik został usunięty.");
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.RemoveUserSuccess });
                }
                AddErrors(resultRemoveUser);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }


        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            AddUserSuccess,
            ChangeUserPasswordSuccess,
            ChangeUserRolesSuccess,
            LockUnlockUser,
            RemoveUserSuccess,
            Error
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion
    }
}
