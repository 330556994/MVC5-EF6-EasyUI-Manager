using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel;

namespace Apps.Models.DEF
{
    public partial class DEF_DefectModel
    {
        public string Id { get; set; }
        [DisplayName("ID")]
        [Required(ErrorMessage = "*")]
        public override string ItemID { get; set; }
        [DisplayName("版本号")]
        [Required(ErrorMessage = "*")]
        public override string VerCode { get; set; }
        [DisplayName("用例编码")]
        [Required(ErrorMessage = "*")]
        public override string Code { get; set; }
        [DisplayName("用例名称")]
        public override string CaseName { get; set; }
        [DisplayName("项目步骤名称")]
        public override string Title { get; set; }
        [DisplayName("测试内容")]
        public override string TestContent { get; set; }
        [DisplayName("测试结果内容")]
        public override string ResultContent { get; set; }
        [DisplayName("创建人")]
        public override string Creator { get; set; }
        [DisplayName("创建人")]
        public string CreatorTitle { get; set; }
        [DisplayName("创建日期")]
        public override DateTime? CrtDt { get; set; }
        [DisplayName("备注")]
        public override string Remark { get; set; }
        [DisplayName("接收者")]
        public override string Receiver { get; set; }
        [DisplayName("接收者")]
        public string ReceiverTitle { get; set; }
        [DisplayName("发送日期")]
        public override DateTime? SendDt { get; set; }
        [DisplayName("关闭状态")]
        public override bool? CloseState { get; set; }
        [DisplayName("关闭人")]
        public override string Closer { get; set; }
        [DisplayName("关闭人")]
        public string CloserTitle { get; set; }
        [DisplayName("关闭日期")]
        public override DateTime? CloseDt { get; set; }
        [DisplayName("消息ID")]
        public override string MessageId { get; set; }
        [DisplayName("排序")]
        public override int? Sort { get; set; }
        [DisplayName("处理状态")]
        public override bool? ProcessState { get; set; }
        [DisplayName("处理人")]
        public override string Processor { get; set; }
        [DisplayName("处理人")]
        public string ProcessorTitle { get; set; }
        [DisplayName("处理日期")]
        public override DateTime? ProcessDt { get; set; }

        [DisplayName("错误级别")]
        public override int? ErrorLevel { get; set; }

        [DisplayName("复杂度")]
        public override int? Complex { get; set; }
        [DisplayName("计划开始日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public override DateTime? PStartDt { get; set; }

        [DisplayName("计划完成日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public override DateTime? PEndDt { get; set; }

        [DisplayName("执行人")]
        public override string Executor { get; set; }

        [DisplayName("执行人")]
        public string ExecutorTitle { get; set; }


    }
}
