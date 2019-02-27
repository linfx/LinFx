//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Microsoft.AspNetCore.Identity
//{
//    public static class IdentityResultExtensions
//    {
//        public static string GetResultAsString(this SignInResult signInResult)
//        {
//            if (signInResult.Succeeded)
//            {
//                return "Succeeded";
//            }

//            if (signInResult.IsLockedOut)
//            {
//                return "IsLockedOut";
//            }

//            if (signInResult.IsNotAllowed)
//            {
//                return "IsNotAllowed";
//            }

//            if (signInResult.RequiresTwoFactor)
//            {
//                return "RequiresTwoFactor";
//            }

//            return "Unknown";
//        }
//    }
//}
