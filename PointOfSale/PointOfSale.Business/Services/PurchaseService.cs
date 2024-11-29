using PointOfSale.Business.Services;
using PointOfSale.Data.Repository;
using PointOfSale.Model;
using System.Linq.Expressions;

public class PurchaseService : IPurchaseService
{
    private readonly IGenericRepository<Purchase> _purchaseRepository;
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<Supplier> _supplierRepository;

    public PurchaseService(IGenericRepository<Purchase> purchaseRepository, IGenericRepository<Product> productRepository, IGenericRepository<Supplier> supplierRepository)
    {
        _purchaseRepository = purchaseRepository;
        _productRepository = productRepository;
        _supplierRepository = supplierRepository;
    }

    public async Task AddPurchase(Purchase purchase)
    {
        if (purchase == null)
        {
            throw new ArgumentNullException(nameof(purchase));
        }

        var product = await _productRepository.Get(p => p.IdProduct == purchase.Product_Id);
        if (product == null)
        {
            throw new InvalidOperationException($"Product with ID {purchase.Product_Id} not found.");
        }

  
        var supplier = await _supplierRepository.Get(s => s.Supplier_Id == purchase.Supplier_Id);
        if (supplier == null)
        {
            throw new InvalidOperationException($"Supplier with ID {purchase.Supplier_Id} not found.");
        }

   
        purchase.Product_Id = product.IdProduct;
        //purchase.P_Supplier_Address = supplier.Supplier_Address;
        purchase.P_Supplier_Name = supplier.Supplier_Name; 
        purchase.Product_Name = product.Brand;


        using var transaction = await _purchaseRepository.Context.Database.BeginTransactionAsync();
        try
        {
            await _purchaseRepository.Add(purchase);


            product.Quantity += purchase.Quantity;


            await _productRepository.Edit(product);


            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new InvalidOperationException("An error occurred while adding the purchase and updating the product quantity.", ex);
        }
    }

    public async Task<Purchase> GetPurchaseById(int id)
    {
        var purchase = await _purchaseRepository.Get(p => p.id == id);
        if (purchase == null)
        {
            throw new InvalidOperationException($"Purchase with ID {id} not found.");
        }
        return purchase;
    }

    public async Task<IQueryable<Purchase>> GetAllPurchases()
    {
        var purchases = await _purchaseRepository.Query(null);
        return purchases;
    }

    public async Task<bool> UpdatePurchase(Purchase purchase)
    {
        if (purchase == null)
        {
            throw new ArgumentNullException(nameof(purchase));
        }

        var existingPurchase = await _purchaseRepository.Get(p => p.id == purchase.id);
        if (existingPurchase == null)
        {
            throw new InvalidOperationException($"Purchase with ID {purchase.id} not found.");
        }

        var quantityDifference = purchase.Quantity - existingPurchase.Quantity;
        existingPurchase.P_Supplier_Name = purchase.P_Supplier_Name;
        //existingPurchase.P_Supplier_Address = purchase.P_Supplier_Address;
        existingPurchase.Product_Name = purchase.Product_Name;
        existingPurchase.Purchase_Price = purchase.Purchase_Price;
        existingPurchase.Quantity = purchase.Quantity;


        var product = await _productRepository.Get(p => p.IdProduct == purchase.Product_Id);
        if (product == null)
        {
            throw new InvalidOperationException($"Product with ID {purchase.Product_Id} not found.");
        }
        product.Quantity += quantityDifference;
        if (product.Quantity<0)
        {
            product.Quantity = 0;
        }

        using var transaction = await _purchaseRepository.Context.Database.BeginTransactionAsync();
        try
        {
            var updateSuccess = await _purchaseRepository.Edit(existingPurchase);
            if (!updateSuccess)
            {
                throw new InvalidOperationException("Failed to update the purchase record.");
            }

            var editSuccess = await _productRepository.Edit(product);
            if (!editSuccess)
            {
                throw new InvalidOperationException("Failed to update the product quantity.");
            }

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new InvalidOperationException("An error occurred while updating the purchase and product quantity.", ex);
        }
    }


    public async Task<bool> DeletePurchase(int idd)
    {
        var purchase = await _purchaseRepository.Get(p => p.id == idd);
        if (purchase == null)
        {
            throw new InvalidOperationException($"Purchase with ID {idd} not found.");
        }

        return await _purchaseRepository.Delete(purchase);
    }

    public async Task<IQueryable<Purchase>> GetPurchasesByFilter(Expression<Func<Purchase, bool>> filter)
    {
        return await _purchaseRepository.Query(filter);
    }
}
