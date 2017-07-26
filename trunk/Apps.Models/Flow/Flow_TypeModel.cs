using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
using System.Collections.Generic;
namespace Apps.Models.Flow
{
    public partial class Flow_TypeModel
    {
        [MaxWordsExpression(50)]
        [Display(Name = "Id")]
        public override string Id { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "类别")]
        public override string Name { get; set; }

        [MaxWordsExpression(500)]
        [Display(Name = "说明")]
        public override string Remark { get; set; }

        [Display(Name = "创建时间")]
        public override DateTime CreateTime { get; set; }

        [Display(Name = "排序")]
        public override int Sort { get; set; }

        public List<Flow_FormModel> formList { get; set; }

    }
}
