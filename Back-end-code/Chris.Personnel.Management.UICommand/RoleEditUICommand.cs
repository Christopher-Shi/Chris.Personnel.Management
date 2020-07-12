using System;
using Chris.Personnel.Management.UICommand.DTO;

namespace Chris.Personnel.Management.UICommand
{
    public class RoleEditUICommand
    {
        public Guid Id { get; set; }
        public RoleDTO Role { get; set; }
    }
}
