
namespace PSXMemoryCard.Forms
{
    partial class SaveFileInfo
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
            this.button1 = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.productLabel = new System.Windows.Forms.Label();
            this.identifierLabel = new System.Windows.Forms.Label();
            this.regionLabel = new System.Windows.Forms.Label();
            this.saveSlotLabel = new System.Windows.Forms.Label();
            this.sizeLabel = new System.Windows.Forms.Label();
            this.iconDisplay = new PSXMemoryCard.InterpolatingPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.iconDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(300, 116);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(156, 16);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(30, 13);
            this.titleLabel.TabIndex = 2;
            this.titleLabel.Text = "Title:";
            // 
            // productLabel
            // 
            this.productLabel.AutoSize = true;
            this.productLabel.Location = new System.Drawing.Point(111, 34);
            this.productLabel.Name = "productLabel";
            this.productLabel.Size = new System.Drawing.Size(75, 13);
            this.productLabel.TabIndex = 3;
            this.productLabel.Text = "Product Code:";
            // 
            // identifierLabel
            // 
            this.identifierLabel.AutoSize = true;
            this.identifierLabel.Location = new System.Drawing.Point(136, 52);
            this.identifierLabel.Name = "identifierLabel";
            this.identifierLabel.Size = new System.Drawing.Size(50, 13);
            this.identifierLabel.TabIndex = 4;
            this.identifierLabel.Text = "Identifier:";
            // 
            // regionLabel
            // 
            this.regionLabel.AutoSize = true;
            this.regionLabel.Location = new System.Drawing.Point(142, 70);
            this.regionLabel.Name = "regionLabel";
            this.regionLabel.Size = new System.Drawing.Size(44, 13);
            this.regionLabel.TabIndex = 5;
            this.regionLabel.Text = "Region:";
            // 
            // saveSlotLabel
            // 
            this.saveSlotLabel.AutoSize = true;
            this.saveSlotLabel.Location = new System.Drawing.Point(130, 88);
            this.saveSlotLabel.Name = "saveSlotLabel";
            this.saveSlotLabel.Size = new System.Drawing.Size(56, 13);
            this.saveSlotLabel.TabIndex = 6;
            this.saveSlotLabel.Text = "Save Slot:";
            // 
            // sizeLabel
            // 
            this.sizeLabel.AutoSize = true;
            this.sizeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sizeLabel.Location = new System.Drawing.Point(1, 113);
            this.sizeLabel.Name = "sizeLabel";
            this.sizeLabel.Size = new System.Drawing.Size(51, 24);
            this.sizeLabel.TabIndex = 7;
            this.sizeLabel.Text = "Size:";
            // 
            // iconDisplay
            // 
            this.iconDisplay.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            this.iconDisplay.Location = new System.Drawing.Point(9, 9);
            this.iconDisplay.Name = "iconDisplay";
            this.iconDisplay.Size = new System.Drawing.Size(96, 96);
            this.iconDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.iconDisplay.TabIndex = 0;
            this.iconDisplay.TabStop = false;
            // 
            // SaveFileInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 146);
            this.Controls.Add(this.sizeLabel);
            this.Controls.Add(this.saveSlotLabel);
            this.Controls.Add(this.regionLabel);
            this.Controls.Add(this.identifierLabel);
            this.Controls.Add(this.productLabel);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.iconDisplay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SaveFileInfo";
            this.Text = "Save Information";
            ((System.ComponentModel.ISupportInitialize)(this.iconDisplay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private InterpolatingPictureBox iconDisplay;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label productLabel;
        private System.Windows.Forms.Label identifierLabel;
        private System.Windows.Forms.Label regionLabel;
        private System.Windows.Forms.Label saveSlotLabel;
        private System.Windows.Forms.Label sizeLabel;
    }
}