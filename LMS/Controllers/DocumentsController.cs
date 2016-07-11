using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMS.Models;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace LMS.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // ***
        public ActionResult AssignmentDocsPartial(int? id)
        {
            Activity activity = db.Activities.Where(c => c.Id == id).FirstOrDefault();
            ShowDocumentsViewModel showDocViewModel = new ShowDocumentsViewModel();
            showDocViewModel.Id = activity.Id;
            showDocViewModel.Name = activity.Name;
            showDocViewModel.ParentType = DocParent.Activity;
            showDocViewModel.ParentClass = "activity";

            showDocViewModel.Documents = db.Documents.Where(m => m.Activity.Id == activity.Id &&
                                                           m.DocumentType == DocType.Assignment).ToList();
            return PartialView("AssignmentDocsPartial", showDocViewModel);
        }

        // ***
        public ActionResult StudentDocsPartial(int? id)
        {
            ApplicationUser student = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            Activity activity = db.Activities.Where(c => c.Id == id).FirstOrDefault();
            ShowDocumentsViewModel showDocViewModel = new ShowDocumentsViewModel();
            showDocViewModel.Id = activity.Id;
            showDocViewModel.Name = activity.Name;
            showDocViewModel.ParentType = DocParent.Activity;
            showDocViewModel.ParentClass = "activity";

            showDocViewModel.Documents = db.Documents.Where(m => m.Activity.Id == activity.Id && 
                                                           m.DocumentType == DocType.Assignment &&
                                                           m.User.UserName == student.UserName).ToList();
            return PartialView("StudentDocsPartial", showDocViewModel);
        }

        // ***
        public ActionResult ReturnToIndex(int? id)
        {
            Document dlDoc = db.Documents.FirstOrDefault(m => m.Id == id);
            if (dlDoc.Course != null)
            {
                Course course = db.Courses.Where(c => c.Id == dlDoc.Course.Id).FirstOrDefault();
                return (ActionResult)FromCourse(course.Id);
            }
            else if (dlDoc.Module != null)
            {
                Module module = db.Modules.Where(c => c.Id == dlDoc.Module.Id).FirstOrDefault();
                return (ActionResult)FromModule(module.Id);
            }
            else
            {
                Activity activity = db.Activities.Where(c => c.Id == dlDoc.Activity.Id).FirstOrDefault();
                return (ActionResult)FromActivity(activity.Id);
            }
        }

        // ***
        public ActionResult ReturnToList(ShowDocumentsViewModel addDocView)
        {
            switch (addDocView.ParentType)
            {
                case DocParent.Course:
                    return RedirectToAction("Index", "Courses");

                case DocParent.Module:
                    Module module = db.Modules.Where(c => c.Id == addDocView.Id).FirstOrDefault();
                    return RedirectToAction("Index", "Modules", new { module.Course.Id });

                case DocParent.Activity:
                    Activity activity = db.Activities.Where(c => c.Id == addDocView.Id).FirstOrDefault();
                    return RedirectToAction("Index", "Activities", new { activity.Module.Id });

                default:
                    return RedirectToAction("Index");
            }
        }

        // ***
        public ActionResult FromCreateView(CreateDocumentViewModel docViewModel)
        {
            switch (docViewModel.ParentType)
            {
                case DocParent.Course:
                    return RedirectToAction("FromCourse", new { docViewModel.Id });

                case DocParent.Module:
                    return RedirectToAction("FromModule", new { docViewModel.Id });

                case DocParent.Activity:
                default:
                    return RedirectToAction("FromActivity", new { docViewModel.Id });
            }
        }

        // ***
        public ActionResult FromCourse(int? id)
        {
            Course course = db.Courses.Where(c => c.Id == id).FirstOrDefault();
            ShowDocumentsViewModel showDocViewModel = new ShowDocumentsViewModel();
            showDocViewModel.Id = course.Id;
            showDocViewModel.Name = course.Name;
            showDocViewModel.ParentType = DocParent.Course;
            showDocViewModel.ParentClass = "course";
            showDocViewModel.Documents = db.Documents.Where(m => m.Course.Id == course.Id).ToList();

            return View("Index", showDocViewModel);
        }

        // ***
        public ActionResult FromModule(int? id)
        {
            Module module = db.Modules.Where(c => c.Id == id).FirstOrDefault();
            ShowDocumentsViewModel showDocViewModel = new ShowDocumentsViewModel();
            showDocViewModel.Id = module.Id;
            showDocViewModel.Name = module.Name;
            showDocViewModel.ParentType = DocParent.Module;
            showDocViewModel.ParentClass = "module";
            showDocViewModel.Documents = db.Documents.Where(m => m.Module.Id == module.Id).ToList();

            return View("Index", showDocViewModel);
        }

        // ***
        public ActionResult FromActivity(int? id)
        {
            Activity activity = db.Activities.Where(c => c.Id == id).FirstOrDefault();
            ShowDocumentsViewModel showDocViewModel = new ShowDocumentsViewModel();
            showDocViewModel.Id = activity.Id;
            showDocViewModel.Name = activity.Name;
            showDocViewModel.ParentType = DocParent.Activity;
            showDocViewModel.ParentClass = "activity";
            showDocViewModel.Documents = db.Documents.Where(m => m.Activity.Id == activity.Id && m.DocumentType != DocType.Assignment).ToList();

            if (User.IsInRole("Student"))
            {
                return View("StudentDocIndex", showDocViewModel);
            }
            else
            {
                return View("ActivityDocIndex", showDocViewModel);
            }
        }

        // GET: Documents
        public ActionResult Index(ShowDocumentsViewModel showDocViewModel)
        {
            return View(showDocViewModel);
        }

        // GET: Documents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // GET: Documents/Create
        public ActionResult Create(ShowDocumentsViewModel addDocView)
        {
            CreateDocumentViewModel createDocView = new CreateDocumentViewModel();
            createDocView.Id = addDocView.Id;
            createDocView.Name = addDocView.Name;
            createDocView.ParentClass = addDocView.ParentClass;
            createDocView.ParentType = addDocView.ParentType;

            createDocView.Document = new Document();

            ApplicationUser currUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (User.IsInRole("Student"))
            {
                createDocView.Document.DocumentType = DocType.Assignment;
            }

            return View(createDocView);
        }

        // POST: Documents/Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDocument([Bind(Include = "Id,Name,ParentClass,ParentType,Document")] CreateDocumentViewModel createDocView, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                Document newDoc = new Document();

                if (file != null && file.ContentLength > 0)
                    try
                    {
                        string fileName1 = Path.GetFileName(file.FileName);
                        string fileName2 = fileName1;
                        string pattern;
                        string replacement;
                        Regex rgx;

                        if (fileName1.Contains("#"))
                        {
                            pattern = "#";
                            replacement = "x";
                            rgx = new Regex(pattern);
                            fileName2 = rgx.Replace(fileName1, replacement);
                        }

                        fileName1 = fileName2;
                        if (fileName1.Contains("%"))
                        {
                            pattern = "%";
                            replacement = "p";
                            rgx = new Regex(pattern);
                            fileName2 = rgx.Replace(fileName1, replacement);
                        }

                        string filePath = Path.Combine(Server.MapPath("~/Content/Files"), fileName2);
                        file.SaveAs(filePath);
                        newDoc.Url = fileName2;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        return View(createDocView);
                    }
                else
                {
                    ViewBag.Message = "You have not specified a file.";
                    return View(createDocView);
                }
                newDoc.User = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                newDoc.Name = createDocView.Document.Name;
                newDoc.Description = createDocView.Document.Description;
                newDoc.DocumentType = createDocView.Document.DocumentType;
                newDoc.CreationDate = DateTime.Now;

                switch (createDocView.ParentType)
                {
                    case DocParent.Course:
                        newDoc.Course = db.Courses.Where(c => c.Id == createDocView.Id).FirstOrDefault();
                        break;

                    case DocParent.Module:
                        newDoc.Module = db.Modules.Where(c => c.Id == createDocView.Id).FirstOrDefault();
                        break;

                    case DocParent.Activity:
                        newDoc.Activity = db.Activities.Where(c => c.Id == createDocView.Id).FirstOrDefault();
                        break;

                    default:
                        break;
                }
                
                db.Documents.Add(newDoc);
                db.SaveChanges();

                switch (createDocView.ParentType)
                {
                    case DocParent.Course:
                        return RedirectToAction("FromCourse", new { createDocView.Id });

                    case DocParent.Module:
                        return RedirectToAction("FromModule", new { createDocView.Id });

                    case DocParent.Activity:
                        return RedirectToAction("FromActivity", new { createDocView.Id });

                    default:
                        return RedirectToAction("Index");
                }
            }
            return View(createDocView);
        }

        // GET: Documents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Documents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,DocumentType,CreationDate")] Document document)
        {
            if (ModelState.IsValid)
            {
                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(document);
        }

        // GET: Documents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Document document = db.Documents.Find(id);
            if (document.Course != null)
            {
                Course course = db.Courses.Where(c => c.Id == document.Course.Id).FirstOrDefault();
                db.Documents.Remove(document);
                db.SaveChanges();
                return (ActionResult)FromCourse(course.Id);
            }
            else if (document.Module != null)
            {
                Module module = db.Modules.Where(c => c.Id == document.Module.Id).FirstOrDefault();
                db.Documents.Remove(document);
                db.SaveChanges();
                return (ActionResult)FromModule(module.Id);
            }
            else
            {
                Activity activity = db.Activities.Where(c => c.Id == document.Activity.Id).FirstOrDefault();
                db.Documents.Remove(document);
                db.SaveChanges();
                return (ActionResult)FromActivity(activity.Id);
            }
        }

        // ***
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
