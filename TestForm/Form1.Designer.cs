namespace TestForm
{
    partial class Form1
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
            this.btnAgregarUsuario = new System.Windows.Forms.Button();
            this.btnAgregarEmpresa = new System.Windows.Forms.Button();
            this.btnObtenEmpresa = new System.Windows.Forms.Button();
            this.btnObtenUsuario = new System.Windows.Forms.Button();
            this.btnCertificados = new System.Windows.Forms.Button();
            this.btnConvertir = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAgregarUsuario
            // 
            this.btnAgregarUsuario.Location = new System.Drawing.Point(12, 41);
            this.btnAgregarUsuario.Name = "btnAgregarUsuario";
            this.btnAgregarUsuario.Size = new System.Drawing.Size(119, 23);
            this.btnAgregarUsuario.TabIndex = 0;
            this.btnAgregarUsuario.Text = "Agregar Usuario";
            this.btnAgregarUsuario.UseVisualStyleBackColor = true;
            this.btnAgregarUsuario.Click += new System.EventHandler(this.btnAgregarUsuario_Click);
            // 
            // btnAgregarEmpresa
            // 
            this.btnAgregarEmpresa.Location = new System.Drawing.Point(12, 12);
            this.btnAgregarEmpresa.Name = "btnAgregarEmpresa";
            this.btnAgregarEmpresa.Size = new System.Drawing.Size(119, 23);
            this.btnAgregarEmpresa.TabIndex = 1;
            this.btnAgregarEmpresa.Text = "Agregar Empresa";
            this.btnAgregarEmpresa.UseVisualStyleBackColor = true;
            this.btnAgregarEmpresa.Click += new System.EventHandler(this.btnAgregarEmpresa_Click);
            // 
            // btnObtenEmpresa
            // 
            this.btnObtenEmpresa.Location = new System.Drawing.Point(12, 95);
            this.btnObtenEmpresa.Name = "btnObtenEmpresa";
            this.btnObtenEmpresa.Size = new System.Drawing.Size(119, 23);
            this.btnObtenEmpresa.TabIndex = 2;
            this.btnObtenEmpresa.Text = "Obtener Empresa";
            this.btnObtenEmpresa.UseVisualStyleBackColor = true;
            this.btnObtenEmpresa.Click += new System.EventHandler(this.btnObtenEmpresa_Click);
            // 
            // btnObtenUsuario
            // 
            this.btnObtenUsuario.Location = new System.Drawing.Point(12, 124);
            this.btnObtenUsuario.Name = "btnObtenUsuario";
            this.btnObtenUsuario.Size = new System.Drawing.Size(119, 23);
            this.btnObtenUsuario.TabIndex = 3;
            this.btnObtenUsuario.Text = "Obtener Usuario";
            this.btnObtenUsuario.UseVisualStyleBackColor = true;
            this.btnObtenUsuario.Click += new System.EventHandler(this.btnObtenUsuario_Click);
            // 
            // btnCertificados
            // 
            this.btnCertificados.Location = new System.Drawing.Point(12, 70);
            this.btnCertificados.Name = "btnCertificados";
            this.btnCertificados.Size = new System.Drawing.Size(119, 23);
            this.btnCertificados.TabIndex = 4;
            this.btnCertificados.Text = "Agregar Certificados";
            this.btnCertificados.UseVisualStyleBackColor = true;
            this.btnCertificados.Click += new System.EventHandler(this.btnCertificados_Click);
            // 
            // btnConvertir
            // 
            this.btnConvertir.Location = new System.Drawing.Point(12, 153);
            this.btnConvertir.Name = "btnConvertir";
            this.btnConvertir.Size = new System.Drawing.Size(119, 23);
            this.btnConvertir.TabIndex = 5;
            this.btnConvertir.Text = "Convertir TXT";
            this.btnConvertir.UseVisualStyleBackColor = true;
            this.btnConvertir.Click += new System.EventHandler(this.btnConvertir_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btnConvertir);
            this.Controls.Add(this.btnCertificados);
            this.Controls.Add(this.btnObtenUsuario);
            this.Controls.Add(this.btnObtenEmpresa);
            this.Controls.Add(this.btnAgregarEmpresa);
            this.Controls.Add(this.btnAgregarUsuario);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAgregarUsuario;
        private System.Windows.Forms.Button btnAgregarEmpresa;
        private System.Windows.Forms.Button btnObtenEmpresa;
        private System.Windows.Forms.Button btnObtenUsuario;
        private System.Windows.Forms.Button btnCertificados;
        private System.Windows.Forms.Button btnConvertir;
    }
}

