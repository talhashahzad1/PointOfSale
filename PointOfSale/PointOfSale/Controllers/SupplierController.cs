using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointOfSale.Business.Contracts;
using PointOfSale.Model;
using PointOfSale.Models;
using System.Threading.Tasks;

namespace PointOfSale.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;

        public SupplierController(ISupplierService supplierService, IMapper mapper)
        {
            _supplierService = supplierService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetSuppliers()
        {
            try
            {
                var suppliers = await _supplierService.GetAllSuppliers();
                var vmSupplierList = _mapper.Map<List<VMSupplier>>(suppliers);
                return View(vmSupplierList);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Failed to load suppliers: {ex.Message}";
                return View(new List<VMSupplier>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> SaveSuppliers(int? id)
        {
            try
            {
                if (id.HasValue)
                {
                    var supplier = await _supplierService.GetSupplierById(id.Value);
                    var vmSupplier = _mapper.Map<VMSupplier>(supplier);
                    return PartialView("SaveSuppliers", vmSupplier); 
                }

                return PartialView("SaveSuppliers", new VMSupplier());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveSuppliers(VMSupplier model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("SaveSuppliers", model); 
            }

            try
            {
                var supplier = _mapper.Map<Supplier>(model);

                if (model.Supplier_Id > 0)
                {
                    bool isUpdated = await _supplierService.UpdateSupplier(supplier);
                    if (!isUpdated)
                    {
                        ModelState.AddModelError("", "Failed to update supplier.");
                        return PartialView("SaveSuppliers", model);
                    }
                }
                else
                {
                    await _supplierService.AddSupplier(supplier);
                }

                return Json(new { success = true }); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return PartialView("SaveSuppliers", model);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            try
            {
                bool isDeleted = await _supplierService.DeleteSupplier(id);
                if (!isDeleted)
                {
                    ViewBag.Error = "Failed to delete supplier.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Failed to delete supplier: {ex.Message}";
            }

            return RedirectToAction("GetSuppliers");
        }
    }

}
