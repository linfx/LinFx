//using LinFx.Extensions.Identity.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Identity.Web.Controllers
//{
//    /// <summary>
//    /// 用户管理
//    /// </summary>
//    [Authorize]
//    public class UsersController : Controller
//    {
//        private readonly RoleManager<IdentityRole> _roleManager;
//        private readonly UserManager<IdentityUser> _userManager;

//        public UsersController(
//            RoleManager<IdentityRole> roleManager,
//            UserManager<IdentityUser> userManager)
//        {
//            _roleManager = roleManager;
//            _userManager = userManager;
//        }

//        /// <summary>
//        /// 列表
//        /// </summary>
//        /// <param name="page"></param>
//        /// <param name="limit"></param>
//        /// <returns></returns>
//        public async Task<IActionResult> Index(int page = 1, int limit = 10)
//        {
//            var count = await _userManager.Users.LongCountAsync();

//            var items = await _userManager.Users
//                .PageBy(page, limit)
//                .ToListAsync();

//            var result = new PagedResult<IdentityUser>(count, items);

//            return View(result);
//        }

//        // GET: User/Details/5
//        public async Task<ActionResult> Details(string id)
//        {
//            var user = await _userManager.FindByIdAsync(id);
//            return View(user);
//        }

//        // GET: User/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: User/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create(IFormCollection collection)
//        {
//            try
//            {
//                // TODO: Add insert logic here

//                return RedirectToAction("Index");
//            }
//            catch
//            {
//                return View();
//            }
//        }

//        // GET: User/Edit/5
//        public async Task<IActionResult> Edit(string id)
//        {
//            var user = await _userManager.FindByIdAsync(id);
//            var roles = _roleManager.Roles;

//            var model = new UserEditModel
//            {
//                User = new UserViewModel
//                {
//                    Id = user.Id,
//                    ConcurrencyStamp = user.ConcurrencyStamp,
//                    Name = user.UserName,
//                    Email = user.Email,
//                    PhoneNumber = user.PhoneNumber,
//                }
//            };

//            var userRoleNames = await _userManager.GetRolesAsync(user);

//            model.Roles = roles.Select(p => new AssignedRoleViewModel
//            {
//                Name = p.Name,
//                IsAssigned = userRoleNames.Any(x => x == p.Name)
//            }).ToArray();

//            return View(model);
//        }

//        // POST: User/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(string id, UserEditModel input)
//        {
//            if (ModelState.IsValid)
//            {
//                var user = await _userManager.FindByIdAsync(id);

//                //TryUpdateModelAsync(user);
//                user.PhoneNumber = input.User.PhoneNumber;

//                await _userManager.UpdateAsync(user);

//                foreach (var role in input.Roles)
//                {
//                    if (role.IsAssigned)
//                        await _userManager.AddToRoleAsync(user, role.Name);
//                    else
//                        await _userManager.RemoveFromRoleAsync(user, role.Name);
//                }

//                return RedirectToAction("Index");
//            }
//            return View();
//        }

//        // GET: User/Delete/5
//        public async Task<IActionResult> Delete(string id)
//        {
//            var user = await _userManager.FindByIdAsync(id);
//            if (user != null)
//            {
//                var result = await _userManager.DeleteAsync(user);
//            }
//            return RedirectToAction(nameof(Index));
//        }
//    }
//}