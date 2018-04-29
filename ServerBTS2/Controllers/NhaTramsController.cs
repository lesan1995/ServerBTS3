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

namespace ServerBTS2.Controllers
{
    [Authorize]
    public class NhaTramsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/NhaTrams
        public IEnumerable<NhaTram> GetNhaTrams()
        {
            if (isAdmin())
                return db.NhaTrams;
            else
            {
                string id = User.Identity.GetUserId();
                var tram = db.Trams.Where(u => u.IDQuanLy == id);
                return db.NhaTrams.Where(u=> tram.Count(p => p.IDTram==u.IDTram)>0).ToList();

            }
        }

        // GET: api/NhaTrams/5
        [ResponseType(typeof(NhaTram))]
        public IHttpActionResult GetNhaTram(int id)
        {
            NhaTram nhaTram = db.NhaTrams.Find(id);
            if (nhaTram == null)
            {
                return NotFound();
            }
            var tmp = db.Trams.SingleOrDefault(u => u.IDTram == nhaTram.IDTram);
            if (!isAccess(tmp.IDQuanLy))
                return StatusCode(HttpStatusCode.Unauthorized);

            return Ok(nhaTram);

        }
        // GET: api/NhaTrams/5
        [ResponseType(typeof(NhaTram))]
        [Route("api/NhaTramByIDTram")]
        public IEnumerable<NhaTram> GetNhaTramByIDTram(int id)
        {
            var tram = db.Trams.SingleOrDefault(u=> u.IDTram== id);
            if (tram == null)
            {
                return null;
            }
            if (!isAccess(tram.IDQuanLy))
                return null;
            return db.NhaTrams.Where(u => u.IDTram== id).ToList();

        }

        // PUT: api/NhaTrams/5
        [Authorize(Roles = "QuanLy")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNhaTram(int id, NhaTram nhaTram)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != nhaTram.IDNhaTram)
            {
                return BadRequest();
            }


            NhaTram nhaTramBefore = db.NhaTrams.Find(id);
            if (nhaTramBefore == null) return NotFound();
            var tmpTram = nhaTramBefore.IDTram;
            var tmpNhaMang = nhaTramBefore.IDNhaMang;
            db.Entry(nhaTram).State = EntityState.Modified;
            nhaTram.IDTram = tmpTram;
            nhaTram.IDNhaMang = tmpNhaMang;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NhaTramExists(id))
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
        // PUT: api/NhaTrams/5
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNhaTram(int id, int idNhaMang)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            NhaMang tmpNhaMang = db.NhaMangs.Find(idNhaMang);
            if (tmpNhaMang == null)
            {
                return NotFound();
            }

            NhaTram nhaTram = db.NhaTrams.Find(id);
            if (nhaTram == null)
            {
                return NotFound();
            }
            int countNhaMang = db.NhaTrams.Where(u => u.IDTram == nhaTram.IDTram&&u.IDNhaMang==idNhaMang).Count();
            if (countNhaMang > 0)
            {
                return BadRequest();
            }
            nhaTram.IDNhaMang = idNhaMang;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NhaTramExists(id))
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

        // POST: api/NhaTrams
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(NhaTram))]
        public IHttpActionResult PostNhaTram(NhaTram nhaTram)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Tram tram = db.Trams.Find(nhaTram.IDTram);
            NhaMang nhaMang = db.NhaMangs.Find(nhaTram.IDNhaMang);
            if(tram==null||nhaMang==null) return NotFound();

            int countNhaMang = db.NhaTrams.Where(u => u.IDTram == nhaTram.IDTram && u.IDNhaMang == nhaTram.IDNhaMang).Count();
            if (countNhaMang > 0)
            {
                return BadRequest();
            }

            db.NhaTrams.Add(nhaTram);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = nhaTram.IDNhaTram }, nhaTram);
        }

        // DELETE: api/NhaTrams/5
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(NhaTram))]
        public IHttpActionResult DeleteNhaTram(int id)
        {
            NhaTram nhaTram = db.NhaTrams.Find(id);
            if (nhaTram == null)
            {
                return NotFound();
            }

            db.NhaTrams.Remove(nhaTram);
            db.SaveChanges();

            return Ok(nhaTram);
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NhaTramExists(int id)
        {
            return db.NhaTrams.Count(e => e.IDNhaTram == id) > 0;
        }
        private bool checkThongSo(int? thongSo)
        {
            if (thongSo < 0 || thongSo > 15 || thongSo == null) return false;
            return true;
        }
    }
}