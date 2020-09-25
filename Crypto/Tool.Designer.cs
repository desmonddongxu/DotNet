namespace Crypto
{
    partial class Tool
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
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.txtPlainText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtEncrypted = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTextToDecrypt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.txtDecrypted = new System.Windows.Forms.TextBox();
            this.txtHASHed = new System.Windows.Forms.TextBox();
            this.btnHASH = new System.Windows.Forms.Button();
            this.lblTextToHASH = new System.Windows.Forms.Label();
            this.txtToHASH = new System.Windows.Forms.TextBox();
            this.btnChooseFile = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.rdEncrypt = new System.Windows.Forms.RadioButton();
            this.rdDecrypt = new System.Windows.Forms.RadioButton();
            this.btnClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Location = new System.Drawing.Point(432, 45);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(75, 23);
            this.btnEncrypt.TabIndex = 0;
            this.btnEncrypt.Text = "Encrypt";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // txtPlainText
            // 
            this.txtPlainText.Location = new System.Drawing.Point(112, 47);
            this.txtPlainText.Name = "txtPlainText";
            this.txtPlainText.Size = new System.Drawing.Size(314, 20);
            this.txtPlainText.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Text to Encrypt";
            // 
            // txtEncrypted
            // 
            this.txtEncrypted.Location = new System.Drawing.Point(112, 77);
            this.txtEncrypted.Multiline = true;
            this.txtEncrypted.Name = "txtEncrypted";
            this.txtEncrypted.Size = new System.Drawing.Size(314, 74);
            this.txtEncrypted.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Encryption Key";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(112, 15);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(314, 20);
            this.textBox3.TabIndex = 5;
            this.textBox3.Text = "zI0IL/Hamswr/7lbcRXe0kPhhfMirCXqxyKGaycrX+Q=";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(68, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Result";
            // 
            // txtTextToDecrypt
            // 
            this.txtTextToDecrypt.Location = new System.Drawing.Point(112, 172);
            this.txtTextToDecrypt.Name = "txtTextToDecrypt";
            this.txtTextToDecrypt.Size = new System.Drawing.Size(314, 20);
            this.txtTextToDecrypt.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 172);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Text to Decrypt";
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Location = new System.Drawing.Point(434, 172);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(75, 23);
            this.btnDecrypt.TabIndex = 9;
            this.btnDecrypt.Text = "Decrypt";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // txtDecrypted
            // 
            this.txtDecrypted.Location = new System.Drawing.Point(112, 201);
            this.txtDecrypted.Multiline = true;
            this.txtDecrypted.Name = "txtDecrypted";
            this.txtDecrypted.Size = new System.Drawing.Size(314, 74);
            this.txtDecrypted.TabIndex = 10;
            // 
            // txtHASHed
            // 
            this.txtHASHed.Location = new System.Drawing.Point(114, 325);
            this.txtHASHed.Multiline = true;
            this.txtHASHed.Name = "txtHASHed";
            this.txtHASHed.Size = new System.Drawing.Size(314, 74);
            this.txtHASHed.TabIndex = 14;
            // 
            // btnHASH
            // 
            this.btnHASH.Location = new System.Drawing.Point(436, 296);
            this.btnHASH.Name = "btnHASH";
            this.btnHASH.Size = new System.Drawing.Size(75, 23);
            this.btnHASH.TabIndex = 13;
            this.btnHASH.Text = "HASH";
            this.btnHASH.UseVisualStyleBackColor = true;
            this.btnHASH.Click += new System.EventHandler(this.btnHASH_Click);
            // 
            // lblTextToHASH
            // 
            this.lblTextToHASH.AutoSize = true;
            this.lblTextToHASH.Location = new System.Drawing.Point(4, 299);
            this.lblTextToHASH.Name = "lblTextToHASH";
            this.lblTextToHASH.Size = new System.Drawing.Size(109, 13);
            this.lblTextToHASH.TabIndex = 12;
            this.lblTextToHASH.Text = "Text to HASH - SALT";
            // 
            // txtToHASH
            // 
            this.txtToHASH.Location = new System.Drawing.Point(114, 296);
            this.txtToHASH.Name = "txtToHASH";
            this.txtToHASH.Size = new System.Drawing.Size(314, 20);
            this.txtToHASH.TabIndex = 11;
            // 
            // btnChooseFile
            // 
            this.btnChooseFile.Location = new System.Drawing.Point(1003, 18);
            this.btnChooseFile.Name = "btnChooseFile";
            this.btnChooseFile.Size = new System.Drawing.Size(162, 23);
            this.btnChooseFile.TabIndex = 15;
            this.btnChooseFile.Text = "Choose the Read File";
            this.btnChooseFile.UseVisualStyleBackColor = true;
            this.btnChooseFile.Click += new System.EventHandler(this.btnChooseFile_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(562, 47);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(603, 352);
            this.dataGridView1.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(559, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(220, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "1st column data (column header not required)";
            // 
            // rdEncrypt
            // 
            this.rdEncrypt.AutoSize = true;
            this.rdEncrypt.Checked = true;
            this.rdEncrypt.Location = new System.Drawing.Point(804, 18);
            this.rdEncrypt.Name = "rdEncrypt";
            this.rdEncrypt.Size = new System.Drawing.Size(61, 17);
            this.rdEncrypt.TabIndex = 19;
            this.rdEncrypt.TabStop = true;
            this.rdEncrypt.Text = "Encrypt";
            this.rdEncrypt.UseVisualStyleBackColor = true;
            // 
            // rdDecrypt
            // 
            this.rdDecrypt.AutoSize = true;
            this.rdDecrypt.Location = new System.Drawing.Point(871, 18);
            this.rdDecrypt.Name = "rdDecrypt";
            this.rdDecrypt.Size = new System.Drawing.Size(62, 17);
            this.rdDecrypt.TabIndex = 20;
            this.rdDecrypt.TabStop = true;
            this.rdDecrypt.Text = "Decrypt";
            this.rdDecrypt.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(1090, 406);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 21;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // Tool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1193, 441);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.rdDecrypt);
            this.Controls.Add(this.rdEncrypt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnChooseFile);
            this.Controls.Add(this.txtHASHed);
            this.Controls.Add(this.btnHASH);
            this.Controls.Add(this.lblTextToHASH);
            this.Controls.Add(this.txtToHASH);
            this.Controls.Add(this.txtDecrypted);
            this.Controls.Add(this.btnDecrypt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtTextToDecrypt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtEncrypted);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPlainText);
            this.Controls.Add(this.btnEncrypt);
            this.Name = "Tool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tool";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.TextBox txtPlainText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtEncrypted;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTextToDecrypt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.TextBox txtDecrypted;
        private System.Windows.Forms.Button btnHASH;
        private System.Windows.Forms.Label lblTextToHASH;
        private System.Windows.Forms.TextBox txtToHASH;
        private System.Windows.Forms.TextBox txtHASHed;
        private System.Windows.Forms.Button btnChooseFile;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rdEncrypt;
        private System.Windows.Forms.RadioButton rdDecrypt;
        private System.Windows.Forms.Button btnClear;
    }
}

