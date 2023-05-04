using CarMaintenance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace CarMaintenance.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        RepairContext db = new RepairContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //var customers = db.Customers.Where(m => m.IsDelete == false).ToList();
            return View();
        }

        public IActionResult CustomerDetail(string? name = null)
        {
            List<Customer> customers;
            if (name != null)
            {
                customers = db.Customers.Where(m => m.Name == name && m.IsDelete == false).ToList();
            }
            else
            {
                customers = db.Customers.Where(m => m.IsDelete == false).ToList();
            }
            return View(customers);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            var customers = new Customer();
            var lastCustomer = db.Customers.OrderByDescending(c => c.Id).FirstOrDefault();
            if (lastCustomer == null)
            {
                customers.Id = 0;
            }
            else
            {
                customers.Id = lastCustomer.Id + 1;
            }
            customers.Name = customer.Name;
            customers.Phone = customer.Phone;
            db.Customers.Add(customers);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var customer = db.Customers.Where(m => m.Id == id).FirstOrDefault();
            customer.IsDelete = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            var customer = db.Customers.Where(m => m.Id == id).FirstOrDefault();
            return View(customer);
        }

        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            var modify = db.Customers.Where(m => m.Id == customer.Id).FirstOrDefault();
            modify.Name = customer.Name;
            modify.Phone = customer.Phone;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult CarDetail(int Id)
        {
            var car = db.Cars.Where(m => m.CustomerId == Id && m.IsDelete == false).ToList();
            return View(car);
        }

        public IActionResult CarCreate()
        {
            ViewBag.CustomerIds = new SelectList(db.Customers, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult CarCreate(Car car)
        {
            var modify = new Car
            {
                CustomerId = car.CustomerId,
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year
            };
            db.Cars.Add(modify);
            db.SaveChanges();
            return RedirectToAction("CarDetail", new { Id = car.CustomerId });
        }

        public IActionResult CarDelete(string Id)
        {
            var car = db.Cars.Where(m => m.Id == Id).FirstOrDefault();
            car.IsDelete = true;
            db.SaveChanges();
            int CustomerId = car.CustomerId;
            return RedirectToAction("CarDetail", new { Id = CustomerId });
        }

        public IActionResult CarEdit(string id)
        {
            var car = db.Cars.Where(m => m.Id == id).FirstOrDefault();
            return View(car);
        }

        [HttpPost]
        public IActionResult CarEdit(Car car)
        {
            var modify = db.Cars.Where(m => m.Id == car.Id).FirstOrDefault();
            modify.Brand = car.Brand;
            modify.Model = car.Model;
            modify.Year = car.Year;
            db.SaveChanges();
            int CustomerId = car.CustomerId;
            return RedirectToAction("CarDetail", new { Id = CustomerId });
        }


        public IActionResult BillDetail(string carId)
        {
            var bill = db.Bills.Where(m => m.CarId == carId).ToList();

            ViewBag.CarId = carId;

            return View(bill);
        }

        public IActionResult BillDelete(int Id)
        {
            var bill = db.Bills.Where(m => m.Id == Id).FirstOrDefault();
            bill.IsDelete = true;
            db.SaveChanges();
            return RedirectToAction("BillDetail");
        }

        public IActionResult BillCreate(string carId)
        {
            //ViewBag.CarId = new SelectList(db.Car, "Id", "Id", carId);

            ViewBag.CarId = carId;
            return View();
        }

        [HttpPost]
        public IActionResult BillCreate(Bill bill, string carId)
        {
            var newbill = new Bill();
            newbill.Id = bill.Id;
            newbill.CarId = carId;
            newbill.Date = DateTime.Now;
            newbill.Price = bill.Price;
            newbill.Project = bill.Project;
            db.Bills.Add(newbill);
            db.SaveChanges();
            string CarId = bill.CarId;
            return RedirectToAction("BillDetail", new { carId = CarId });

        }

        public IActionResult BillEdit(string carId)
        {
            var bill = db.Bills.Where(m => m.CarId == carId).FirstOrDefault();
            return View(bill);
        }

        [HttpPost]
        public IActionResult BillEdit(Bill bill)
        {
            var modify = db.Bills.Where(m => m.CarId == bill.CarId).FirstOrDefault();
            modify.Date = bill.Date;
            modify.Price = bill.Price;
            modify.Project = bill.Project;
            db.SaveChanges();

            string CarId = bill.CarId;
            return RedirectToAction("BillDetail", new { carId = CarId });
        }

        public IActionResult Search(string key)
        {
            List<Customer> search = db.Customers.Where(m => m.Name == key).ToList();
            List<Car> search1 = db.Cars.Where(m => m.Id == key).ToList();

            if (search.Any() == true)
            {
                string customerName = key;
                return RedirectToAction("CustomerDetail", new { Name = customerName });
                //return View(search);
            }
            else 
            {
                //int customerId = search1.CustomerId;
                //return RedirectToAction("CarDetail", new { Id = customerId });
                return View("CarDetail",search1);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
