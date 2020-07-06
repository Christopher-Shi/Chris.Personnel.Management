using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Chris.Personnel.Management.LogicService;
using Chris.Personnel.Management.QueryService;
using Chris.Personnel.Management.ViewModel;
using System.Collections.Generic;
using Chris.Personnel.Management.Common.Enums;
using Chris.Personnel.Management.UICommand;
using Chris.Personnel.Management.ViewModel.Filters;

namespace Chris.Personnel.Management.API.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserLogicService _userLogicService;
        private readonly IUserQueryService _userQueryService;

        public UsersController(
            IUserLogicService userLogicService,
            IUserQueryService userQueryService)
        {
            _userLogicService = userLogicService;
            _userQueryService = userQueryService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IEnumerable<UserFormViewModel>> GetUsers()
        {
            return await _userQueryService.GetAll();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<UserFormViewModel> GetUser(Guid id)
        {
            return await _userQueryService.Get(id);
        }

        [Route("pagination")]
        [HttpGet]
        public async Task<UserPaginationViewModel> GetByPage(
            string trueName, Gender? gender, IsEnabled? isEnabled,
            int current, int pageSize, string orderByPropertyName, bool isAsc)
        {
            return await _userQueryService.GetByPage(new UserFilters(trueName, gender, isEnabled),
                current, pageSize, orderByPropertyName, isAsc);
        }

        //// PUT: api/Users/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUser(Guid id, User user)
        //{
        //    if (id != user.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task PostUser(UserAddUICommand command)
        {
            await _userLogicService.Add(command);
        }

        //// DELETE: api/Users/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<User>> DeleteUser(Guid id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();

        //    return user;
        //}

        //private bool UserExists(Guid id)
        //{
        //    return _context.Users.Any(e => e.Id == id);
        //}
    }
}
