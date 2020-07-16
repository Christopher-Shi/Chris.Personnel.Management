using System;
using Chris.Personnel.Management.Common.Enums;
using Chris.Personnel.Management.Common.Extensions;
using Chris.Personnel.Management.Entity;

namespace Chris.Personnel.Management.EF.Storage.BasicData
{
    internal class UserCreator
    {
        private UserCreator()
        {

        }

        public static User[] Create()
        {
            var user = User.Create(
                "Admin",
                "admin123",
                "Admin",
                Gender.Male,
                "140226199401294051",
                "13259769759",
                new Guid("32ec1e12-fe6d-4606-902e-6705beb0afc1"),
                null,
                DateTime.Parse("2020-07-09 23:15:14"));
            user.ForceId(new Guid("32ec1e37-fe6d-4606-902e-6705beb0afc0"));

            return new[]
            {
                user
            };
        }
    }
}
