using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PointOfSale.Business.Contracts;
using PointOfSale.Business.Services;
using PointOfSale.Model;
using PointOfSale.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PointOfSale.API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    [Authorize]
    public class PurchaseController : Controller
    {
        private readonly IPurchaseService _purchaseService;
        private readonly ISupplierService _supplierService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public PurchaseController(IPurchaseService purchaseService,
                                   ISupplierService supplierService,
                                   IProductService productService,
                                   IMapper mapper)
        {
            _purchaseService = purchaseService;
            _supplierService = supplierService;
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VMPurchase>> GetPurchaseById(int id)
        {
            try
            {
                var purchase = await _purchaseService.GetPurchaseById(id);
                if (purchase == null)
                    return NotFound($"Purchase with ID {id} not found.");

                var vmPurchase = _mapper.Map<VMPurchase>(purchase);
                return Ok(vmPurchase);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPurchases()
        {
            try
            {
                var purchases = await _purchaseService.GetAllPurchases();
                var vmPurchases = _mapper.ProjectTo<VMPurchase>(purchases);
                return View(await vmPurchases.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }


        [HttpGet("SavePurchase/{id?}")]
        public async Task<IActionResult> LoadPurchaseForm(int? id)
        {
            try
            {
                ViewBag.Suppliers = await GetSuppliersForDropdown();
                ViewBag.Products = await GetProductsForDropdown();

                if (id.HasValue)
                {
                    var purchase = await _purchaseService.GetPurchaseById(id.Value);
                    if (purchase == null)
                        return NotFound($"Purchase with ID {id.Value} not found.");

                    var vmPurchase = _mapper.Map<VMPurchase>(purchase);
                    return PartialView("SavePurchase", vmPurchase);
                }

                return PartialView("SavePurchase", new VMPurchase());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("SavePurchase")]
        public async Task<IActionResult> SavePurchase([FromBody] VMPurchase model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Suppliers = await GetSuppliersForDropdown();
                ViewBag.Products = await GetProductsForDropdown();
                return PartialView("SavePurchase", model);
            }

            try
            {
                var purchase = _mapper.Map<Purchase>(model);

                if (model.Id > 0)
                {
                    var updated = await _purchaseService.UpdatePurchase(purchase);
                    if (!updated)
                        throw new Exception("Failed to update purchase.");
                }
                else
                {
                    await _purchaseService.AddPurchase(purchase);
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                ViewBag.Suppliers = await GetSuppliersForDropdown();
                ViewBag.Products = await GetProductsForDropdown();
                return PartialView("SavePurchase", model);
            }
        }


        [HttpDelete]
        public async Task<IActionResult> DeletePurchase(int id)
        {
            try
            {
                var purchase = await _purchaseService.GetPurchaseById(id);
                if (purchase != null)
                {
                    await _purchaseService.DeletePurchase(id);
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Purchase not found." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }



        [HttpGet("filter")]
        public async Task<ActionResult<IQueryable<VMPurchase>>> GetPurchasesByFilter([FromQuery] string supplierName)
        {
            try
            {
                Expression<Func<Purchase, bool>> filter = p => p.P_Supplier_Name.Contains(supplierName);
                var filteredPurchases = await _purchaseService.GetPurchasesByFilter(filter);

                var vmPurchases = _mapper.ProjectTo<VMPurchase>(filteredPurchases);
                return Ok(await vmPurchases.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        private async Task<SelectList> GetSuppliersForDropdown()
        {
            var suppliers = await _supplierService.GetAllSuppliers();
            return new SelectList(suppliers, "Supplier_Id", "Supplier_Name");
        }

        private async Task<SelectList> GetProductsForDropdown()
        {
            var products = await _productService.List();
            return new SelectList(products, "IdProduct", "Brand");
        }
    }
}
