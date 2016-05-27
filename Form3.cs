using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Tamir.SharpSsh;

namespace DDV
{
    public partial class Form3 : Form
    {
        string interface_folder = "";
        string dest_folder = "";

        public Form3(string input_folder, string output_folder)
        {
            this.interface_folder = input_folder;
            this.dest_folder = output_folder;

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = privatekey_input.ShowDialog();

            if (dr == DialogResult.OK)
            {
                if (privatekey_input.FileName == "")
                {
                    MessageBox.Show("Please select your Private Key file.", "Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                btn_privatekey_input.Text = privatekey_input.FileName + " Selected";
                //privatekey_input.FileName
            }
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            if (host_input.Text == "") 
            {
                MessageBox.Show("Please input a Host.", "Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (user_input.Text == "")
            {
                MessageBox.Show("Please input a User.", "Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Scp sshCp = new Scp(host_input.Text, user_input.Text);

            if (password_input.Text != "")
            {
                sshCp.Password = password_input.Text;
            }
            else if (privatekey_input.FileName != "")
            {
                sshCp.AddIdentityFile(privatekey_input.FileName);
            }
            else
            {
                MessageBox.Show("Please input a password or private key file.", "Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (serverlocation_input.Text == "")
            {
                MessageBox.Show("Please input a Server Side location to store the file.", "Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            sshCp.OnTransferStart += new FileTransferEvent(sshCp_OnTransferStart);
            sshCp.OnTransferProgress += new FileTransferEvent(sshCp_OnTransferProgress);
            sshCp.OnTransferEnd += new FileTransferEvent(sshCp_OnTransferEnd);

            sshCp.Connect();

            string destination = serverlocation_input.Text;
            if (!destination.EndsWith("/"))
            {
                destination += '/';
            }
            destination += "dnadata/" + this.dest_folder;

            sshCp.To(this.interface_folder, destination, true);

            sshCp.Close();

            MessageBox.Show("Transfer complete!", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void sshCp_OnTransferStart(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            progressBar1.Maximum = 3000;
            progressBar1.Step = 1;
            progressBar1.Value = 0;
            progressBar1.Update();
            progressBar1.Refresh();
        }

        private void sshCp_OnTransferProgress(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
        }

        private void sshCp_OnTransferEnd(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            progressBar1.Value += 1;
            progressBar1.Update();
            progressBar1.Refresh();
        }
    }
}
