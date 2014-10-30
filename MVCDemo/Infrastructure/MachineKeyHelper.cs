using System;
using System.Security.Principal;
using SWS = System.Web.Security;
using SSC = System.Security.Claims;

namespace MVCDemo.Infrastructure
{
    /// <summary>
    /// See http://brockallen.com/2012/06/21/use-the-machinekey-api-to-protect-values-in-asp-net/
    /// </summary>
    public class MachineKeyHelper
    {
        internal static string GetMachineKeyPurpose(IPrincipal user, string appName)
        {
            SSC.ClaimsPrincipal cp = user as SSC.ClaimsPrincipal;
            if (cp == null)
                return null;
            string nameIdentifier = cp.GetNameIdentiferValue();
            if (nameIdentifier == null)
                return null;
            return string.Format("{0}:NameIdentifier:{1}", appName, nameIdentifier);
        }

        internal static string Protect(byte[] data, IPrincipal user, string appName)
        {
            string appPurpose = GetMachineKeyPurpose(user, appName);
            if (string.IsNullOrWhiteSpace(appPurpose))
            {
                string username = "unknown";
                if ((user != null) && (user.Identity != null) && user.Identity.IsAuthenticated)
                    username = user.Identity.Name;
                throw new ArgumentException(string.Format("MachineKeyHelper.Protect could not get purpose for user \"{0}\"", username), "user");
            }
            var value = SWS.MachineKey.Protect(data, appPurpose);
            return Convert.ToBase64String(value);
        }

        internal static string Protect1252(string dataStr, IPrincipal user, string appName)
        {
            return Protect(System.Text.Encoding.GetEncoding(1252).GetBytes(dataStr), user, appName);
        }

        internal static byte[] Unprotect(string value, IPrincipal user, string appName)
        {
            string appPurpose = GetMachineKeyPurpose(user, appName);
            if (string.IsNullOrWhiteSpace(appPurpose))
            {
                string username = "unknown";
                if ((user != null) && (user.Identity != null) && user.Identity.IsAuthenticated)
                    username = user.Identity.Name;
                throw new ArgumentException(string.Format("MachineKeyHelper.Protect could not get purpose for user \"{0}\"", username), "user");
            }
            var bytes = Convert.FromBase64String(value);
            return SWS.MachineKey.Unprotect(bytes, appPurpose);
        }

        internal static string UnprotectTo1252String(string value, IPrincipal user, string appName)
        {
            byte[] bytes = Unprotect(value, user, appName);
            return System.Text.Encoding.GetEncoding(1252).GetString(bytes);
        }
    }
}