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
    public class UsersController : ApiController
    {
        private Electric_CheckContext db = new Electric_CheckContext();

        // GET: api/Users
        ///<summary>
        ///获取所有的用户信息
        ///</summary>
        ///<remarks>
        ///获取所有的用户信息，传给前端
        /// </remarks>
        [Route("GetAllUsers")]
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        // GET: api/Users/5
        ///<summary>
        ///根据用户ID获取用户信息
        ///</summary>
        ///<remarks>
        ///根据用户ID获取用户信息，并返回给前端
        /// </remarks>
        [Route("GetOneUser")]
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(string id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                //return NotFound();
                //return Json<dynamic>(new { msg = "NotFound" }); // 返回json数据
                return Content<string>(HttpStatusCode.BadRequest, "NotFound");
                //return Redirect("https://www.baidu.com");
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [Route("PutUser")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(string id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Account)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        ///<summary>
        ///提交用户信息
        ///</summary>
        ///<remarks>
        ///提交用户信息，并返回用户信息给前端
        /// </remarks>
        [Route("PostUser")]
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (user.Account == null || user.Account == "")
                //return Content<string>(HttpStatusCode.BadRequest, "account required");
                return Json<dynamic>(new { msg = "account required" });

            if (user.Password == null || user.Password == "")
                user.Password = "1234";

            if (user.Name == null || user.Name == "")
                user.Name = "用户";

            if (user.Age == null || user.Age == 0)
                user.Age = 21;

            if (user.Type == null || user.Type == "")
                user.Type = "3";

            if (user.Sex == null || user.Sex == "")
                user.Sex = "0";

            if (user.Address == null || user.Address == "")
                user.Address = "上海电力大学";

            if (user.Avatar == null || user.Avatar == "")
                user.Avatar = user.Sex == "0" ? "boy" : "girl";

            if (user.EntryDate == null)
                user.EntryDate = DateTime.Now;

            db.Users.Add(user);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.Account))
                {
                    //return Conflict();

                    return Json<dynamic>(new { msg = "account registered" });
                }
                else
                {
                    throw;
                }
            }

            //return CreatedAtRoute("DefaultApi", new { id = user.Account }, user);

            return Ok(user);
        }

        // DELETE: api/Users/5
        ///<summary>
        ///根据用户ID删除用户
        ///</summary>
        ///<remarks>
        ///根据用户ID删除用户，返回被删除的用户信息
        /// </remarks>
        [Route("DeleteUser")]
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(string id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                //return NotFound();
                return Content<string>(HttpStatusCode.BadRequest, "NotFound");
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(string id)
        {
            return db.Users.Count(e => e.Account == id) > 0;
        }
    }
}