using System.Collections.Generic;
using System.Linq;
using Apps.Web.Core;
using Apps.Spl.IBLL;
using Apps.Locale;
using System.Web.Mvc;
using Apps.Common;
using Apps.IBLL;
using Apps.Models.Spl;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;
using System;
using ClosedXML.Excel;
using System.IO;

namespace Apps.Web.Areas.Spl.Controllers
{
    public class PersonController : BaseController
    {
        [Dependency]
        public ISpl_PersonBLL m_BLL { get; set; }
        ValidationErrors errors = new ValidationErrors();

        [SupportFilter]
        public ActionResult Index()
        {
            
            return View();
        }
        [HttpPost]
        [SupportFilter(ActionName = "Index")]
        public JsonResult GetList(GridPager pager, string queryStr)
        {
            List<Spl_PersonModel> list = m_BLL.GetList(ref pager, queryStr);
            GridRows<Spl_PersonModel> grs = new GridRows<Spl_PersonModel>();
            grs.rows = list;
            grs.total = pager.totalRows;
            return Json(grs);
        }
        #region 创建
        [SupportFilter]
        public ActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Create(Spl_PersonModel model)
        {
            model.Id = ResultHelper.NewId;
            model.CreateTime = ResultHelper.NowTime;
            if (model != null && ModelState.IsValid)
            {

                if (m_BLL.Create(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "创建", "Spl_Person");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "创建", "Spl_Person");
                    return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.InsertFail));
            }
        }
        #endregion

        #region 修改
        [SupportFilter]
        public ActionResult Edit(string id)
        {
            
            Spl_PersonModel entity = m_BLL.GetById(id);
            return View(entity);
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Edit(Spl_PersonModel model)
        {
            if (model != null && ModelState.IsValid)
            {

                if (m_BLL.Edit(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "修改", "Spl_Person");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "修改", "Spl_Person");
                    return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.EditFail));
            }
        }
        #endregion

        #region 详细
        [SupportFilter]
        public ActionResult Details(string id)
        {
            
            Spl_PersonModel entity = m_BLL.GetById(id);
            return View(entity);
        }

        #endregion

        #region 删除
        [HttpPost]
        [SupportFilter]
        public JsonResult Delete(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (m_BLL.Delete(ref errors, id))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "Spl_Person");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "Spl_Person");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail));
            }


        }
        #endregion

        #region 导入

        [HttpPost]
        [SupportFilter]
        public ActionResult Import(string filePath,int flag)
        {
            var personList = new List<Spl_PersonModel>();
            bool checkResult = false;
            //校验数据
            if (flag == 0)
            {
                checkResult = m_BLL.CheckImportData(Utils.GetMapPath(filePath), personList, ref errors);
            }
            else
            {
                checkResult = m_BLL.CheckImportBatchData(Utils.GetMapPath(filePath), personList, ref errors);
            }
             //校验通过直接保存
             if (checkResult)
             {
                 m_BLL.SaveImportData(personList);
                 LogHandler.WriteServiceLog(GetUserId(), "导入成功", "成功", "导入", "Spl_Person");
                 return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
             }
             else
             {
                 string ErrorCol = errors.Error;
                 LogHandler.WriteServiceLog(GetUserId(), ErrorCol, "失败", "导入", "Spl_Person");
                 return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol));
              }
        
        }

        [HttpPost]
        [SupportFilter(ActionName="Export")]
        public JsonResult CheckExportData()
        {
            List<Spl_PersonModel> list = m_BLL.GetList(ref setNoPagerAscById, "");
            if (list.Count().Equals(0))
            {
                return Json(JsonHandler.CreateMessage(0, "没有可以导出的数据"));
            }
            else
            {
                return Json(JsonHandler.CreateMessage(1, "可以导出"));
            }
        }

        [SupportFilter]
        public ActionResult Export()
        {
            var exportSpource = this.GetExportData();
            var dt = JsonConvert.DeserializeObject<DataTable>(exportSpource.ToString());

            var exportFileName = string.Concat(
                "Person",
                DateTime.Now.ToString("yyyyMMddHHmmss"),
                ".xlsx");

            return new ExportExcelResult
            {
                SheetName = "人员列表",
                FileName = exportFileName,
                ExportData = dt
            };
        }
        public ActionResult ExportHard()
        {
            //1.创建作业本
            //2.添加工作簿（信息表，可以同时导出多个sheet）
            //3.添加标题（合并列）
            //4.填充数据（制定填充或者列表填充，列表填充参考简单导出一节）
            //5.格式化数据（格式化销售额为美元）
            //6.计算平均值（销售额平均值）
            //下面列举一下ClosedXML大概能做什么
            //1.对指定单元格进行和并 2.格式化化数据（可以格式化日期，数字，自定义格式应有尽有，非常方便）3.计算（可以对范围的值进行求和，求平均，求条数，求最大小值等等，异常方便）4.设置Excel的各种样式（边框，单元格颜色，宽度等等）
            //
            //--创建作业本
            var wb = new XLWorkbook();
            //--Adding a worksheet
            var ws = wb.Worksheets.Add("信息表");
            //--Adding text
            //Title
            ws.Cell("B2").Value = "联系人信息表";

            //姓名
            ws.Cell("B3").Value = "姓名";
            ws.Cell("B4").Value = "牛掰掰";
            ws.Cell("B5").Value = "很多岁";
            ws.Cell("B6").SetValue("光头强");  //另一种值得设置方式

            //性别
            ws.Cell("C3").Value = "性别";
            ws.Cell("C4").Value = "女";
            ws.Cell("C5").Value = "男";
            ws.Cell("C6").SetValue("未知"); 

            // 年龄
            ws.Cell("D3").Value = "年龄";
            ws.Cell("D4").Value = 13;
            ws.Cell("D5").Value = 88;
            ws.Cell("D6").SetValue(46);

            //电话
            ws.Cell("E3").Value = "电话";
            ws.Cell("E4").Value = "10000";
            ws.Cell("E5").Value = "10086";
            ws.Cell("E6").SetValue("10010"); 

            //金额
            ws.Cell("F3").Value = "本季度销售业绩";
            ws.Cell("F4").Value = "6892651";
            ws.Cell("F5").Value = "698000";
            ws.Cell("F6").SetValue("9653445"); 
            //--Defining ranges
            //From worksheet
            var rngTable = ws.Range("B2:F6");

            //From another range
            var rngDates = rngTable.Range("D3:D5"); // The address is relative to rngTable (NOT the worksheet)
            var rngNumbers = rngTable.Range("E3:E5"); // The address is relative to rngTable (NOT the worksheet)
            //--Formatting dates and numbers
            //Using a OpenXML's predefined formats
            //rngDates.Style.NumberFormat.NumberFormatId = 15;

            //Using a custom format
            rngNumbers.Style.NumberFormat.Format = "$ #,##0";
            //--Format title cell in one shot
            rngTable.FirstCell().Style
                .Font.SetBold()
                .Fill.SetBackgroundColor(XLColor.AppleGreen)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //--Merge title cells
            rngTable.FirstRow().Merge(); // We could've also used: rngTable.Range("A1:E1").Merge() or rngTable.Row(1).Merge()

          
            //--Formatting headers
            var rngHeaders = rngTable.Range("A2:E2"); // The address is relative to rngTable (NOT the worksheet)
            rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Font.Bold = true;
            rngHeaders.Style.Font.FontColor = XLColor.DarkBlue;
            rngHeaders.Style.Fill.BackgroundColor = XLColor.Aqua;
            //--Create an Excel table with the data portion
            var rngData = ws.Range("B3:F6");
            var excelTable = rngData.CreateTable();

            // Add the totals row
            excelTable.ShowTotalsRow = true;
            // Put the average on the field "Income"
            // Notice how we're calling the cell by the column name
            excelTable.Field("本季度销售业绩").TotalsRowFunction = XLTotalsRowFunction.Average;
            // Put a label on the totals cell of the field "DOB"
            excelTable.Field("电话").TotalsRowLabel = "平均值:";
            //--Add thick borders
            //Add thick borders to the contents of our spreadsheet
            ws.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thick;

            // You can also specify the border for each side:
            ws.FirstColumn().Style.Border.LeftBorder = XLBorderStyleValues.Thick;
            ws.LastColumn().Style.Border.RightBorder = XLBorderStyleValues.Thick;
            ws.FirstRow().Style.Border.TopBorder = XLBorderStyleValues.Thick;
            ws.LastRow().Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            //--Adjust column widths to their content
            ws.Columns().AdjustToContents(); // You can also specify the range of columns to adjust, e.g.
                                             // ws.Columns(2, 6).AdjustToContents(); or ws.Columns("2-6").AdjustToContents();
                                             //wb.SaveAs("ExcelSample.xlsx");
                                             //如果文件太大，建议保存后通知用户下载，这是通过保存的形式，如果很小就直接用流的方式输出到下载就可以了

            //Console.WriteLine("Total bytes = " + memoryStream.ToArray().Length);
            var exportFileName = string.Concat(
                    "ExcelSample",
                    DateTime.Now.ToString("yyyyMMddHHmmss"),
                    ".xlsx");

            return new ExportExcelResult(wb)
            {
                SheetName = "人员列表",
                FileName = exportFileName,
                Workbook = wb
            };
           
         
        }
        
        private JArray GetExportData()
        {
            List<Spl_PersonModel> list = m_BLL.GetList(ref setNoPagerAscById, "");
            JArray jObjects = new JArray();

            foreach (var item in list)
            {
                var jo = new JObject();
                jo.Add("Id", item.Id);
                jo.Add("Name", item.Name);
                jo.Add("Sex", item.Sex);
                jo.Add("Age", item.Age);
                jo.Add("IDCard", item.IDCard);
                jo.Add("Phone", item.Phone);
                jo.Add("Email", item.Email);
                jo.Add("Address", item.Address);
                jo.Add("CreateTime", item.CreateTime);
                jo.Add("Region", item.Region);
                jo.Add("Category", item.Category);
                jObjects.Add(jo);
            }
            return jObjects;
        }
        #endregion
    }
}
