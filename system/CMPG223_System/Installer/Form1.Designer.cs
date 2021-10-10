
namespace Installer
{
    partial class frmInstaller
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInstaller));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnInstall = new System.Windows.Forms.Button();
            this.pbxInstallLogo = new System.Windows.Forms.PictureBox();
            this.lblInstallTitle = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxInstallLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.Controls.Add(this.btnInstall, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.pbxInstallLogo, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblInstallTitle, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(411, 433);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnInstall
            // 
            this.btnInstall.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnInstall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInstall.Location = new System.Drawing.Point(139, 343);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(131, 35);
            this.btnInstall.TabIndex = 1;
            this.btnInstall.Text = "Install";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click_1);
            // 
            // pbxInstallLogo
            // 
            this.pbxInstallLogo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbxInstallLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbxInstallLogo.Image = global::Installer.Properties.Resources.org_logo;
            this.pbxInstallLogo.Location = new System.Drawing.Point(154, 22);
            this.pbxInstallLogo.Name = "pbxInstallLogo";
            this.pbxInstallLogo.Size = new System.Drawing.Size(100, 100);
            this.pbxInstallLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxInstallLogo.TabIndex = 2;
            this.pbxInstallLogo.TabStop = false;
            // 
            // lblInstallTitle
            // 
            this.lblInstallTitle.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblInstallTitle.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblInstallTitle, 3);
            this.lblInstallTitle.Font = new System.Drawing.Font("Century Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstallTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(26)))), ((int)(((byte)(74)))));
            this.lblInstallTitle.Location = new System.Drawing.Point(45, 144);
            this.lblInstallTitle.Name = "lblInstallTitle";
            this.lblInstallTitle.Size = new System.Drawing.Size(320, 24);
            this.lblInstallTitle.TabIndex = 3;
            this.lblInstallTitle.Text = "New Hope Adoption Manager";
            // 
            // frmInstaller
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(411, 433);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmInstaller";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Hope Adoption Manager";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxInstallLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.PictureBox pbxInstallLogo;
        private System.Windows.Forms.Label lblInstallTitle;
    }
}

