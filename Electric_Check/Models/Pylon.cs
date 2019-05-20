using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Electric_Check.Models
{
    public class Pylon
    {
        [Key]
        [Display(Name = "铁塔编号")]
        [Required(ErrorMessage = "铁塔编号不能为空")]
        [StringLength(10, ErrorMessage = "铁塔编号最大长度为10")]
        public string Number { get; set; }

        [Display(Name = "铁塔介绍")]
        [Required(ErrorMessage = "铁塔介绍不能为空")]
        [StringLength(500, ErrorMessage = "铁塔介绍最大长度为500")]
        public string Introduce { get; set; }

        [Display(Name = "铁塔经度")]
        [Required(ErrorMessage = "铁塔经度不能为空")]
        [StringLength(50, ErrorMessage = "铁塔经度最大长度为50")]
        public string Lng { get; set; }

        [Display(Name = "铁塔纬度")]
        [Required(ErrorMessage = "铁塔纬度不能为空")]
        [StringLength(50, ErrorMessage = "铁塔纬度最大长度为50")]
        public string Lat { get; set; }

        [Display(Name = "铁塔地址")]
        [Required(ErrorMessage = "铁塔地址不能为空")]
        [StringLength(100, ErrorMessage = "铁塔地址最大长度为100")]
        public string Address { get; set; }

        [Display(Name = "铁塔总问题数")]
        public int Problems { get; set; }

        [Display(Name = "铁塔当前状态")]
        [StringLength(1, ErrorMessage = "铁塔当前状态的最大长度为1，（正常0、故障1、巡检中2、维修中3）")]
        public string State { get; set; }
    }

    public class PylonContext : DbContext
    {
        public PylonContext() { }
        public DbSet<Pylon> Pylons { get; set; }
    }
}