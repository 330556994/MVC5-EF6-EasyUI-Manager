using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel;

namespace Apps.Models.DEF
{
    public partial class DEF_CaseTypeModel
    {
        [MaxWordsExpression(36)]
        [Display(Name = "Id")]
        public override  string Id { get; set; }

        [MaxWordsExpression(200)]
        [Display(Name = "名称")]
        public override string Name { get; set; }

        [MaxWordsExpression(36)]
        [Display(Name = "上级")]
        public override string ParentId { get; set; }

        [Display(Name = "IsLast")]
        public override bool IsLast { get; set; }
        public string state { get; set; }
    }

    public partial class DEF_CaseTypeModelByComTree
    {
        public string id { get; set; }
        public string text { get; set; }
        public string state { get; set; }
    }
}
