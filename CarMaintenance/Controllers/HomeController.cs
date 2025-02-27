using CarMaintenance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace CarMaintenance.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //RepairContext db = new RepairContext();
        private RepairContext _db;
        public HomeController(ILogger<HomeController> logger , RepairContext repairContext)
        {
            _logger = logger;
            _db = repairContext;
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
                customers = _db.Customers.Where(m => m.Name.Contains(name) && m.IsDelete == false).ToList();
            }
            else
            {
                customers = _db.Customers.Where(m => m.IsDelete == false).ToList();
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
            var lastCustomer = _db.Customers.OrderByDescending(c => c.Id).FirstOrDefault();
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
            _db.Customers.Add(customers);
            _db.SaveChanges();
            return RedirectToAction("CustomerDetail");
        }

        public IActionResult Delete(int id)
        {
            var customer = _db.Customers.FirstOrDefault(m => m.Id == id);
            customer.IsDelete = true;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {


            var customer = _db.Customers.FirstOrDefault(m => m.Id == id);

       
            return View(customer);
        }

        [HttpPost]
        public IActionResult Edit(Customer customer)
        {

            var modify = _db.Customers.FirstOrDefault(m => m.Id == customer.Id);
            modify.Name = customer.Name;
            modify.Phone = customer.Phone;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult CarDetail(int Id)
        {
            var car = _db.Cars.Where(m => m.CustomerId == Id && m.IsDelete == false).ToList();
            return View(car);
        }

        public IActionResult CarCreate()
        {
            ViewBag.CustomerIds = new SelectList(_db.Customers, "Id", "Name");
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
            _db.Cars.Add(modify);
            _db.SaveChanges();
            return RedirectToAction("CarDetail", new { Id = car.CustomerId });
        }

        public IActionResult CarDelete(string Id)
        {
            var car = _db.Cars.FirstOrDefault(m => m.Id == Id);
            car.IsDelete = true;
            _db.SaveChanges();
            int CustomerId = car.CustomerId;
            return RedirectToAction("CarDetail", new { Id = CustomerId });
        }

        public IActionResult CarEdit(string id)
        {
            var car = _db.Cars.FirstOrDefault(m => m.Id == id);
            return View(car);
        }

        [HttpPost]
        public IActionResult CarEdit(Car car)
        {
            var modify = _db.Cars.FirstOrDefault(m => m.Id == car.Id);
            modify.Brand = car.Brand;
            modify.Model = car.Model;
            modify.Year = car.Year;
            _db.SaveChanges();
            int CustomerId = car.CustomerId;
            return RedirectToAction("CarDetail", new { Id = CustomerId });
        }


        public IActionResult BillDetail(string carId)
        {
            var bill = _db.Bills.Where(m => m.CarId == carId).ToList();

            ViewBag.CarId = carId;

            return View(bill);
        }

        public IActionResult BillDelete(int Id)
        {
            var bill = _db.Bills.FirstOrDefault(m => m.Id == Id);
            bill.IsDelete = true;
            _db.SaveChanges();
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
            _db.Bills.Add(newbill);
            _db.SaveChanges();
            string CarId = bill.CarId;
            return RedirectToAction("BillDetail", new { carId = CarId });

        }

        public IActionResult BillEdit(string carId)
        {
            var bill = _db.Bills.FirstOrDefault(m => m.CarId == carId);
            return View(bill);
        }

        [HttpPost]
        public IActionResult BillEdit(Bill bill)
        {
            var modify = _db.Bills.FirstOrDefault(m => m.CarId == bill.CarId);
            modify.Date = bill.Date;
            modify.Price = bill.Price;
            modify.Project = bill.Project;
            _db.SaveChanges();

            string CarId = bill.CarId;
            return RedirectToAction("BillDetail", new { carId = CarId });
        }

        public IActionResult Search(string key)
        {
            List<Customer> search = _db.Customers.Where(m => m.Name.Contains(key)).ToList();
            List<Car> search1 = _db.Cars.Where(m => m.Id.Contains(key)).ToList();

            if (search.Any() == true)
            {
                //string customerName = key;
                return RedirectToAction("CustomerDetail", new { Name = key });
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
