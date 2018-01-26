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

namespace ShareAVilla.Controllers
{
    public class MailboxesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Mailboxes
        public async Task<ActionResult> Index()
        {
            return View(await db.Mailbox.ToListAsync());
        }

        // GET: Mailboxes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mailbox mailbox = await db.Mailbox.FindAsync(id);
            if (mailbox == null)
            {
                return HttpNotFound();
            }
            return View(mailbox);
        }

        // GET: Mailboxes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mailboxes/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Mailbox mailbox)
        {
            if (ModelState.IsValid)
            {
                db.Mailbox.Add(mailbox);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(mailbox);
        }

        // GET: Mailboxes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mailbox mailbox = await db.Mailbox.FindAsync(id);
            if (mailbox == null)
            {
                return HttpNotFound();
            }
            return View(mailbox);
        }

        // POST: Mailboxes/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,hasNewMail")] Mailbox mailbox)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mailbox).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(mailbox);
        }

        // GET: Mailboxes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mailbox mailbox = await db.Mailbox.FindAsync(id);
            if (mailbox == null)
            {
                return HttpNotFound();
            }
            return View(mailbox);
        }

        // POST: Mailboxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Mailbox mailbox = await db.Mailbox.FindAsync(id);
            db.Mailbox.Remove(mailbox);
            await db.SaveChangesAsync();
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
