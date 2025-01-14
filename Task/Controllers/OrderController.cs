using Core.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Task.Controllers
{

    //[Authorize]
    public class OrderController : Controller
    {

        private readonly IGenericRepo<Product> _productRepo;
        private readonly IGenericRepo<Order> _orderRepo;

        public OrderController(IGenericRepo<Product> productRepo,IGenericRepo<Order> orderRepo)
        {
            _productRepo = productRepo;
            _orderRepo = orderRepo;
        }

        public IActionResult Create()
        {
            ViewBag.Products = new SelectList(_productRepo.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Order order, List<OrderItem> orderItems)
        {
            ViewBag.Products = new SelectList(_productRepo.GetAll(), "Id", "Name");

            if (ModelState.IsValid)
            {
                order.OrderTime = DateTime.Now;


                foreach (var item in orderItems) 
                {
                    var product = _productRepo.GetById(item.productId);
                    if (product != null) {
                        item.UnitPrice = product.price;
                        item.ItemTotal = item.UnitPrice * item.Quantity;
                        item.ImageName = product.ImageName;
                        item.productName = product.Name;
                    };
                }
                
                order.OrderItems = orderItems;
                order.Total = orderItems.Sum(i => i.ItemTotal);


                _orderRepo.Add(order);
                return RedirectToAction("Index");
            }
            
            return View(order);
        }

        public JsonResult GetProductPrice(int productId)
        {
            var product = _productRepo.GetById(productId);
            return Json(product?.price);
        }
        
        public JsonResult GetProductImage(int productId)
        {
            var product = _productRepo.GetById(productId);
            return Json(product?.ImageName);
        }

        public IActionResult Index()
        {

            var orders = _orderRepo.GetAll().Where(x =>x.IsDeleted != true);
            return View(orders);
        }

        public IActionResult Details(int id)
        {
            var order = _orderRepo.GetById(id);

            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }


        public IActionResult Delete(int id)
        {
            var product = _orderRepo.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            _orderRepo.Delete(id);
            return RedirectToAction("Index");
        }


    }
}
