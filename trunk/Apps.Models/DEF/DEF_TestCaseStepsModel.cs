using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel;
namespace Apps.Models.DEF
{
   public partial class DEF_TestCaseStepsModel
   {
       [DisplayName("ID")]
       public override string ItemID { get; set; }
       [DisplayName("用例编码")]
       public override string Code { get; set; }
       [DisplayName("标题")]
       [Required(ErrorMessage = "*")]
       public override string Title { get; set; }
       [DisplayName("测试内容")]
       public override string TestContent { get; set; }
       [DisplayName("状态")]
       public override bool? state { get; set; }
       [DisplayName("排序")]
       public override int? sort { get; set; }


    }
}
