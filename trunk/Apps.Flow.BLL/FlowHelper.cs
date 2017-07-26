using Apps.Models.Enum;
using Apps.Models.Flow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apps.Flow.BLL
{
    public class FlowHelper
    {
        //获取指定类型的HTML表单
        public string GetInput(string type, string id, string attrNo)
        {
            string str = "";
            if (type == "文本")
            {
                str = "<input id='" + id + "' class='input'  name='" + attrNo + "'  type='text' />";
            }
            else if (type == "多行文本")
            {
                str = "<textarea id='" + id + "' class='input' name='" + attrNo + "' ></textarea>";
            }
            else if (type == "日期")
            {
                str = "<input type='text' class='input' name='" + attrNo + "' class='Wdate' onfocus=\"WdatePicker({dateFmt:'yyyy-MM-dd'})\"  id='" + id + "'  />";
            }
            else if (type == "时间")
            {
                str = "<input type='text' class='input' name='" + attrNo + "' class='Wdate' onfocus=\"WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})\"  id='" + id + "'  />";
            }
            else if (type == "数字")
            {
                str = "<input type='number' class='input' name='" + attrNo + "'  id='" + id + "'  />";
            }
            return str;
        }
        //对比条件
        public bool Judge(string attrType, string rVal, string cVal, string lVal)
        {
            if (attrType == "数字")
            {
                double rVald = Convert.ToDouble(rVal);
                double lVald = Convert.ToDouble(lVal);
                if (cVal == "==")
                {
                    if (rVald == lVald)//为真
                    {
                        return false;
                    }
                }
                if (cVal == ">")
                {
                    if (rVald > lVald)//为真
                    {
                        return false;
                    }
                }
                if (cVal == "<")
                {
                    if (rVald < lVald)//为真
                    {
                        return false;
                    }
                }
                if (cVal == ">=")
                {
                    if (rVald >= lVald)//为真
                    {
                        return false;
                    }
                }
                if (cVal == "<=")
                {
                    if (rVald <= lVald)//为真
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        //获取对应的提交的值
        public string GetFormAttrVal(string attrId, Flow_FormModel formModel, Flow_FormContentModel formContentModel)
        {
            //获得对象的类型，model
            Type formContentType = formContentModel.GetType();
            Type formType = formModel.GetType();
           
            //查找名称为"A-Z"的属性
            string[] arrStr = { "AttrA", "AttrB", "AttrC", "AttrD", "AttrE", "AttrF", "AttrG", "AttrH", "AttrI", "AttrJ", "AttrK"
                                  , "AttrL", "AttrM", "AttrN", "AttrO", "AttrP", "AttrQ", "AttrR", "AttrS", "AttrT", "AttrU"
                                  , "AttrV", "AttrW", "AttrX", "AttrY", "AttrZ"};
            foreach (string str in arrStr)
            {
               
                object o = formType.GetProperty(str).GetValue(formModel, null);
                object v = formContentType.GetProperty(str).GetValue(formContentModel, null);
                if (o != null)
                {
                    //查找model类的Class对象的"str"属性的值
                    if (o.ToString() == attrId) {
                        return v.ToString();
                    }
                }
            }
            return "";
        }

        public string GetCurrentStepCheckIdByStepCheckModelList(List<Flow_FormContentStepCheckModel> stepCheckModelList)
        {
            string stepCheckId = "";
            for (int i = stepCheckModelList.Count() - 1; i >= 0; i--)
            {
                //获得在进行中的单子
                if (stepCheckModelList[i].State == (int)FlowStateEnum.Progress)// || stepCheckModelList[i].State == (int)FlowStateEnum.Reject
                {
                    stepCheckId = stepCheckModelList[i].Id;
                    if (i != 0)//查看上一个审核状态
                    {
                        if (stepCheckModelList[i - 1].State != 1)//查看上一步是否没有审核完成或是不通过
                        {
                            stepCheckId = "";//等于空，终止于上一环节
                        }
                    }
                }
            }
            return stepCheckId;
        }
    }
}