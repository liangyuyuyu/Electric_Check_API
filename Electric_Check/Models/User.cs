using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Electric_Check.Models
{
    public class User
    {
        [Key]
        [Display(Name= "账号")]
        [Required(ErrorMessage = "账号不能为空")]
        [StringLength(11, ErrorMessage = "用户账号最大长度为11")]
        public string Account { get; set; }

        [Display(Name = "密码")]
        [StringLength(30, ErrorMessage = "用户密码最大长度为30")]
        public string Password { get; set; }

        [Display(Name = "姓名")]
        [StringLength(30, ErrorMessage = "用户姓名最大长度为30")]
        public string Name { get; set; }

        [Display(Name = "年龄")]
        public int Age { get; set; }

        [Display(Name = "用户类型")]
        [StringLength(1, ErrorMessage = "用户类型最大长度为1，管理员0、巡检人员1、检修人员2、普通用户3")]
        public string Type { get; set; }

        [Display(Name = "性别")]
        [StringLength(1, ErrorMessage = "用户性别最大长度为1，男0、女1")]
        public string Sex { get; set; }

        [Display(Name = "住址")]
        [StringLength(100, ErrorMessage = "用户地址最大长度为200")]
        public string Address { get; set; }

        [Display(Name = "头像")]
        [StringLength(100, ErrorMessage = "用户头像最大长度为100")]
        public string Avatar { get; set; }

        [Display(Name = "入职日期")]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime? EntryDate { get; set; }


        [Display(Name = "员工添加人姓名")]
        [StringLength(50, ErrorMessage = "员工添加人姓名最大长度为50")]
        public string AddPersonName { get; set; }

        [Display(Name = "员工添加人手机号")]
        [StringLength(11, ErrorMessage = "员工添加人手机号最大长度为11")]
        public string AddPersonPhone { get; set; }
    }

    public class ElectricCheckContext : DbContext
    {
        public ElectricCheckContext() { }
        public DbSet<User> Users { get; set; }
        public DbSet<Pylon> Pylons { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Problem> Problems { get; set; }
    }
}