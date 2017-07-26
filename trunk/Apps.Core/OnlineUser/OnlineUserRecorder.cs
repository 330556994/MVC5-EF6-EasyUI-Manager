
using System;
using System.Collections.Generic;
using System.Threading;

namespace Apps.Core.OnlineStat
{
	/// <summary>
	/// �����û���¼��
	/// </summary>
    public class OnlineUserRecorder
    {
        // �����û����ݿ�
        private OnlineUserDB m_db = null;
        // ������� A, ���ڽ�������
        private Queue<OnlineUserCmdBase> m_cmdQueueA = null;
        // ������� X, ����ִ������
        private Queue<OnlineUserCmdBase> m_cmdQueueX = null;
        // ��æ��־
        private bool m_isBusy = false;
        // �ϴ�ͳ��ʱ��
        private DateTime m_lastStatisticTime = new DateTime(0);
        // �û���ʱ������
        private int m_userTimeOutMinute = 20;
        // ͳ��ʱ����
        private int m_statisticEventInterval = 60;

        #region �๹����
        /// <summary>
        /// ��Ĭ�Ϲ�����
        /// </summary>
        internal OnlineUserRecorder()
        {
            this.m_db = new OnlineUserDB();

            // ��ʼ���������
            this.m_cmdQueueA = new Queue<OnlineUserCmdBase>();
            this.m_cmdQueueX = new Queue<OnlineUserCmdBase>();
        }
        #endregion

        /// <summary>
        /// ���û��ȡ�û���ʱ������
        /// </summary>
        internal int UserTimeOutMinute
        {
            set
            {
                this.m_userTimeOutMinute = value;
            }

            get
            {
                return this.m_userTimeOutMinute;
            }
        }

        /// <summary>
        /// ���û��ȡͳ��ʱ����(��λ��)
        /// </summary>
        internal int StatisticEventInterval
        {
            set
            {
                this.m_statisticEventInterval = value;
            }

            get
            {
                return this.m_statisticEventInterval;
            }
        }

        /// <summary>
        /// ���������û���Ϣ
        /// </summary>
        /// <param name="onlineUser"></param>
        public void Persist(OnlineUser onlineUser)
        {
            // ����ɾ������
            OnlineUserDeleteCmd delCmd = new OnlineUserDeleteCmd(this.m_db, onlineUser);
            // ������������
            OnlineUserInsertCmd insCmd = new OnlineUserInsertCmd(this.m_db, onlineUser);

            // ��������ӵ�����
            this.m_cmdQueueA.Enqueue(delCmd);
            this.m_cmdQueueA.Enqueue(insCmd);

            // �����������
            this.BeginProcessCmdQueue();
        }

        /// <summary>
        /// ɾ�������û���Ϣ
        /// </summary>
        /// <param name="onlineUser"></param>
        public void Delete(OnlineUser onlineUser)
        {
            // ����ɾ������
            OnlineUserDeleteCmd delCmd = new OnlineUserDeleteCmd(this.m_db, onlineUser);

            // ��������ӵ�����
            this.m_cmdQueueA.Enqueue(delCmd);

            // �����������
            this.BeginProcessCmdQueue();
        }

        /// <summary>
        /// ��ȡ�����û��б�
        /// </summary>
        /// <returns></returns>
        public IList<OnlineUser> GetUserList()
        {
            return this.m_db.Select();
        }

        /// <summary>
        /// ��ȡ�����û�����
        /// </summary>
        /// <returns></returns>
        public int GetUserCount()
        {
            return this.m_db.Count();
        }

        /// <summary>
        /// �첽��ʽ�����������
        /// </summary>
        private void BeginProcessCmdQueue()
        {
            if (this.m_isBusy)
                return;

            // δ������ͳ�Ƶ�ʱ��
            if (DateTime.Now - this.m_lastStatisticTime < TimeSpan.FromSeconds(this.StatisticEventInterval))
                return;

            Thread t = null;

            t = new Thread(new ThreadStart(this.ProcessCmdQueue));
            t.Start();
        }

        /// <summary>
        /// �����������
        /// </summary>
        private void ProcessCmdQueue()
        {
            lock (this)
            {
                if (this.m_isBusy)
                    return;

                // δ������ͳ�Ƶ�ʱ��
                if (DateTime.Now - this.m_lastStatisticTime < TimeSpan.FromSeconds(this.StatisticEventInterval))
                    return;

                this.m_isBusy = true;

                // ������ʱ����, ���ڽ���
                Queue<OnlineUserCmdBase> tempQ = null;

                // ���������������
                tempQ = this.m_cmdQueueA;
                this.m_cmdQueueA = this.m_cmdQueueX;
                this.m_cmdQueueX = tempQ;
                tempQ = null;

                while (this.m_cmdQueueX.Count > 0)
                {
                    // ��ȡ����
                    OnlineUserCmdBase cmd = this.m_cmdQueueX.Peek();

                    if (cmd == null)
                        break;

                    // ִ������
                    cmd.Execute();

                    // �Ӷ������Ƴ�����
                    this.m_cmdQueueX.Dequeue();
                }

				// �����ʱ�û�
				this.m_db.ClearTimeOut(this.UserTimeOutMinute);
				// ����
				this.m_db.Sort();

                this.m_lastStatisticTime = DateTime.Now;
                this.m_isBusy = false;
            }
        }
    }
}