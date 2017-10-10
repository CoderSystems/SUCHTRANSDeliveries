using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inc2SuchTrans.Models;
using Inc2SuchTrans.BLL;
using Inc2SuchTrans.CustomFilters;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace Inc2SuchTrans.Controllers
{
    public class AdminController : BaseController
    {
        ApplicationDbContext context;
        STLogisticsEntities db = new STLogisticsEntities();
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        CustomerLogic cLogic = new CustomerLogic();

        EmployeeLogic elogic = new EmployeeLogic();
        DeliveryLogic dlogic = new DeliveryLogic();
        DeliveryJobLogic djLogic = new DeliveryJobLogic();
        TruckDriverLogic tdLogic = new TruckDriverLogic();
        FleetLogic flogic = new FleetLogic();
        JobQueueLogic jqLogic = new JobQueueLogic();

        /// <summary>
        /// 
        /// </summary>
        public AdminController()
        {
            context = new ApplicationDbContext();
        }



        // GET: Admin
        [AuthLog(Roles = "Admin, Driver")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AuthLog(Roles = "Admin")]
        public ActionResult AllEmployees()
        {
            try
            {
                List<Employee> empList = elogic.getAllEmployees();
                return View(empList);
            }
            catch (Exception e)
            {
                Danger("Oops! Something went wrong.. <br> Error: " + e.Message + "Message: " + e.StackTrace
                    + "<br> Please contact website support.");
                return View();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthLog(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id != null)
            {
                try
                {
                    Employee emp = elogic.searchEmployee(id);
                    if (emp != null)
                    {
                        return View(emp);
                    }
                    else
                    {
                        Danger("Could not find employee details");
                        return View();
                    }
                }
                catch (Exception e)
                {
                    Danger("Oops! Something went wrong.. <br> Error: " + e.Message + "Message: "
                        + e.StackTrace + "<br> Please contact website support.");
                    return View();
                }
            }
            else
            {
                Danger("Error retrieving details..");
                return View();
            }
        }

        [AuthLog(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if(id != null)
            {
                Employee emp = elogic.searchEmployee(id);
                return View(emp);
            }
            else
            {
                Danger("Error retrieving details..");
                return View("YourProfile");
            }
        }

        [HttpPost]
        [AuthLog(Roles = "Admin")]
        public ActionResult Edit(int id, string EmployeeName, string EmployeeSurname, string City, string Address, string PostalCode, string Position, string ContactNumber)
        {
            if (String.IsNullOrEmpty(EmployeeName))
            {
                Danger("Please Enter a Name!");
                Employee emp = elogic.searchEmployee(id);
                return View(emp);
            }

            try
            {
                Employee emp = elogic.searchEmployee(id);
                if (emp != null)
                {
                    emp.EmployeeName = EmployeeName;
                    emp.EmployeeSurname = EmployeeSurname;
                    emp.City = City;
                    emp.Address = Address;
                    emp.PostalCode = PostalCode;
                    emp.Position = Position;
                    emp.ContactNumber = ContactNumber;
                    emp.LastModified = System.DateTime.Now;

                    elogic.updateDetails(emp);
                }
                Success("Successfully Updated Details!");
                return RedirectToAction("Details", new { id = emp.EmployeeID });
            }
            catch (Exception e)
            {
                Danger("Oops! Something went wrong.. <br> Please contact support..");
                return View();
                throw e;
            }

        }

        [AuthLog(Roles = "Admin")]
        public ActionResult YourProfile()
        {
            try
            {
                int i = elogic.getCurrentEployeeID(User.Identity.GetUserId());
                Employee emp = elogic.searchEmployee(i);
                if (emp != null)
                {
                    return View(emp);
                }
                else
                {
                    Danger("Error retrieving details..");
                    return View();
                }
            }
            catch (Exception e)
            {
                Danger("Oops! Something went wrong.. <br> Error: " + e.Message +
                    "Message: " + e.StackTrace);
                return View();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        /// <summary>
        /// 
        /// </summary>
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AuthLog(Roles = "Admin")]
        public ActionResult Register()
        {
            IEnumerable<SelectListItem> items = context.Roles.Select(c => new SelectListItem
            {
                Value = c.Name,
                Text = c.Name

            });
            ViewBag.Role = items;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AuthLog(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, string Role)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //Assign role to user here
                    await this.UserManager.AddToRoleAsync(user.Id, Role);
                    //

                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    if(Role == "Admin" || Role == "Driver")
                    {
                        return RedirectToAction("AdditionalInformation", new { userId = user.Id, Position = Role });
                    }         
                    if(Role == "Customer")
                    {
                        return RedirectToAction("Register", "Account", new { model = model });
                    }               

                }
                AddErrors(result);
            }


            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [AuthLog(Roles = "Super Admin, Admin, Operations Manager, Driver,")]
        public ActionResult AdditionalInformation(string userId)
        {
            IEnumerable<SelectListItem> items = db.AspNetRoles.Select(c => new SelectListItem
            {
                Value = c.Name,
                Text = c.Name
            });
            ViewBag.Role = items;
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="EmployeeName"></param>
        /// <param name="EmployeeSurname"></param>
        /// <param name="IDNumber"></param>
        /// <param name="City"></param>
        /// <param name="Address"></param>
        /// <param name="PostalCode"></param>
        /// <param name="Position"></param>
        /// <param name="ContactNumber"></param>
        /// <returns></returns
        [AuthLog(Roles = "Super Admin, Admin, Operations Manager, Driver")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdditionalInformation(string userId, string EmployeeName, string EmployeeSurname, string IDNumber, string City, string Address, string PostalCode, string Position, string ContactNumber)
        {
            Employee employee = new Employee();

            employee.UserID = userId;
            employee.EmployeeName = EmployeeName;
            employee.EmployeeSurname = EmployeeSurname;
            employee.IDNumber = IDNumber;
            employee.City = City;
            employee.Address = Address;
            employee.PostalCode = PostalCode;
            employee.Position = Position;
            employee.ContactNumber = ContactNumber;
            employee.DateCreated = System.DateTime.Now;
            employee.LastModified = System.DateTime.Now;
            elogic.addEmployee(employee);

            if (Position == "Driver")
            {
                TruckDriver driver = new TruckDriver();
                driver.EmpID = employee.EmployeeID;
                driver.Avail = true;
                driver.PriorityStatus = false;
                tdLogic.addDriver(driver);
            }

            Success("Successfully Registered!");
            return RedirectToAction("Index", "Admin");

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AuthLog(Roles = "Admin")]
        public ActionResult AllDeliveries(string refkey)
        {
            if (!String.IsNullOrEmpty(refkey))
            {
                try
                {
                    List<Delivery> result = db.Delivery.ToList().Where(x => x.DeliveryRef == refkey).ToList();
                    return View(result);
                }
                catch (Exception e)
                {
                    Danger("Oops! Something went wrong.. <br> Please contact support");
                    List<Delivery> del = dlogic.returnAllDeliveries();
                    return View(del);
                    throw e;
                }
            }

            else
            {
                try
                {
                    List<Delivery> del = dlogic.returnAllDeliveries();
                    return View(del);
                }
                catch (Exception e)
                {
                    Danger("Oops! Something went wrong.. <br> Please contact support..");
                    return View();
                    throw e;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthLog(Roles = "Admin")]
        public ActionResult DeliveryDetails(int? id)
        {
            try
            {
                Delivery del = dlogic.searchDelivery(id);

                if (del != null)
                {
                    return View(del);
                }
                else
                {
                    Danger("Error retrieving details..");
                    return View();
                }
            }
            catch (Exception e)
            {
                Danger("Oops! Something went wrong.. <br> Error: " + e.Message + "Message: "
                    + e.StackTrace);
                return View();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AuthLog(Roles = "Admin")]
        public ActionResult DeliveryJobs()
        {
            try
            {
                List<Deliveryjob> result = djLogic.getAllDeliveryJobs();
                if (result != null)
                {
                    return View(result);
                }
                else
                {
                    Danger("Could not retrieve any list");
                    return View();
                }
            }
            catch (Exception e)
            {
                Danger("Oops! Something went wrong.. <br> Error: " + e.Message + "<br>Message: " + e.StackTrace);
                return View();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthLog(Roles = "Admin")]
        public ActionResult UpdateDeliveryJobStatus(int id)
        {
            try
            {
                Deliveryjob dj = db.Deliveryjob.Find(id);
                return View(dj);
            }
            catch (Exception e)
            {
                Danger("Unfortunately something went wrong.. <br>Please contact support <br>Error: " + e.Message + "<br> StackTrace: " + e.StackTrace);
                return View();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthLog(Roles = "Admin")]
        public ActionResult UpdateDeliveryJobStatus([Bind(Include = "JobID,DelID,TruckID,DriverID,JobStatus,PortDelay")] Deliveryjob dj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(dj).State = EntityState.Modified;
                    //db.SaveChanges();

                    Delivery del = db.Delivery.Where(x => x.DelID == dj.DelID).FirstOrDefault();
                    del.DeliveryStatus = dj.JobStatus;
                    db.Entry(del).State = EntityState.Modified;

                    if (dj.JobStatus.ToLower() == "delivered")
                    {
                        JobQueue jq = db.JobQueue.Where(x => x.JobID == dj.JobID).FirstOrDefault();
                        if (jq != null)
                        {
                            db.JobQueue.Attach(jq);
                            db.JobQueue.Remove(jq);

                            TruckDriver td = db.TruckDriver.Where(x => x.DriverID == dj.DriverID).FirstOrDefault();
                            Fleet truck = db.Fleet.Where(x => x.TruckId == dj.TruckID).FirstOrDefault();

                            if(td!=null&&truck!=null)
                            {
                                td.Avail = true;
                                truck.Availability = true;
                            }
                            else
                                if(td!=null&&truck==null)
                            {
                                td.Avail = true;
                            }
                            else
                                if(td==null&&truck!=null)
                            {
                                truck.Availability = true;
                            }

                            db.Entry(td).State = EntityState.Modified;
                            db.Entry(truck).State = EntityState.Modified;
                        }
                    }
                    db.SaveChanges();

                    return RedirectToAction("DeliveryJobs", "Admin");
                }
                catch (Exception e)
                {
                    Danger("Oops!! Something went wrong.. Please contact support. <br> Error: " + e.Message + "<br> StackTrace: " + e.StackTrace);
                    return View();
                }
            }
            else
            {
                ViewBag.DelID = new SelectList(db.Delivery, "DelID", "PickUpArea", dj.DelID);
                ViewBag.DriverID = new SelectList(db.TruckDriver, "DriverID", "DriverID", dj.DriverID);
                ViewBag.TruckID = new SelectList(db.Fleet, "TruckId", "TruckNumberPlate", dj.TruckID);
                return View(dj);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AuthLog(Roles = "Admin")]
        public ActionResult JobQueue()
        {
            try
            {
                List<JobQueue> result = jqLogic.returnJobQueue();
                if (result != null)
                {
                    return View(result);
                }
                else
                {
                    Danger("Could not retrieve any list");
                    return View();
                }
            }
            catch (Exception e)
            {
                Danger("Oops! Something went wrong.. <br> Error: " + e.Message + "<br>Message: " + e.StackTrace);
                return View();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AuthLog(Roles = "Admin")]
        public ActionResult ScheduleJobs()
        {
            int priorityScore = 1;
            try
            {
                foreach (Deliveryjob dj in djLogic.getAllDeliveryJobs())
                {
                    if (dj.JobStatus.ToLower() == "priority" && dj.DriverID == null && dj.TruckID == null)
                    {
                        priorityScore = 3;
                        foreach (TruckDriver td in tdLogic.returnAllDrivers())
                        {
                            if (td.Avail == true)
                            {
                                dj.DriverID = td.DriverID;
                                TruckDriver driver = tdLogic.searchDriver(td.DriverID);
                                if (driver != null)
                                {
                                    driver.Avail = false;
                                    tdLogic.updateStatus(driver);
                                    break;
                                }
                            }
                        }

                        foreach (Fleet f in flogic.returnAllTrucks())
                        {
                            if (f.Availability == true)
                            {
                                dj.TruckID = f.TruckId;
                                Fleet truck = flogic.searchTruck(f.TruckId);
                                if (f != null)
                                {
                                    truck.Availability = false;
                                    flogic.updateStatus(truck);
                                    break;
                                }
                            }
                        }
                    }
                    else
                        if (dj.JobStatus.ToLower() == "priority" && dj.DriverID != null && dj.TruckID == null)
                    {
                        priorityScore = 3;
                        foreach (Fleet f in flogic.returnAllTrucks())
                        {
                            if (f.Availability == true)
                            {
                                dj.TruckID = f.TruckId;
                                Fleet truck = flogic.searchTruck(f.TruckId);
                                if (f != null)
                                {
                                    truck.Availability = false;
                                    flogic.updateStatus(truck);
                                    break;
                                }
                            }
                        }
                    }
                    else
                        if (dj.JobStatus.ToLower() == "priority" && dj.DriverID == null && dj.TruckID != null)
                    {
                        priorityScore = 3;
                        foreach (TruckDriver td in tdLogic.returnAllDrivers())
                        {
                            if (td.Avail == true)
                            {
                                dj.DriverID = td.DriverID;
                                TruckDriver driver = tdLogic.searchDriver(td.DriverID);
                                if (driver != null)
                                {
                                    driver.Avail = false;
                                    tdLogic.updateStatus(driver);
                                    break;
                                }
                            }
                        }
                    }

                    else
                    if (dj.JobStatus.ToLower() == "waiting" && dj.DriverID == null && dj.TruckID == null)
                    {
                        priorityScore = 1;
                        foreach (TruckDriver td in tdLogic.returnAllDrivers())
                        {
                            if (td.Avail == true)
                            {
                                dj.DriverID = td.DriverID;
                                TruckDriver driver = tdLogic.searchDriver(td.DriverID);
                                if (driver != null)
                                {
                                    driver.Avail = false;
                                    tdLogic.updateStatus(driver);
                                    break;
                                }
                            }
                        }

                        foreach (Fleet f in flogic.returnAllTrucks())
                        {
                            if (f.Availability == true)
                            {
                                dj.TruckID = f.TruckId;
                                Fleet truck = flogic.searchTruck(f.TruckId);
                                if (f != null)
                                {
                                    truck.Availability = false;
                                    flogic.updateStatus(truck);
                                    break;
                                }
                            }
                        }

                    }
                    else
                        if (dj.JobStatus.ToLower() == "waiting" && dj.DriverID != null && dj.TruckID == null)
                    {
                        priorityScore = 1;
                        foreach (Fleet f in flogic.returnAllTrucks())
                        {
                            if (f.Availability == true)
                            {
                                dj.TruckID = f.TruckId;
                                Fleet truck = flogic.searchTruck(f.TruckId);
                                if (f != null)
                                {
                                    truck.Availability = false;
                                    flogic.updateStatus(truck);
                                    break;
                                }
                            }
                        }
                    }
                    else
                        if (dj.JobStatus.ToLower() == "waiting" && dj.DriverID == null && dj.TruckID != null)
                    {
                        priorityScore = 1;
                        foreach (TruckDriver td in tdLogic.returnAllDrivers())
                        {
                            if (td.Avail == true)
                            {
                                dj.DriverID = td.DriverID;
                                TruckDriver driver = tdLogic.searchDriver(td.DriverID);
                                if (driver != null)
                                {
                                    driver.Avail = false;
                                    tdLogic.updateStatus(driver);
                                    break;
                                }
                            }
                        }
                    }

                    //else if (dj.JobStatus.ToLower() == "delivered")
                    //{
                    //    dj.DriverID = null;
                    //    dj.TruckID = null;
                    //    djLogic.updateDetails(dj);
                    //    break;
                    //}

                    djLogic.updateDetails(dj);
                    JobQueue jq = jqLogic.searchItem(dj.JobID);
                    if (jq == null)
                    {
                        JobQueue newjq = new JobQueue();
                        newjq.JobID = dj.JobID;
                        newjq.PriorityScore = priorityScore;
                        newjq.QueueStatus = dj.JobStatus;

                        jqLogic.addJobToQueue(newjq);
                    }
                }
                Information("Scheduled results");
                return RedirectToAction("JobQueue", "Admin");
            }
            catch (Exception e)
            {
                Danger("Oops! Something went wrong.. <br> Error: " + e.Message + "<br>Message: " + e.StackTrace);
                return RedirectToAction("JobQueue", "Admin");
            }
        }

        [AuthLog(Roles = "Admin")]
        public ActionResult CustomerEdit(int? id)
        {
            try
            {
                Customer cust = cLogic.searchCustomer(id);
                return View(cust);
            }
            catch (Exception e)
            {
                Danger("Oops! SOmething went wrong.. Please contact support");
                return View();
                throw e;
            }
        }

        [AuthLog(Roles = "Admin")]
[HttpPost]
public ActionResult CustomerEdit(int id, string CustomerName, string CustomerSurname, string IDNumber, string City, string CustomerAddress, string PostalCode, string Email, string ContactNumber, string LoyaltyPoints)
{
    if (String.IsNullOrEmpty(CustomerName))
    {
        Danger("Please Enter Your Name!");
        Customer cust = cLogic.searchCustomer(id);
        return View(cust);
    }

    try
    {
        Customer cust = cLogic.searchCustomer(id);
        if (cust != null)
        {
            cust.UserID = User.Identity.GetUserId();
            cust.CustomerName = CustomerName;
            cust.CustomerSurname = CustomerSurname;
            cust.IDNumber = IDNumber;
            cust.City = City;
            cust.CustomerAddress = CustomerAddress;
            cust.PostalCode = PostalCode;
            cust.Email = Email;
            cust.ContactNumber = ContactNumber;
            cust.LastModified = System.DateTime.Now;
            cust.LoyaltyPoints = Convert.ToInt16(LoyaltyPoints);
            cLogic.updateDetails(cust);
        }
        Success("Successfully Updated Details!");
        return RedirectToAction("CustomerDetails", new { id = cust.CustomerID });
    }
    catch (Exception e)
    {
        Danger("Oops! Something went wrong.. <br> Please contact support..");
        return View();
        throw e;
    }
}

/// <summary>
/// 
/// </summary>
/// <param name="id"></param>
/// <returns></returns>
[AuthLog(Roles = "Admin")]
public ActionResult CustomerDetails(int? id)
{
    try
    {
        //id = cLogic.getCurrentUserId(User.Identity.GetUserId());
        Customer cust = cLogic.searchCustomer(id);
        return View(cust);
    }
    catch (Exception e)
    {
        Danger("Oops! Something went wrong.. <br> Cannot retrieve your details. Please contact support..");
        return View();
        throw e;
    }
}
    }
}