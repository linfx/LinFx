//using Identity.Domain.Models;
//using LinFx;
//using LinFx.Security.Authorization.Permissions;
//using Microsoft.AspNetCore.Identity;
//using System.Threading.Tasks;

//namespace Identity.Application
//{
//    public class PermissionStore : IPermissionStore
//    {
//        private readonly RoleManager<Role> _roleManager;
//        private readonly UserManager<User> _userManager;

//        public PermissionStore(
//            RoleManager<Role> roleManager, 
//            UserManager<User> userManager)
//        {
//            _roleManager = roleManager;
//            _userManager = userManager;
//        }

//        public Task<bool> IsGrantedAsync([NotNull] string name, [CanBeNull] string providerName, [CanBeNull] string providerKey)
//        {
//            var result = name == Permissions.User.Index &&
//                         providerName == UserPermissionValueProvider.ProviderName;
//            //&&providerKey == AuthTestController.FakeUserId.ToString();

//            return Task.FromResult(result);
//        }
//    }
//}