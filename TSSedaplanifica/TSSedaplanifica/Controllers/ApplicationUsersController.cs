﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TSSedaplanifica.Common;
using TSSedaplanifica.Helpers;
using TSSedaplanifica.Models.ApplicationUser;

namespace TSSedaplanifica.Controllers
{
    [Authorize(Roles = "Administrador")]

    public class ApplicationUsersController : Controller
    {
        private readonly IUserHelper _userHelper;

        public ApplicationUsersController(IUserHelper userHelper)
        {
            _userHelper = userHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _userHelper.ListRoleUserAsync());
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RoleUserModelView user = await _userHelper.ByIdRoleUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            ViewData["RoleId"] = new SelectList(await _userHelper.ListRolesAsync(), "Id", "Name", user.RoleId);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserId,FullName,RoleId")] RoleUserModelView model)
        {
            if (id != model.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Response response = await _userHelper.UserRoleAddEditAsync(model);

                ModelState.AddModelError(string.Empty, response.Message);

                if(response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["RoleId"] = new SelectList(await _userHelper.ListRolesAsync(), "Id", "Name", model.RoleId);

            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RoleUserModelView user = await _userHelper.ByIdRoleUserAsync(id);

            return View(user);
        }

        // POST: States/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            Response response = await _userHelper.DeleteUserRoleAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
