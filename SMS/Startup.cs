using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Linq;
using SMS.Models;
using System;
using System.Collections.Generic;

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
            AddParentRole();
            AddAccountantRole();
            AddDesignations();
            AddAttendanceStatus();
        }
        private void AddAccountantRole()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleName = "accountant";
            if (!roleManager.RoleExists(roleName))
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var role = new IdentityRole
                {
                    Name = roleName
                };
                var result = roleManager.Create(role);
                var defaultUserName = "accountant@sms.com";
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
                            throw new System.Exception("Could not add accountant user to admin role");
                        }
                    }
                    else
                    {
                        throw new System.Exception("Could not create accountant user.");
                    }
                }
                else
                {
                    throw new System.Exception("Could not create accountant role");
                }
            }
        }

        private void AddParentRole()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleName = "parent";
            if (!roleManager.RoleExists(roleName))
            {
                var role = new IdentityRole
                {
                    Name = roleName
                };
                var result = roleManager.Create(role);
                if (!result.Succeeded)
                {
                    throw new Exception("Could not create Parent role");
                }
            }
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

        private void AddDesignations()
        {
            var designationsInDbCount = db.Designations.Count();
            if(designationsInDbCount == 0)
            {
                var designations = new List<Designation>
                {
                    new Designation
                    {
                        Title = "Teacher"
                    },
                    new Designation
                    {
                        Title = "Principal"
                    },
                    new Designation
                    {
                        Title = "Clerk"
                    },
                    new Designation
                    {
                        Title = "Security Gaurd"
                    }
                };
                foreach (var designation in designations)
                {
                    db.Designations.Add(designation);
                }
                db.SaveChanges();
            }
        }
    }
}
