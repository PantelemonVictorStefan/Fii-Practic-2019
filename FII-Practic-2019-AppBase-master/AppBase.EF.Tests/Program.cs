using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace AppBase.EF.Tests
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new AppBaseEntities())
            {
                #region Deleted everything
                using (var cmd = ctx.Database.Connection.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM [dbo].[UserInRoles] WHERE 1 = 1;
                        DELETE FROM [dbo].[Rights] WHERE 1 = 1;
                        DELETE FROM [dbo].[Roles] WHERE 1 = 1;
                        DELETE FROM [dbo].[Users] WHERE 1 = 1;
                        DELETE FROM [dbo].[Functions] WHERE 1 = 1;
                        ";
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Everything deleted");
                }
                #endregion

                #region Create roles
                var roleNames = new string[] { "Admin", "User" };
                var roles = new List<Role>();
                foreach (var name in roleNames)
                {
                    var role = new Role()
                    {
                        RoleName = name
                    };

                    #region Create functions and rights
                    for (var i = 0; i < 10; i++)
                        role.Rights.Add(new Right() { Function = new Function() { FunctionName = name + "Function" + (i + 1) } });
                    #endregion

                    Console.WriteLine("Creating role \"" + name + "\"");

                    roles.Add(ctx.Roles.Add(role));
                }
                #endregion

                #region Create users
                for (var i = 0; i < 90; i++)
                {
                    var user = new User()
                    {
                        UserName = "user_" + (i + 1),
                        Email = "user_" + (i + 1) + "@email.com",
                        FirstName = (i + 1) + " first name",
                        LastName = (i + 1) + " last name",
                        BirthDate = DateTime.Now
                    };
                    user.Roles.Add(roles[i % roles.Count]);

                    Console.WriteLine("Creating user \"" + user.UserName + "\"");
                    ctx.Users.Add(user);
                }
                #endregion

                ctx.SaveChanges();

                Console.WriteLine("Getting TOP 100 users");
                var usersQuery = (DbQuery<User>)ctx.Users
                    .AsNoTracking()
                    .OrderBy(user => user.UserName)
                    .Skip(0)
                    .Take(100);

                var users = usersQuery.ToList();

                Console.WriteLine("Generated SQL is:\n" +
                    usersQuery.Sql + "\n\n");

                foreach (var user in usersQuery)
                    Console.WriteLine("UserName: {0}, Email: {1}",
                        user.UserName, user.Email);
            }

            Console.ReadKey();
        }
    }
}
