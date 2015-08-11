using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using VodafoneWeb.Models;

namespace VodafoneWeb.Controllers
{
    [Authorize]
    public class DealersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //private ApplicationDbContext db;

        //public DealersController()
        //{
        //    this.db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
        //}

        // GET: Dealers
        public async Task<ActionResult> Index()
        {
            return View(await db.Dealers.ToListAsync());
        }

        // GET: Dealers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dealer dealer = await db.Dealers.FindAsync(id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            return View(dealer);
        }

        // GET: Dealers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dealers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,DealerCode,IsActive")] Dealer dealer)
        {
            if (ModelState.IsValid)
            {
                db.Dealers.Add(dealer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(dealer);
        }

        // GET: Dealers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dealer dealer = await db.Dealers.FindAsync(id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            return View(dealer);
        }

        // POST: Dealers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,DealerCode,IsActive")] Dealer dealer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dealer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(dealer);
        }

        // GET: Dealers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dealer dealer = await db.Dealers.FindAsync(id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            return View(dealer);
        }

        // POST: Dealers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Dealer dealer = await db.Dealers.FindAsync(id);
            db.Dealers.Remove(dealer);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Get: Dealers/SelectDealer
        public async Task<ActionResult> SelectDealer()
        {
            return PartialView(await db.Dealers.ToListAsync());
        }

        // POST: Dealers/SelectDealer/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToRouteResult SelectDealer([Bind(Include = "ID,Name,DealerCode,IsActive")] Dealer dealer)
        {
            HelperMethods.SetDealerId(dealer.ID);
            return RedirectToAction("Index", "Home");
        }

        //public async Task<ActionResult> _SelectedDealer()
        //{
        //    int? id = HelperMethods.GetDealerId();
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Dealer dealer = await db.Dealers.FindAsync(id);
        //    if (dealer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return PartialView(dealer);
        //}


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
