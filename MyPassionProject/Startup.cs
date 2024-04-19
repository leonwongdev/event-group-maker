using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using MyPassionProject.Models;
using Owin;
using System.Diagnostics;

[assembly: OwinStartupAttribute(typeof(MyPassionProject.Startup))]
namespace MyPassionProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        // In this method we will create default User roles and Admin user for login
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // In Startup iam creating first Admin Role and creating a default Admin User
            Debug.WriteLine("createRolesandUsers");
            Debug.WriteLine(!roleManager.RoleExists("Admin"));
            if (!roleManager.RoleExists("Admin"))
            {

                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

            }
        }
    }
}
