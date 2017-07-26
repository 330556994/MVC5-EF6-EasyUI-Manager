using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
namespace Apps.Models.Flow
{
    public partial class Flow_FormContentModel
    {
        [MaxWordsExpression(50)]
        [Display(Name = "ID")]
        public override string Id { get; set; }

        [MaxWordsExpression(200)]
        [Display(Name = "标题")]
        public override string Title { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "发起用户")]
        public override string UserId { get; set; }

        public string UserName { get; set; }
        [MaxWordsExpression(50)]
        [Display(Name = "对应表单")]
        public override string FormId { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "公文级别")]
        public override string FormLevel { get; set; }

        [Display(Name = "创建时间")]
        public override DateTime CreateTime { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrA")]
        public override string AttrA { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrB")]
        public override string AttrB { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrC")]
        public override string AttrC { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrD")]
        public override string AttrD { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrE")]
        public override string AttrE { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrF")]
        public override string AttrF { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrG")]
        public override string AttrG { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrH")]
        public override string AttrH { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrI")]
        public override string AttrI { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrJ")]
        public override string AttrJ { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrK")]
        public override string AttrK { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrL")]
        public override string AttrL { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrM")]
        public override string AttrM { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrN")]
        public override string AttrN { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrO")]
        public override string AttrO { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrP")]
        public override string AttrP { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrQ")]
        public override string AttrQ { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrR")]
        public override string AttrR { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrS")]
        public override string AttrS { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrT")]
        public override string AttrT { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrU")]
        public override string AttrU { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrV")]
        public override string AttrV { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrW")]
        public override string AttrW { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrX")]
        public override string AttrX { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrY")]
        public override string AttrY { get; set; }

        [MaxWordsExpression(2048)]
        [Display(Name = "AttrZ")]
        public override string AttrZ { get; set; }

        [MaxWordsExpression(4000)]
        [Display(Name = "CustomMember")]
        public override string CustomMember { get; set; }

        [Display(Name = "截至时间")]
        public override DateTime TimeOut { get; set; }
        public  string CurrentStep{ get; set; }
        public  string StepCheckId { get; set; }
        public  string Action { get; set; }

        public int CurrentState { get; set; }
    }
}
