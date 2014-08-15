using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TourGuide.Models;
using PagedList;
using System.Data.Entity.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace TourGuide.Controllers
{
    public class LocationController : Controller
    {

        private TourGuideEntities db = new TourGuideEntities();

        // GET: Location
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.StateSortParm = sortOrder == "State" ? "state_desc" : "State";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var locations = from l in db.Locations
                            select l;
            if (!String.IsNullOrEmpty(searchString))
            {
                locations = locations.Where(l => l.Name.ToUpper().Contains(searchString.ToUpper())
                                       || l.State.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    locations = locations.OrderByDescending(l => l.Name);
                    break;
                case "State":
                    locations = locations.OrderBy(l => l.State);
                    break;
                case "state_desc":
                    locations = locations.OrderByDescending(l => l.State);
                    break;
                default:
                    locations = locations.OrderBy(l => l.Name);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(locations.ToPagedList(pageNumber, pageSize));
        }

        // GET: Location/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // GET: Location/Create
        [Authorize(Users = "teamproject@tourguide.com")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Location/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "teamproject@tourguide.com")]
        public ActionResult Create([Bind(Include = "ID,Name,State")] Location location)
        {
            if (ModelState.IsValid)
            {
                var Locations = from l in db.Locations
                                where l.Name == location.Name
                                where l.State == location.State
                                select l;

                if (Locations.Any())
                {
                    ModelState.AddModelError("Name", "This city already exists.");
                    return View(location);
                }


                db.Locations.Add(location);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(location);
        }

        // GET: Location/Edit/5
        [Authorize(Users = "teamproject@tourguide.com")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Location/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "teamproject@tourguide.com")]
        public ActionResult Edit([Bind(Include = "ID,Name,State")] Location location)
        {
            if (ModelState.IsValid)
            {
                db.Entry(location).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(location);
        }

        // GET: Location/Delete/5
        [Authorize(Users = "teamproject@tourguide.com")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Location/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "teamproject@tourguide.com")]
        public ActionResult DeleteConfirmed(int id)
        {
            Location location = db.Locations.Find(id);
            db.Locations.Remove(location);
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

        public string locations { get; set; }
    }
}