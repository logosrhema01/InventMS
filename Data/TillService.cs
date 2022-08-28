using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace InventMS.Data
{
    public class TillService
    {
        #region Property
        private readonly ApplicationDbContext _appDbContext;
        #endregion

        #region Constructor
        public TillService(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        #endregion

        #region Get List of Till
        public async Task<List<Till>> GetAllTillAsync(ApplicationUser user)
        {
            var userRole = await _appDbContext.UserRoles.FirstOrDefaultAsync(u => u.UserId.Equals(user.Id));
            if(userRole != null)
            {
                var role = await _appDbContext.Roles.FirstOrDefaultAsync(r => r.Id.Equals(userRole.RoleId));
                if(role.Name == "Administrators")
                {
                    return await _appDbContext.Till
                        .Include(c => c.User)
                        .OrderByDescending(t => t.Start)
                        .Take(20)
                        .ToListAsync();
                }
            }
            return await _appDbContext.Till
                .Where(t => t.User.Id.Equals(user.Id))
                .OrderByDescending(t => t.Start)
                .Take(10)
                .ToListAsync();
        }
        #endregion

        #region Insert Till
        public async Task<Till> StartSales(ApplicationUser user)
        {
            var _user = _appDbContext.Users.FirstOrDefault(c=> c.Id.Equals(user.Id));
            Till till = new Till { User = _user, Start = DateTime.Now };
            await _appDbContext.Till.AddAsync(till);
            await _appDbContext.SaveChangesAsync();
            return till;
        }
        #endregion

        #region Get Till by Id
        public async Task<Till> GetTillAsync(Guid Id)
        {
            Till till = await _appDbContext.Till.FirstOrDefaultAsync(c => c.Id.Equals(Id));
            return till;
        }
        #endregion

        #region Get Till by Id
        public async Task<Till> GetUserTill(ApplicationUser user)
        {
            Till till = await _appDbContext.Till.OrderByDescending(t => t.Start).FirstOrDefaultAsync(c => c.User.Id.Equals(user.Id));
            if (till != null && EndDatePast(till.Start)) till.End = DateTime.Now;
            if(till is null || till.End != null)
            {
                var newTill = await StartSales(user);
                return newTill;
            }
            return till;
        }
        #endregion

        #region EndSales Till
        public async Task<bool> EndSales(ApplicationUser user, Guid tillId)
        {
            var till = await _appDbContext.Till.FirstOrDefaultAsync(t => t.Id.Equals(tillId) && t.User.Equals(user));
            till.End = DateTime.Now;
            _appDbContext.Till.Update(till);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        #endregion

        //#region Delete Till
        //public async Task<bool> DeleteTillAsync(ApplicationUser user,Till till)
        //{
        //    var userRole = await _appDbContext.UserRoles.FirstOrDefaultAsync(u => u.UserId.Equals(user.Id));
        //    if (userRole != null)
        //    {
        //        var role = await _appDbContext.Roles.FirstOrDefaultAsync(r => r.Id.Equals(userRole.RoleId));
        //        if (role.Name == "Administrators")
        //        {
        //            _appDbContext.Remove(till);
        //            await _appDbContext.SaveChangesAsync();
        //            return true;
        //        }
        //        return false;
        //    }
        //    return false;
        //}
        //#endregion

        public static bool EndDatePast(DateTime start)
        {
            var expectedEnd = start.AddHours(8);
            var currentTime = start.AddHours(DateTime.Now.Subtract(start).Hours);
            var compare = DateTime.Compare(currentTime, expectedEnd);
            if(compare < 0)
            {
                return false;
            }
            return true;
        }
    }
}
