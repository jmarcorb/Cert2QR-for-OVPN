namespace Cert2QR_for_OVPN
{
    partial class frmQRProfilePrinter
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmQRProfilePrinter));
            this.labelSelectedOVPNfile = new System.Windows.Forms.Label();
            this.tbSelectedOVPNfile = new System.Windows.Forms.TextBox();
            this.btnSelectOVPNfile = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.infoLecturaCA = new System.Windows.Forms.Label();
            this.infoLecturaCert = new System.Windows.Forms.Label();
            this.infoLecturaKey = new System.Windows.Forms.Label();
            this.infoLecturaConfig = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSaveServerConfigCert = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnCertKeyUser = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelSelectedOVPNfile
            // 
            this.labelSelectedOVPNfile.AutoSize = true;
            this.labelSelectedOVPNfile.Location = new System.Drawing.Point(12, 33);
            this.labelSelectedOVPNfile.Name = "labelSelectedOVPNfile";
            this.labelSelectedOVPNfile.Size = new System.Drawing.Size(152, 13);
            this.labelSelectedOVPNfile.TabIndex = 0;
            this.labelSelectedOVPNfile.Text = "Perfil(es) VPN seleccionado(s):";
            // 
            // tbSelectedOVPNfile
            // 
            this.tbSelectedOVPNfile.Location = new System.Drawing.Point(15, 49);
            this.tbSelectedOVPNfile.Name = "tbSelectedOVPNfile";
            this.tbSelectedOVPNfile.ReadOnly = true;
            this.tbSelectedOVPNfile.Size = new System.Drawing.Size(336, 20);
            this.tbSelectedOVPNfile.TabIndex = 1;
            // 
            // btnSelectOVPNfile
            // 
            this.btnSelectOVPNfile.Location = new System.Drawing.Point(357, 46);
            this.btnSelectOVPNfile.Name = "btnSelectOVPNfile";
            this.btnSelectOVPNfile.Size = new System.Drawing.Size(88, 23);
            this.btnSelectOVPNfile.TabIndex = 2;
            this.btnSelectOVPNfile.Text = "Abrir";
            this.btnSelectOVPNfile.UseVisualStyleBackColor = true;
            this.btnSelectOVPNfile.Click += new System.EventHandler(this.btnSelectOVPNfile_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.flowLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(15, 75);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(430, 79);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Comprobaciones";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.infoLecturaCA);
            this.flowLayoutPanel1.Controls.Add(this.infoLecturaCert);
            this.flowLayoutPanel1.Controls.Add(this.infoLecturaKey);
            this.flowLayoutPanel1.Controls.Add(this.infoLecturaConfig);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(424, 60);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // infoLecturaCA
            // 
            this.infoLecturaCA.AutoSize = true;
            this.infoLecturaCA.Location = new System.Drawing.Point(3, 0);
            this.infoLecturaCA.Name = "infoLecturaCA";
            this.infoLecturaCA.Size = new System.Drawing.Size(35, 13);
            this.infoLecturaCA.TabIndex = 0;
            this.infoLecturaCA.Text = "label1";
            this.infoLecturaCA.Visible = false;
            // 
            // infoLecturaCert
            // 
            this.infoLecturaCert.AutoSize = true;
            this.infoLecturaCert.Location = new System.Drawing.Point(3, 13);
            this.infoLecturaCert.Name = "infoLecturaCert";
            this.infoLecturaCert.Size = new System.Drawing.Size(35, 13);
            this.infoLecturaCert.TabIndex = 1;
            this.infoLecturaCert.Text = "label1";
            this.infoLecturaCert.Visible = false;
            // 
            // infoLecturaKey
            // 
            this.infoLecturaKey.AutoSize = true;
            this.infoLecturaKey.Location = new System.Drawing.Point(3, 26);
            this.infoLecturaKey.Name = "infoLecturaKey";
            this.infoLecturaKey.Size = new System.Drawing.Size(35, 13);
            this.infoLecturaKey.TabIndex = 2;
            this.infoLecturaKey.Text = "label1";
            this.infoLecturaKey.Visible = false;
            // 
            // infoLecturaConfig
            // 
            this.infoLecturaConfig.AutoSize = true;
            this.infoLecturaConfig.Location = new System.Drawing.Point(3, 39);
            this.infoLecturaConfig.Name = "infoLecturaConfig";
            this.infoLecturaConfig.Size = new System.Drawing.Size(35, 13);
            this.infoLecturaConfig.TabIndex = 3;
            this.infoLecturaConfig.Text = "label1";
            this.infoLecturaConfig.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSaveServerConfigCert);
            this.groupBox2.Location = new System.Drawing.Point(15, 171);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(430, 52);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "PASO 1 (opcional): Tarjetas Comunes a todos los usuarios";
            // 
            // btnSaveServerConfigCert
            // 
            this.btnSaveServerConfigCert.Enabled = false;
            this.btnSaveServerConfigCert.Location = new System.Drawing.Point(6, 19);
            this.btnSaveServerConfigCert.Name = "btnSaveServerConfigCert";
            this.btnSaveServerConfigCert.Size = new System.Drawing.Size(408, 23);
            this.btnSaveServerConfigCert.TabIndex = 7;
            this.btnSaveServerConfigCert.Text = "Guardar Configuración VPN y Certificado de Servidor en PDF";
            this.btnSaveServerConfigCert.UseVisualStyleBackColor = true;
            this.btnSaveServerConfigCert.Click += new System.EventHandler(this.btnSaveServerConfigCert_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(457, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.salirToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.fileToolStripMenuItem.Text = "Archivo";
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.salirToolStripMenuItem.Text = "Salir";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.salirToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.helpToolStripMenuItem.Text = "Ayuda";
            this.helpToolStripMenuItem.Visible = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnCertKeyUser);
            this.groupBox3.Location = new System.Drawing.Point(15, 229);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(430, 57);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "PASO 2: Tarjetas Particulares de CADA USUARIO";
            // 
            // btnCertKeyUser
            // 
            this.btnCertKeyUser.Enabled = false;
            this.btnCertKeyUser.Location = new System.Drawing.Point(9, 19);
            this.btnCertKeyUser.Name = "btnCertKeyUser";
            this.btnCertKeyUser.Size = new System.Drawing.Size(408, 23);
            this.btnCertKeyUser.TabIndex = 9;
            this.btnCertKeyUser.Text = "Guardar toda la config de usuario en PDF";
            this.btnCertKeyUser.UseVisualStyleBackColor = true;
            this.btnCertKeyUser.Click += new System.EventHandler(this.btnCertKeyUser_Click);
            // 
            // frmQRProfilePrinter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 294);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSelectOVPNfile);
            this.Controls.Add(this.tbSelectedOVPNfile);
            this.Controls.Add(this.labelSelectedOVPNfile);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "frmQRProfilePrinter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Impresión de configuración VPN en formato QR";
            this.Load += new System.EventHandler(this.frmQRProfilePrinter_Load);
            this.groupBox1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelSelectedOVPNfile;
        private System.Windows.Forms.TextBox tbSelectedOVPNfile;
        private System.Windows.Forms.Button btnSelectOVPNfile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label infoLecturaCA;
        private System.Windows.Forms.Label infoLecturaCert;
        private System.Windows.Forms.Label infoLecturaKey;
        private System.Windows.Forms.Label infoLecturaConfig;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
        private System.Windows.Forms.Button btnSaveServerConfigCert;
        private System.Windows.Forms.Button btnCertKeyUser;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}

