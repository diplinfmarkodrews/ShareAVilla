using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShareAVilla.Models;
using ShareAVilla.Models.Accommodation;

namespace ShareAVilla.Controllers
{
    public class AccommodationsController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();


        public static async Task<Accommodation> LoadAccommodation(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Configuration.LazyLoadingEnabled = false;
                Accommodation accom = await db.Accommodations.Include(p => p.BedRooms).Where(p => p.ID == id).FirstOrDefaultAsync();
               
                accom.AccomProfile = await db.AccommProfile.FindAsync(accom.AccomProfileID);
                return accom;

            }
        }
        //// GET: Accommodations
        //public async Task<ActionResult> Index()
        //{
        //    var accommodations = db.Accommodations.Include(a => a.AccomProfile);
        //    return View(await accommodations.ToListAsync());
        //}

        //// GET: Accommodations/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Accommodation accommodation = await db.Accommodations.FindAsync(id);
        //    if (accommodation == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(accommodation);
        //}

        //// GET: Accommodations/Create
        //public ActionResult Create()
        //{
        //    ViewBag.AccomProfileID = new SelectList(db.AccommProfile, "ID", "AType");
        //    return View();
        //}

        //// POST: Accommodations/Create
        //// Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        //// finden Sie unter https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "ID,AccomProfileID,Verified,objState")] Accommodation accommodation)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Accommodations.Add(accommodation);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.AccomProfileID = new SelectList(db.AccommProfile, "ID", "AType", accommodation.AccomProfileID);
        //    return View(accommodation);
        //}

        //// GET: Accommodations/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Accommodation accommodation = await db.Accommodations.FindAsync(id);
        //    if (accommodation == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.AccomProfileID = new SelectList(db.AccommProfile, "ID", "AType", accommodation.AccomProfileID);
        //    return View(accommodation);
        //}

        //// POST: Accommodations/Edit/5
        //// Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        //// finden Sie unter https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "ID,AccomProfileID,Verified,objState")] Accommodation accommodation)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(accommodation).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.AccomProfileID = new SelectList(db.AccommProfile, "ID", "AType", accommodation.AccomProfileID);
        //    return View(accommodation);
        //}

        //// GET: Accommodations/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Accommodation accommodation = await db.Accommodations.FindAsync(id);
        //    if (accommodation == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(accommodation);
        //}

        //// POST: Accommodations/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Accommodation accommodation = await db.Accommodations.FindAsync(id);
        //    db.Accommodations.Remove(accommodation);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
