using PointOfSale.Model;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PointOfSale.Business.Services
{
    public interface IPurchaseService
    {
 
        Task AddPurchase(Purchase purchase);

      
        Task<Purchase> GetPurchaseById(int id);

        Task<IQueryable<Purchase>> GetAllPurchases();

        
        Task<bool> UpdatePurchase(Purchase purchase);

    
        Task<bool> DeletePurchase(int id);

      
        Task<IQueryable<Purchase>> GetPurchasesByFilter(Expression<Func<Purchase, bool>> filter);
    }
}
