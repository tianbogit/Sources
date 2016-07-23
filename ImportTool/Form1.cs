using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImportTool
{
    public partial class Form1 : Form
    {
        private List<Record> data;
        private BindingList<Record> successData;
        private BindingList<Record> failData;
        private BindingList<Record> hasData;

        delegate void RefreshViewHandler();

        public Form1()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = true;
            data = new List<Record>();
            successData = new BindingList<Record>();
            failData = new BindingList<Record>();
            hasData = new BindingList<Record>();
        }

        private void initWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            statusLoad.Visible = true;

            Record record = new Record();
            for (int i = 0; i < 50; i++)
            {
                record = new Record()
                {
                    姓 = "田" + i,
                    名 = "tian" + i,
                    主手机号码 = "66666666",
                    电子邮箱 = "898s9f8s9f",
                    证件类型 = "sfsfsfsfsf"
                };
                data.Add(record);
            }
            System.Threading.Thread.Sleep(1000);
            e.Result = data;
        }

        private void initWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dataGridView1.DataSource = e.Result as List<Record>;
            statusLoad.Visible = false;

            this.lbCurrentNum.Text = "0";
            this.lbCurrentNum.Visible = true;
            this.lbTotal.Text = "/" + data.Count;
            this.lbTotal.Visible = true;

        }

        private void initWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        /// <summary>
        /// 异步处理操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void syncWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < data.Count; i++)
            {
                Record current = data[i];
                current.SyncState = State.Sync;

                System.Threading.Thread.Sleep(500);


                if (dataGridView1.InvokeRequired)
                {
                    RefreshViewHandler rfHandler = dataGridView1.Refresh;
                    dataGridView1.Invoke(rfHandler);
                }
                else
                {
                    dataGridView1.Refresh();
                }

                if (dataGridView2.InvokeRequired)
                {
                    RefreshViewHandler rfHandler = dataGridView2.Refresh;
                    dataGridView2.Invoke(rfHandler);
                }
                else
                {
                    dataGridView2.Refresh();
                }
                if (dataGridView3.InvokeRequired)
                {
                    RefreshViewHandler rfHandler = dataGridView3.Refresh;
                    dataGridView3.Invoke(rfHandler);
                }
                else
                {
                    dataGridView3.Refresh();
                }

                ProcessTask(current);

                syncWorker.ReportProgress(i + 1);


            }
        }

        void ProcessTask(Record record)
        {
            if (int.Parse(record.姓.Replace("田", "")) % 5 == 0)
            {
                record.SyncState = State.Fail;
                failData.Add(record);
            }

            else if (int.Parse(record.姓.Replace("田", "")) % 7 == 0)
            {
                record.SyncState = State.Fail;
                hasData.Add(record);
            }
            else
            {
                record.SyncState = State.Success;
                successData.Add(record);
            }
        }

        private void syncWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;

            this.lbCurrentNum.Text = e.ProgressPercentage.ToString();

        }

        /// <summary>
        /// 处理完成执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void syncWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tab.TabPages[0].Text = "处理成功记录";
            dataGridView1.DataSource = successData;
            dataGridView2.DataSource = hasData;
            dataGridView3.DataSource = failData;
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            this.progressBar1.Maximum = data.Count;
            syncWorker.RunWorkerAsync();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            initWorker.RunWorkerAsync();
        }
    }



}
