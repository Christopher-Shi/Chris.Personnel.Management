using System;
using Chris.Personnel.Management.Common;
using Chris.Personnel.Management.Entity;

namespace Chris.Personnel.Management.EF.Storage.BasicData
{
    internal class RoleCreator
    {
        private RoleCreator()
        {

        }

        public static Role[] Create()
        {
            var role = Role.Create("管理员",
                null, 
                new Guid("32ec1e37-fe6d-4606-902e-6705beb0afc0"),
                DateTime.Parse("2020-07-09 23:15:14"));
            role.ForceId(new Guid("32ec1e12-fe6d-4606-902e-6705beb0afc1"));

            return new[]
            {
                role
            };
        }
    }
}
