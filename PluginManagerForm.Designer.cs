namespace MDIPaint
{
    partial class PluginManagerForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAuthor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDownload = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colAuthor,
            this.colVersion,
            this.colDownload});
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(776, 353);
            this.dataGridView1.TabIndex = 0;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(200, 386);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(143, 52);
            this.buttonSave.TabIndex = 1;
            this.buttonSave.Text = "Сохранить";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(409, 386);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(143, 52);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Отменить";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // colName
            // 
            this.colName.HeaderText = "Название";
            this.colName.MinimumWidth = 6;
            this.colName.Name = "colName";
            this.colName.Width = 125;
            // 
            // colAuthor
            // 
            this.colAuthor.HeaderText = "Автор";
            this.colAuthor.MinimumWidth = 6;
            this.colAuthor.Name = "colAuthor";
            this.colAuthor.Width = 125;
            // 
            // colVersion
            // 
            this.colVersion.HeaderText = "Версия";
            this.colVersion.MinimumWidth = 6;
            this.colVersion.Name = "colVersion";
            this.colVersion.Width = 125;
            // 
            // colDownload
            // 
            this.colDownload.HeaderText = "Загружать";
            this.colDownload.MinimumWidth = 6;
            this.colDownload.Name = "colDownload";
            this.colDownload.Width = 125;
            // 
            // PluginManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.dataGridView1);
            this.Name = "PluginManagerForm";
            this.Text = "PluginManagerForm";
            this.Load += new System.EventHandler(this.PluginManagerForm_Load);
            this.Click += new System.EventHandler(this.PluginManagerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAuthor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVersion;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colDownload;
    }
}