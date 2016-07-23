using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Drawing;

namespace ImportTool
{
    class Record
    {
        #region 属性

        public Image StateImage { get; private set; }

        public string 姓 { get; set; }
        public string 名 { get; set; }
        public string 主手机号码 { get; set; }
        public string 电子邮箱 { get; set; }
        public string 证件类型 { get; set; }

        public string Message { get; set; }
        #endregion

        private State st;
        public State SyncState
        {
            get
            {
                return st;
            }
            set
            {
                st = value;
                switch (st)
                {
                    case State.Wait:
                        StateImage = Properties.Resources.Wait;
                        break;
                    case State.Sync:
                        StateImage = Properties.Resources.Progress;
                        break;
                    case State.Success:
                        StateImage = Properties.Resources.Sucessful;
                        break;
                    case State.Fail:
                        StateImage = Properties.Resources.Error;
                        break;
                }
            }
        }

        public Record()
        {
            SyncState = State.Wait;
            Message = string.Empty;
        }

    }
    enum State { Wait = 0, Sync, Success, Fail };


}