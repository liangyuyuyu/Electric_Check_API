using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
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

        ///<summary>
        ///根据电塔的编号获取未解决的问题信息
        ///</summary>
        ///<remarks>
        ///根据电塔的编号获取未解决的问题信息，并返回给前端
        /// </remarks>
        [Route("GetProblemByPylonNumber")]
        public IQueryable<Problem> GetProblemByPylonNumber(string PylonNumber)
        {
            return db.Problems.Where(c => c.PylonNumber.Equals(PylonNumber) && c.State.Equals("0")).OrderByDescending(problem => problem.CreatedDate);
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

        ///<summary>
        ///改变问题的介绍、图片、状态信息
        ///</summary>
        ///<remarks>
        ///添加问题信息，并返回用户信息给前端
        /// </remarks>
        [Route("ChangeProblemInfo")]
        [ResponseType(typeof(Problem))]
        public IHttpActionResult ChangeProblemInfo(string Number, string Describle, string Pictures, string State)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Problem checkProblem = db.Problems.Where(c => c.Number == Number).FirstOrDefault();//先查找出要修改的对象

            if (Describle != "无")
                checkProblem.Describle = Describle;

            if (Pictures != "无")
                checkProblem.Pictures = Pictures;

            if (State != "无")
                checkProblem.State = State;

            checkProblem.CreatedDate = DateTime.Now;

            db.SaveChanges();

            return Ok(checkProblem);
            //return CreatedAtRoute("DefaultApi", new { controller = "Problems", id = problem.Number }, problem);
        }

        ///<summary>
        ///完成一个维修任务后，改变多个问题的状态为已完成
        ///</summary>
        ///<remarks>
        ///完成一个维修任务后，改变多个问题的状态，并返回用户信息给前端
        /// </remarks>
        [Route("ChangeProblemsState")]
        [ResponseType(typeof(Problem))]
        public IHttpActionResult ChangeProblemsState(string TaskProblemNumber)
        {
            string[] ProblemArray = Regex.Split(TaskProblemNumber, "___", RegexOptions.IgnoreCase);

            for (int j = 0; j < ProblemArray.Length; j++)
            {
                string number = ProblemArray[j];

                Problem problem = db.Problems.Where(c => c.Number == number).FirstOrDefault();//先查找出要修改的对象

                if (problem == null)
                {
                    return Content<string>(HttpStatusCode.BadRequest, "NotFound");
                }

                problem.State = "2";
            }

            db.SaveChanges();

            return Ok();
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

            if (problem.Pictures == null || problem.Pictures == "")
                problem.Pictures = "";

            if (problem.Videos == null || problem.Videos == "")
                problem.Videos = "";

            if (problem.State == null || problem.State == "")
                problem.State = "0";

            if (problem.RepairPeople == null || problem.RepairPeople == "")
                problem.RepairPeople = "";

            if (problem.RepairReport == null || problem.RepairReport == "")
                problem.RepairReport = "";

            if (problem.CheckPeoplePhone == null || problem.CheckPeoplePhone == "")
                problem.CheckPeoplePhone = "";

            problem.CreatedDate = DateTime.Now;
            problem.EndDate = problem.CreatedDate;
            problem.CompletedDate = problem.CreatedDate;

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

        ///<summary>
        ///根据任务表中的问题编号删除问题
        ///</summary>
        ///<remarks>
        ///根据任务表中的问题编号删除问题，返回被删除的问题信息
        /// </remarks>
        [Route("DeleteProblemByTaskProblemNumber")]
        [ResponseType(typeof(Problem))]
        public IHttpActionResult DeleteProblemByTaskProblemNumber(string TaskProblemNumber)
        {
            string[] ProblemsArray = Regex.Split(TaskProblemNumber, "---", RegexOptions.IgnoreCase);

            // 循环查询及修改
            for (int i = 0; i < ProblemsArray.Length; i++)
            {

                if (ProblemsArray[i] != "" && ProblemsArray[i] != null)
                {
                    string[] ProblemArray = Regex.Split(ProblemsArray[i], "___", RegexOptions.IgnoreCase);

                    for (int j = 0; j < ProblemArray.Length; j++)
                    {
                        string number = ProblemArray[j];

                        Problem problem = db.Problems.Where(c => c.Number == number).FirstOrDefault();//先查找出要修改的对象

                        if (problem == null)
                        {
                            return Content<string>(HttpStatusCode.BadRequest, "NotFound");
                        }

                        db.Problems.Remove(problem);
                    }
                }
            }
            db.SaveChanges();

            return Ok();
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

        private bool ProblemExists(string id)
        {
            return db.Problems.Count(e => e.Number == id) > 0;
        }
    }
}