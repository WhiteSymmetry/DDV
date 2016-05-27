namespace DDV
{
    partial class Form3
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.host_input = new System.Windows.Forms.TextBox();
            this.user_input = new System.Windows.Forms.TextBox();
            this.password_input = new System.Windows.Forms.TextBox();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_privatekey_input = new System.Windows.Forms.Button();
            this.serverlocation_input = new System.Windows.Forms.TextBox();
            this.privatekey_input = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(51, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Host:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(51, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "User:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(23, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Password:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(221, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "OR";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Private Key:";
            // 
            // host_input
            // 
            this.host_input.Location = new System.Drawing.Point(94, 6);
            this.host_input.Name = "host_input";
            this.host_input.Size = new System.Drawing.Size(278, 20);
            this.host_input.TabIndex = 5;
            // 
            // user_input
            // 
            this.user_input.Location = new System.Drawing.Point(94, 32);
            this.user_input.Name = "user_input";
            this.user_input.Size = new System.Drawing.Size(278, 20);
            this.user_input.TabIndex = 6;
            // 
            // password_input
            // 
            this.password_input.Location = new System.Drawing.Point(94, 58);
            this.password_input.Name = "password_input";
            this.password_input.Size = new System.Drawing.Size(278, 20);
            this.password_input.TabIndex = 7;
            // 
            // btnTransfer
            // 
            this.btnTransfer.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnTransfer.Location = new System.Drawing.Point(160, 187);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(60, 28);
            this.btnTransfer.TabIndex = 42;
            this.btnTransfer.Text = "Transfer";
            this.btnTransfer.UseVisualStyleBackColor = false;
            this.btnTransfer.Click += new System.EventHandler(this.btnTransfer_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 221);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(360, 23);
            this.progressBar1.TabIndex = 43;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 168);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 13);
            this.label6.TabIndex = 44;
            this.label6.Text = "Server Location:";
            // 
            // btn_privatekey_input
            // 
            this.btn_privatekey_input.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btn_privatekey_input.Location = new System.Drawing.Point(94, 109);
            this.btn_privatekey_input.Name = "btn_privatekey_input";
            this.btn_privatekey_input.Size = new System.Drawing.Size(278, 28);
            this.btn_privatekey_input.TabIndex = 45;
            this.btn_privatekey_input.Text = "Browse";
            this.btn_privatekey_input.UseVisualStyleBackColor = false;
            this.btn_privatekey_input.Click += new System.EventHandler(this.button1_Click);
            // 
            // serverlocation_input
            // 
            this.serverlocation_input.Location = new System.Drawing.Point(119, 161);
            this.serverlocation_input.Name = "serverlocation_input";
            this.serverlocation_input.Size = new System.Drawing.Size(253, 20);
            this.serverlocation_input.TabIndex = 46;
            // 
            // privatekey_input
            // 
            this.privatekey_input.FileName = "openFileDialog1";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 256);
            this.Controls.Add(this.serverlocation_input);
            this.Controls.Add(this.btn_privatekey_input);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnTransfer);
            this.Controls.Add(this.password_input);
            this.Controls.Add(this.user_input);
            this.Controls.Add(this.host_input);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(400, 420);
            this.MinimumSize = new System.Drawing.Size(400, 240);
            this.Name = "Form3";
            this.ShowIcon = false;
            this.Text = "Transfer to Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox host_input;
        private System.Windows.Forms.TextBox user_input;
        private System.Windows.Forms.TextBox password_input;
        private System.Windows.Forms.Button btnTransfer;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btn_privatekey_input;
        private System.Windows.Forms.TextBox serverlocation_input;
        private System.Windows.Forms.OpenFileDialog privatekey_input;
    }
}