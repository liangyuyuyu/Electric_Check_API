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
            return db.Pylons.OrderBy(pylon => pylon.Number);
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
        ///修改一个电塔的状态，及其负责人
        ///</summary>
        ///<remarks>
        ///修改一个电塔的状态，及其负责人，并返回用户信息给前端
        /// </remarks>
        [Route("ChangePylonState")]
        [ResponseType(typeof(Pylon))]
        public IHttpActionResult ChangePylonState(string Number, string State, string ResponsiblePeople)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Pylon pylon = db.Pylons.Where(c => c.Number == Number).FirstOrDefault();//先查找出要修改的对象

            pylon.State = State;

            if(ResponsiblePeople == "null")
            {
                pylon.CurrentResponsiblePerson = "";
            }
            else
            {
                pylon.CurrentResponsiblePerson = ResponsiblePeople;
            }

            db.SaveChanges();
            return Ok(pylon);
        }

        ///<summary>
        ///修改多个电塔的状态,及其电塔的负责人
        ///</summary>
        ///<remarks>
        ///修改多个电塔的状态，及其电塔的负责人，并返回用户信息给前端
        /// </remarks>
        [Route("ChangePylonsState")]
        [ResponseType(typeof(Pylon))]
        public IHttpActionResult ChangePylonsState(string Numbers, string States, string ResponsiblePeople)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string[] NumberArray = Numbers.Split(',');
            string[] StateArray = States.Split(',');

            // 循环查询及修改
            for (int i = 0; i < StateArray.Length; i++)
            {
                string number = NumberArray[i];

                Pylon pylon = db.Pylons.Where(c => c.Number == number).FirstOrDefault();//先查找出要修改的对象

                pylon.State = StateArray[i];

                pylon.CurrentResponsiblePerson = ResponsiblePeople;

                db.SaveChanges();
            }

            return Ok();
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

            if (pylon.CurrentResponsiblePerson == null || pylon.CurrentResponsiblePerson == "")
                pylon.CurrentResponsiblePerson = "无";

            if (pylon.Pictures == null || pylon.Pictures == "")
                pylon.Pictures = "";

            pylon.CreatedDate = DateTime.Now;

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

        public string Options()
        {
            return null; // HTTP 200 response with empty body
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