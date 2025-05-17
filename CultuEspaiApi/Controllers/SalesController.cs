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
    public class SalesController : ApiController
    {
        private espaiCulturalEntities db = new espaiCulturalEntities();

        // GET: api/Sales
        [ResponseType(typeof(Sales))]
        public IHttpActionResult GetSales()
        {
            db.Configuration.LazyLoadingEnabled = false;

            var sales = db.Sales
                .Select(s => new
                {
                    s.SalaID,
                    s.Nom,
                    s.MetresQuadrats,
                    s.Aforament,
                    s.CadiresFixes,
                    s.Descripcio,
                    s.ButacaMax
                }).ToList();
            
            return Ok(sales);
        }

        // GET: api/Sales/5
        [ResponseType(typeof(Sales))]
        public async Task<IHttpActionResult> GetSales(int id)
        {
            IHttpActionResult result;

            var sales = await db.Sales
                .Where(s => s.SalaID == id)
                .Select(s => new
                {
                    s.SalaID,
                    s.Nom,
                    s.MetresQuadrats,
                    s.Aforament,
                    s.CadiresFixes,
                    s.Descripcio,
                    s.ButacaMax
                })
                .FirstOrDefaultAsync();

            if (sales == null)
            {
                return NotFound();
            }
            else
            {
                result = Ok(sales);
            }

            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SalesExists(int id)
        {
            return db.Sales.Count(e => e.SalaID == id) > 0;
        }
    }
}