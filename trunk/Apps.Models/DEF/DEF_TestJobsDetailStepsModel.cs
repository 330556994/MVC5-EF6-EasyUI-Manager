using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel;

namespace Apps.Models.DEF
{
    public partial class DEF_TestJobsDetailStepsModel
    {
        public  string Id { get; set; }
        [DisplayName("ID")]
        [Required(ErrorMessage = "*")]
        public override string ItemID { get; set; }

        [DisplayName("版本号")]
        [Required(ErrorMessage = "*")]
        public override string VerCode { get; set; }
        [DisplayName("用例编码")]
        [Required(ErrorMessage = "*")]
        public override string Code { get; set; }
        [DisplayName("测试步骤")]
        [Required(ErrorMessage = "*")]
        public override string Title { get; set; }
        [DisplayName("测试内容")]
        public override string TestContent { get; set; }
        [DisplayName("结果")]
        public override bool? Result { get; set; }
        [DisplayName("排序")]
        public override int? Sort { get; set; }
        [DisplayName("测试结果内容")]
        public override string ResultContent { get; set; }
        [DisplayName("执行序号")]
        public override int? ExSort { get; set; }
        [DisplayName("步骤类型")]
        public override int StepType { get; set; }

        [DisplayName("测试人")]
        public override string Tester { get; set; }
        [DisplayName("测试时间")]
        public override DateTime? TestDt { get; set; }

        [DisplayName("开发者")]
        public override string Developer { get; set; }
        [DisplayName("计划开始时间")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public override DateTime? PlanStartDt { get; set; }
        [DisplayName("计划完成时间")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public override DateTime? PlanEndDt { get; set; }
        [DisplayName("实际完成时间")]
        public override DateTime? FinDt { get; set; }
        [DisplayName("开发完成标志")]
        public override bool? DevFinFlag { get; set; }
        [DisplayName("请求测试标志")]
        public override bool? TestRequestFlag { get; set; }
        [DisplayName("用例名称")]
        public string CodeName { get; set; }
    }
}
