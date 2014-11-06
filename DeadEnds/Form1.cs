using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeadEnds
{
    public partial class Form1 : Form
    {

        private List<System.IO.FileInfo> filenames;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dg = new FolderBrowserDialog();
            dg.ShowNewFolderButton = false;
            dg.RootFolder = System.Environment.SpecialFolder.Desktop;
            if (dg.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dg.SelectedPath;
            }
        }

        // TODO Count occurences and order list
        // TODO Progressbar
        // TODO Warning for files that are directly accessed
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(textBox1.Text);
                filenames = getFiles(directory);
                List<System.IO.FileInfo> usedFiles = getUsage(new System.IO.FileInfo(textBox2.Text));

                /*foreach (System.IO.FileInfo file in filenames)
                {
                    ListViewItem item = new ListViewItem(new[] { file.Name, "0" });
                    listView1.Items.Add(item);
                }*/
                MessageBox.Show("Checked usage of " + filenames.Count + " files", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Select a project folder", "No folder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                OpenFileDialog dg = new OpenFileDialog();
                dg.InitialDirectory = textBox1.Text;
                if (dg.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = dg.FileName;
                }
            }
            else
            {
                MessageBox.Show("Select a project folder first", "No folder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private List<System.IO.FileInfo> getFiles(System.IO.DirectoryInfo directory)
        {
            List<System.IO.FileInfo> filenames = new List<System.IO.FileInfo>();
            System.IO.DirectoryInfo[] directories = directory.GetDirectories();
            System.IO.FileInfo[] files = directory.GetFiles();
            foreach (System.IO.FileInfo file in files)
            {
                filenames.Add(file);
            }
            foreach (System.IO.DirectoryInfo dir in directories)
            {
                filenames.AddRange(getFiles(dir));
            }
            return filenames;
        }

        private List<System.IO.FileInfo> getUsage(System.IO.FileInfo file)
        {
            List<System.IO.FileInfo> usedFiles = new List<System.IO.FileInfo>();
            String content = System.IO.File.ReadAllText(file.FullName);
            foreach (System.IO.FileInfo currentFile in filenames) {
                if (content.Contains(currentFile.Name))
                {
                    if (!usedFiles.Contains(file))
                    {
                        usedFiles.Add(file);
                        usedFiles.AddRange(getUsage(file));
                    }
                }
            }
            return usedFiles;
        }

    }
}
