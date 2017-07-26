using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel;

namespace Apps.Models.DEF
{
    public partial class DEF_TestJobsDetailRelationModel
    {
        public  string Id { get; set; }
        [DisplayName("版本号")]
        [Required(ErrorMessage = "*")]
        public override string VerCode { get; set; }
        [DisplayName("主用例编码")]
        [Required(ErrorMessage = "*")]
        public override string PCode { get; set; }
        [DisplayName("子用例编码")]
        [Required(ErrorMessage = "*")]
        public override string CCode { get; set; }
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
    }
}
