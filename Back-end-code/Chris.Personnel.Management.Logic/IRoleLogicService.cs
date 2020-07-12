using System.Threading.Tasks;
using Chris.Personnel.Management.UICommand;

namespace Chris.Personnel.Management.LogicService
{
    public interface IRoleLogicService
    {
        Task Add(RoleAddUICommand command);
        Task Edit(RoleEditUICommand command);
        Task Delete(RoleDeleteUICommand command);
    }
}