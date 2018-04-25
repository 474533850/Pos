using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using POS.Common;
using POS.Model;

namespace POS.Control
{
    public partial class Control : UserControl
    {
        public Control()
        {
            InitializeComponent();
            lueLocalTable.Properties.DataSource = AppConst.dicDB;
        }

        private string localTable;
        /// <summary>
        /// 本地表
        /// </summary>
        public string LocalTable
        {
            get
            {
                return localTable;
            }

            set
            {
                localTable = value;
                lueLocalTable.EditValue = value;
            }
        }

        private string serverTable;
        /// <summary>
        /// 服务器数据库表
        /// </summary>
        public string ServerTable
        {
            get
            {
                return serverTable;
            }

            set
            {
                serverTable = value;
                txtServiceTable.Text = value;
            }
        }

        private List<DBRelation> data;
        /// <summary>
        /// 数据源
        /// </summary>
        public List<DBRelation> Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
                gdDBMap.DataSource = value;
            }
        }
    }
}
