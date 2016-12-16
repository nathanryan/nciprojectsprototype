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
    public class StudentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Students
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else { searchString = currentFilter; }

            ViewBag.CurrentFilter = searchString;

            var students = from s in db.Students
                           select s;

            if (!String.IsNullOrEmpty(searchString)) { students = students.Where(s => s.lname.ToUpper().Contains(searchString.ToUpper()) || s.fname.ToUpper().Contains(searchString.ToUpper())); }

            //sort Students by Last Name
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.lname);
                    break;
                default:
                    students = students.OrderBy(s => s.lname);
                    break;
            }
            int pageSize = 6; int pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize));
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Include(s => s.Files).SingleOrDefault(s => s.StudentID == id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "ID", "course_name");
            ViewBag.StreamID = new SelectList(db.Streams, "ID", "stream_name");
            ViewBag.SupervisorID = new SelectList(db.Supervisors, "ID", "SupervisorDetails");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentID,StudentNumber,fname,lname,CourseID,StreamID,SupervisorID")] Student student, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var avatar = new File
                    {
                        FileName = System.IO.Path.GetFileName(upload.FileName),
                        FileType = FileType.Avatar,
                        ContentType = upload.ContentType
                    };
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        avatar.Content = reader.ReadBytes(upload.ContentLength);
                    }
                    student.Files = new List<File> { avatar };
                }

                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Courses, "ID", "course_name", student.CourseID);
            ViewBag.StreamID = new SelectList(db.Streams, "ID", "stream_name", student.StreamID);
            ViewBag.SupervisorID = new SelectList(db.Supervisors, "ID", "SupervisorDetails", student.SupervisorID);
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Include(s => s.Files).SingleOrDefault(s => s.StudentID == id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "ID", "course_name", student.CourseID);
            ViewBag.StreamID = new SelectList(db.Streams, "ID", "stream_name", student.StreamID);
            ViewBag.SupervisorID = new SelectList(db.Supervisors, "ID", "SupervisorDetails", student.SupervisorID);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, HttpPostedFileBase upload)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var studentToUpdate = db.Students.Find(id);
            if (TryUpdateModel(studentToUpdate, "",
                new string[] { "StudentNumber", "fname", "lname", "CourseID", "StreamID", "SupervisorID"}))
            {
                try
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        if (studentToUpdate.Files.Any(f => f.FileType == FileType.Avatar))
                        {
                            db.Files.Remove(studentToUpdate.Files.First(f => f.FileType == FileType.Avatar));
                        }
                        var avatar = new File
                        {
                            FileName = System.IO.Path.GetFileName(upload.FileName),
                            FileType = FileType.Avatar,
                            ContentType = upload.ContentType
                        };
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            avatar.Content = reader.ReadBytes(upload.ContentLength);
                        }
                        studentToUpdate.Files = new List<File> { avatar };
                    }
                    db.Entry(studentToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            ViewBag.CourseID = new SelectList(db.Courses, "ID", "course_name", studentToUpdate.CourseID);
            ViewBag.StreamID = new SelectList(db.Streams, "ID", "stream_name", studentToUpdate.StreamID);
            ViewBag.SupervisorID = new SelectList(db.Supervisors, "ID", "SupervisorDetails", studentToUpdate.SupervisorID);
            return View(studentToUpdate);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
