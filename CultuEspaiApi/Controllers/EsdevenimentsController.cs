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
    public class EsdevenimentsController : ApiController
    {
        private espaiCulturalEntities db = new espaiCulturalEntities();

        // GET: api/Esdeveniments
        [ResponseType(typeof(Esdeveniments))]
        public IHttpActionResult GetEsdeveniments()
        {
            db.Configuration.LazyLoadingEnabled = false;

            var esdeveniments = db.Esdeveniments
                .Select(e => new
                {
                    e.EsdevenimentID,
                    e.Nom,
                    e.Descripcio,
                    e.SalaID,
                    e.DataInici,
                    e.DataFi,
                    e.Aforament,
                    e.OrganitzadorID,
                    e.Estat,
                    e.DePagament,
                    e.Preu,
                    e.EntradesDisp
                }).ToList();

            return Ok(esdeveniments);
        }

        // PUT: api/Esdeveniments/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEsdeveniments(int id, Esdeveniments esdeveniments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != esdeveniments.EsdevenimentID)
            {
                return BadRequest();
            }

            var existingEsdeveniment = await db.Esdeveniments.FindAsync(id);
            if (existingEsdeveniment == null)
            {
                return NotFound();
            }

            existingEsdeveniment.Nom = esdeveniments.Nom;
            existingEsdeveniment.Descripcio = esdeveniments.Descripcio;
            existingEsdeveniment.SalaID = esdeveniments.SalaID;
            existingEsdeveniment.DataInici = esdeveniments.DataInici;
            existingEsdeveniment.DataFi = esdeveniments.DataFi;
            existingEsdeveniment.Aforament = esdeveniments.Aforament;
            existingEsdeveniment.OrganitzadorID = esdeveniments.OrganitzadorID;
            existingEsdeveniment.Estat = esdeveniments.Estat;
            existingEsdeveniment.DePagament = esdeveniments.DePagament;
            existingEsdeveniment.Preu = esdeveniments.Preu;
            existingEsdeveniment.EntradesDisp = esdeveniments.EntradesDisp;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EsdevenimentsExists(id))
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

        // POST: api/Esdeveniments
        [ResponseType(typeof(Esdeveniments))]
        public async Task<IHttpActionResult> PostEsdeveniments(Esdeveniments esdeveniment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var lastEsdeveniment = db.Esdeveniments
                .OrderByDescending(e => e.EsdevenimentID)
                .FirstOrDefault();
            
            if (lastEsdeveniment != null)
            {
                esdeveniment.EsdevenimentID = lastEsdeveniment.EsdevenimentID + 1;
            }
            else
            {
                esdeveniment.EsdevenimentID = 1;
            }

            db.Esdeveniments.Add(esdeveniment);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EsdevenimentsExists(esdeveniment.EsdevenimentID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = esdeveniment.EsdevenimentID }, esdeveniment);
        }

        // DELETE: api/Esdeveniments/5
        [HttpDelete]
        [Route("api/Esdeveniments/{id}")]
        public async Task<IHttpActionResult> DeleteEsdeveniments(int id)
        {
            var esdeveniments = await db.Esdeveniments
                .Include(e => e.Entrades)
                .Include(e => e.Xats)
                .FirstOrDefaultAsync(e => e.EsdevenimentID == id);
            
            if (esdeveniments == null)
            {
                return NotFound();
            }

            db.Esdeveniments.Remove(esdeveniments);
            await db.SaveChangesAsync();

            return Ok(esdeveniments);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EsdevenimentsExists(int id)
        {
            return db.Esdeveniments.Count(e => e.EsdevenimentID == id) > 0;
        }
    }
}