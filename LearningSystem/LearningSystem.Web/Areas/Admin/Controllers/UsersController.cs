namespace LearningSystem.Web.Areas.Admin.Controllers
{
    using LearningSystem.Data.Models;
    using LearningSystem.Services.Admin;
    using LearningSystem.Web.Areas.Admin.Models.Users;
    using LearningSystem.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UsersController : BaseAdminController
    {
        private readonly IAdminUserService users;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersController(
            IAdminUserService users,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.users = users;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }


        public async Task<IActionResult> Index()
        {
            var users = await this.users.AllAsync();
            var roles = await this.roleManager
                .Roles
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                })
                .ToListAsync();

            return View(new UsersViewModel
            {
                Users = users,
                Roles = roles
            });
        }


        [HttpPost]
        public async Task<IActionResult> ChangeRoleMembership(EditUserRoleFormModel model)
        {
            bool roleExists = await this.roleManager.RoleExistsAsync(model.Role);
            User user = await this.userManager.FindByIdAsync(model.UserId);
            bool userExists = user != null;

            if (!roleExists || !userExists)
            {
                TempData.AddErrorMessage("Invalid Identity Details.");
                ModelState.AddModelError(String.Empty, "Invalid Identity Details.");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            bool userInRoleExist = await this.userManager.IsInRoleAsync(user, model.Role);
            

            if (model.Submit== "Add To Role")
            {
                if (userInRoleExist)
                {
                    TempData.AddWarningMessage($"User {user.UserName} has already been added to the {model.Role} role.");
                    return RedirectToAction(nameof(Index));
                }

                var result = await this.userManager.AddToRoleAsync(user, model.Role);

                if (result.Succeeded)
                {
                    TempData.AddSuccessMessage($"User {user.UserName} successfully added to the {model.Role} role.");
                }
                else
                {
                    TempData.AddErrorMessage($"Adding user {user.UserName} to the {model.Role} role failed.");
                }
            }
            else if (model.Submit == "Remove From Role")
            {
                if (!userInRoleExist)
                {
                    TempData.AddWarningMessage($"User {user.UserName} does not exist in the {model.Role} role.");
                    return RedirectToAction(nameof(Index));
                }

                var result = await this.userManager.RemoveFromRoleAsync(user, model.Role);

                if (result.Succeeded)
                {
                    TempData.AddSuccessMessage($"User {user.UserName} successfully removed from the {model.Role} role.");
                }
                else
                {
                    TempData.AddErrorMessage($"Removing user {user.UserName} from the {model.Role} role failed.");
                }

            }
            else
            {
                TempData.AddErrorMessage("Command is not supported.");
            }

            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> UserRoles(string userId)
        {
            User user = await this.userManager.FindByIdAsync(userId);
            var roles = await this.roleManager.Roles.ToListAsync();

            if (user==null)
            {
                return this.NotFound();
            }

            var userRoles = new List<SelectListItem>();

            foreach (var role in roles)
            {
                bool userInRoleExist = await this.userManager.IsInRoleAsync(user, role.Name);
                if (userInRoleExist)
                {
                    userRoles.Add(new SelectListItem {Text = role.Name,Value = role.Name});
                }
            }
            return this.View(new UserRolesViewModel
            {
                UserId = user.Id,
                Username = user.UserName,
                Roles=userRoles
            });
        }
    }
}