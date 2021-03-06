﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace meijing.ui
{
    using meijing.ui.module;

    public partial class frmDeleteDrv : Form
    {
        public frmDeleteDrv(IList<Device> devices)
        {
            InitializeComponent();
            this.listView.SetDevices(devices);
        }

        private void cancel_buttom_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            try
            {
                foreach(var drv in this.listView.SelectedDevices()) 
                {
                    drv.DeleteIt();
                }
                this.DialogResult = DialogResult.OK;
            }
            catch(Exception ex)
            {
                MyMessageBox.ShowMessage("错误", "删除设备失败!", ex.ToString());
            }
        }
    }
}
