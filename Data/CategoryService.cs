using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace InventMS.Data
{
    public class CategoryService
    {
        #region Property
        private readonly ApplicationDbContext _appDbContext;
        #endregion

        #region Constructor
        public CategoryService(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        #endregion

        #region Get List of Category
        public async Task<List<Category>> GetAllCategoryAsync()
        {
            return await _appDbContext.Category.ToListAsync();
        }
        #endregion

        #region Insert Category
        public async Task<bool> InsertCategoryAsync(Category category)
        {
            await _appDbContext.Category.AddAsync(category);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Get Category by Id
        public async Task<Category> GetCategoryAsync(int Id)
        {
            Category category = await _appDbContext.Category.FirstOrDefaultAsync(c => c.Id.Equals(Id));
            return category;
        }
        #endregion

        #region Update Category
        public async Task<bool> UpdateCategoryAsync(Category category)
        {
             _appDbContext.Category.Update(category);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Delete Category
        public async Task<bool> DeleteCategoryAsync(Category category)
        {
            _appDbContext.Remove(category);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
