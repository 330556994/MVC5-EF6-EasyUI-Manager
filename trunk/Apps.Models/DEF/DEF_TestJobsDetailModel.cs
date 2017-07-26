using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel;

namespace Apps.Models.DEF
{
    public partial class DEF_TestJobsDetailModel
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
        [RegularExpression(@"[\w\W]{0,2000}", ErrorMessage = "{0}为1－2000字。")] //此默认生成的正则为允许任意字符，请根据业务逻辑修改
        public override string Description { get; set; }
        [DisplayName("结果")]
        public override bool? Result { get; set; }
        [DisplayName("排序")]
        public override int? Sort { get; set; }
    }
}
