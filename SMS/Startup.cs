using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Linq;
using SMS.Models;

[assembly: OwinStartupAttribute(typeof(SMS.Startup))]
namespace SMS
{
    public partial class Startup
    {

        private Entities db = new Entities();
        private ApplicationDbContext context = new ApplicationDbContext();

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            AddAdminRole();
        }

        private void AddAdminRole()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleName = "admin";
            if (!roleManager.RoleExists(roleName))
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var role = new IdentityRole
                {
                    Name = roleName
                };
                var result = roleManager.Create(role);
                var defaultUserName = "admin@sms.com";
                var defaultPassword = "updating";
                if (result.Succeeded)
                {
                    var user = new ApplicationUser
                    {
                        UserName = defaultUserName
                    };
                    result = userManager.Create(user, defaultPassword);
                    if (result.Succeeded)
                    {
                        result = userManager.AddToRole(user.Id, roleName);
                        if (!result.Succeeded)
                        {
                            throw new System.Exception("Could not add admin user to admin role");
                        }
                    }
                    else
                    {
                        throw new System.Exception("Could not create admin user.");
                    }
                }
                else
                {
                    throw new System.Exception("Could not create admin role");
                }
            }
        }
    }
}
