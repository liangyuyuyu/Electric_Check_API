using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Electric_Check.Models
{
    public class Task
    {
        [Key]
        [Display(Name = "任务编号")]
        [Required(ErrorMessage = "任务编号不能为空")]
        [StringLength(50, ErrorMessage = "任务编号最大长度为50")]
        public string Number { get; set; }

        [Display(Name = "任务介绍")]
        [Required(ErrorMessage = "任务介绍不能为空")]
        [StringLength(2000, ErrorMessage = "任务介绍最大长度为2000")]
        public string Introduce { get; set; }

        [Display(Name = "任务站点路线")]
        [Required(ErrorMessage = "任务站点路线不能为空")]
        [StringLength(100, ErrorMessage = "任务站点路线最大长度为100，（1,2,3,...）")]
        public string Routes { get; set; }

        [Display(Name = "任务站点数")]
        [Required(ErrorMessage = "任务站点数不能为空")]
        public int Count { get; set; }

        [Display(Name = "任务当前状态")]
        [StringLength(1, ErrorMessage = "任务当前状态的最大长度为1，（未开始0、进行中1、已完成2）")]
        public string State { get; set; }

        [Display(Name = "任务类型")]
        [StringLength(1, ErrorMessage = "任务类型的最大长度为1，（巡检0、维修1）")]
        public string Type { get; set; }

        [Display(Name = "任务创建日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "任务规定完成的日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime EndDate { get; set; }

        [Display(Name = "负责人姓名")]
        [Required(ErrorMessage = "负责人姓名不能为空")]
        [StringLength(200, ErrorMessage = "负责人姓名最大长度为200，梁宇宇,王倩倩,...")]
        public string ResponsiblePeople { get; set; }

        [Display(Name = "任务站点的完成情况")]
        [StringLength(100, ErrorMessage = "任务站点的完成情况的最大长度为100，（0,0,0,...   未开始0、进行中1、已完成2、有问题3）")]
        public string Progress { get; set; }

        [Display(Name = "负责人完成任务后的报告")]
        //[Required(ErrorMessage = "负责人报告不能为空")]
        [StringLength(2000, ErrorMessage = "负责人报告最大长度为2000")]
        public string Report { get; set; }

        [Display(Name = "任务完成日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime CompletedDate { get; set; }
    }

    public class TaskContext : DbContext
    {
        public TaskContext() { }
        public DbSet<Task> Tasks { get; set; }
    }
}