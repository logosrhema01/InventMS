using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace InventMS.Data
{
    public class ProductService
    {
        #region Property
        private readonly ApplicationDbContext _appDbContext;
        #endregion

        #region Constructor
        public ProductService(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        #endregion

        #region Get List of Product
        public async Task<List<Product>> GetAllProductAsync()
        {
            return await _appDbContext.Product.Include(c=> c.Category).ToListAsync();
        }
        #endregion

        #region Insert Product
        public async Task<bool> InsertProductAsync(Product product, int categoryId )
        {
            Category category = await _appDbContext.Category.FirstOrDefaultAsync(c => c.Id.Equals(categoryId));
            product.Category = category;
            await _appDbContext.Product.AddAsync(product);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Get Product by Id
        public async Task<Product> GetProductAsync(Guid Id)
        {
            Product product = await _appDbContext.Product.FirstOrDefaultAsync(c => c.Id.Equals(Id));
            return product;
        }
        #endregion

        #region Get Product by BarCode
        public async Task<Product> GetProductByBarCodeAsync(string barCode)
        {
            try
            {
                Product product = await _appDbContext.Product.FirstAsync(c => c.BarCode.Equals(barCode));
                return product;
            }
            catch (Exception ex)
            {
                Console.WriteLine("err");
                throw ex;
            }
        }
        #endregion

        #region Update Product
        public async Task<bool> UpdateProductAsync(Product product, int categoryId)
        {
            Category category = await _appDbContext.Category.FirstOrDefaultAsync(c => c.Id.Equals(categoryId));
            product.Category = category;
            _appDbContext.Product.Update(product);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Delete Product
        public async Task<bool> DeleteProductAsync(Product product)
        {
            _appDbContext.Remove(product);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
