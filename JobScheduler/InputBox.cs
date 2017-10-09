using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms
{
    public partial class InputBox : Form
    {
        public static string Ask(string title = "InputBox", string defaultValue = "")
        {
            InputBox form = new InputBox();
            form.Text = title;
            form.Value = defaultValue;
            form.ShowDialog();
            return form.Value;
        }
        
        private InputBox()
        {
            InitializeComponent();
        }

        public string Value
        {
            get { return inputTextBox.Text; }
            set { inputTextBox.Text = value; }
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
