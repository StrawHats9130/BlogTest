namespace BlogTest.Migrations
{
    using BlogTest.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BlogTest.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BlogTest.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var roleManger = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManger.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                roleManger.Create(new IdentityRole { Name = "Moderator" });
            }

            #region Add User creation and role assignment
            var userStore = new UserStore<ApplicationUser>(context);
            var userManger = new UserManager<ApplicationUser>(userStore);

            if (!context.Users.Any(u => u.Email == "yaremab@icloud.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "yaremab@icloud.com",
                    Email = "yaremab@icloud.com",
                    FirstName = "Benjamin",
                    LastName ="Yarema",
                    DisplayName = "BenLee"

                };
                //Creates the user in the DB
                userManger.Create(user, "Abc&123!");

                //here we attach the admin role to this user
                userManger.AddToRoles(user.Id, "Admin");
            }

            if (!context.Users.Any(u => u.Email == "JasonTwichell@coderfoundry.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "JasonTwichell@coderfoundry.com",
                    Email = "JasonTwichell@coderfoundry.com",
                    FirstName = "Jason",
                    LastName = "Twichell",
                    DisplayName = "Prof"

                };
                //Creates the user in the DB
                userManger.Create(user, "Abc&123!");

                //here we attach the admin role to this user
                userManger.AddToRoles(user.Id, "Moderator");
            }

            if (!context.Users.Any(u => u.Email == "arussell@coderfoundry.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "arussell@coderfoundry.com",
                    Email = "arussell@coderfoundry.com",
                    FirstName = "Andrew",
                    LastName = "Russell",
                    DisplayName = "Prof's SideKick"

                };
                //Creates the user in the DB
                userManger.Create(user, "Abc&123!");

                //here we attach the admin role to this user
                userManger.AddToRoles(user.Id, "Moderator");
            }

            #endregion

            #region 





            #endregion

        }
    }
}
