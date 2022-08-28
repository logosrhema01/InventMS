using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace InventMS.Data
{
    public class TransactionService
    {
        #region Property
        private readonly ApplicationDbContext _appDbContext;
        private readonly ProductService _productService;
        #endregion

        #region Constructor
        public TransactionService(ApplicationDbContext appDbContext, ProductService productService)
        {
            _appDbContext = appDbContext;
            _productService = productService;
        }
        #endregion

        #region Get List of Transaction
        public async Task<List<Transaction>> GetAllTransactionAsync(string userId)
        {
            var userRole = await _appDbContext.UserRoles.FirstOrDefaultAsync(u => u.UserId.Equals(userId));
            if(userRole != null)
            {
                Console.WriteLine(userRole.RoleId);
                var role = await _appDbContext.Roles.FirstOrDefaultAsync(r => r.Id.Equals(userRole.RoleId));
                if(role.Name == "Administrators")
                {
                    return await _appDbContext.Transaction
                        .Include(c => c.User)
                        .Include(t => t.Till)
                        .OrderByDescending(t => t.TxnDateTime)
                        .Take(20)
                        .ToListAsync();
                }
            }
            return await _appDbContext.Transaction
                .Include(t => t.Till)
                .Where(t => t.User.Id.Equals(userId))
                .OrderByDescending(t => t.TxnDateTime)
                .Take(10)
                .ToListAsync();
        }
        #endregion

        #region Insert Transaction
        public async Task<bool> InsertTransactionAsync(List<Order> orders, ApplicationUser user, Guid TillId, double TotalPrice)
        {
            var _user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id.Equals(user.Id));
            Transaction transaction = new Transaction() { TxnDateTime = DateTime.Now, User = _user };

            List<Product> products = new List<Product>();


            foreach (Order order in orders)
            {
                Console.WriteLine(order.Quantity);
                var product = await _productService.GetProductAsync(order.Product.Id);
                product.Quantity -= order.Quantity;
                order.Product = product;
                _appDbContext.Product.Update(product);
                products.Add(order.Product);
            }

            transaction.Products = products;
            transaction.TotalPrice = TotalPrice;
            transaction.Till = await _appDbContext.Till.FirstOrDefaultAsync(t=>t.Id.Equals(TillId));
            await _appDbContext.Transaction.AddAsync(transaction);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Get Transaction by Id
        public async Task<Transaction> GetTransactionAsync(Guid Id)
        {
            Transaction transaction = await _appDbContext.Transaction.FirstOrDefaultAsync(c => c.Id.Equals(Id));
            return transaction;
        }
        #endregion

        #region Update Transaction
        public async Task<bool> UpdateTransactionAsync(Transaction transaction)
        {
            _appDbContext.Transaction.Update(transaction);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Delete Transaction
        public async Task<bool> DeleteTransactionAsync(Transaction transaction)
        {
            _appDbContext.Remove(transaction);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        #endregion

        private async Task UpdateStock(Order order)
        {
            var product = await _productService.GetProductAsync(order.Product.Id);
            product.Quantity -= order.Quantity;
            _appDbContext.Product.Update(product);
        }
    }
}
