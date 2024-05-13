
namespace WordPad
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Load = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.RTBTextBox = new System.Windows.Forms.RichTextBox();
            this.Undo = new System.Windows.Forms.Button();
            this.Redo = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.Listbox = new System.Windows.Forms.ListBox();
            this.Redobox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // Load
            // 
            this.Load.Font = new System.Drawing.Font("新細明體", 18F);
            this.Load.Location = new System.Drawing.Point(12, 12);
            this.Load.Name = "Load";
            this.Load.Size = new System.Drawing.Size(106, 36);
            this.Load.TabIndex = 0;
            this.Load.Text = "讀取";
            this.Load.UseVisualStyleBackColor = true;
            this.Load.Click += new System.EventHandler(this.Load_Click);
            // 
            // Save
            // 
            this.Save.Font = new System.Drawing.Font("新細明體", 18F);
            this.Save.Location = new System.Drawing.Point(124, 12);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(106, 36);
            this.Save.TabIndex = 1;
            this.Save.Text = "儲存";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // RTBTextBox
            // 
            this.RTBTextBox.Font = new System.Drawing.Font("新細明體", 18F);
            this.RTBTextBox.Location = new System.Drawing.Point(12, 54);
            this.RTBTextBox.Name = "RTBTextBox";
            this.RTBTextBox.Size = new System.Drawing.Size(545, 384);
            this.RTBTextBox.TabIndex = 2;
            this.RTBTextBox.Text = "";
            this.RTBTextBox.TextChanged += new System.EventHandler(this.RTBTextBox_TextChanged);
            // 
            // Undo
            // 
            this.Undo.Font = new System.Drawing.Font("新細明體", 18F);
            this.Undo.Location = new System.Drawing.Point(570, 12);
            this.Undo.Name = "Undo";
            this.Undo.Size = new System.Drawing.Size(106, 36);
            this.Undo.TabIndex = 3;
            this.Undo.Text = "復原";
            this.Undo.UseVisualStyleBackColor = true;
            this.Undo.Click += new System.EventHandler(this.Undo_Click);
            // 
            // Redo
            // 
            this.Redo.Font = new System.Drawing.Font("新細明體", 18F);
            this.Redo.Location = new System.Drawing.Point(682, 12);
            this.Redo.Name = "Redo";
            this.Redo.Size = new System.Drawing.Size(106, 36);
            this.Redo.TabIndex = 4;
            this.Redo.Text = "重做";
            this.Redo.UseVisualStyleBackColor = true;
            this.Redo.Click += new System.EventHandler(this.Redo_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Listbox
            // 
            this.Listbox.FormattingEnabled = true;
            this.Listbox.ItemHeight = 12;
            this.Listbox.Location = new System.Drawing.Point(570, 54);
            this.Listbox.Name = "Listbox";
            this.Listbox.Size = new System.Drawing.Size(218, 184);
            this.Listbox.TabIndex = 5;
            // 
            // Redobox
            // 
            this.Redobox.FormattingEnabled = true;
            this.Redobox.ItemHeight = 12;
            this.Redobox.Location = new System.Drawing.Point(570, 254);
            this.Redobox.Name = "Redobox";
            this.Redobox.Size = new System.Drawing.Size(218, 184);
            this.Redobox.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Redobox);
            this.Controls.Add(this.Listbox);
            this.Controls.Add(this.Redo);
            this.Controls.Add(this.Undo);
            this.Controls.Add(this.RTBTextBox);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.Load);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Load;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.RichTextBox RTBTextBox;
        private System.Windows.Forms.Button Undo;
        private System.Windows.Forms.Button Redo;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ListBox Listbox;
        private System.Windows.Forms.ListBox Redobox;
    }
}

