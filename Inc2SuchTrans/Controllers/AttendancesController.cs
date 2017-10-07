using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using Inc2SuchTrans.Models;

namespace WebApplication1.Controllers
{
    public class AttendancesController : Controller
    {
        private STLogisticsEntities db = new STLogisticsEntities();
        Attendance at = new Attendance();
        // GET: Attendances
        public ActionResult Index()
        {
            var attendances = db.Attendance.Include(a => a.DriverNumber);
            return View(attendances.ToList());
        }

        // GET: Attendances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendance.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // GET: Attendances/Create
        public ActionResult Create()
        {
            Attendance newatt = new Attendance();
            newatt.Date = DateTime.Today;
            newatt.Check_In_Time = DateTime.Now;
            ViewBag.DriverID = new SelectList(db.TruckDriver, "DriverID", "DriverID");
            return View(newatt);
        }

        // POST: Attendances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AttendanceID,AttendanceDate,CheckInTime,CheckOutTime,DriverID")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Attendance.Add(attendance);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    return RedirectToAction("Create");
                }
            }

            ViewBag.DriverID = new SelectList(db.TruckDriver, "DriverID", "LastName", attendance.DriverNumber);
            return View(attendance);
        }

        // GET: Attendances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendance.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            ViewBag.DriverID = new SelectList(db.TruckDriver, "DriverID", "LastName", attendance.DriverNumber);
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AttendanceID,AttendanceDate,CheckInTime,CheckOutTime,DriverID")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attendance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DriverID = new SelectList(db.TruckDriver, "DriverID", "LastName", attendance.DriverNumber);
            return View(attendance);
        }

        // GET: Attendances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendance.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Attendance attendance = db.Attendance.Find(id);
            db.Attendance.Remove(attendance);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //create pdf
        public FileResult CreatePfd()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created 
            string strPDFFileName = string.Format("AttendanceRecords" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Rectangle rec = new Rectangle(PageSize.A4);
            Document doc = new Document(rec);
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table with 5 columns
            PdfPTable tableLayout = new PdfPTable(7);
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table


            string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);

            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            //Add Content to PDF 
            doc.Add(Add_Content_To_PDF(tableLayout));

            // Closing the document
            doc.Close();



            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return File(workStream, "application/pdf", strPDFFileName);

        }

        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)
        {

            float[] headers = { 25, 25, 25, 25, 25, 25, 25 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top

            List<Attendance> attList = db.Attendance.ToList<Attendance>();

            tableLayout.AddCell(new PdfPCell(new Phrase("\n" + "\n" + "DRIVER ATTENDANCE LIST", new Font(Font.FontFamily.TIMES_ROMAN, 20, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 6, HorizontalAlignment = Element.ALIGN_CENTER });

            ////Add header
            AddCellToHeader(tableLayout, "ATTENDANCE NO.");
            AddCellToHeader(tableLayout, "ATTENDANCE DATE");
            AddCellToHeader(tableLayout, "NAME");
            AddCellToHeader(tableLayout, "SURNAME");
            AddCellToHeader(tableLayout, "DRIVER NUMBER");
            AddCellToHeader(tableLayout, "CHECK-IN-TIME");
            AddCellToHeader(tableLayout, "CHECK-OUT-TIME");

            ////Add body

            foreach (var dr in attList)
            {

                AddCellToBody(tableLayout, dr.Attendance_No.ToString());
                AddCellToBody(tableLayout, dr.Date.ToString());
                //AddCellToBody(tableLayout, dr.TruckDriver.FirstName);
                //AddCellToBody(tableLayout, dr.TruckDriver.LastName);
                AddCellToBody(tableLayout, dr.TruckDriver.DriverID.ToString());
                AddCellToBody(tableLayout, dr.Check_In_Time.ToString());
                AddCellToBody(tableLayout, dr.Check_Out_Time.ToString());

            }

            return tableLayout;
        }

        // Method to add single cell to the Header
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.TIMES_ROMAN, 12, 1, iTextSharp.text.BaseColor.BLUE))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 6, BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255) });
        }

        // Method to add single cell to the body
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.TIMES_ROMAN, 12, 1, iTextSharp.text.BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 6, BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255) });
        }



    }
}
