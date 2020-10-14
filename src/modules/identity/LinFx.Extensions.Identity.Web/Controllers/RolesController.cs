using LinFx.Extensions.Identity.Authorization;
using LinFx.Extensions.Identity.Models;
using LinFx.Security.Authorization.Permissions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Web.Controllers
{
    /// <summary>
    /// 角色管理
    /// </summary>
    [Authorize]
    public class RolesController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IPermissionDefinitionManager _permissionDefinitionManager;

        public RolesController(
            RoleManager<Role> roleManager,
            UserManager<User> userManager,
            IPermissionDefinitionManager permissionDefinitionManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _permissionDefinitionManager = permissionDefinitionManager;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var items = await _roleManager.Roles
                .ToListAsync();

            return View(items);
        }

        /// <summary>
        /// 明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Details(int id)
        {
            return View();
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        ///// <summary>
        ///// 创建
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(RoleViewModel input)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var role = new IdentityRole();

        //            if (await TryUpdateModelAsync(role))
        //            {

        //                var result = await _roleManager.CreateAsync(role);
        //                if (result.Succeeded)
        //                {
        //                    return RedirectToAction(nameof(Index));
        //                }
        //                foreach (var error in result.Errors)
        //                {
        //                    ModelState.AddModelError(string.Empty, error.Description);
        //                }
        //            }
        //        }

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var model = new ApplicationRoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
                ConcurrencyStamp = role.ConcurrencyStamp,
            };
            return View(model);
        }

        ///// <summary>
        ///// 修改
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, ApplicationRoleViewModel input)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var roleToUpdate = await _roleManager.FindByIdAsync(id);

        //        if (await TryUpdateModelAsync(roleToUpdate))
        //        {
        //            await _roleManager.SetRoleNameAsync(roleToUpdate, input.Name);
        //            var result = await _roleManager.UpdateAsync(roleToUpdate);
        //            if (result.Succeeded)
        //            {
        //                return RedirectToAction(nameof(Index));
        //            }
        //            foreach (var error in result.Errors)
        //            {
        //                ModelState.AddModelError(string.Empty, error.Description);
        //            }
        //        }
        //    }
        //    return View();
        //}

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Role/Permissions/5
        public async Task<IActionResult> Permissions(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var claims = await _roleManager.GetClaimsAsync(role);
            var groups = _permissionDefinitionManager.GetGroups();

            var model = new PermissionsViewModel
            {
                Groups = groups.Select(p1 =>
                {
                    var claim1 = new Claim(p1.Name, p1.Name);
                    return new PermissionGroupViewModel
                    {
                        Name = p1.Name,
                        DisplayName = p1.DisplayName,
                        IsGranted = claims.Any(m => m.Type == claim1.Type && m.Value == claim1.Value),
                        Permissions = p1.Permissions.Select(p2 =>
                        {
                            var claim2 = new Claim(p1.Name, p2.Name);
                            return new PermissionGrantViewModel
                            {
                                Name = p2.Name,
                                DisplayName = p2.DisplayName,
                                IsGranted = claims.Any(m => m.Type == claim2.Type && m.Value == claim2.Value),
                                Children = p2.Children.Select(p3 =>
                                {
                                    var claim3 = new Claim(p1.Name, p3.Name);
                                    return new PermissionGrantViewModel
                                    {
                                        Name = p3.Name,
                                        DisplayName = p3.DisplayName,
                                        IsGranted = claims.Any(m => m.Type == claim3.Type && m.Value == claim3.Value),
                                    };
                                }).ToList()
                            };
                        }).ToList()
                    };
                }).ToList()
            };
            return View(model);
        }

        // POST: Role/Permissions/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Permissions(string id, PermissionsViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            var claims = await _roleManager.GetClaimsAsync(role);

            foreach (var p1 in model.Groups)
            {
                var claim1 = new Claim(p1.Name, p1.Name);
                await UpdatePermissionAsync(claim1, p1.IsGranted);

                foreach (var p2 in p1.Permissions)
                {
                    var claim2 = new Claim(p1.Name, p2.Name);
                    await UpdatePermissionAsync(claim2, p2.IsGranted);

                    foreach (var p3 in p2.Children)
                    {
                        var claim3 = new Claim(p1.Name, p3.Name);
                        await UpdatePermissionAsync(claim3, p3.IsGranted);
                    }
                }
            }

            async Task UpdatePermissionAsync(Claim claim, bool isGranted)
            {
                if (isGranted)
                {
                    if (!claims.Any(m => m.Type == claim.Type && m.Value == claim.Value))
                        await _roleManager.AddClaimAsync(role, claim);
                }
                else
                    await _roleManager.RemoveClaimAsync(role, claim);
            }

            return RedirectToAction("Index");
        }
    }
}