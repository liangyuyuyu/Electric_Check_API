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
    public class ProblemsController : ApiController
    {
        private ElectricCheckContext db = new ElectricCheckContext();

        // GET: api/Problems
        ///<summary>
        ///获取所有的问题信息
        ///</summary>
        ///<remarks>
        ///获取所有的问题信息，传给前端
        /// </remarks>
        [Route("GetAllProblems")]
        public IQueryable<Problem> GetProblems()
        {
            return db.Problems;
        }

        // GET: api/Problems/5
        ///<summary>
        ///根据问题编号获取问题信息
        ///</summary>
        ///<remarks>
        ///根据问题编号获取问题信息，并返回给前端
        /// </remarks>
        [Route("GetOneProblem")]
        [ResponseType(typeof(Problem))]
        public IHttpActionResult GetProblem(string id)
        {
            Problem problem = db.Problems.Find(id);
            if (problem == null)
            {
                //return NotFound();
                return Content<string>(HttpStatusCode.BadRequest, "NotFound");
            }

            return Ok(problem);
        }

        // PUT: api/Problems/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProblem(string id, Problem problem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != problem.Number)
            {
                return BadRequest();
            }

            db.Entry(problem).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProblemExists(id))
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

        // POST: api/Problems
        ///<summary>
        ///添加问题信息
        ///</summary>
        ///<remarks>
        ///添加问题信息，并返回用户信息给前端
        /// </remarks>
        [Route("PostProblem")]
        [ResponseType(typeof(Problem))]
        public IHttpActionResult PostProblem(Problem problem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            problem.CreatedDate = DateTime.Now;

            db.Problems.Add(problem);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ProblemExists(problem.Number))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(problem);
            //return CreatedAtRoute("DefaultApi", new { controller = "Problems", id = problem.Number }, problem);
        }

        // DELETE: api/Problems/5
        ///<summary>
        ///根据问题编号删除问题
        ///</summary>
        ///<remarks>
        ///根据问题编号删除问题，返回被删除的问题信息
        /// </remarks>
        [Route("DeleteProblem")]
        [ResponseType(typeof(Problem))]
        public IHttpActionResult DeleteProblem(string id)
        {
            Problem problem = db.Problems.Find(id);
            if (problem == null)
            {
                //return NotFound();
                return Content<string>(HttpStatusCode.BadRequest, "NotFound");
            }

            db.Problems.Remove(problem);
            db.SaveChanges();

            return Ok(problem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProblemExists(string id)
        {
            return db.Problems.Count(e => e.Number == id) > 0;
        }
    }
}