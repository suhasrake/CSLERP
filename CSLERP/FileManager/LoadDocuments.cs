using CSLERP.DBData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSLERP.FileManager
{
    public partial class LoadDocuments : Form
    {
        string dir;
        string subDir = "";
        string docId;

        public LoadDocuments()
        {
            InitializeComponent();
            btnView.Enabled = false;
        }
        public LoadDocuments(string path, string sd, string dId, string hStr)
        {
            InitializeComponent();
            dir = path + "\\" + sd;
            docId = dId;
            subDir = sd;
            this.lblHeading.Text = hStr;
            btnView.Enabled = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog op1 = new OpenFileDialog();
            op1.Filter = "Pdf Files|*.pdf|all files|*.*"; ////"jpeg|*.jpg|bmp|*.bmp|all files|*.*"
            op1.ShowDialog();
            //op1.Multiselect = true;
            txtShow.Text = op1.FileName;
            string sourcepath = op1.FileName;
            if (txtShow.Text.Length != 0)
            {
                btnView.Enabled = true;
            }
        }
        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtDescription.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Please enter the document description");
                    return;
                }
                if (txtShow.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Please select file to upload");
                    return;
                }
                string path = txtShow.Text.Trim();
                string nm = Path.GetFileName(path);

                documentStorage ds = new documentStorage();
                DocumentStorageDB dsdb = new DocumentStorageDB();
                ds.DocumentID = docId;
                ds.Directory = subDir;
                ds.DocumentSubID = subDir;
                ds.FileName = nm;
                ds.Description = txtDescription.Text;
                //ds.ProtectionLevel = 1;
                if (dsdb.validateDocumentDetails(ds))
                {
                    //------------convert document to binary
                    try
                    {
                        byte[] byteArray = null;
                        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                        BinaryReader br = new BinaryReader(fs);
                        //byteArray = br.ReadBytes((int)fs.Length);
                        //string byteStr = Convert.ToBase64String(byteArray);
                        //ds.DocumentContent = byteStr;
                        ds.DocumentContent = Convert.ToBase64String(br.ReadBytes((int)fs.Length));

                        br.Close();
                        fs.Close();
                    }
                    catch (Exception)
                    {
                    }
                    //------------
                    if (dsdb.iskDocumentDuplication(ds))
                    {
                        
                        DialogResult dialog = MessageBox.Show("Document with same name existing. Replace ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            if (dsdb.UpdateDocumentDetails(ds))
                            {
                                MessageBox.Show("Document Replaced Sucessfuly");

                            }
                            else
                            {
                                MessageBox.Show("Failed to replace Document");
                            }
                        }
                    }
                    else
                    {
                        if (dsdb.InsertDocumentDetails(ds))
                        {
                            MessageBox.Show("Document uploaded Sucessfuly");

                        }
                        else
                        {
                            MessageBox.Show("Failed to Load document. Check");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Validation failed. Please verify the details");
                }
                txtShow.Text = "";
                txtDescription.Text = "";
                btnView.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("btnUpload_Click() : File upload Failed");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(txtShow.Text);
        }
    }
}
