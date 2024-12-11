namespace CSDNLaTexWordAddIn
{
    partial class CSDNRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public CSDNRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tab1 = this.Factory.CreateRibbonTab();
            this.GroupCSDN = this.Factory.CreateRibbonGroup();
            this.buttonCopy = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.GroupCSDN.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.GroupCSDN);
            this.tab1.Label = "TabAddIns";
            this.tab1.Name = "tab1";
            // 
            // GroupCSDN
            // 
            this.GroupCSDN.Items.Add(this.buttonCopy);
            this.GroupCSDN.Label = "CSDN";
            this.GroupCSDN.Name = "GroupCSDN";
            // 
            // buttonCopy
            // 
            this.buttonCopy.Label = "以CSDN公式格式复制";
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ButtonCopy_Click);
            // 
            // CSDNRibbon
            // 
            this.Name = "CSDNRibbon";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.CSDNRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.GroupCSDN.ResumeLayout(false);
            this.GroupCSDN.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup GroupCSDN;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonCopy;
    }

    partial class ThisRibbonCollection
    {
        internal CSDNRibbon CSDNRibbon
        {
            get { return this.GetRibbon<CSDNRibbon>(); }
        }
    }
}
