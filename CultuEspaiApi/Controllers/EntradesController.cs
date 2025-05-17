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

//Ok
namespace CultuEspaiApi.Controllers
{
    public class EntradesController : ApiController
    {
        private espaiCulturalEntities db = new espaiCulturalEntities();

        // GET: api/Entrades/5 by id user
        [HttpGet]
        [Route("api/Entrades/user/{id}")]
        public IHttpActionResult GetEntradesByIdUser(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;

            var entrades = db.Entrades
                .Where(e => e.UsuariID == id)
                .Select(e => new
                {
                    e.EntradaID,
                    e.UsuariID,
                    e.EsdevenimentID,
                    e.Quantitat,
                    e.NumeroButaca
                }).ToList();

            return Ok(entrades);
        }

        // POST: api/Entrades
        [ResponseType(typeof(Entrades))]
        public async Task<IHttpActionResult> PostEntrades(Entrades entrada)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var lastEntrada = db.Entrades
                .OrderByDescending(e => e.EntradaID)
                .FirstOrDefault();
            
            if (lastEntrada != null)
            {
                entrada.EntradaID = lastEntrada.EntradaID + 1;
            }
            else
            {
                entrada.EntradaID = 1;
            }

            db.Entrades.Add(entrada);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EntradesExists(entrada.EntradaID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = entrada.EntradaID }, entrada);
        }

        // DELETE: api/Entrades/5
        [HttpDelete]
        [Route("api/Entrades/{id}")]
        public async Task<IHttpActionResult> DeleteEntrades(int id)
        {
            var entrades = await db.Entrades.FindAsync(id);
            if (entrades == null)
            {
                return NotFound();
            }
            db.Entrades.Remove(entrades);
            await db.SaveChangesAsync();

            return Ok(entrades);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EntradesExists(int id)
        {
            return db.Entrades.Count(e => e.EntradaID == id) > 0;
        }
    }
}