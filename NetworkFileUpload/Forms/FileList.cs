using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetworkFileUpload.Classes;

namespace NetworkFileUpload
{
    public partial class FileList : Form
    {
        private List<FileDisplayData> receivedFiles;

        public FileList(List<FileDisplayData> files)
        {
            InitializeComponent();

            this.Text = "File List with Icons";
            this.Size = new Size(400, 300);
            this.receivedFiles = files;

            listView1 = new ListView
            {
                View = View.Details,
                FullRowSelect = true,
            };

            // Add columns
            listView1.Columns.Add("File Name", 250);
            listView1.SmallImageList = new ImageList();
            imageList1 = listView1.SmallImageList;
            toolTip1 = new ToolTip();

            // Load sample files with icons

            LoadFileList();

            // Handle mouse move for tooltips
            listView1.MouseMove += ListView1_MouseMove;
        }

        private void LoadFileList()
        {
            // Define file items with associated icons and tooltips
            List<FileDisplayData> files = this.receivedFiles;

            // Load icons into ImageList
            imageList1.Images.Add("Created", SystemIcons.Information.ToBitmap()); // Green check (example)
            imageList1.Images.Add("Deleted", SystemIcons.Warning.ToBitmap()); // Red alert (example)
            imageList1.Images.Add("Renamed", SystemIcons.Question.ToBitmap()); // Yellow clock (example)
            imageList1.Images.Add("Changed", SystemIcons.Exclamation.ToBitmap()); 

            foreach (FileDisplayData file in files)
            {
                ListViewItem item = new ListViewItem(file.FileName);
                item.ImageKey = file.IconType;
                item.Tag = file.ToolTip; // Store tooltip text

                listView1.Items.Add(item);
            }
        }

        private void ListView1_MouseMove(object sender, MouseEventArgs e)
        {
            ListViewItem item = listView1.GetItemAt(e.X, e.Y);
            if (item != null)
            {
                toolTip1.SetToolTip(listView1, item.Tag.ToString());
            }
        }

        public void RefreshFileList(List<FileDisplayData> updatedFiles)
        {
            this.receivedFiles = updatedFiles; // Update the reference
            LoadFileList(); // Refresh UI
        }
    }
}
