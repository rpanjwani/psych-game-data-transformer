using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace psych_game_data_transformer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ddlGames.Items.Add("SOPT");
            ddlGames.Items.Add("Xylophone");
            ddlGames.Items.Add("Whackamole");
            ddlGames.SelectedIndex = 0;
        }


        private void btnOutputFolder_Click(object sender, EventArgs e)
        {
            DialogResult result = folderOutputBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtOutputFolder.Text = 
                    getShortenedPath (folderOutputBrowser.SelectedPath);
            }
        }

        private void btnInputFolder_Click(object sender, EventArgs e)
        {
            DialogResult result = folderInputBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtInputFolder.Text = 
                    getShortenedPath(folderInputBrowser.SelectedPath);
            }
        }

        /// <summary>
        /// Returns first folder + ... + last folder as a shortened path 
        /// for display
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string getShortenedPath(string path)
        {
            if (String.IsNullOrWhiteSpace(path))
                return "";

            var folders = path.Split('\\');
            return folders.First() +
                  (String.IsNullOrWhiteSpace(folders.Last()) ? 
                  "" : "\\...\\") +
            folders.Last();
        }

        private void btnTransformData_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrWhiteSpace(txtInputFolder.Text))
            {
                MessageBox.Show("Please select INPUT directory");
                return;
            }
            if (String.IsNullOrWhiteSpace(txtOutputFolder.Text))
            {
                MessageBox.Show("Please select OUTPUT directory");
                return;
            }

            if (ddlGames.Text == "SOPT")
            {
                try
                {
                    new SoptTransformer().Transform(
                        folderInputBrowser.SelectedPath, folderOutputBrowser.SelectedPath);
                    MessageBox.Show("SOPT transformation done.");
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else if (ddlGames.Text == "Xylophone")
            {
                try
                {
                    new XylophoneTransformer().Transform(
                        folderInputBrowser.SelectedPath, folderOutputBrowser.SelectedPath);
                    MessageBox.Show("Xylophone transformation done.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else if(ddlGames.Text == "Whackamole")
            {
                try
                {
                    new WhackaMoleTransformer().Transform(
                        folderInputBrowser.SelectedPath, folderOutputBrowser.SelectedPath);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }

        }
    }
}
