
using MasterEntryDemo.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MasterEntryDemo.Controllers
{
    public class OrderController : Controller
    {

        private OnlineShopContext context;

        public OrderController()
        {
            context = new OnlineShopContext();
        }

        // GET: Order
        public ActionResult Index()
        {
            //using (var context = new OnlineShopContext())
            //{
                //List<Customer> OrderAndCustomerList = context.Customers.Include(m => m.Orders).ToList();
		var d = context.Orders.Where(m => m.CustomerId == new Guid("c4a3fa80-ffbf-4a44-a6a0-3560b95a5274")).ToList();   // Get Single Person Record
                List<Customer> OrderAndCustomerList = context.Customers.Include(m => m.Orders).ToList();

               
                return View(OrderAndCustomerList);
            //}
        }




        public ActionResult SaveOrder(string name, String address, Order[] order)
        {
            using (var context = new OnlineShopContext())
            {
                using (DbContextTransaction dbTran = context.Database.BeginTransaction())
                {
                    try
                    {
                        string result = "Error! Order Is Not Complete!";
                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(address)) //&& order != null
                        {

                            Customer customer = new Customer();
                            customer.CustomerId = Guid.NewGuid();
                            customer.Name = name;
                            customer.Address = address;
                            customer.OrderDate = DateTime.Now;
                            context.Customers.Add(customer);
                            context.SaveChanges();

                            var cutomerId = customer.CustomerId;

                            if (order != null)
                            {
                                foreach (var item in order)
                                {
                                    var orderId = Guid.NewGuid();
                                    Order O = new Order();
                                    O.OrderId = orderId;
                                    O.ProductName = item.ProductName;
                                    O.Quantity = item.Quantity;
                                    O.Price = item.Price;
                                    O.Amount = item.Amount;
                                    O.CustomerId = cutomerId;
                                    context.Orders.Add(O);
                                }
                                context.SaveChanges();
                                dbTran.Commit();
                                result = "Success! Order Is Complete!";
                            }
                            else
                            {
                                dbTran.Rollback();
                            }

                        }
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception)
                    {
                        dbTran.Rollback();
                        throw;
                    }
                }
            }
        }


    }
}