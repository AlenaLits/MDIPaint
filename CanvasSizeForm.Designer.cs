namespace MDIPaint
{
    partial class CanvasSizeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.labelширина = new System.Windows.Forms.Label();
            this.labelвысота = new System.Windows.Forms.Label();
            this.numericUpDownWidth = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownHeight = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(115, 208);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(133, 38);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(373, 208);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(133, 38);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // labelширина
            // 
            this.labelширина.AutoSize = true;
            this.labelширина.Location = new System.Drawing.Point(45, 78);
            this.labelширина.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelширина.Name = "labelширина";
            this.labelширина.Size = new System.Drawing.Size(61, 16);
            this.labelширина.TabIndex = 4;
            this.labelширина.Text = "Ширина:";
            // 
            // labelвысота
            // 
            this.labelвысота.AutoSize = true;
            this.labelвысота.Location = new System.Drawing.Point(313, 78);
            this.labelвысота.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelвысота.Name = "labelвысота";
            this.labelвысота.Size = new System.Drawing.Size(58, 16);
            this.labelвысота.TabIndex = 5;
            this.labelвысота.Text = "Высота:";
            // 
            // numericUpDownWidth
            // 
            this.numericUpDownWidth.Location = new System.Drawing.Point(128, 78);
            this.numericUpDownWidth.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDownWidth.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownWidth.Name = "numericUpDownWidth";
            this.numericUpDownWidth.Size = new System.Drawing.Size(120, 22);
            this.numericUpDownWidth.TabIndex = 6;
            this.numericUpDownWidth.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // numericUpDownHeight
            // 
            this.numericUpDownHeight.Location = new System.Drawing.Point(386, 76);
            this.numericUpDownHeight.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDownHeight.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownHeight.Name = "numericUpDownHeight";
            this.numericUpDownHeight.Size = new System.Drawing.Size(120, 22);
            this.numericUpDownHeight.TabIndex = 7;
            this.numericUpDownHeight.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // CanvasSizeForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(651, 327);
            this.Controls.Add(this.numericUpDownHeight);
            this.Controls.Add(this.numericUpDownWidth);
            this.Controls.Add(this.labelвысота);
            this.Controls.Add(this.labelширина);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CanvasSizeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Размер холста";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label labelширина;
        private System.Windows.Forms.Label labelвысота;
        private System.Windows.Forms.NumericUpDown numericUpDownWidth;
        private System.Windows.Forms.NumericUpDown numericUpDownHeight;
    }
}