using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
namespace Apps.Models.Flow
{
    public partial class Flow_FormAttrModel
    {
        [MaxWordsExpression(50)]
        [Display(Name = "ID")]
        public override string Id { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "字段标题")]
        public override string Title { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "字段英文名称")]
        public override string Name { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "类型")]
        public override string AttrType { get; set; }//文本,日期,数字,多行文本

        [MaxWordsExpression(500)]
        [Display(Name = "校验脚本")]
        public override string CheckJS { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "所属类别")]
        public override string TypeId { get; set; }

        public  string TypeName { get; set; }

        [Display(Name = "创建时间")]
        public override DateTime? CreateTime { get; set; }
    }
}
