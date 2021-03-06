﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TourGuide.Models;
using PagedList;

namespace TourGuide.Controllers
{
    public class HotelController : Controller
    {
        private TourGuideEntities db = new TourGuideEntities();

        // GET: Hotel
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
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

            var hotels = from h in db.Hotels
                         select h;

            if (!String.IsNullOrEmpty(searchString))
            {
                hotels = hotels.Where(h =>
                  h.Name.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    hotels = hotels.OrderByDescending(h => h.Name);
                    break;
                case "City":
                    hotels = hotels.OrderBy(h => h.Location.Name);
                    break;
                case "city_desc":
                    hotels = hotels.OrderByDescending(h => h.Location.Name);
                    break;
                case "State":
                    hotels = hotels.OrderBy(h => h.Location.State);
                    break;
                case "state_desc":
                    hotels = hotels.OrderByDescending(h => h.Location.State);
                    break;
                default:
                    hotels = hotels.OrderBy(h => h.Name);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(hotels.ToPagedList(pageNumber, pageSize));
        }

        // GET: Hotel/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // GET: Hotel/Create
        [Authorize(Users = "teamproject@tourguide.com")]
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(db.Locations, "ID", "Name", "State");
            return View();
        }

        // POST: Hotel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "teamproject@tourguide.com")]
        public ActionResult Create([Bind(Include = "ID,CityID,Name,Address,ZipCode,Phone,URL,Rating,Lat,Long")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                db.Hotels.Add(hotel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityID = new SelectList(db.Locations, "ID", "Name", hotel.CityID);
            return View(hotel);
        }

        // GET: Hotel/Edit/5
        [Authorize(Users = "teamproject@tourguide.com")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(db.Locations, "ID", "Name", hotel.CityID);
            return View(hotel);
        }

        // POST: Hotel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "teamproject@tourguide.com")]
        public ActionResult Edit([Bind(Include = "ID,CityID,Name,Address,ZipCode,Phone,URL,Rating,Lat,Long")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hotel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(db.Locations, "ID", "Name", hotel.CityID);
            return View(hotel);
        }

        // GET: Hotel/Delete/5
        [Authorize(Users = "teamproject@tourguide.com")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // POST: Hotel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "teamproject@tourguide.com")]
        public ActionResult DeleteConfirmed(int id)
        {
            Hotel hotel = db.Hotels.Find(id);
            db.Hotels.Remove(hotel);
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
