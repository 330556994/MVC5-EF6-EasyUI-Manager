using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Apps.DEF.IBLL;
using Apps.Common;
using Apps.Models.DEF;
using Apps.Web.Core;
using System.Collections.Generic;
using Apps.Locale;

namespace Apps.Web.Areas.Def.Controllers
{
    public class CaseTypeController : BaseController
    {
       [Dependency]
        public IDEF_CaseTypeBLL m_BLL { get; set; }
        ValidationErrors errors = new ValidationErrors();
        
        [SupportFilter]
        public ActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        [SupportFilter(ActionName = "Index")]
        public JsonResult GetList(string id)
        {
            List<DEF_CaseTypeModel> list = m_BLL.GetList(id, null);
            var json = from r in list
                       select new DEF_CaseTypeModel()
                       {
                           Id = r.Id,
                           Name = r.Name,
                           ParentId = r.ParentId,
                           IsLast = r.IsLast,
                           state = (m_BLL.GetList(r.Id, null).Count > 0) ? "closed" : "open"
                       };


            return Json(json);
        }

        [HttpPost]
        public JsonResult GetListByComTree(string id, string allFlag)
        {
            List<DEF_CaseTypeModel> list = m_BLL.GetList(id, allFlag);
            var json = from r in list
                       select new DEF_CaseTypeModelByComTree()
                       {
                           id = r.Id,
                           text = r.Name,
                           state = (m_BLL.GetList(r.Id, null).Count > 0) ? "closed" : "open"
                       };


            return Json(json);
        }

        #region 创建
        [SupportFilter]
        public ActionResult Create(string id)
        {

            
            DEF_CaseTypeModel entity = new DEF_CaseTypeModel()
            {
                ParentId = id,
            };
            return View(entity);
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Create(DEF_CaseTypeModel model)
        {
            model.Id = ResultHelper.NewId;
            model.IsLast = false;
            if (model != null && ModelState.IsValid)
            {
                if (m_BLL.Create(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "创建", "DEF_CaseType");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "创建", "DEF_CaseType");
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
            
            DEF_CaseTypeModel entity = m_BLL.GetById(id);
            return View(entity);
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Edit(DEF_CaseTypeModel model)
        {
            if (model != null && ModelState.IsValid)
            {

                if (m_BLL.Edit(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "修改", "DEF_CaseType");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "修改", "DEF_CaseType");
                    return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ":"+ErrorCol));
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
            
            DEF_CaseTypeModel entity = m_BLL.GetById(id);
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
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "DEF_CaseType");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "DEF_CaseType");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail));
            }


        }
        #endregion
    }
}
