using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CultuEspaiApi.Models;

namespace CultuEspaiApi.Controllers
{
    public class CaracteristiquesSalesController : ApiController
    {
        private espaiCulturalEntities db = new espaiCulturalEntities();

        // GET: api/CaracteristiquesSales
        public IQueryable<CaracteristiquesSales> GetCaracteristiquesSales()
        {
            return db.CaracteristiquesSales;
        }

        // GET: api/CaracteristiquesSales/5
        [ResponseType(typeof(CaracteristiquesSales))]
        public async Task<IHttpActionResult> GetCaracteristiquesSales(int id)
        {
            CaracteristiquesSales caracteristiquesSales = await db.CaracteristiquesSales.FindAsync(id);
            if (caracteristiquesSales == null)
            {
                return NotFound();
            }

            return Ok(caracteristiquesSales);
        }

        // PUT: api/CaracteristiquesSales/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCaracteristiquesSales(int id, CaracteristiquesSales caracteristiquesSales)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != caracteristiquesSales.CaracteristicaID)
            {
                return BadRequest();
            }

            db.Entry(caracteristiquesSales).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CaracteristiquesSalesExists(id))
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

        // POST: api/CaracteristiquesSales
        [ResponseType(typeof(CaracteristiquesSales))]
        public async Task<IHttpActionResult> PostCaracteristiquesSales(CaracteristiquesSales caracteristiquesSales)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CaracteristiquesSales.Add(caracteristiquesSales);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = caracteristiquesSales.CaracteristicaID }, caracteristiquesSales);
        }

        // DELETE: api/CaracteristiquesSales/5
        [ResponseType(typeof(CaracteristiquesSales))]
        public async Task<IHttpActionResult> DeleteCaracteristiquesSales(int id)
        {
            CaracteristiquesSales caracteristiquesSales = await db.CaracteristiquesSales.FindAsync(id);
            if (caracteristiquesSales == null)
            {
                return NotFound();
            }

            db.CaracteristiquesSales.Remove(caracteristiquesSales);
            await db.SaveChangesAsync();

            return Ok(caracteristiquesSales);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CaracteristiquesSalesExists(int id)
        {
            return db.CaracteristiquesSales.Count(e => e.CaracteristicaID == id) > 0;
        }
    }
}