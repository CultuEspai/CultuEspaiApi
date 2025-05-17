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

// OK
namespace CultuEspaiApi.Controllers
{
    public class UsuarisController : ApiController
    {
        private espaiCulturalEntities db = new espaiCulturalEntities();

        // GET: api/Usuaris
        [ResponseType(typeof(Usuaris))]
        public IHttpActionResult GetUsuaris()
        {
            db.Configuration.LazyLoadingEnabled = false;
            
            var usuaris = db.Usuaris
                .Select(u => new
                {
                    u.UsuariID,
                    u.Nom,
                    u.Correu,
                    u.Contrasenya,
                    u.TipusUsuari,
                    u.Actiu,
                    u.ContrasenyaHash
                }).ToList();

            return Ok(usuaris);
        }

        // GET: api/Usuaris/5
        [ResponseType(typeof(Usuaris))]
        public async Task<IHttpActionResult> GetUsuaris(int id)
        {
            IHttpActionResult result;

            var user = await db.Usuaris
                .Where(u => u.UsuariID == id)
                .Select(u => new
                {
                    u.UsuariID,
                    u.Nom,
                    u.Correu,
                    u.Contrasenya,
                    u.TipusUsuari,
                    u.Actiu,
                    u.ContrasenyaHash
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                result = Ok(user);
            }

            return result;
        }

        // PUT: api/Usuaris/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUsuaris(int id, Usuaris usuari)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuari.UsuariID)
            {
                return BadRequest();
            }

            var existingUser = await db.Usuaris.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Nom = usuari.Nom;
            existingUser.Correu = usuari.Correu;
            existingUser.Contrasenya = usuari.Contrasenya;
            existingUser.TipusUsuari = usuari.TipusUsuari;
            existingUser.Actiu = usuari.Actiu;
            existingUser.ContrasenyaHash = usuari.ContrasenyaHash;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarisExists(id))
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

        // POST: api/Usuaris
        [ResponseType(typeof(Usuaris))]
        public IHttpActionResult PostUsuaris(Usuaris usuari)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var lastUser = db.Usuaris.OrderByDescending(u => u.UsuariID).FirstOrDefault();
            if (lastUser != null)
            {
                usuari.UsuariID = lastUser.UsuariID + 1;
            }
            else
            {
                usuari.UsuariID = 1;
            }

            db.Usuaris.Add(usuari);

            try
            {
                db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict();  
            }

            return CreatedAtRoute("DefaultApi", new { id = usuari.UsuariID }, usuari);
        }

        // DELETE: api/Usuaris/5
        [ResponseType(typeof(Usuaris))]
        public async Task<IHttpActionResult> DeleteUsuaris(int id)
        {
            Usuaris usuaris = await db.Usuaris.FindAsync(id);
            if (usuaris == null)
            {
                return NotFound();
            }

            db.Usuaris.Remove(usuaris);
            await db.SaveChangesAsync();

            return Ok(usuaris);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsuarisExists(int id)
        {
            return db.Usuaris.Count(e => e.UsuariID == id) > 0;
        }
    }
}