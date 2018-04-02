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
            AddDummyParent();
            AddDummyClasses();
            AddAttendanceStatus();
            AddDummySubjects();
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

        private void AddDummyClasses()
        {
            var classesInDbCount = db.Classes.Count();
            if (classesInDbCount == 0)
            {
                var classes = new List<Class>
                {
                    new Class
                    {
                       Title="Nursery",
                       Fee=1000,
                    },
                    new Class
                    {
                        Title = "1",
                        Fee=1000,
                    },
                    new Class
                    {
                        Title = "2",
                        Fee=1000,
                    },
                    new Class
                    {
                        Title = "3",
                        Fee=1000,
                    }
                };
                foreach (var tempClass in classes)
                {
                    db.Classes.Add(tempClass);
                }
                db.SaveChanges();
            }
        }

        private void AddDummyParent()
        {
            var parentsInDbCount = db.Parents.Count();
            if (parentsInDbCount == 0)
            {

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var defaultUserName = "parent@sms.com";
                var defaultPassword = "updating";
                var user = new ApplicationUser
                {
                    UserName = defaultUserName
                };
                var result = userManager.Create(user, defaultPassword);
                if (!result.Succeeded)
                {
                    throw new Exception("Cannot create a dummy user for dummy parent ");
                }
                var parent = new Parent
                {
                    Name = "Dummy Parent",
                    CNIC = "1234",
                    Contact = "+9241",
                    UserId = user.Id,
                };
                db.Parents.Add(parent);
                db.SaveChanges();
            }
        }


        private void AddAttendanceStatus()
        {
            var attendanceStatusCount = db.AttendanceStatus.Count();
            if (attendanceStatusCount == 0)
            {
                var attendanceStatusList = new List<AttendanceStatu>
                {
                    new AttendanceStatu
                    {
                        Title = "P"
                    },
                    new AttendanceStatu
                    {
                        Title = "A"
                    },
                    new AttendanceStatu
                    {
                        Title = "L"
                    }
                };
                foreach (var attendanceStatus in attendanceStatusList)
                {
                    db.AttendanceStatus.Add(attendanceStatus);
                }
                db.SaveChanges();
            }
        }
        private void AddDummySubjects()
        {
            var subjectsInDbCount = db.Subjects.Count();
            if (subjectsInDbCount == 0)
            {
                var subjects = new List<Subject>
                {
                    new Subject
                    {
                       Title="English",
                    },
                    new Subject
                    {
                        Title = "Urdu",
                        
                    },
                    new Subject
                    {
                        Title = "Mathematics",
                        
                    },
                    new Subject
                    {
                        Title = "Islamiyat",
                        
                    }
                };
                foreach (var tempSubject in subjects)
                {
                    db.Subjects.Add(tempSubject);
                }
                db.SaveChanges();
            }
        }


    }
}
