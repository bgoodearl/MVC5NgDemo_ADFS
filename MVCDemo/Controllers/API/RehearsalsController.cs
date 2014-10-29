using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using BGoodMusic.EFDAL;
using BGoodMusic.Models;

namespace MVCDemo.Controllers.API
{
    public class RehearsalsController : ApiController
    {
        private BGoodMusicDBContext db = new BGoodMusicDBContext();

        // GET: api/Rehearsals
        public IQueryable<Rehearsal> GetRehearsals()
        {
            return db.Rehearsals;
        }

        // GET: api/Rehearsals/5
        [ResponseType(typeof(Rehearsal))]
        public IHttpActionResult GetRehearsal(int id)
        {
            Rehearsal rehearsal = db.Rehearsals.Find(id);
            if (rehearsal == null)
            {
                return NotFound();
            }

            return Ok(rehearsal);
        }

        // PUT: api/Rehearsals/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRehearsal(int id, Rehearsal rehearsal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rehearsal.Id)
            {
                return BadRequest();
            }

            db.Entry(rehearsal).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RehearsalExists(id))
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

        // POST: api/Rehearsals
        [ResponseType(typeof(Rehearsal))]
        public IHttpActionResult PostRehearsal(Rehearsal rehearsal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Rehearsals.Add(rehearsal);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = rehearsal.Id }, rehearsal);
        }

        // DELETE: api/Rehearsals/5
        [ResponseType(typeof(Rehearsal))]
        public IHttpActionResult DeleteRehearsal(int id)
        {
            Rehearsal rehearsal = db.Rehearsals.Find(id);
            if (rehearsal == null)
            {
                return NotFound();
            }

            db.Rehearsals.Remove(rehearsal);
            db.SaveChanges();

            return Ok(rehearsal);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RehearsalExists(int id)
        {
            return db.Rehearsals.Count(e => e.Id == id) > 0;
        }
    }
}