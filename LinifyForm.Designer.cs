namespace ImageProcessing
{
    partial class LinifyForm
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
            this.horizontalNodesCount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.verticalNodesCount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lineThickness = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tboxSteps = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Horizontal Nodes";
            // 
            // horizontalNodesCount
            // 
            this.horizontalNodesCount.AccessibleName = "tBoxHNodes";
            this.horizontalNodesCount.Location = new System.Drawing.Point(149, 56);
            this.horizontalNodesCount.Name = "horizontalNodesCount";
            this.horizontalNodesCount.Size = new System.Drawing.Size(100, 25);
            this.horizontalNodesCount.TabIndex = 1;
            this.horizontalNodesCount.Text = "50";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Vertical Nodes";
            // 
            // verticalNodesCount
            // 
            this.verticalNodesCount.AccessibleName = "tBoxVNodes";
            this.verticalNodesCount.Location = new System.Drawing.Point(149, 93);
            this.verticalNodesCount.Name = "verticalNodesCount";
            this.verticalNodesCount.Size = new System.Drawing.Size(100, 25);
            this.verticalNodesCount.TabIndex = 3;
            this.verticalNodesCount.Text = "100";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Line Thickness";
            // 
            // lineThickness
            // 
            this.lineThickness.AccessibleName = "tBoxLnThickness";
            this.lineThickness.Location = new System.Drawing.Point(149, 133);
            this.lineThickness.Name = "lineThickness";
            this.lineThickness.Size = new System.Drawing.Size(100, 25);
            this.lineThickness.TabIndex = 5;
            this.lineThickness.Text = "1";
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(134, 211);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 27);
            this.btnCancel.TabIndex = 24;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(26, 211);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 27);
            this.btnOK.TabIndex = 23;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 180);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 15);
            this.label4.TabIndex = 25;
            this.label4.Text = "Steps";
            // 
            // tboxSteps
            // 
            this.tboxSteps.Location = new System.Drawing.Point(149, 170);
            this.tboxSteps.Name = "tboxSteps";
            this.tboxSteps.Size = new System.Drawing.Size(100, 25);
            this.tboxSteps.TabIndex = 26;
            this.tboxSteps.Text = "1000";
            // 
            // LinifyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Controls.Add(this.tboxSteps);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lineThickness);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.verticalNodesCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.horizontalNodesCount);
            this.Controls.Add(this.label1);
            this.Name = "LinifyForm";
            this.Text = "LinifyForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox horizontalNodesCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox verticalNodesCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox lineThickness;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox tboxSteps;
        private System.Windows.Forms.Label label4;
    }
}