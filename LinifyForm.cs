using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ImageProcessing
{
    public partial class LinifyForm : Form
    {
        public LinifyForm()
        {
            InitializeComponent();
            btnOK.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
        }
        public int Thickness
        {
            get
            {
                if (string.IsNullOrEmpty(lineThickness.Text))
                    lineThickness.Text = "1";
                return Convert.ToInt32(lineThickness.Text);
            }
            set { lineThickness.Text = value.ToString(); }
        }
        public int HorizontalNodesCount
        {
            get
            {
                if (string.IsNullOrEmpty(horizontalNodesCount.Text))
                    horizontalNodesCount.Text = "20";
                return Convert.ToInt32(horizontalNodesCount.Text);
            }
            set { horizontalNodesCount.Text = value.ToString(); }
        }
        public int VerticalNodesCount
        {
            get
            {
                if (string.IsNullOrEmpty(verticalNodesCount.Text))
                    verticalNodesCount.Text = "20";
                return Convert.ToInt32(verticalNodesCount.Text);
            }
            set { verticalNodesCount.Text = value.ToString(); }
        }
        public int StepCount
        {
            get
            {
                if (string.IsNullOrEmpty(tboxSteps.Text))
                    tboxSteps.Text = "10";
                return Convert.ToInt32(tboxSteps.Text);
            }
            set { tboxSteps.Text = value.ToString(); }
        }
    }
}
