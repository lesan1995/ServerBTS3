using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ServerBTS2.Models;
using Microsoft.AspNet.Identity;
using System.Text.RegularExpressions;

namespace ServerBTS2.Controllers
{
    [Authorize]
    public class HinhAnhTramsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/HinhAnhTrams
        public IEnumerable<HinhAnhTram> GetHinhAnhTrams()
        {
            if (isAdmin())
                return db.HinhAnhTrams;
            else
            {
                string id = User.Identity.GetUserId();
                var tram = db.Trams.Where(u => u.IDQuanLy == id);
                return db.HinhAnhTrams.Where(u => tram.Count(p => p.IDTram == u.IDTram) > 0).ToList();

            }

        }

        // GET: api/HinhAnhTrams/5
        [ResponseType(typeof(HinhAnhTram))]
        public IHttpActionResult GetHinhAnhTram(int id)
        {
            HinhAnhTram hinhAnhTram = db.HinhAnhTrams.Find(id);
            if (hinhAnhTram == null)
            {
                return NotFound();
            }
            var tmp = db.Trams.SingleOrDefault(u => u.IDTram == hinhAnhTram.IDTram);
            if (!isAccess(tmp.IDQuanLy)) return StatusCode(HttpStatusCode.Unauthorized);
            return Ok(hinhAnhTram);
        }
        // GET: api/HinhAnhTrams/5
        [ResponseType(typeof(HinhAnhTram))]
        [Route("api/HinhAnhTramsByIDTram")]
        public IHttpActionResult GetHinhAnhsTramByIDTram(int id)
        {
            var tmp = db.Trams.SingleOrDefault(u => u.IDTram == id);
            if (tmp == null) return NotFound();
            if (!isAccess(tmp.IDQuanLy)) return StatusCode(HttpStatusCode.Unauthorized);
            var hinhAnhTram = db.HinhAnhTrams.Where(u=>u.IDTram==id);
            if (hinhAnhTram == null)
            {
                return NotFound();
            }
            
            return Ok(hinhAnhTram);
        }
        // GET: api/HinhAnhTrams/5
        [ResponseType(typeof(HinhAnhTram))]
        [Route("api/HinhAnhTramByIDTram")]
        public IHttpActionResult GetHinhAnhTramByIDTram(int id)
        {
            var tmp = db.Trams.SingleOrDefault(u => u.IDTram == id);
            if (tmp == null) return NotFound();
            if (!isAccess(tmp.IDQuanLy)) return StatusCode(HttpStatusCode.Unauthorized);
            var hinhAnhTram = db.HinhAnhTrams.Where(u => u.IDTram == id).Take(1);
            if (hinhAnhTram == null)
            {
                return NotFound();
            }

            return Ok(hinhAnhTram);
        }

        // PUT: api/HinhAnhTrams/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutHinhAnhTram(int id, HinhAnhTram hinhAnhTram)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hinhAnhTram.IDHinhAnh)
            {
                return BadRequest();
            }
            HinhAnhTram hinhAnhTramBefore = db.HinhAnhTrams.Find(id);
            if (hinhAnhTramBefore == null) return NotFound();
            var tmp = db.Trams.SingleOrDefault(u => u.IDTram == hinhAnhTramBefore.IDTram);
            if (!isAccess(tmp.IDQuanLy))
                return StatusCode(HttpStatusCode.Unauthorized);

            hinhAnhTramBefore.Ten = hinhAnhTram.Ten;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HinhAnhTramExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/HinhAnhTrams
        [ResponseType(typeof(HinhAnhTram))]
        public IHttpActionResult PostHinhAnhTram(HinhAnhTram hinhAnhTram)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var tmp = db.Trams.SingleOrDefault(u => u.IDTram == hinhAnhTram.IDTram);
            if(tmp==null) return NotFound();
            if (!isAccess(tmp.IDQuanLy))
                return StatusCode(HttpStatusCode.Unauthorized);

            db.HinhAnhTrams.Add(hinhAnhTram);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = hinhAnhTram.IDHinhAnh }, hinhAnhTram);
        }

        // DELETE: api/HinhAnhTrams/5
        [ResponseType(typeof(HinhAnhTram))]
        public IHttpActionResult DeleteHinhAnhTram(int id)
        {
            HinhAnhTram hinhAnhTram = db.HinhAnhTrams.Find(id);
            if (hinhAnhTram == null)
            {
                return NotFound();
            }
            var tmp = db.Trams.SingleOrDefault(u => u.IDTram == hinhAnhTram.IDTram);
            if (!isAccess(tmp.IDQuanLy)) return StatusCode(HttpStatusCode.Unauthorized);

            db.HinhAnhTrams.Remove(hinhAnhTram);
            db.SaveChanges();

            return Ok(hinhAnhTram);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HinhAnhTramExists(int id)
        {
            return db.HinhAnhTrams.Count(e => e.IDHinhAnh == id) > 0;
        }
        private bool isAdmin()
        {
            var idUser = User.Identity.GetUserId();
            var role = db.UserBTSs.SingleOrDefault(u => u.IDUser == idUser).ChucVu;
            return role.Equals("Admin");
        }
        private bool isAccess(string idQuanLy)
        {
            if (isAdmin()) return true;
            return idQuanLy.Equals(User.Identity.GetUserId());
        }
    }
}