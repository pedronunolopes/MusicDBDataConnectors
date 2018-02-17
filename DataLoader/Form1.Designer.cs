namespace WindowsFormsApp1
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
            this.txtBoxArtistName = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGetData = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtBoxArtistName
            // 
            this.txtBoxArtistName.Location = new System.Drawing.Point(147, 47);
            this.txtBoxArtistName.Name = "txtBoxArtistName";
            this.txtBoxArtistName.Size = new System.Drawing.Size(474, 31);
            this.txtBoxArtistName.TabIndex = 0;
            this.txtBoxArtistName.Text = "U2";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(0, 174);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(1719, 426);
            this.textBox2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Artist Name:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnGetData
            // 
            this.btnGetData.Location = new System.Drawing.Point(638, 44);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(181, 37);
            this.btnGetData.TabIndex = 3;
            this.btnGetData.Text = "Get Artist Data";
            this.btnGetData.UseVisualStyleBackColor = true;
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(841, 44);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(174, 37);
            this.button1.TabIndex = 4;
            this.button1.Text = "Get Event Data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1030, 44);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(174, 37);
            this.button2.TabIndex = 5;
            this.button2.Text = "Load Venues Location";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1721, 600);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnGetData);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.txtBoxArtistName);
            this.Name = "Form1";
            this.Text = "MusicDB External Data";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBoxArtistName;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGetData;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

