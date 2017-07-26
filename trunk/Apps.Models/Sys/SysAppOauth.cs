using System;

namespace Apps.Model.Sys
{
    /// <summary>
    /// ����ƽ̨:ʵ����
    /// </summary>
    [Serializable]
    public partial class SysAppOauth
    {
        public SysAppOauth()
        { }
        #region Model
        private int _id;
        private string _title = "";
        private string _img_url = "";
        private string _app_id;
        private string _app_key;
        private string _remark = "";
        private int _sort_id = 99;
        private int _is_lock = 0;
        private string _api_path = "";
        /// <summary>
        /// ����ID
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public string title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// ��ʾͼƬ
        /// </summary>
        public string img_url
        {
            set { _img_url = value; }
            get { return _img_url; }
        }
        /// <summary>
        /// AppId
        /// </summary>
        public string app_id
        {
            set { _app_id = value; }
            get { return _app_id; }
        }
        /// <summary>
        /// AppKey
        /// </summary>
        public string app_key
        {
            set { _app_key = value; }
            get { return _app_key; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public string remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public int sort_id
        {
            set { _sort_id = value; }
            get { return _sort_id; }
        }
        /// <summary>
        /// �Ƿ�����
        /// </summary>
        public int is_lock
        {
            set { _is_lock = value; }
            get { return _is_lock; }
        }
        /// <summary>
        /// �ӿ�Ŀ¼
        /// </summary>
        public string api_path
        {
            set { _api_path = value; }
            get { return _api_path; }
        }
        #endregion Model

    }
}