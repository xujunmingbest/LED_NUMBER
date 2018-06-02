namespace UserLedTest
{
    partial class LedCtrl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer component = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        /// 
        /*
        protected override void Dispose(bool disposing)
        {
            if (disposing && (component != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        */
        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // LedCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Black;
            this.ForeColor = System.Drawing.Color.Lime;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "LedCtrl";
            this.Size = new System.Drawing.Size(35, 52);
            this.SizeChanged += new System.EventHandler(this.LedCtrl_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.LedCtrl_Paint);
            this.Resize += new System.EventHandler(this.LedCtrl_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
