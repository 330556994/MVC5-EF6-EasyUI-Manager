using System;
using Apps.Common;
using Apps.Models.DEF;
using System.Collections.Generic;
using Apps.Models;
namespace Apps.DEF.IBLL
{
    public partial interface IDEF_DefectBLL
    {
        bool AllSet(ref ValidationErrors errors, string begintime, string endtime, string member, string ids);
        bool CheckAll(ref ValidationErrors errors, string vercode, string userid);
        bool SetErrorLevel(ref ValidationErrors errors, int errorlevel, string ids);
        bool SetProcessState(ref ValidationErrors errors, bool state, string ids, string userid);
        bool SetMessageId(ref ValidationErrors errors, string ids, string messageId, string receiverTitle);
        bool CreateDefectReport(ref ValidationErrors errors, string vercode, string creator);
        bool Delete(ref ValidationErrors errors, string itemid,string vercode ,string code);
        bool DeleteCollection(ref ValidationErrors errors, string ids);
        bool entityIsExist(string itemid,string vercode ,string code);
        DEF_Defect GetById(string itemid,string vercode ,string code);
        List<DEF_DefectModel> GetList(ref GridPager pager, string querystr, string vercode, bool ok, bool no);
        DEF_DefectModel GetModelById(string itemid,string vercode ,string code);
        List<DEF_DefectModel> GetListByVerCode(ref GridPager pager, string vercode);
        bool SetDefectCloseState(ref ValidationErrors errors, bool closeState, string ids, string closer);
        bool UpdateRemark(ref ValidationErrors errors, string id, string remark);
        DEF_DefectModel GetModelByComplexId(string id);
        List<DEF_DefectModel> Query(ref GridPager pager, string vercode, string querystr);
        //负载均衡
        List<DEF_DefectModel> GetListByVerCode(ref GridPager pager, string vercode, bool ok, bool no);
        List<DEF_DefectModel> GetList2(ref GridPager pager, string querystr, string vercode, bool all, bool ok, bool no);
        List<DEF_DefectModel> GetListByVerCode2(ref GridPager pager, string vercode, bool all, bool ok, bool no);
    }
}
