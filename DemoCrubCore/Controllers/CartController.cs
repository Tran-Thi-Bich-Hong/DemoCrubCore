using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoCrubCore.Data;
using DemoCrubCore.Models;
using DemoCrubCore.Helper;
using Microsoft.AspNetCore.Http;
using System.Transactions;

namespace DemoCrubCore.Controllers
{
    public class CartController : Controller
    {
        private readonly saledbContext _context;
        public CartController (saledbContext context)
        {
            _context = context;
        }
        public List<CartItems>Carts//lấy DS giỏ hàng
        {
            get
            {
                var data = HttpContext.Session.Get<List<CartItems>>("Giohang");
                if(data == null)
                {
                    data = new List<CartItems>();
                }
                return data;
            }
        }
        public IActionResult Index()
        {
            return View(Carts);
        }
        public IActionResult AddtoCart(int id)
        {
            var myCart = Carts;
            var item = myCart.SingleOrDefault(p => p.ProductId == id);
            if(item == null)
            {
                var product = _context.Products.SingleOrDefault(p => p.Id == id);
                item = new CartItems {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ImageProduct = product.Image,
                    Price = product.Price.Value,
                    Quantity = 1
                };
                myCart.Add(item);
            }
            else
            {
                item.Quantity++;
            }
            HttpContext.Session.Set("Giohang", myCart);
            return RedirectToAction("ListCarts");
        }
        private int Count()
        {
            int n = 0;
            var data = HttpContext.Session.Get<List<CartItems>>("Giohang");
            if (data != null)
            {
                n = data.Sum(s => s.Quantity);
            }
            return n;
            
        }
        private decimal Total()
        {
            decimal total = 0;
            var data = HttpContext.Session.Get<List<CartItems>>("Giohang");
            if (data != null)
            {
                total = data.Sum(s => s.thanhtien);
            }
            return total;
        }
        public ActionResult ListCarts()//hiển thị giỏ hàng
        {
            List<CartItems> carts = Carts;

            if (carts.Count == 0)//gio hang chua co Sp
            {
                return RedirectToAction("Store", "Products");//DSSP
            }
            ViewData["CountProduct"] = Count();
            ViewData["Total"] = Total();

            return View(carts);
        }
        public ActionResult Delete(int id)
        {
            List<CartItems> carts = Carts;//lấy giỏ hàng
            CartItems c = carts.Find(s => s.ProductId == id);

            if (c != null)
            {
                var pro = carts.FirstOrDefault(s => s.ProductId == id);
                carts.Remove(pro);
                HttpContext.Session.Set("Giohang", carts);
                return RedirectToAction("ListCarts");
            }
            if (carts.Count == 0)
            {
                return RedirectToAction("Store", "Products");
            }
            return RedirectToAction("ListCarts");
        }
        public ActionResult OrderProduct()
        {

            using (TransactionScope tranScope = new TransactionScope())
            {
                try
                {
                    SaleOrder order = new SaleOrder();
                    if (_context.SaleOrders.Select(s => s.Id).Count() == 0)
                    {
                        order.Id = 1;
                    }
                    else
                    {
                        int p = _context.SaleOrders.Select(s => s.Id).Max();
                        order.Id = p;
                        order.Id++;
                    }
                    order.CreatedDate = DateTime.Now;
                    order.Amount = Total();
                    _context.SaleOrders.Add(order);
                    _context.SaveChangesAsync();
                    //order = dt.Orders.OrderByDescending(s => s.OrderID).Take(1).SingleOrDefault();
                    List<CartItems> carts = Carts;//lấy giỏ hàng
                    foreach (var item in carts)
                    {
                       
                        OrderDetail d = new Models.OrderDetail();
                        if (_context.OrderDetails.Select(s => s.Id).Count() == 0)
                        {
                            d.Id = 1;
                        }
                        else
                        {
                            int p = _context.OrderDetails.Select(s => s.Id).Max();
                            d.Id = p;
                            d.Id++;
                        }
                        d.OrderId = order.Id;
                        d.ProductId = item.ProductId;
                        d.Num = item.Quantity.ToString();
                        d.UnitPrice = item.Price;
                      

                        _context.OrderDetails.Add(d);
                    }
                    _context.SaveChangesAsync();
                    tranScope.Complete();
                    var data = HttpContext.Session.Get<List<CartItems>>("Giohang");
                    data = null;
                    HttpContext.Session.Set("Giohang", data);
                }
                catch (Exception)
                {
                    tranScope.Dispose();
                    return RedirectToAction("ListCarts");

                }
            }
            return RedirectToAction("ListCarts", "Cart");
        }
        public ActionResult OrderDetailList()
        {
            var p = _context.OrderDetails.OrderByDescending(s => s.Id).Select(s => s).ToList();
            return View(p);
        }
    }
}
