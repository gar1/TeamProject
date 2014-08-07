using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TourGuide.Models;


namespace TourGuide.Controllers
{
    public class RestaurantController : Controller
    {
        private TourGuideEntities db = new TourGuideEntities();
        // GET: Restaurant
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CitySortParm = sortOrder == "City" ? "city_desc" : "City";
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
            var restaurants = from r in db.Restaurants
                           select r;

            if (!String.IsNullOrEmpty(searchString))
            {
                restaurants = restaurants.Where(r => r.Name.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    restaurants = restaurants.OrderByDescending(r => r.Name);
                    break;
                case "City":
                    restaurants = restaurants.OrderBy(r => r.Location.Name);
                    break;
                case "city_desc":
                    restaurants = restaurants.OrderByDescending(r => r.Location.Name);
                    break;
                case "State":
                    restaurants = restaurants.OrderBy(r => r.Location.State);
                    break;
                case "state_desc":
                    restaurants = restaurants.OrderByDescending(r => r.Location.State);
                    break;
                default:
                    restaurants = restaurants.OrderBy(r => r.Name);
                    break;
            }
            //int pageSize = 3;
            //int pageNumber = (page ?? 1);
            //return View(restaurants.ToPagedList(pageNumber, pageSize));
            return View(restaurants.ToList());
        }


        // GET: Restaurant/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            return View(restaurant);
        }

        // GET: Restaurant/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(db.Locations, "ID", "Name");
            return View();
        }

        // POST: Restaurant/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CityID,Name,Address,ZipCode,Phone,URL,Rating,Lat,Long")] Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                db.Restaurants.Add(restaurant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityID = new SelectList(db.Locations, "ID", "Name", restaurant.CityID);
            return View(restaurant);
        }

        // GET: Restaurant/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(db.Locations, "ID", "Name", restaurant.CityID);
            return View(restaurant);
        }

        // POST: Restaurant/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CityID,Name,Address,ZipCode,Phone,URL,Rating,Lat,Long")] Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(restaurant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(db.Locations, "ID", "Name", restaurant.CityID);
            return View(restaurant);
        }

        // GET: Restaurant/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            return View(restaurant);
        }

        // POST: Restaurant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Restaurant restaurant = db.Restaurants.Find(id);
            db.Restaurants.Remove(restaurant);
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
