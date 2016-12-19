using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NCIProjects.Models;

using System.Data.Entity.Infrastructure;
using PagedList;

namespace NCIProjects.Controllers
{
    public class SubmissionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Submissions
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else { searchString = currentFilter; }

            ViewBag.CurrentFilter = searchString;

            //var submissions = db.Submissions.Include(s => s.Student).Include(s => s.StudentTechnologies);
            var submissions = db.Submissions.Include(s => s.Student);

            if(!String.IsNullOrEmpty(searchString)) { submissions = submissions.Where(s => s.Student.lname.ToUpper().Contains(searchString.ToUpper()) || s.Student.fname.ToUpper().Contains(searchString.ToUpper())); }

            //sort Students by Last Name
            switch (sortOrder)
            {
                case "name_desc":
                    submissions = submissions.OrderByDescending(s => s.Student.lname);
                    break;
                default:
                    submissions = submissions.OrderBy(s => s.Student.lname);
                    break;
            }
            int pageSize = 2; int pageNumber = (page ?? 1);

            return View(submissions.ToPagedList(pageNumber, pageSize));
        }

        // GET: Submissions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Submission submission = db.Submissions.Include(s => s.Student.Files).SingleOrDefault(s => s.ID == id);
            //  Submission submission = db.Submissions.Find(id);
            // Student student = db.Students.Include(s => s.Files).SingleOrDefault(s => s.StudentID == id);
            if (submission == null)
            {
                return HttpNotFound();
            }
            return View(submission);
        }

        // GET: Submissions/Create
        public ActionResult Create()
        {
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "StudentDetails");
          //ViewBag.StudentTechnologiesID = new SelectList(db.StudentTechnologies, "ID", "ID");
            return View();
        }

        // POST: Submissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,StudentID,linkedin_url,project_title,short_desc,long_desc,StudentTechnologiesID")] Submission submission)
        {
            if (ModelState.IsValid)
            {
                db.Submissions.Add(submission);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "StudentDetails", submission.StudentID);
          //ViewBag.StudentTechnologiesID = new SelectList(db.StudentTechnologies, "ID", "ID", submission.StudentTechnologiesID);
            return View(submission);
        }

        // GET: Submissions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Submission submission = db.Submissions.Find(id);
            if (submission == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "StudentDetails", submission.StudentID);
          //ViewBag.StudentTechnologiesID = new SelectList(db.StudentTechnologies, "ID", "ID", submission.StudentTechnologiesID);
            return View(submission);
        }

        // POST: Submissions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,StudentID,linkedin_url,project_title,short_desc,long_desc,StudentTechnologiesID")] Submission submission)
        {
            if (ModelState.IsValid)
            {
                db.Entry(submission).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "StudentDetails", submission.StudentID);
           //ViewBag.StudentTechnologiesID = new SelectList(db.StudentTechnologies, "ID", "ID", submission.StudentTechnologiesID);
            return View(submission);
        }

        // GET: Submissions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Submission submission = db.Submissions.Find(id);
            if (submission == null)
            {
                return HttpNotFound();
            }
            return View(submission);
        }

        // POST: Submissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Submission submission = db.Submissions.Find(id);
            db.Submissions.Remove(submission);
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
    }
}
