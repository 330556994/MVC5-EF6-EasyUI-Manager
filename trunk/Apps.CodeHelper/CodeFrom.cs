using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Apps.CodeHelper
{
    public partial class CodeFrom : Form
    {


        static string conn = "";
        public CodeFrom()
        {
            InitializeComponent();
        }

        private void CodeFrom_Load(object sender, EventArgs e)
        {
            init();
        }


        private void init()
        {
            string strXmlPath = "Config.xml";
            txt_SQL.Text = XmlHelper.GetXmlFileValue(strXmlPath, "DataBase");


            conn = txt_SQL.Text;//"Integrated Security=SSPI;Initial Catalog='AppDB';Data Source='.';User ID='sa';Password='zhaoyun123!@#';Connect Timeout=30";
            Dictionary<string, string> tables = SqlHelper.GetAllTableName(conn);
            lb_Tables.DataSource = new BindingSource(tables, null);
            lb_Tables.DisplayMember = "key";
            lb_Tables.DisplayMember = "value";
        }
        private void lb_Tables_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                OutputCode();
            }
            catch { }
        }
        //输出代码
        private void OutputCode()
        {
            string tableName = lb_Tables.Text;

            //获取IDAL
            //txt_IDAL.Text = GetIDAL(tableName);
            ////获取IDAL
            //txt_DAL.Text = GetDAL(tableName);
            ////获取IBLL
            //txt_IBLL.Text = GetIBLL(tableName);
            ////获取BLL
            //txt_BLL.Text = GetBLL(tableName);
            ////获取Model
            //txt_Model.Text = GetModel(tableName);
            //获取Model
            txt_Controller.Text = GetController(tableName);
            //获取Model
            txt_Index.Text = GetIndex(tableName);
            //获取Model
            txt_Create.Text = GetCreate(tableName);
            //获取Model
            txt_Edit.Text = GetEdit(tableName);
            //获取Common
            txt_Common.Text = GetCommon(tableName);





            //tp_DAL.Text = tableName + "DAL";
            //tp_IDAL.Text = "I" + tableName + "DAL";
            //tp_IBLL.Text = tableName + "IBLL";
            //tp_BLL.Text = tableName + "BLL";
        }

        //#region IDAL
        //public string GetIDAL(string tableName)
        //{
        //    string leftStr = GetLeftStr(tableName);
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("using " + txt_prefix.Text + ".Models;\r\n");
        //    sb.Append("using System.Linq;\r\n");
        //    sb.Append("namespace " + txt_prefix.Text + "." + (leftStr=="Sys."?"":leftStr) + "IDAL\r\n");
        //    sb.Append("{\r\n");
        //    sb.Append("    public interface I" + tableName + "Repository\r\n");
        //    sb.Append("    {\r\n");
        //    sb.Append("        IQueryable<" + tableName + "> GetList(DBContainer db);\r\n");
        //    sb.Append("        int Create(" + tableName + " entity);\r\n");
        //    sb.Append("        int Delete(string id);\r\n");
        //    sb.Append("        void Delete(DBContainer db, string[] deleteCollection);\r\n");
        //    sb.Append("        int Edit(" + lb_Tables.Text + " entity);\r\n");
        //    sb.Append("        " + tableName + " GetById(string id);\r\n");
        //    sb.Append("        \r\n");
        //    sb.Append("    }\r\n");
        //    sb.Append("}");
        //    return sb.ToString();
        //}
        //#endregion
        //#region DAL
        //public string GetDAL(string tableName)
        //{
        //    string leftStr = GetLeftStr(tableName);
        //    StringBuilder sb = new StringBuilder();

        //    sb.Append("using System;\r\n");
        //    sb.Append("using System.Linq;\r\n");
        //    sb.Append("using " + txt_prefix.Text + "." + leftStr + "IDAL;\r\n");
        //    sb.Append("using " + txt_prefix.Text + ".Models;\r\n");
        //    sb.Append("using System.Data;\r\n");
        //    sb.Append("\r\n");
        //    sb.Append("namespace " + txt_prefix.Text + "." + (leftStr == "Sys." ? "" : leftStr) + "DAL\r\n");
        //    sb.Append("{\r\n");
        //    sb.Append("    public class " + tableName + "Repository : I" + tableName + "Repository, IDisposable\r\n");
        //    sb.Append("    {\r\n");
        //    sb.Append("\r\n");
        //    sb.Append("        public IQueryable<" + tableName + "> GetList(DBContainer db)\r\n");
        //    sb.Append("        {\r\n");
        //    sb.Append("            IQueryable<" + tableName + "> list = db." + tableName + ".AsQueryable();\r\n");
        //    sb.Append("            return list;\r\n");
        //    sb.Append("        }\r\n");
        //    sb.Append("\r\n");
        //    sb.Append("        public int Create(" + tableName + " entity)\r\n");
        //    sb.Append("        {\r\n");
        //    sb.Append("            using (DBContainer db = new DBContainer())\r\n");
        //    sb.Append("            {\r\n");
        //    sb.Append("                db." + tableName + ".AddObject(entity);\r\n");
        //    sb.Append("                return db.SaveChanges();\r\n");
        //    sb.Append("            }\r\n");
        //    sb.Append("        }\r\n");
        //    sb.Append("\r\n");
        //    sb.Append("        public int Delete(string id)\r\n");
        //    sb.Append("        {\r\n");
        //    sb.Append("            using (DBContainer db = new DBContainer())\r\n");
        //    sb.Append("            {\r\n");
        //    sb.Append("                " + tableName + " entity = db." + tableName + ".SingleOrDefault(a => a.Id == id);\r\n");
        //    sb.Append("                if (entity != null)\r\n");
        //    sb.Append("                {\r\n");
        //    sb.Append("\r\n");
        //    sb.Append("                    db." + tableName + ".DeleteObject(entity);\r\n");
        //    sb.Append("                }\r\n");
        //    sb.Append("                return db.SaveChanges();\r\n");
        //    sb.Append("            }\r\n");
        //    sb.Append("        }\r\n");
        //    sb.Append("\r\n");
        //    sb.Append("        public void Delete(DBContainer db, string[] deleteCollection)\r\n");
        //    sb.Append("        {\r\n");
        //    sb.Append("            IQueryable<" + tableName + "> collection = from f in db." + tableName + "\r\n");
        //    sb.Append("                                               where deleteCollection.Contains(f.Id)\r\n");
        //    sb.Append("                                               select f;\r\n");
        //    sb.Append("            foreach (var deleteItem in collection)\r\n");
        //    sb.Append("            {\r\n");
        //    sb.Append("                db." + tableName + ".DeleteObject(deleteItem);\r\n");
        //    sb.Append("            }\r\n");
        //    sb.Append("        }\r\n");
        //    sb.Append("\r\n");
        //    sb.Append("        public int Edit(" + tableName + " entity)\r\n");
        //    sb.Append("        {\r\n");
        //    sb.Append("            using (DBContainer db = new DBContainer())\r\n");
        //    sb.Append("            {\r\n");
        //    sb.Append("                db." + tableName + ".Attach(entity);\r\n");
        //    sb.Append("                db.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);\r\n");
        //    sb.Append("                return db.SaveChanges();\r\n");
        //    sb.Append("            }\r\n");
        //    sb.Append("        }\r\n");
        //    sb.Append("\r\n");
        //    sb.Append("        public " + tableName + " GetById(string id)\r\n");
        //    sb.Append("        {\r\n");
        //    sb.Append("            using (DBContainer db = new DBContainer())\r\n");
        //    sb.Append("            {\r\n");
        //    sb.Append("                return db." + tableName + ".SingleOrDefault(a => a.Id == id);\r\n");
        //    sb.Append("            }\r\n");
        //    sb.Append("        }\r\n");
        //    sb.Append("\r\n");
        //    sb.Append("        public bool IsExist(string id)\r\n");
        //    sb.Append("        {\r\n");
        //    sb.Append("            using (DBContainer db = new DBContainer())\r\n");
        //    sb.Append("            {\r\n");
        //    sb.Append("                " + tableName + " entity = GetById(id);\r\n");
        //    sb.Append("                if (entity != null)\r\n");
        //    sb.Append("                    return true;\r\n");
        //    sb.Append("                return false;\r\n");
        //    sb.Append("            }\r\n");
        //    sb.Append("        }\r\n");
        //    sb.Append("        public void Dispose()\r\n");
        //    sb.Append("        {\r\n");
        //    sb.Append("\r\n");
        //    sb.Append("        }\r\n");
        //    sb.Append("    }\r\n");
        //    sb.Append("}\r\n");

        //    return sb.ToString();
        //}
        //#endregion
        //#region IBLL
        //public string GetIBLL(string tableName)
        //{
        //    string leftStr = GetLeftStr(tableName);
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("using System.Collections.Generic;\r\n");
        //    sb.Append("using " + txt_prefix.Text + ".Common;\r\n");
        //    sb.Append("using " + txt_prefix.Text + ".Models." + leftStr.Replace(".", "") + ";\r\n");
        //    sb.Append("namespace " + txt_prefix.Text + "." + (leftStr == "Sys." ? "" : leftStr) + "IBLL\r\n");
        //    sb.Append("{\r\n");
        //    sb.Append("    public interface I" + tableName + "BLL\r\n");
        //    sb.Append("    {\r\n");
        //    sb.Append("        List<" + tableName + "Model> GetList(ref GridPager pager, string queryStr);\r\n");
        //    sb.Append("        bool Create(ref ValidationErrors errors, " + tableName + "Model model);\r\n");
        //    sb.Append("        bool Delete(ref ValidationErrors errors, string id);\r\n");
        //    sb.Append("        bool Delete(ref ValidationErrors errors, string[] deleteCollection);\r\n");
        //    sb.Append("        bool Edit(ref ValidationErrors errors, " + tableName + "Model model);\r\n");
        //    sb.Append("        " + tableName + "Model GetById(string id);\r\n");
        //    sb.Append("        \r\n");
        //    sb.Append("    }\r\n");
        //    sb.Append("}\r\n");
        //    return sb.ToString();
        //}
        //#endregion
        //#region BLL
        //public string GetBLL(string tableName)
        //{
        //    string leftStr = GetLeftStr(tableName);
        //    List<string> fields = SqlHelper.GetColumnField(conn, tableName);
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("using System;\r\n");
        //    sb.Append("using System.Collections.Generic;\r\n");
        //    sb.Append("using System.Linq;\r\n");
        //    sb.Append("using Microsoft.Practices.Unity;\r\n");
        //    sb.Append("using " + txt_prefix.Text + ".Models;\r\n");
        //    sb.Append("using " + txt_prefix.Text + ".Common;\r\n");
        //    sb.Append("using System.Transactions;\r\n");
        //    sb.Append("using " + txt_prefix.Text + ".Models." + leftStr.Replace(".", "") + ";\r\n");
        //    sb.Append("using " + txt_prefix.Text + "." + (leftStr == "Sys." ? "" : leftStr) + "IBLL;\r\n");
        //    sb.Append("using " + txt_prefix.Text + "." + (leftStr == "Sys." ? "" : leftStr) + "IDAL;\r\n");
        //    sb.Append("using " + txt_prefix.Text + ".BLL.Core;\r\n");
        //    sb.Append("\r\n");
        //    sb.Append("namespace " + txt_prefix.Text + "." + (leftStr == "Sys." ? "" : leftStr) + "BLL\r\n");
        //    sb.Append("{\r\n");
        //    sb.Append("    public class " + tableName + "BLL : BaseBLL, I" + tableName + "BLL\r\n");
        //    sb.Append("    {\r\n");
        //    sb.Append("        [Dependency]\r\n");
        //    sb.Append("        public I" + tableName + "Repository m_Rep { get; set; }\r\n");
        //    sb.Append("\r\n");
        //    sb.Append("        public List<" + tableName + "Model> GetList(ref GridPager pager, string queryStr)\r\n");
        //    sb.Append("        {\r\n");
        //    sb.Append("\r\n");
        //    sb.Append("            IQueryable<" + tableName + "> queryData = null;\r\n");
        //    sb.Append("            if (!string.IsNullOrWhiteSpace(queryStr))\r\n");
        //    sb.Append("            {\r\n");
        //    sb.Append("                queryData = m_Rep.GetList(db).Where(a => a.Name.Contains(queryStr) || a.Note.Contains(queryStr));\r\n");
        //    sb.Append("            }\r\n");
        //    sb.Append("            else\r\n");
        //    sb.Append("            {\r\n");
        //    sb.Append("                queryData = m_Rep.GetList(db);\r\n");
        //    sb.Append("            }\r\n");
        //    sb.Append("            pager.totalRows = queryData.Count();\r\n");
        //    sb.Append("            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);\r\n");
        //    sb.Append("            return CreateModelList(ref queryData);\r\n");
        //    sb.Append("        }\r\n");
        //    sb.Append("        private List<" + tableName + "Model> CreateModelList(ref IQueryable<" + tableName + "> queryData)\r\n");
        //    sb.Append("        {\r\n");
        //    sb.Append("\r\n");
        //    sb.Append("            List<" + tableName + "Model> modelList = (from r in queryData\r\n");
        //    sb.Append("                                              select new " + tableName + "Model\r\n");
        //    sb.Append("                                              {\r\n");
        //    int count = 0;
        //    foreach (string field in fields)
        //    {
        //        count++;
        //        if (fields.Count == count)
        //            sb.Append("                                                  " + field + " = r." + field + "\r\n");
        //        else
        //            sb.Append("                                                  " + field + " = r." + field + ",\r\n");
        //    }
        //    sb.Append("                                              }).ToList();\r\n");
        //    sb.Append("            return modelList;\r\n");
        //    sb.Append("        }\r\n");
        //    sb.Append("\r\n");
        //    sb.Append("        public bool Create(ref ValidationErrors errors, " + tableName + "Model model)\r\n");
        //    sb.Append("        {\r\n");
        //    sb.Append("            try\r\n");
        //    sb.Append("            {\r\n");
        //    sb.Append("                " + tableName + " entity = m_Rep.GetById(model.Id);\r\n");
        //    sb.Append("                if (entity != null)\r\n");
        //    sb.Append("                {\r\n");
        //    sb.Append("                    errors.Add(Resource.PrimaryRepeat);\r\n");
        //    sb.Append("                    return false;\r\n");
        //    sb.Append("                }\r\n");
        //    sb.Append("                entity = new " + tableName + "();\r\n");
        //    foreach (string field in fields)
        //    {
        //        sb.Append("                entity." + field + " = model." + field + ";\r\n");
        //    }
        //    sb.Append("                if (m_Rep.Create(entity) == 1)\r\n");
        //    sb.Append("                {\r\n");
        //    sb.Append("                    return true;\r\n");
        //    sb.Append("                }\r\n");
        //    sb.Append("                else\r\n");
        //    sb.Append("                {\r\n");
        //    sb.Append("                    errors.Add(Resource.InsertFail);\r\n");
        //    sb.Append("                    return false;\r\n");
        //    sb.Append("                }\r\n");
        //    sb.Append("            }\r\n");
        //    sb.Append("            catch (Exception ex)\r\n");
        //    sb.Append("            {\r\n");
        //    sb.Append("                errors.Add(ex.Message);\r\n");
        //    sb.Append("                ExceptionHander.WriteException(ex);\r\n");
        //    sb.Append("                return false;\r\n");
        //    sb.Append("            }\r\n");
        //    sb.Append("        }\r\n");
        //    sb.Append("\r\n");
        //    sb.Append("        public bool Delete(ref ValidationErrors errors, string id)\r\n");
        //    sb.Append("        {\r\n");
        //    sb.Append("            try\r\n");
        //    sb.Append("            {\r\n");
        //    sb.Append("                if (m_Rep.Delete(id) == 1)\r\n");
        //    sb.Append("                {\r\n");
        //    sb.Append("                    return true;\r\n");
        //    sb.Append("                }\r\n");
        //    sb.Append("                else\r\n");
        //    sb.Append("                {\r\n");
        //    sb.Append("                    return false;\r\n");
        //    sb.Append("                }\r\n");
        //    sb.Append("            }\r\n");
        //    sb.Append("            catch (Exception ex)\r\n");
        //    sb.Append("            {\r\n");
        //    sb.Append("                errors.Add(ex.Message);\r\n");
        //    sb.Append("                ExceptionHander.WriteException(ex);\r\n");
        //    sb.Append("                return false;\r\n");
        //    sb.Append("            }\r\n");
        //    sb.Append("        }\r\n");
        //    sb.Append("\r\n");
        //    sb.Append("        public bool Delete(ref ValidationErrors errors, string[] deleteCollection)\r\n");
        //    sb.Append("        {\r\n");
        //    sb.Append("            try\r\n");
        //    sb.Append("            {\r\n");
        //    sb.Append("                if (deleteCollection != null)\r\n");
        //    sb.Append("                {\r\n");
        //    sb.Append("                    using (TransactionScope transactionScope = new TransactionScope())\r\n");
        //    sb.Append("                    {\r\n");
        //    sb.Append("                        if (m_Rep.Delete(deleteCollection) == deleteCollection.Length)\r\n");
        //    sb.Append("                        {\r\n");
        //    sb.Append("                            transactionScope.Complete();\r\n");
        //    sb.Append("                            return true;\r\n");
        //    sb.Append("                        }\r\n");
        //    sb.Append("                        else\r\n");
        //    sb.Append("                        {\r\n");
        //    sb.Append("                            Transaction.Current.Rollback();\r\n");
        //    sb.Append("                            return false;\r\n");
        //    sb.Append("                        }\r\n");
        //    sb.Append("                    }\r\n");
        //    sb.Append("                }\r\n");
        //    sb.Append("                return false;\r\n");
        //    sb.Append("            }\r\n");
        //    sb.Append("            catch (Exception ex)\r\n");
        //    sb.Append("            {\r\n");
        //    sb.Append("                errors.Add(ex.Message);\r\n");
        //    sb.Append("                ExceptionHander.WriteException(ex);\r\n");
        //    sb.Append("                return false;\r\n");
        //    sb.Append("            }\r\n");
        //    sb.Append("        }\r\n");
        //    sb.Append("        public bool Edit(ref ValidationErrors errors, " + tableName + "Model model)\r\n");
        //    sb.Append("        {\r\n");
        //    sb.Append("            try\r\n");
        //    sb.Append("            {\r\n");
        //    sb.Append("                " + tableName + " entity = m_Rep.GetById(model.Id);\r\n");
        //    sb.Append("                if (entity == null)\r\n");
        //    sb.Append("                {\r\n");
        //    sb.Append("                    errors.Add(Resource.Disable);\r\n");
        //    sb.Append("                    return false;\r\n");
        //    sb.Append("                }\r\n");
        //    foreach (string field in fields)
        //    {
        //        sb.Append("                entity." + field + " = model." + field + ";\r\n");
        //    }
        //    sb.Append("\r\n");
        //    sb.Append("               if (m_Rep.Edit(entity) == 1)\r\n");
        //    sb.Append("                {\r\n");
        //    sb.Append("                    return true;\r\n");
        //    sb.Append("                }\r\n");
        //    sb.Append("                else\r\n");
        //    sb.Append("                {\r\n");
        //    sb.Append("                    errors.Add(Resource.NoDataChange);\r\n");
        //    sb.Append("                    return false;\r\n");
        //    sb.Append("                }\r\n");
        //    sb.Append("            }\r\n");
        //    sb.Append("            catch (Exception ex)\r\n");
        //    sb.Append("            {\r\n");
        //    sb.Append("                errors.Add(ex.Message);\r\n");
        //    sb.Append("                ExceptionHander.WriteException(ex);\r\n");
        //    sb.Append("                return false;\r\n");
        //    sb.Append("            }\r\n");
        //    sb.Append("        }\r\n");
        //    sb.Append("\r\n");
        //    sb.Append("        public bool IsExists(string id)\r\n");
        //    sb.Append("        {\r\n");
        //    sb.Append("            if (db." + tableName + ".SingleOrDefault(a => a.Id == id) != null)\r\n");
        //    sb.Append("           {\r\n");
        //    sb.Append("                return true;\r\n");
        //    sb.Append("           }\r\n");
        //    sb.Append("            return false;\r\n");
        //    sb.Append("        }\r\n");
        //    sb.Append("\r\n");
        //    sb.Append("        public " + tableName + "Model GetById(string id)\r\n");
        //    sb.Append("        {\r\n");
        //    sb.Append("            if (IsExist(id))\r\n");
        //    sb.Append("            {\r\n");
        //    sb.Append("                " + tableName + " entity = m_Rep.GetById(id);\r\n");
        //    sb.Append("                " + tableName + "Model model = new " + tableName + "Model();\r\n");
        //    foreach (string field in fields)
        //    {
        //        sb.Append("                model." + field + " = entity." + field + ";\r\n");
        //    }
        //    sb.Append("                return model;\r\n");
        //    sb.Append("            }\r\n");
        //    sb.Append("            else\r\n");
        //    sb.Append("            {\r\n");
        //    sb.Append("                return null;\r\n");
        //    sb.Append("            }\r\n");
        //    sb.Append("        }\r\n");
        //    sb.Append("\r\n");
        //    sb.Append("        public bool IsExist(string id)\r\n");
        //    sb.Append("        {\r\n");
        //    sb.Append("            return m_Rep.IsExist(id);\r\n");
        //    sb.Append("        }\r\n");
        //    sb.Append("    }\r\n");
        //    sb.Append(" }\r\n");
        //    return sb.ToString();
        //}
        //#endregion
        //#region Model
        //public string GetModel(string tableName)
        //{
        //    string leftStr = GetLeftStr(tableName);
        //    List<CompleteField> fields = SqlHelper.GetColumnCompleteField(conn, tableName);

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("using System;\r\n");
        //    sb.Append("using System.ComponentModel.DataAnnotations;\r\n");
        //    sb.Append("using " + txt_prefix.Text + ".Models;\r\n");
        //    sb.Append("namespace " + txt_prefix.Text + ".Models." + leftStr.Replace(".", "") + "\r\n");
        //    sb.Append("{\r\n");
        //    sb.Append("    public class " + tableName + "Model\r\n");
        //    sb.Append("    {\r\n");
        //    foreach (CompleteField field in fields)
        //    {
        //        sb.Append("" + SetValid(field) + "");
        //        sb.Append("        [Display(Name = \"" + (field.remark == "" && field.remark.Length < 10 ? field.name : field.remark) + "\")]\r\n");
        //        sb.Append("        public " + SqlHelper.GetType(field.xType) + " " + field.name + " { get; set; }\r\n");
        //        sb.Append("\r\n");
        //    }
        //    sb.Append("     }\r\n");
        //    sb.Append("}\r\n");


        //    return sb.ToString();
        //}
        //#endregion
        #region Controller
        public string GetController(string tableName)
        {
            string leftStr = GetLeftStr(tableName);
            List<CompleteField> fields = SqlHelper.GetColumnCompleteField(conn, tableName);

            StringBuilder sb = new StringBuilder();
            sb.Append("using System.Collections.Generic;\r\n");
            sb.Append("using System.Linq;\r\n");
            sb.Append("using Apps.Web.Core;\r\n");
            sb.Append("using Apps."+(leftStr=="Sys"?"":(leftStr+"."))+"IBLL;\r\n");
            sb.Append("using Apps.Locale;\r\n");
            sb.Append("using System.Web.Mvc;\r\n");
            sb.Append("using " + txt_prefix.Text + ".Common;\r\n");
            sb.Append("using " + txt_prefix.Text + ".IBLL;\r\n");
            sb.Append("using " + txt_prefix.Text + ".Models."+leftStr+";\r\n");
            sb.Append("using Microsoft.Practices.Unity;\r\n");
            sb.Append("\r\n");
            sb.Append("namespace " + txt_prefix.Text + ".Web" + (leftStr=="Sys"?"":(".Areas."+leftStr)) + ".Controllers\r\n");
            sb.Append("{\r\n");
            sb.Append("    public class " + tableName.Replace(leftStr+"_","") + "Controller : BaseController\r\n");
            sb.Append("    {\r\n");
            sb.Append("        [Dependency]\r\n");
            sb.Append("        public I" + tableName + "BLL m_BLL { get; set; }\r\n");
            sb.Append("        ValidationErrors errors = new ValidationErrors();\r\n");
            sb.Append("        \r\n");
            sb.Append("        [SupportFilter]\r\n");
            sb.Append("        public ActionResult Index()\r\n");
            sb.Append("        {\r\n");
            sb.Append("            return View();\r\n");
            sb.Append("        }\r\n");
            sb.Append("        [HttpPost]\r\n");
             sb.Append("       [SupportFilter(ActionName=\"Index\")]\r\n");
            sb.Append("        public JsonResult GetList(GridPager pager, string queryStr)\r\n");
            sb.Append("        {\r\n");
            sb.Append("            List<" + tableName + "Model> list = m_BLL.GetList(ref pager, queryStr);\r\n");
            sb.Append("            GridRows<" + tableName + "Model> grs = new GridRows<" + tableName + "Model>();\r\n");
            sb.Append("            grs.rows = list;\r\n");
            sb.Append("            grs.total = pager.totalRows;\r\n");
            sb.Append("            return Json(grs);\r\n");
            sb.Append("        }\r\n");

            sb.Append("        #region 创建\r\n");
            sb.Append("        [SupportFilter]\r\n");
            sb.Append("        public ActionResult Create()\r\n");
            sb.Append("        {\r\n");
            sb.Append("            ViewBag.Perm = GetPermission();\r\n");
            sb.Append("            return View();\r\n");
            sb.Append("        }\r\n");
            sb.Append("\r\n");
            sb.Append("        [HttpPost]\r\n");
            sb.Append("        [SupportFilter]\r\n");
            sb.Append("        public JsonResult Create(" + tableName + "Model model)\r\n");
            sb.Append("        {\r\n");
            sb.Append("            model.Id = ResultHelper.NewId;\r\n");
            sb.Append("            model.CreateTime = ResultHelper.NowTime;\r\n");
            sb.Append("            if (model != null && ModelState.IsValid)\r\n");
            sb.Append("            {\r\n");
            sb.Append("\r\n");
            sb.Append("                if (m_BLL.Create(ref errors, model))\r\n");
            sb.Append("                {\r\n");
            sb.Append("                    LogHandler.WriteServiceLog(GetUserId(), \"" + fields[0].name + "\" + model.Id + \"," + fields[1].name + "\" + model.Name, \"成功\", \"创建\", \""+(txt_ModelName.Text==""?tableName:txt_ModelName.Text)+"\");\r\n");
            sb.Append("                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));\r\n");
            sb.Append("                }\r\n");
            sb.Append("                else\r\n");
            sb.Append("                {\r\n");
            sb.Append("                    string ErrorCol = errors.Error;\r\n");
            sb.Append("                    LogHandler.WriteServiceLog(GetUserId(), \"" + fields[0].name + "\" + model.Id + \"," + fields[1].name + "\" + model.Name + \",\" + ErrorCol, \"失败\", \"创建\", \""+(txt_ModelName.Text==""?tableName:txt_ModelName.Text)+"\");\r\n");
            sb.Append("                    return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol));\r\n");
            sb.Append("                }\r\n");
            sb.Append("            }\r\n");
            sb.Append("            else\r\n");
            sb.Append("            {\r\n");
            sb.Append("                return Json(JsonHandler.CreateMessage(0, Resource.InsertFail));\r\n");
            sb.Append("            }\r\n");
            sb.Append("        }\r\n");
            sb.Append("        #endregion\r\n");
            sb.Append("\r\n");
            sb.Append("        #region 修改\r\n");
            sb.Append("        [SupportFilter]\r\n");
            sb.Append("        public ActionResult Edit(string id)\r\n");
            sb.Append("        {\r\n");
            sb.Append("            ViewBag.Perm = GetPermission();\r\n");
            sb.Append("            " + tableName + "Model entity = m_BLL.GetById(id);\r\n");
            sb.Append("            return View(entity);\r\n");
            sb.Append("        }\r\n");
            sb.Append("\r\n");
            sb.Append("        [HttpPost]\r\n");
            sb.Append("        [SupportFilter]\r\n");
            sb.Append("        public JsonResult Edit(" + tableName + "Model model)\r\n");
            sb.Append("        {\r\n");
            sb.Append("            if (model != null && ModelState.IsValid)\r\n");
            sb.Append("            {\r\n");
            sb.Append("\r\n");
            sb.Append("                if (m_BLL.Edit(ref errors, model))\r\n");
            sb.Append("                {\r\n");
            sb.Append("                    LogHandler.WriteServiceLog(GetUserId(), \"" + fields[0].name + "\" + model.Id + \"," + fields[1].name + "\" + model.Name, \"成功\", \"修改\", \""+(txt_ModelName.Text==""?tableName:txt_ModelName.Text)+"\");\r\n");
            sb.Append("                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));\r\n");
            sb.Append("                }\r\n");
            sb.Append("                else\r\n");
            sb.Append("                {\r\n");
            sb.Append("                    string ErrorCol = errors.Error;\r\n");
            sb.Append("                    LogHandler.WriteServiceLog(GetUserId(), \"" + fields[0].name + "\" + model.Id + \"," + fields[1].name + "\" + model.Name + \",\" + ErrorCol, \"失败\", \"修改\", \""+(txt_ModelName.Text==""?tableName:txt_ModelName.Text)+"\");\r\n");
            sb.Append("                    return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ErrorCol));\r\n");
            sb.Append("                }\r\n");
            sb.Append("            }\r\n");
            sb.Append("            else\r\n");
            sb.Append("            {\r\n");
            sb.Append("                return Json(JsonHandler.CreateMessage(0, Resource.EditFail));\r\n");
            sb.Append("            }\r\n");
            sb.Append("        }\r\n");
            sb.Append("        #endregion\r\n");
            sb.Append("\r\n");
            sb.Append("        #region 详细\r\n");
            sb.Append("        [SupportFilter]\r\n");
            sb.Append("        public ActionResult Details(string id)\r\n");
            sb.Append("        {\r\n");
            sb.Append("            ViewBag.Perm = GetPermission();\r\n");
            sb.Append("            " + tableName + "Model entity = m_BLL.GetById(id);\r\n");
            sb.Append("            return View(entity);\r\n");
            sb.Append("        }\r\n");
            sb.Append("\r\n");
            sb.Append("        #endregion\r\n");
            sb.Append("\r\n");
            sb.Append("        #region 删除\r\n");
            sb.Append("        [HttpPost]\r\n");
            sb.Append("        [SupportFilter]\r\n");
            sb.Append("        public JsonResult Delete(string id)\r\n");
            sb.Append("        {\r\n");
            sb.Append("            if (!string.IsNullOrWhiteSpace(id))\r\n");
            sb.Append("            {\r\n");
            sb.Append("                if (m_BLL.Delete(ref errors, id))\r\n");
            sb.Append("                {\r\n");
            sb.Append("                    LogHandler.WriteServiceLog(GetUserId(), \"Id:\" + id, \"成功\", \"删除\", \""+(txt_ModelName.Text==""?tableName:txt_ModelName.Text)+"\");\r\n");
            sb.Append("                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));\r\n");
            sb.Append("                }\r\n");
            sb.Append("                else\r\n");
            sb.Append("                {\r\n");
            sb.Append("                    string ErrorCol = errors.Error;\r\n");
            sb.Append("                    LogHandler.WriteServiceLog(GetUserId(), \"" + fields[0].name + "\" + id + \",\" + ErrorCol, \"失败\", \"删除\", \""+(txt_ModelName.Text==""?tableName:txt_ModelName.Text)+"\");\r\n");
            sb.Append("                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));\r\n");
            sb.Append("                }\r\n");
            sb.Append("            }\r\n");
            sb.Append("            else\r\n");
            sb.Append("            {\r\n");
            sb.Append("                return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail));\r\n");
            sb.Append("            }\r\n");
            sb.Append("\r\n");
            sb.Append("\r\n");
            sb.Append("        }\r\n");
            sb.Append("        #endregion\r\n");
          
            sb.Append("    }\r\n");
            sb.Append("}\r\n");
            return sb.ToString();
        }
        #endregion
        #region Index
        public string GetIndex(string tableName)
        {
            string leftStr = GetLeftStr(tableName);
            List<CompleteField> fields = SqlHelper.GetColumnCompleteField(conn, tableName);

            StringBuilder sb = new StringBuilder();
            sb.Append("@using " + txt_prefix.Text + ".Web.Core;\r\n");
            sb.Append("@using " + txt_prefix.Text + ".Web;\r\n");
            sb.Append("@using " + txt_prefix.Text + ".Common;\r\n");
            sb.Append("@using " + txt_prefix.Text + ".Models.Sys;\r\n");
            sb.Append("@using " + txt_prefix.Text + ".Locale;\r\n");
            sb.Append("@{\r\n");
            sb.Append("    ViewBag.Title = \"" + (txt_ModelName.Text == "" ? "主页" : txt_ModelName.Text) + "\";\r\n");
            sb.Append("    Layout = \"~/Views/Shared/_Index_Layout.cshtml\";\r\n");
            sb.Append("    List<permModel> perm = null;\r\n");
            sb.Append("}\r\n");
            sb.Append("<div class=\"mvctool\">\r\n");
            sb.Append("    <input id=\"txtQuery\" type=\"text\" class=\"searchText\" />\r\n");
            sb.Append("    @Html.ToolButton(\"btnQuery\", \"fa fa-search\", Resource.Query,ref perm, \"Query\", true)\r\n");
            sb.Append("    @Html.ToolButton(\"btnCreate\", \"fa fa-plus\", Resource.Create,ref perm, \"Create\", true)\r\n");
            sb.Append("    @Html.ToolButton(\"btnEdit\", \"fa fa-pencil\", Resource.Edit,ref perm, \"Edit\", true)\r\n");
            sb.Append("    @Html.ToolButton(\"btnDetails\", \"fa fa-list\", Resource.Details,ref perm, \"Details\", true)\r\n");
            sb.Append("    @Html.ToolButton(\"btnDelete\", \"fa fa-trash\", Resource.Delete,ref perm, \"Delete\", true)\r\n");
         
            sb.Append("</div>\r\n");
            sb.Append("<table id=\"List\"></table>\r\n");
            sb.Append("\r\n");
            sb.Append("<div id=\"modalwindow\" class=\"easyui-window\" style=\"width:800px; height:400px;\" data-options=\"modal:true,closed:true,minimizable:false,shadow:false\"></div>\r\n");
            sb.Append("@Html.Partial(\"~/Views/Shared/_Partial_AutoGrid.cshtml\")\r\n");
            sb.Append("<script type=\"text/javascript\">\r\n");
            sb.Append("    $(function () {\r\n");
            sb.Append("        $('#List').datagrid({\r\n");
            sb.Append("            url: '@Url.Action(\"GetList\")',\r\n");
            sb.Append("            width:SetGridWidthSub(10),\r\n");
            sb.Append("            methord: 'post',\r\n");
            sb.Append("            height: SetGridHeightSub(45),\r\n");
            sb.Append("            fitColumns: true,\r\n");
            bool isSort = false;
            foreach (CompleteField field in fields)
            {
                if (field.name == "Sort")
                {
                    isSort = true; 
                }
            }
            if (isSort)
            {
                sb.Append("            sortName: 'Sort',\r\n");
                sb.Append("            sortOrder: 'asc',\r\n");
            }
            else
            {
                sb.Append("            sortName: 'CreateTime',\r\n");
                sb.Append("            sortOrder: 'desc',\r\n");
            }
          
            sb.Append("            idField: 'Id',\r\n");
            sb.Append("            pageSize: 15,\r\n");
            sb.Append("            pageList: [15, 20, 30, 40, 50],\r\n");
            sb.Append("            pagination: true,\r\n");
            sb.Append("            striped: true, //奇偶行是否区分\r\n");
            sb.Append("            singleSelect: true,//单选模式\r\n");
            sb.Append("            //rownumbers: true,//行号\r\n");
            sb.Append("            columns: [[\r\n");
            foreach (CompleteField field in fields)
            {
                if (field.name == "Id")
                {
                    sb.Append("                { field: '" + field.name + "', title: '" + (field.remark == "" ? field.name : field.remark) + "', width: 80,hidden:true },\r\n");
                }
                else
                {
                    sb.Append("                { field: '" + field.name + "', title: '" + (field.remark == "" ? field.name : field.remark) + "', width: 80,sortable:true },\r\n");
                }
            }
            sb.Remove(sb.ToString().LastIndexOf(','), 1);
            sb.Append("            ]]\r\n");
            sb.Append("        });\r\n");
            sb.Append("    });\r\n");
          
            sb.Append("    //ifram 返回\r\n");
            sb.Append("    function frameReturnByClose() {\r\n");
            sb.Append("        $(\"#modalwindow\").window('close');\r\n");
            sb.Append("    }\r\n");
            sb.Append("    function frameReturnByReload(flag) {\r\n");
            sb.Append("        if (flag)\r\n");
            sb.Append("            $(\"#List\").datagrid('load');\r\n");
            sb.Append("        else\r\n");
            sb.Append("            $(\"#List\").datagrid('reload');\r\n");
            sb.Append("    }\r\n");
            sb.Append("    function frameReturnByMes(mes) {\r\n");
            sb.Append("        $.messageBox5s('@Resource.Tip', mes);\r\n");
            sb.Append("    }\r\n");
            sb.Append("    $(function () {\r\n");
            sb.Append("        $(\"#btnCreate\").click(function () {\r\n");
            sb.Append("            $(\"#modalwindow\").html(\"<iframe width='100%' height='100%' scrolling='auto' frameborder='0'' src='@Url.Action(\"Create\")'></iframe>\");\r\n");
            sb.Append("            $(\"#modalwindow\").window({ title: '@Resource.Create', width: 700, height: 400, iconCls: 'fa fa-plus' }).window('open');\r\n");
            sb.Append("        });\r\n");
            sb.Append("        $(\"#btnEdit\").click(function () {\r\n");
            sb.Append("            var row = $('#List').datagrid('getSelected');\r\n");
            sb.Append("            if (row != null) {\r\n");
            sb.Append("                $(\"#modalwindow\").html(\"<iframe width='100%' height='99%'  frameborder='0' src='@Url.Action(\"Edit\")?id=\" + row.Id + \"&Ieguid=\" + GetGuid() + \"'></iframe>\");\r\n");
            sb.Append("                $(\"#modalwindow\").window({ title: '@Resource.Edit', width: 700, height: 400, iconCls: 'fa fa-pencil' }).window('open');\r\n");
            sb.Append("            } else { $.messageBox5s('@Resource.Tip', '@Resource.PlaseChooseToOperatingRecords'); }\r\n");
            sb.Append("        });\r\n");
            sb.Append("        $(\"#btnDetails\").click(function () {\r\n");
            sb.Append("            var row = $('#List').datagrid('getSelected');\r\n");
            sb.Append("            if (row != null) {\r\n");
            sb.Append("                $(\"#modalwindow\").html(\"<iframe width='100%' height='98%' scrolling='auto' frameborder='0' src='@Url.Action(\"Details\")?id=\" + row.Id + \"&Ieguid=\" + GetGuid() + \"'></iframe>\");\r\n");
            sb.Append("                $(\"#modalwindow\").window({ title: '@Resource.Details', width: 700, height: 400, iconCls: 'fa fa-list' }).window('open');\r\n");
            sb.Append("            } else { $.messageBox5s('@Resource.Tip', '@Resource.PlaseChooseToOperatingRecords'); }\r\n");
            sb.Append("	        });\r\n");
            sb.Append("        $(\"#btnQuery\").click(function () {\r\n");
            sb.Append("            var queryStr = $(\"#txtQuery\").val();\r\n");
            sb.Append("            if (queryStr == null) {\r\n");
            sb.Append("                queryStr = \"%\";\r\n");
            sb.Append("            }\r\n");
            sb.Append("            $(\"#List\").datagrid(\"load\", { queryStr: encodeURI(queryStr) });\r\n");
            sb.Append("\r\n");
            sb.Append("        });\r\n");
            sb.Append("        $(\"#btnDelete\").click(function () {\r\n");
            sb.Append("             dataDelete(\"@Url.Action(\"Delete\")\", \"List\");\r\n");
            sb.Append("	        });\r\n");
            sb.Append("    });\r\n");
            sb.Append("</script>\r\n");

            return sb.ToString();
        }
          #endregion
        #region Create
        public string GetCreate(string tableName)
        {
            string leftStr = GetLeftStr(tableName);
            List<CompleteField> fields = SqlHelper.GetColumnCompleteField(conn, tableName);

            StringBuilder sb = new StringBuilder();
            sb.Append("@model " + txt_prefix.Text + ".Models." + (leftStr == "Sys" ? "" : (leftStr + ".")) + "" + tableName + "Model\r\n");
            sb.Append("@using " + txt_prefix.Text + ".Web.Core;\r\n");
            sb.Append("@using " + txt_prefix.Text + ".Common;\r\n");
            sb.Append("@using " + txt_prefix.Text + ".Models." + leftStr + ";\r\n");
            sb.Append("@using " + txt_prefix.Text + ".Web;\r\n");
            sb.Append("@using " + txt_prefix.Text + ".Locale;\r\n");
            sb.Append("@using " + txt_prefix.Text + ".Models.Sys;\r\n");
            sb.Append("@{\r\n");
            sb.Append("    ViewBag.Title = \"创建\";\r\n");
            sb.Append("    Layout = \"~/Views/Shared/_Index_LayoutEdit.cshtml\";\r\n");
            sb.Append("    List<permModel> perm = null;\r\n");
            sb.Append("}\r\n");
            sb.Append("\r\n");
            sb.Append("<script type=\"text/javascript\">\r\n");
            sb.Append("$(function () {\r\n");
            sb.Append("    $(\"#btnSave\").click(function () {\r\n");
            sb.Append("        if ($(\"form\").valid()) {\r\n");
            sb.Append("            $.ajax({\r\n");
            sb.Append("                url: \"@Url.Action(\"Create\")\",\r\n");
            sb.Append("                type: \"Post\",\r\n");
            sb.Append("                data: $(\"form\").serialize(),\r\n");
            sb.Append("                dataType: \"json\",\r\n");
            sb.Append("                success: function (data) {\r\n");
            sb.Append("                    if (data.type == 1) {\r\n");
            sb.Append("                        window.parent.frameReturnByMes(data.message);\r\n");
            sb.Append("                        window.parent.frameReturnByReload(true);\r\n");
            sb.Append("                        window.parent.frameReturnByClose()\r\n");
            sb.Append("                    }\r\n");
            sb.Append("                    else {\r\n");
            sb.Append("                        window.parent.frameReturnByMes(data.message);\r\n");
            sb.Append("                    }\r\n");
            sb.Append("                }\r\n");
            sb.Append("            });\r\n");
            sb.Append("        }\r\n");
            sb.Append("        return false;\r\n");
            sb.Append("    });\r\n");
            sb.Append("    $(\"#btnReturn\").click(function () {\r\n");
            sb.Append("         window.parent.frameReturnByClose();\r\n");
            sb.Append("    });\r\n");
            sb.Append("});\r\n");
            sb.Append("</script>\r\n");
            sb.Append("<div class=\"mvctool bgb\">\r\n");
            sb.Append("@Html.ToolButton(\"btnSave\", \"fa fa-save\", Resource.Save,ref perm, \"Save\", true)\r\n");
            sb.Append("@Html.ToolButton(\"btnReturn\", \"fa fa-reply\", Resource.Reply,false)\r\n");
            sb.Append("</div>\r\n");
            sb.Append("@using (Html.BeginForm())\r\n");
            sb.Append("{\r\n");
            foreach (CompleteField field in fields)
            {
                if (field.name == "Id")
                {
                    sb.Append("             @Html.HiddenFor(model => model." + field.name + ")\r\n");
                }
                if (field.name == "CreateTime")
                {
                    sb.Append("             <input id=\"CreateTime\" type=\"hidden\" name=\"CreateTime\" value=\"2000-1-1\" />\r\n");
                }
            }
            sb.Append(" <table class=\"formtable\">\r\n");
            sb.Append("    <tbody>\r\n");
            foreach (CompleteField field in fields)
            {
                if (field.name != "Id" && field.name != "CreateTime")
                {
                    if (field.xType == "104" || field.xType == "bool")
                    {
                        sb.Append("        <tr>\r\n");
                        sb.Append("            <th>\r\n");
                        sb.Append("                @Html.LabelFor(model => model." + field .name+ ")：\r\n");
                        sb.Append("            </th>\r\n");
                        sb.Append("            <td>\r\n");
                        sb.Append("                  @Html.SwitchButtonByEdit(\"" + field.name + "\", true)\r\n");
                        sb.Append("            </td>\r\n");
                        sb.Append("            <td>@Html.ValidationMessageFor(model => model." + field.name + ")</td>\r\n");
                        sb.Append("        </tr>\r\n");

                    }
                    else if (field.name.ToLower().Contains("img") || field.name.ToLower().Contains("photo"))
                    {
                        sb.Append("        <tr>\r\n");
                        sb.Append("            <th>\r\n");
                        sb.Append("                @Html.LabelFor(model => model." + field.name + ")：\r\n");
                        sb.Append("            </th>\r\n");
                        sb.Append("            <td>\r\n");
                        sb.Append("             @Html.HiddenFor(model => model."+field.name+")\r\n");
                        sb.Append("             <img class=\"expic\" src=\"/Content/Images/NotPic.jpg\" /><br />\r\n");
                        sb.Append("             <a href=\"javascript:$('#FileUpload').trigger('click');\" class=\"files\">@Resource.Browse</a>\r\n");
                        sb.Append("             <input type=\"file\" class=\"displaynone\" id=\"FileUpload\" name=\"FileUpload\" onchange=\"Upload('SingleFile', '" + field.name+"', 'FileUpload','1','1');\" />\r\n");
                        sb.Append("             <span class=\"uploading\">@Resource.Uploading</span>\r\n");
                        sb.Append("            </td>\r\n");
                        sb.Append("            <td>@Html.ValidationMessageFor(model => model."+field.name+")</td>\r\n");
                        sb.Append("        </tr>\r\n");
                    }
                    else
                    {
                        sb.Append("        <tr>\r\n");
                        sb.Append("            <th>\r\n");
                        sb.Append("                @Html.LabelFor(model => model." + field.name + ")：\r\n");
                        sb.Append("            </th>\r\n");
                        sb.Append("            <td>\r\n");
                        sb.Append("                @Html.EditorFor(model => model." + field.name + ")\r\n");
                        sb.Append("            </td>\r\n");
                        sb.Append("            <td>@Html.ValidationMessageFor(model => model." + field.name + ")</td>\r\n");
                        sb.Append("        </tr>\r\n");
                    }
                }
            }
            sb.Append("    </tbody>\r\n");
            sb.Append("</table>\r\n");
            sb.Append("}\r\n");
            return sb.ToString();
        }
        #endregion
        #region Edit
        public string GetEdit(string tableName)
        {
            string leftStr = GetLeftStr(tableName);
            List<CompleteField> fields = SqlHelper.GetColumnCompleteField(conn, tableName);

            StringBuilder sb = new StringBuilder();
            sb.Append("@model " + txt_prefix.Text + ".Models." + (leftStr == "Sys" ? "" : (leftStr + ".")) + "" + tableName + "Model\r\n");
            sb.Append("@using " + txt_prefix.Text + ".Web.Core;\r\n");
            sb.Append("@using " + txt_prefix.Text + ".Common;\r\n");
            sb.Append("@using " + txt_prefix.Text + ".Models." + leftStr.Replace(".","") + ";\r\n");
            sb.Append("@using " + txt_prefix.Text + ".Web;\r\n");
            sb.Append("@using " + txt_prefix.Text + ".Locale;\r\n");
            sb.Append("@using " + txt_prefix.Text + ".Models.Sys;\r\n");
            sb.Append("@{\r\n");
            sb.Append("    ViewBag.Title = \"修改\";\r\n");
            sb.Append("    Layout = \"~/Views/Shared/_Index_LayoutEdit.cshtml\";\r\n");
            sb.Append("    List<permModel> perm = null;\r\n");
            sb.Append("}\r\n");
            sb.Append("\r\n");
            sb.Append("<script type=\"text/javascript\">\r\n");
            sb.Append("$(function () {\r\n");
            sb.Append("    $(\"#btnSave\").click(function () {\r\n");
            sb.Append("        if ($(\"form\").valid()) {\r\n");
            sb.Append("            $.ajax({\r\n");
            sb.Append("                url: \"@Url.Action(\"Edit\")\",\r\n");
            sb.Append("                type: \"Post\",\r\n");
            sb.Append("                data: $(\"form\").serialize(),\r\n");
            sb.Append("                dataType: \"json\",\r\n");
            sb.Append("                success: function (data) {\r\n");
            sb.Append("                    if (data.type == 1) {\r\n");
            sb.Append("                        window.parent.frameReturnByMes(data.message);\r\n");
            sb.Append("                        window.parent.frameReturnByReload(true);\r\n");
            sb.Append("                        window.parent.frameReturnByClose()\r\n");
            sb.Append("                    }\r\n");
            sb.Append("                    else {\r\n");
            sb.Append("                        window.parent.frameReturnByMes(data.message);\r\n");
            sb.Append("                    }\r\n");
            sb.Append("                }\r\n");
            sb.Append("            });\r\n");
            sb.Append("        }\r\n");
            sb.Append("        return false;\r\n");
            sb.Append("    });\r\n");
            sb.Append("    $(\"#btnReturn\").click(function () {\r\n");
            sb.Append("         window.parent.frameReturnByClose();\r\n");
            sb.Append("    });\r\n");
            sb.Append("});\r\n");
            sb.Append("</script>\r\n");
            sb.Append("<div class=\"mvctool bgb\">\r\n");
            sb.Append("@Html.ToolButton(\"btnSave\", \"fa fa-save\", Resource.Save,ref perm, \"Save\", true)\r\n");
            sb.Append("@Html.ToolButton(\"btnReturn\", \"fa fa-reply\", Resource.Reply,false)\r\n");
            sb.Append("</div>\r\n");
            sb.Append("@using (Html.BeginForm())\r\n");
            sb.Append("{\r\n");
            foreach (CompleteField field in fields)
            {
                if (field.name == "Id" || field.name == "CreateTime")
                {
                    sb.Append("             @Html.HiddenFor(model => model." + field.name + ")\r\n");
                }
            }
            sb.Append(" <table class=\"formtable\">\r\n");
            sb.Append("    <tbody>\r\n");
            foreach (CompleteField field in fields)
            {
                if (field.name != "Id")
                {
                    if (field.xType == "104" || field.xType == "bool")
                    {
                        sb.Append("        <tr>\r\n");
                        sb.Append("            <th>\r\n");
                        sb.Append("                @Html.LabelFor(model => model." + field.name + ")：\r\n");
                        sb.Append("            </th>\r\n");
                        sb.Append("            <td >\r\n");
                        sb.Append("               @Html.SwitchButtonByEdit(\"" + field.name + "\", Model." + field.name + ")\r\n");
                        sb.Append("            </td>\r\n");
                        sb.Append("            <td>@Html.ValidationMessageFor(model => model." + field.name + ")</td>\r\n");
                        sb.Append("        </tr>\r\n");

                    }
                    else if (field.name.ToLower().Contains("img") || field.name.ToLower().Contains("photo"))
                    {
                        sb.Append("        <tr>\r\n");
                        sb.Append("            <th>\r\n");
                        sb.Append("                @Html.LabelFor(model => model." + field.name + ")：\r\n");
                        sb.Append("            </th>\r\n");
                        sb.Append("            <td>\r\n");
                        sb.Append("             @Html.HiddenFor(model => model." + field.name + ")\r\n");
                        sb.Append("             <img class=\"expic\" src=\"@((Model." + field.name + "==null||Model." + field.name + "==\"\")?\"/Content/Images/NotPic.jpg\":Model." + field.name + ")\" /><br />\r\n");
                        sb.Append("             <a href=\"javascript:$('#FileUpload').trigger('click');\" class=\"files\">@Resource.Browse</a>\r\n");
                        sb.Append("             <input type=\"file\" class=\"displaynone\" id=\"FileUpload\" name=\"FileUpload\" onchange=\"Upload('SingleFile', '" + field.name + "', 'FileUpload','1','1');\" />\r\n");
                        sb.Append("             <span class=\"uploading\">@Resource.Uploading</span>\r\n");
                        sb.Append("            </td>\r\n");
                        sb.Append("            <td>@Html.ValidationMessageFor(model => model." + field.name + ")</td>\r\n");
                        sb.Append("        </tr>\r\n");
                    }
                    else
                    {
                        sb.Append("        <tr>\r\n");
                        sb.Append("            <th>\r\n");
                        sb.Append("                @Html.LabelFor(model => model." + field.name + ")：\r\n");
                        sb.Append("            </th>\r\n");
                        sb.Append("            <td >\r\n");
                        sb.Append("                @Html.EditorFor(model => model." + field.name + ")\r\n");
                        sb.Append("            </td>\r\n");
                        sb.Append("            <td>@Html.ValidationMessageFor(model => model." + field.name + ")</td>\r\n");
                        sb.Append("        </tr>\r\n");
                    }
                }
            }
            sb.Append("    </tbody>\r\n");
            sb.Append("</table>\r\n");
            sb.Append("}\r\n");
            return sb.ToString();
        }
        #endregion

        #region Common
        public string GetCommon(string tableName)
        {
            string leftStr = GetLeftStr(tableName);
            List<CompleteField> fields = SqlHelper.GetColumnCompleteField(conn, tableName);

            StringBuilder sb = new StringBuilder();
            sb.Append("//获取要修改变得内容并赋予表单\r\n");
            foreach (CompleteField field in fields)
            {
                sb.Append("$(\"#" + field.name + "\").val(data." + field.name + ");\r\n");
            }

            for (int i = 1; i < 471; i++)
            {
                sb.Append("<li class=\"pic_"+i+"\"></li>");
            }

                return sb.ToString();
        }
        #endregion
        //获取前缀
        public string GetLeftStr(string tableName)
        {
            //生成代码
            string nameSpace = "";
            if (lb_Tables.Text.IndexOf("_") > 0)
            {
                nameSpace = tableName.Substring(0, tableName.IndexOf("_"));
            }
            else
            {
                nameSpace = "Sys";
            }
            return nameSpace;
        }
        //textbox全选
        private void anyTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == '\x1')
            {
                ((TextBox)sender).SelectAll();
                e.Handled = true;
            }
        }

        //处理注解
        public string SetValid(CompleteField field)//0不能为空
        {
            string validStr = "";
            string xtype = SqlHelper.GetType(field.xType);
            if (xtype == "string")
            {
                //不能为空
                if (field.isNullAble == "0" && field.name.ToLower() != "id")//一般ID为主键
                {
                    validStr = validStr + "        [NotNullExpression]\r\n";
                }
                validStr = validStr + "        [MaxWordsExpression(" + field.length + ")]\r\n";
            }
            else if (xtype == "int" && field.isNullAble == "0")//冲突处理，不能自定义注解
            {
                validStr = validStr + "        [Required(ErrorMessage= \"{0}只能填写数字\")]\r\n";
            }
            else if (field.xType == "int")
            {
                validStr = validStr + "        [IsNumberExpression]\r\n";
            }
            return validStr;
        }

        private void btn_Generate_Click(object sender, EventArgs e)
        {
            string sPath = "C:\\MyCodeHelper";
            if (!Directory.Exists(sPath))
            {
                Directory.CreateDirectory(sPath);
            }

        }

        private void btn_EditSQLCon_Click(object sender, EventArgs e)
        {
            string strXmlPath = "Config.xml";
            XmlHelper.SetXmlFileValue(strXmlPath, "DataBase", txt_SQL.Text);
            MessageBox.Show("修改成功，重新载入");
            init();
        }

        private void txt_ModelName_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                OutputCode();
            }
            catch { }
        }
    }
}
