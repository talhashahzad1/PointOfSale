using PointOfSale.Business.Contracts;
using PointOfSale.Data.Repository;
using PointOfSale.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PointOfSale.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IGenericRepository<Supplier> _supplierRepository;

        public SupplierService(IGenericRepository<Supplier> supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<IEnumerable<Supplier>> GetAllSuppliers()
        {
            var query = await _supplierRepository.Query();
            return query.ToList();
        }

        public async Task<Supplier?> GetSupplierById(int id)
        {
            return await _supplierRepository.Get(s => s.Supplier_Id == id);
        }

        public async Task<Supplier> AddSupplier(Supplier supplier)
        {
            return await _supplierRepository.Add(supplier);
        }

        public async Task<bool> UpdateSupplier(Supplier supplier)
        {
            return await _supplierRepository.Edit(supplier);
        }

        public async Task<bool> DeleteSupplier(int id)
        {
            var supplier = await _supplierRepository.Get(s => s.Supplier_Id == id);
            if (supplier == null)
                return false;

            return await _supplierRepository.Delete(supplier);
        }
    }
}
