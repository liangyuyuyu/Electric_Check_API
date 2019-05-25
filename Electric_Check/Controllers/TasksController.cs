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
    public class TasksController : ApiController
    {
        private ElectricCheckContext db = new ElectricCheckContext();

        // GET: api/Tasks
        ///<summary>
        ///获取所有的任务信息
        ///</summary>
        ///<remarks>
        ///获取所有的任务信息，传给前端
        /// </remarks>
        [Route("GetAllTasks")]
        public IQueryable<Task> GetTasks()
        {
            return db.Tasks;
        }

        // GET: api/Tasks/5
        ///<summary>
        ///根据任务编号获取任务信息
        ///</summary>
        ///<remarks>
        ///根据任务编号获取任务信息，并返回给前端
        /// </remarks>
        [Route("GetTaskByID")]
        [ResponseType(typeof(Task))]
        public IHttpActionResult GetTaskByID(string id)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                //return NotFound();
                return Content<string>(HttpStatusCode.BadRequest, "NotFound");
            }

            return Ok(task);
        }

        ///<summary>
        ///根据任务负责人获取任务信息
        ///</summary>
        ///<remarks>
        ///根据任务负责人获取任务信息，并返回给前端
        /// </remarks>
        [Route("GetTaskByUser")]
        public IQueryable<Task> GetTaskByUser(string userName)
        {
            return db.Tasks.Where(c => c.ResponsiblePeople.Contains(userName)).OrderByDescending(task => task.CreatedDate);
        }

        ///<summary>
        ///根据任务发起人的手机号获取任务信息
        ///</summary>
        ///<remarks>
        ///根据任务发起人的手机号获取任务信息，并返回给前端
        /// </remarks>
        [Route("GetTaskByReleasePersonPhone")]
        public IQueryable<Task> GetTaskByReleasePersonPhone(string ReleasePersonPhone)
        {
            return db.Tasks.Where(c => c.ReleasePersonPhone.Contains(ReleasePersonPhone)).OrderByDescending(task => task.CreatedDate);
        }

        // PUT: api/Tasks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTask(string id, Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != task.Number)
            {
                return BadRequest();
            }

            db.Entry(task).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
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

        // POST: api/Tasks
        ///<summary>
        ///添加任务信息
        ///</summary>
        ///<remarks>
        ///添加任务信息，并返回用户信息给前端
        /// </remarks>
        [Route("PostTask")]
        [ResponseType(typeof(Task))]
        public IHttpActionResult PostTask(Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (task.State == null || task.State == "")
                task.State = "0";
            if (task.Type == null || task.Type == "")
                task.Type = "0";
            if (task.Report == null || task.Report == "")
                task.Report = "";
            if (task.ReleasePersonName == null || task.ReleasePersonName == "")
                task.ReleasePersonName = "";
            if (task.ReleasePersonPhone == null || task.ReleasePersonPhone == "")
                task.ReleasePersonPhone = "";

            task.CreatedDate = DateTime.Now;
            task.CompletedDate = task.EndDate;

            db.Tasks.Add(task);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (TaskExists(task.Number))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(task);
            //return CreatedAtRoute("DefaultApi", new { controller = "Tasks", id = task.Number }, task);
        }

        // DELETE: api/Tasks/5
        ///<summary>
        ///根据任务编号删除任务
        ///</summary>
        ///<remarks>
        ///根据任务编号删除任务，返回被删除的任务信息
        /// </remarks>
        [Route("DeleteTask")]
        [ResponseType(typeof(Task))]
        public IHttpActionResult DeleteTask(string id)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                //return NotFound();
                return Content<string>(HttpStatusCode.BadRequest, "NotFound");
            }

            db.Tasks.Remove(task);
            db.SaveChanges();

            return Ok(task);
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

        private bool TaskExists(string id)
        {
            return db.Tasks.Count(e => e.Number == id) > 0;
        }
    }
}