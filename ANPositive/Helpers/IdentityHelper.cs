using ANPositive.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace ANPositive.Helpers
{
    public class IdentityHelper
    {
        internal static void SeedIdentities(DbContext context)
        {
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(store: new UserStore<ApplicationUser>(context));
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(store: new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists(RoleNames.roleAdministrator))
            {
                IdentityResult roleResult = roleManager.Create(new IdentityRole(RoleNames.roleAdministrator));
            }

            if (!roleManager.RoleExists(RoleNames.roleEditor))
            {
                IdentityResult roleResult = roleManager.Create(new IdentityRole(RoleNames.roleEditor));
            }

            if (!roleManager.RoleExists(RoleNames.roleVisitor))
            {
                IdentityResult roleResult = roleManager.Create(new IdentityRole(RoleNames.roleVisitor));
            }

            string userName = "evren@ajansnaber.com";
            string password = "M@ster.01";

            ApplicationUser user = userManager.FindByName(userName);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = userName,
                    Email = userName,
                    EmailConfirmed = true,
                    TwoFactorEnabled = true,
                    firstName = "AN",
                    lastName = "Positive",
                    title = "System Administrator"
                };

                IdentityResult userResult = userManager.Create(user, password);
                if (userResult.Succeeded)
                {
                    IdentityResult result = userManager.AddToRole(user.Id, RoleNames.roleAdministrator);
                }
            }
        }
    }
}