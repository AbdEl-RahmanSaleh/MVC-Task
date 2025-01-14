using Core.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task.Helper;
using Task.Models;

namespace Task.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IGenericRepo<Product> _productRepo; 
        public ProductController(IGenericRepo<Product> productRepo)
        {
            _productRepo = productRepo;
        }
        public IActionResult Index(ProductFilteration search)
        {
            var products = _productRepo.GetAll().Where(x => x.IsDeleted is false);

            if (!string.IsNullOrEmpty(search.ProductName))
            {
                products = products.Where(p => p.Name.Contains(search.ProductName, StringComparison.OrdinalIgnoreCase));
            }

            if (search.MinPrice.HasValue)
            {
                products = products.Where(p => p.price >= search.MinPrice.Value);
            }

            if (search.MaxPrice.HasValue)
            {
                products = products.Where(p => p.price <= search.MaxPrice.Value);
            }

            if (search.StartDate.HasValue)
            {
                products = products.Where(p => p.CreatedAt.Date >= search.StartDate.Value.Date);
            }

            if (search.EndDate.HasValue)
            {
                products = products.Where(p => p.CreatedAt.Date <= search.EndDate.Value.Date);
            }

            ViewBag.Name = search.ProductName;
            ViewBag.MinPrice = search.MinPrice;
            ViewBag.MaxPrice = search.MaxPrice;
            ViewBag.FromDate = search.StartDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = search.EndDate?.ToString("yyyy-MM-dd");

            return View(products);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            if (ModelState.IsValid) {
                if (product.Image != null) 
                {
                    product.ImageName = await DocumentSettings.UploadFile(product.Image,"images");
                    product.CreatedAt = DateTime.Now;
                }
                _productRepo.Add(product);               
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public IActionResult Update(int? id)
        {
            if (id is null)
                return BadRequest();

            var product = _productRepo.GetById(id);

            if (product is null)
                return NotFound();


            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            Product existingProduct = _productRepo.GetById(product.Id);

            if (ModelState.IsValid)
            {
                if(product.Image != null)
                {
                    DocumentSettings.DeleteFile("images",existingProduct.ImageName);
                    existingProduct.ImageName = await DocumentSettings.UploadFile(product.Image, "images");
                }
                existingProduct.Name = product.Name;
                existingProduct.price = product.price;
                _productRepo.Update(existingProduct);
                return RedirectToAction("Index");
            }
            return View(product);
        }


        public IActionResult Details(int id)
        {
            var product = _productRepo.GetById(id);
            if (product == null)
            {
                return NotFound();
            }


            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = _productRepo.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            _productRepo.Delete(id); 
            return RedirectToAction("Index");
        }


    }
}
