using AppBase.ORM.Entities;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace AppBase.ORM.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var conn = new SqlConnection(CommonHelpers.GetConnectionString()))
            {
                conn.Open();

                #region Deleted everything
                using (var cmd = conn.CreateCommand())
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

                    var repo = role.CreateRepository(conn);
                    repo.InsertOrUpdate(role);

                    roles.Add(role);
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
                    var repo = user.CreateRepository(conn);
                    repo.InsertOrUpdate(user);
                }
                #endregion
            }

            Console.ReadKey();
        }
    }
}
