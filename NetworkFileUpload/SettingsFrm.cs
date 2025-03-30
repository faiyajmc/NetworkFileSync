using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace NetworkFileUpload
{
    public partial class SettingsFrm : Form
    {
        public SettingsFrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = FileSelect();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = FileSelect();
        }

        private string FileSelect()
        {
            using (var fbd = new CommonOpenFileDialog { IsFolderPicker = true })
            {
                fbd.InitialDirectory = "C:\\Users";
                if (fbd.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    return fbd.FileName;
                }
            }

            return null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text)|| string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Please enter appropriate values");

                return;
            }

            if (!String.IsNullOrWhiteSpace(Properties.Settings.Default["SrcFldr"].ToString()))
            {
                DialogResult result = MessageBox.Show("This will reset the file tracking for the current source directory, continue?",
                                      "Confirm Reset",
                                      MessageBoxButtons.YesNo,
                                      MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    return;
                }
            }

            Properties.Settings.Default["SrcFldr"] = textBox1.Text;
            Properties.Settings.Default["DstFldr"] = textBox2.Text;
            Properties.Settings.Default.Save();

            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
