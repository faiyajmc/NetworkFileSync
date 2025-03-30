using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace NetworkFileUpload
{
    public partial class MainFrm : Form
    {
        MainFormFunctions main = new MainFormFunctions();

        public MainFrm()
        {
            InitializeComponent();

            this.Enabled = false;

            main.InitializeFileTracking();

            main.childfrm = new FileList(main.fileList);

            panel1.Controls.Clear();
            panel1.Controls.Add(main.childfrm);
            main.childfrm.Show();

        }


        

    }
}
