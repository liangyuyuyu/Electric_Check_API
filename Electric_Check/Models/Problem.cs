using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Electric_Check.Models
{
    public class Problem
    {
        [Key]
        [Display(Name = "问题编号")]
        [Required(ErrorMessage = "问题编号不能为空")]
        [StringLength(50, ErrorMessage = "问题编号最大长度为50")]
        public string Number { get; set; }

        [Display(Name = "电塔编号")]
        [Required(ErrorMessage = "电塔编号不能为空")]
        [StringLength(10, ErrorMessage = "电塔编号最大长度为10")]
        public string PylonNumber { get; set; }

        [Display(Name = "问题描述")]
        [Required(ErrorMessage = "问题描述不能为空")]
        [StringLength(2000, ErrorMessage = "问题描述最大长度为2000")]
        public string Describle { get; set; }

        [Display(Name = "问题的图片")]
        [StringLength(1000, ErrorMessage = "问题的图片最大长度为1000")]
        public string Pictures { get; set; }

        [Display(Name = "问题的视频")]
        [StringLength(1000, ErrorMessage = "问题的视频最大长度为1000")]
        public string Videos { get; set; }

        [Display(Name = "问题上传的时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "巡检人员姓名")]
        [Required(ErrorMessage = "巡检人员姓名不能为空")]
        [StringLength(200, ErrorMessage = "巡检人员姓名最大长度为200，梁宇宇,王倩倩,...")]
        public string CheckPeople { get; set; }

        [Display(Name = "巡检人员手机号")]
        [Required(ErrorMessage = "巡检人员手机号不能为空")]
        [StringLength(11, ErrorMessage = "巡检人员手机号最大长度为11，梁宇宇,王倩倩,...")]
        public string CheckPeoplePhone { get; set; }

        [Display(Name = "解决问题的截止时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime EndDate { get; set; }

        [Display(Name = "问题当前的状态")]
        [StringLength(1, ErrorMessage = "问题当前的状态最大长度为1，（未解决0、进行中1、已解决2、延期3）")]
        public string State { get; set; }

        [Display(Name = "问题的维修人员")]
        [StringLength(200, ErrorMessage = "问题的维修人员最大长度为200，梁宇宇,王倩倩,...")]
        public string RepairPeople { get; set; }

        [Display(Name = "维修人员的报告")]
        //[Required(ErrorMessage = "负责人报告不能为空")]
        [StringLength(2000, ErrorMessage = "维修人员的报告最大长度为2000")]
        public string RepairReport { get; set; }

        [Display(Name = "问题的完成时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime CompletedDate { get; set; }
    }

    public class ProblemContext : DbContext
    {
        public ProblemContext() { }
        public DbSet<Problem> Problems { get; set; }
    }
}