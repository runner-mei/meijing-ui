﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using meijing.ui.module;
using System.Threading;

namespace meijing.ui
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            GetSystemIcon.InitMainTreeImage();
            GetSystemIcon.InitTabViewImage();

            if (!SystemManager.DEBUG_MODE)
            {
                //非Debug模式的时候,UT菜单不可使用
                toolStripMenuItem12.Visible = false;
            }
            this.Text += "  " + SystemManager.Version;
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            if (SystemManager.MONO_MODE)
            {
                this.Text += " MONO";
            }
            //长时间操作时候，实时提示进度在状态栏中
            SystemManager.ActionDone += new EventHandler<ActionDoneEventArgs>(
                (x, y) =>
                {
                    //1.lblAction 没有InvokeRequired
                    //2.DoEvents必须
                    lblAction.Text = y.Message;
                    Application.DoEvents();
                }
            );
        }

        /// <summary>
        /// 多文档视图管理
        /// </summary>
        Dictionary<String, TabPage> ViewTabList = new Dictionary<String, TabPage>();
        int trigger_selected = 11;
        int trigger_unselected = 10;
        int trigger_root_selected = 9;
        int trigger_root_unselected = 8;
        int if_unselected = 7;
        int if_selected = 6;
        int if_root_selected = 5;
        int if_root_unselected = 4;
        int device_unselected = 3;
        int device_selected = 2;

        int link_selected = 17;
        int link_unselected = 18;
     
        /// <summary>
        /// Load Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Load(object sender, EventArgs e)
        {
            loadDevices();
            loadLinks();
        }


        #region"数据库连接"

        private void loadLinks()
        {
            this.trvsrvlst.BeginUpdate();
            try
            {
                var links = Link.FindBy<Link>(null);
                var linksNode = this.trvsrvlst.Nodes.Find("links", false).First();
                linksNode.Nodes.Clear();
                foreach (var drv in links)
                {
                    var node = linksNode.Nodes.Add("link-" + drv.Id, drv.Name,
                        link_unselected, link_selected);
                    node.Tag = drv;
                    loadRules(node, drv);
                }
            }
            catch (Exception e)
            {
                MyMessageBox.ShowMessage("哦，出错了！", "载入线路列表出错了!", e.ToString());
            }
            finally
            {
                this.trvsrvlst.EndUpdate();
            }
        }

        private void loadRules(TreeNode node, Link link)
        {
            node = node.Nodes.Add("trigger-" + link.Id, "任务", trigger_root_unselected,
                trigger_root_selected);
            node.Name = "triggers";
            var triggers = link.Children<Trigger>(null);
            foreach (var trigger in triggers)
            {
                var n = node.Nodes.Add("trigger-" + trigger.Id, trigger.Name,
                    trigger_unselected, trigger_selected);
                n.Tag = trigger;
            }
        }


        private void loadDevices() {
            this.trvsrvlst.BeginUpdate();
            try
            {
                var devices = Device.FindBy<Device>(null);
                var devicesNode = this.trvsrvlst.Nodes.Find("devices", false).First();
                devicesNode.Nodes.Clear();
                foreach (var drv in devices)
                {
                    var node = devicesNode.Nodes.Add("device-" + drv.Id, drv.Address, 
                        device_unselected, device_selected);
                    node.Tag = drv;
                    loadInterfaces(node, drv);
                    loadRules(node, drv);
                }
            }
            catch (Exception e)
            {
                MyMessageBox.ShowMessage("哦，出错了！", "载入设备列表出错了!", e.ToString());
            }
            finally
            {
                this.trvsrvlst.EndUpdate();
            }
        }

        private void loadInterfaces(TreeNode node, Device drv)
        {
            node = node.Nodes.Add("if-" + drv.Id, "端口", if_root_unselected, if_root_selected);
            node.Name = "interfaces";
            var interfaces = drv.Children<Interface>(null);
            foreach (var ifc in interfaces)
            {
                var n = node.Nodes.Add("if-" + ifc.Id, ifc.IfDescr, if_unselected, if_selected);
                n.Tag = ifc;
            }
        }

        private void loadRules(TreeNode node, Device drv)
        {
            node = node.Nodes.Add("trigger-" + drv.Id, "任务", trigger_root_unselected, 
                trigger_root_selected);
            node.Name = "triggers";
            var triggers = drv.Children<Trigger>(null);
            foreach (var trigger in triggers)
            {
                var n = node.Nodes.Add("trigger-" + trigger.Id, trigger.Name,
                    trigger_unselected, trigger_selected);
                n.Tag = trigger;
            }
        }


        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadDevices();
        }

        /// <summary>
        /// Expand All
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpandAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.trvsrvlst.BeginUpdate();
            this.trvsrvlst.CollapseAll();
            this.trvsrvlst.EndUpdate();
        }
        /// <summary>
        /// Collapse All
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollapseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.trvsrvlst.BeginUpdate();
            this.trvsrvlst.ExpandAll();
            this.trvsrvlst.EndUpdate();
        }
        /// <summary>
        /// Exit Application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion



        #region "Help"
        /// <summary>
        /// About
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemManager.OpenForm(new frmAbout(), true, true);
        }
        /// <summary>
        /// Thanks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThanksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyMessageBox.ShowMessage("Thanks", "DreamOMS",
                                     meijing.ui.module.GetResource.GetImage(meijing.ui.module.ImageType.Smile),
                                     new System.IO.StreamReader("ReleaseNote.txt").ReadToEnd());
        }

        #endregion


        #region "Tree"
        private IDictionary<Device, IList<Interface>> GetAllDevicesAndPorts() 
        {
            var node = this.trvsrvlst.Nodes.Find("devices", false).First();
            IDictionary<Device, IList<Interface>> devices = new Dictionary<Device, IList<Interface>>();
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                var drv = node.Nodes[i].Tag as Device;
                var linkNode = node.Nodes[i].Nodes.Find("interfaces", false).First();
                devices[drv] = GetLinks(linkNode);
            }
            return devices;
        }
        private IList<Interface> GetLinks(TreeNode node)
        {
            List<Interface> interfaces = new List<Interface>();
            if (null == node)
            {
                return interfaces;
            }

            for (int i = 0; i < node.Nodes.Count; i++)
            {
                interfaces.Add(node.Nodes[i].Tag as Interface);
            }
            return interfaces;
        }
        private IList<Device> GetAllDevices()
        {
            return GetAllDevices(this.trvsrvlst.Nodes.Find("devices", false).First());
        }

        private IList<Device> GetAllDevices(TreeNode node) {

            List<Device> devices = new List<Device>();
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                devices.Add(node.Nodes[i].Tag as Device);
            }
            return devices;
        }

        private void trvsrvlst_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (null == e.Node.Tag) {
                if ("devices" == e.Node.Name) {
                    List<Device> devices = new List<Device>();
                    for(int i =0; i < e.Node.Nodes.Count; i ++) {
                        devices.Add(e.Node.Nodes[i].Tag as Device);
                    }
                    showDevices(GetAllDevices(e.Node));
                }
                else if ("interfaces" == e.Node.Name)
                {
                    List<Interface> interfaces = new List<Interface>();
                    for (int i = 0; i < e.Node.Nodes.Count; i++)
                    {
                        interfaces.Add(e.Node.Nodes[i].Tag as Interface);
                    }
                    showInterfaces(e.Node.Parent.Tag as Device, interfaces);
                }
                else if ("triggers" == e.Node.Name)
                {
                    List<Trigger> triggers = new List<Trigger>();
                    for (int i = 0; i < e.Node.Nodes.Count; i++)
                    {
                        triggers.Add(e.Node.Nodes[i].Tag as Trigger);
                    }
                    showTriggers(e.Node.Parent.Tag as Device, triggers);
                }
                return;
            }
            var drv = e.Node.Tag as Device;
            if (null != drv)
            {
                return;
            }
            var ifc = e.Node.Tag as Interface;
            if (null != ifc)
            {
                return;
            }
            var rule = e.Node.Tag as Trigger;
            if (null != rule)
            {
                return;
            }
        }

        public void showDevices(IList<Device> devices)
        {
            listView.BeginUpdate();
            try
            {
                listView.Columns.Clear();
                listView.Items.Clear();
                listView.Columns.Add("Name", "名称", 200);
                listView.Columns.Add("Address", "管理地址", 100);
                listView.Columns.Add("Description", "描述", 400);
                listView.Columns.Add("CreatedAt", "创建时间", 150);
                listView.Columns.Add("UpdatedAt", "最后修改时间", 150);

                foreach (var drv in devices) { 
                    var item = listView.Items.Add(drv.Name);
                    item.SubItems.Add(drv.Address);
                    item.SubItems.Add(drv.Description);
                    item.SubItems.Add(drv.CreatedAt.ToString());
                    item.SubItems.Add(drv.UpdatedAt.ToString());
                }
            }
            finally {
                listView.EndUpdate();
            }
        }

        public void showInterfaces(Device drv, List<Interface> interfaces)
        {
            listView.BeginUpdate();
            try
            {
                listView.Columns.Clear();
                listView.Items.Clear();
                listView.Columns.Add("IfIndex", "索引", 200);
                listView.Columns.Add("IfPhysAddress", "地址", 100);
                listView.Columns.Add("IfDescr", "描述", 400);
                listView.Columns.Add("IfType", "类型", 150);

                foreach (var ifc in interfaces)
                {
                    var item = listView.Items.Add(ifc.IfIndex.ToString());
                    item.SubItems.Add(ifc.IfPhysAddress);
                    item.SubItems.Add(ifc.IfDescr);
                    item.SubItems.Add(SystemManager.GetPortType(ifc.IfType));
                }
            }
            finally
            {
                listView.EndUpdate();
            }
        }

        public void showTriggers(Device drv, List<Trigger> triggers )
        {
            listView.BeginUpdate();
            try
            {
                listView.Columns.Clear();
                listView.Items.Clear();
                listView.Columns.Add("Name", "名称", 200);
                listView.Columns.Add("Expression", "调度间隔", 150);
                listView.Columns.Add("Type", "类型", 150);
                listView.Columns.Add("Description", "描述", 400);
                

                foreach (var ifc in triggers)
                {
                    var item = listView.Items.Add(ifc.Name);
                    item.SubItems.Add(SystemManager.GetExpressionDescription(ifc.Expression));
                    if ("metric_rule" == ifc.Type)
                    {
                        item.SubItems.Add("定时采集指标 " + ifc.GetString("metric"));
                    }
                    else
                    {
                        item.SubItems.Add(ifc.Type);
                    }
                    item.SubItems.Add(ifc.Description);

                }
            }
            finally
            {
                listView.EndUpdate();
            }
        }

        public void showDevice(Device drv)
        {

        }

        public void showInterface(Interface drv)
        {

        }

        public void showTrigger(Trigger drv)
        {

        }
        #endregion

        private void addDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemManager.OpenForm(new frmAddDrv(), true, false);
            loadDevices();
        }

        private void deleteDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemManager.OpenForm(new frmDeleteDrv(GetAllDevices()), true, false);
            loadDevices();
        }

        private void addLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemManager.OpenForm(new frmLink(GetAllDevicesAndPorts()), true, false);
            loadLinks();
        }

        private void deleteLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemManager.OpenForm(new frmDeleteLink(), true, false);
            loadLinks();
        }

        private void addTriggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = this.trvsrvlst.SelectedNode;
            if (null == node.Tag)
            {
                if ("links" == node.Name)
                {
                    frmTrigger.ShowAdd(GetLinks(node), "设备:", Metric.LINKS);
                }
                else if ("devices" == node.Name)
                {
                    frmTrigger.ShowAdd(GetAllDevices(node), "设备:", Metric.DEVICES);
                }
                else if ("interfaces" == node.Name)
                {
                    frmTrigger.ShowAdd(GetAllDevicesAndPorts(), "设备:", "端口号:", Metric.INTERFACES);
                }
                else if ("triggers" == node.Name)
                {
                    List<Trigger> triggers = new List<Trigger>();
                    for (int i = 0; i < node.Nodes.Count; i++)
                    {
                        triggers.Add(node.Nodes[i].Tag as Trigger);
                    }
                    showTriggers(node.Parent.Tag as Device, triggers);
                }
                return;
            }
            var drv = node.Tag as Device;
            if (null != drv)
            {
                frmTrigger.ShowAdd(GetAllDevices(node), "设备:", Metric.DEVICES);
                return;
            }
            var ifc = node.Tag as Interface;
            if (null != ifc)
            {
                frmTrigger.ShowAdd(GetAllDevicesAndPorts(), "设备:", "端口号:", Metric.INTERFACES);
                return;
            }
            var rule = node.Tag as Trigger;
            if (null != rule)
            {
                //frmTrigger.ShowEdit(GetAllDevices(node), "设备:");
                return;
            }
        }

        private void deleteTriggerToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


    }
}