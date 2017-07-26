using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel;

namespace Apps.Models.DEF
{
    public partial class DEF_TestJobsDetailItemModel
    {
        
        public  string Id { get; set; }
        [DisplayName("版本号")]
        [Required(ErrorMessage = "*")]
        public override string VerCode { get; set; }
        [DisplayName("用例编码")]
        [Required(ErrorMessage = "*")]
        public override string Code { get; set; }
        [DisplayName("名称")]
        [Required(ErrorMessage = "*")]
        public override string Name { get; set; }
        [DisplayName("说明")]
        public override string Description { get; set; }
        [DisplayName("测试结果")]
        public override bool? Result { get; set; }
        [DisplayName("排序")]
        public override int? Sort { get; set; }
        [DisplayName("执行序号")]
        public override int? ExSort { get; set; }
        [DisplayName("锁定状态")]
        public override bool Lock { get; set; }

        [DisplayName("开发者")]
        public override string Developer { get; set; }
        [DisplayName("测试者")]
        public override string Tester { get; set; }
        [DisplayName("实际完成时间")]
        public override DateTime? FinDt { get; set; }
        [DisplayName("开发完成标志")]
        public override bool? DevFinFlag { get; set; }
        [DisplayName("请求测试标志")]
        public override bool? TestRequestFlag { get; set; }

    }
}
