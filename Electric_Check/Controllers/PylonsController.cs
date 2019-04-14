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
using Electric_Check.Models;

namespace Electric_Check.Controllers
{
    public class PylonsController : ApiController
    {
        private ElectricCheckContext db = new ElectricCheckContext();

        ///<summary>
        ///获取所有的电塔信息
        ///</summary>
        ///<remarks>
        ///获取所有的电塔信息，传给前端
        /// </remarks>
        [Route("GetAllPylons")]
        public IQueryable<Pylon> GetPylons()
        {
            return db.Pylons;
        }

        // GET: api/Pylons/5
        ///<summary>
        ///根据电塔编号获取电塔信息
        ///</summary>
        ///<remarks>
        ///根据电塔编号获取电塔信息，并返回给前端
        /// </remarks>
        [Route("GetOnePylon")]
        [ResponseType(typeof(Pylon))]
        public IHttpActionResult GetPylon(string id)
        {
            Pylon pylon = db.Pylons.Find(id);
            if (pylon == null)
            {
                //return NotFound();
                return Content<string>(HttpStatusCode.BadRequest, "NotFound");
            }

            return Ok(pylon);
        }

        // PUT: api/Pylons/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPylon(string id, Pylon pylon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pylon.Number)
            {
                return BadRequest();
            }

            db.Entry(pylon).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PylonExists(id))
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
        
        ///<summary>
        ///添加电塔信息
        ///</summary>
        ///<remarks>
        ///添加电塔信息，并返回用户信息给前端
        /// </remarks>
        [Route("PostPylon")]
        [ResponseType(typeof(Pylon))]
        public IHttpActionResult PostPylon(Pylon pylon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (pylon.Problems == null)
                pylon.Problems = 0;

            if (pylon.State == null || pylon.State == "")
                pylon.State = "0";

            db.Pylons.Add(pylon);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PylonExists(pylon.Number))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(pylon);
            //return CreatedAtRoute("DefaultApi", new { controller = "Pylons", id = pylon.Number }, pylon);
        }

        // DELETE: api/Pylons/5
        ///<summary>
        ///根据电塔编号删除电塔
        ///</summary>
        ///<remarks>
        ///根据电塔编号删除电塔，返回被删除的用户信息
        /// </remarks>
        [Route("DeletePylon")]
        [ResponseType(typeof(Pylon))]
        public IHttpActionResult DeletePylon(string id)
        {
            Pylon pylon = db.Pylons.Find(id);
            if (pylon == null)
            {
                //return NotFound();
                return Content<string>(HttpStatusCode.BadRequest, "NotFound");
            }

            db.Pylons.Remove(pylon);
            db.SaveChanges();

            return Ok(pylon);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PylonExists(string id)
        {
            return db.Pylons.Count(e => e.Number == id) > 0;
        }
    }
}