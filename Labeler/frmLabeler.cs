using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Labeler
{
    public partial class frmLabeler : Form
    {

        Color Class1Color = Color.Red;
        Color Class2Color = Color.Green;
        bool expanded = false;
        int score = 0;
        int TopScore = 0;

        public frmLabeler()
        {
            InitializeComponent();
        }

        private void frmLabeler_Load(object sender, EventArgs e)
        {
            txtClass1Prefix_TextChanged(null, null);
            txtClass2Prefix_TextChanged(null, null);
            expanded = false;
        }

        private void lvImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lvImages.SelectedItems.Count <= 0)
                    return;

                picImage.Image = Image.FromFile(lvImages.SelectedItems[0].Text);
                lblTag.Text = lvImages.SelectedItems[0].SubItems[1].Text;
                lblTag.ForeColor = lvImages.SelectedItems[0].ForeColor;
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLoadImages_Click(object sender, EventArgs e)
        {
            try
            {
                fbdImages.ShowDialog();
                if (fbdImages.SelectedPath != null && fbdImages.SelectedPath != String.Empty)
                {
                    txtImagesPath.Text = fbdImages.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtImagesPath_TextChanged(object sender, EventArgs e)
        {
            try
            {
                LoadImages(txtImagesPath.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClass1_Click(object sender, EventArgs e)
        {
            try
            {
                Class1();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClass2_Click(object sender, EventArgs e)
        {
            try
            {
                Class2();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void LoadImages(string ImagesPath, bool all = true)
        {

            try
            {

                lvImages.View = View.Details ;
                lvImages.Columns.Clear();

                lvImages.Columns.Add("Images", 500);
                lvImages.Columns.Add("Type", 50);


                lvImages.Items.Clear();

                ImageList imgs = new ImageList();
                imgs.ImageSize = new Size(100, 100);

                String[] paths = { };
                paths = Directory.GetFiles(ImagesPath);

                prgLoad.Maximum = paths.Count();
                prgLoad.Step = 1;
                prgLoad.Value = 0;

                lvImages.SmallImageList = imgs;
                string class1check;
                string class2check;

                int Index = 0;
                foreach (String path in paths)
                {
                    try
                    {
                        class1check = Path.Combine(txtClass1Destination.Text, txtClass1Prefix.Text + "." + Path.GetFileName(path));
                        class2check = Path.Combine(txtClass2Destination.Text, txtClass2Prefix.Text + "." + Path.GetFileName(path));

                        if (!all)
                            if (File.Exists(class1check) || File.Exists(class2check))
                                continue;

                        Image img = Image.FromFile(path);
                        imgs.Images.Add(path, img);
                        ListViewItem item = new ListViewItem(path, Index);

                        if (File.Exists(class1check)) {
                            item.SubItems.Add(txtClass1Prefix.Text);
                            item.ForeColor = Class1Color;
                        }
                        else if (File.Exists(class2check)) {
                            item.SubItems.Add(txtClass2Prefix.Text);
                            item.ForeColor = Class2Color;
                        }
                        else
                            item.SubItems.Add("");

                        lvImages.Items.Add(item);
                        Index++;
                        prgLoad.PerformStep();
                    }
                    catch (Exception)
                    {
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Class1() {
            try
            {
                if (lvImages.SelectedItems.Count <= 0)
                    return;
                String destination;
                String oldPath;
                destination = Path.Combine(txtClass1Destination.Text, txtClass1Prefix.Text + "."+ Path.GetFileName(lvImages.SelectedItems[0].Text));

                oldPath = Path.Combine(txtClass2Destination.Text, txtClass2Prefix.Text + "." + Path.GetFileName(lvImages.SelectedItems[0].Text));

                if (File.Exists(oldPath)) {
                    File.Delete(oldPath);
                    score = 0;
                }

                try
                {
                    File.Copy(lvImages.SelectedItems[0].Text, destination);

                }
                catch (Exception ex )
                {
                    MessageBox.Show(ex.Message);
                }

                lvImages.SelectedItems[0].SubItems[1].Text = txtClass1Prefix.Text;
                lvImages.SelectedItems[0].ForeColor = Class1Color;
                lblTag.Text = txtClass1Prefix.Text;
                lblTag.ForeColor = Class1Color;
                score += 1;
                lblScore.Text = "Score " + score.ToString();
                if (score > TopScore)
                {
                    TopScore = score;
                    lblTopScore.Text = "Top Score " + TopScore.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Class2()
        {
            try
            {
                if (lvImages.SelectedItems.Count <= 0)
                    return;
                String destination;
                String oldPath;
                destination = Path.Combine(txtClass2Destination.Text, txtClass2Prefix.Text + "." + Path.GetFileName(lvImages.SelectedItems[0].Text));
                oldPath = Path.Combine(txtClass1Destination.Text, txtClass1Prefix.Text + "." + Path.GetFileName(lvImages.SelectedItems[0].Text));

                if (File.Exists(oldPath))
                {
                    File.Delete(oldPath);
                    score = 0;
                }

                try
                {
                    File.Copy(lvImages.SelectedItems[0].Text, destination);

                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                lvImages.SelectedItems[0].SubItems[1].Text = txtClass2Prefix.Text;
                lvImages.SelectedItems[0].ForeColor = Class2Color;
                lblTag.Text = txtClass2Prefix.Text;
                lblTag.ForeColor = Class2Color;
                score += 1;
                lblScore.Text = "Score " + score.ToString();
                if (score > TopScore) {
                    TopScore = score;
                    lblTopScore.Text = "Top Score " + TopScore.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmLabeler_KeyUp(object sender, KeyEventArgs e)
        {
            try
              {
                if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.Left)
                {
                    Class1();
                    if (lvImages.SelectedItems.Count > 0)
                    {
                        this.lvImages.Items[lvImages.SelectedItems[0].Index].Focused = false;
                        this.lvImages.Items[lvImages.SelectedItems[0].Index + 1].Focused = true;
                        this.lvImages.Items[lvImages.SelectedItems[0].Index + 1].Selected = true;
                        this.lvImages.Items[lvImages.SelectedItems[0].Index].Selected = false;
                    }
                }
                if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.Right)
                {
                    Class2();
                    if (lvImages.SelectedItems.Count > 0)
                    {
                        this.lvImages.Items[lvImages.SelectedItems[0].Index].Focused = false;
                        this.lvImages.Items[lvImages.SelectedItems[0].Index + 1].Focused = true;
                        this.lvImages.Items[lvImages.SelectedItems[0].Index + 1].Selected = true;
                        this.lvImages.Items[lvImages.SelectedItems[0].Index].Selected = false;
                    }
                }
                if (expanded) {
                    if (e.KeyCode == Keys.Up)
                    {
                        if (lvImages.SelectedItems.Count > 0)
                        {
                            if (lvImages.SelectedItems[0].Index > 0) {
                                this.lvImages.Items[lvImages.SelectedItems[0].Index - 1].Focused = true;
                                this.lvImages.Items[lvImages.SelectedItems[0].Index - 1].Selected = true;
                            }
                        }
                    }
                    if (e.KeyCode == Keys.Down)
                    {
                        if (lvImages.SelectedItems.Count > 0)
                        {
                            if (lvImages.SelectedItems[0].Index < lvImages.Items.Count - 1)
                            {
                                this.lvImages.Items[lvImages.SelectedItems[0].Index].Focused = false;
                                this.lvImages.Items[lvImages.SelectedItems[0].Index + 1].Focused = true;
                                this.lvImages.Items[lvImages.SelectedItems[0].Index + 1].Selected = true;
                                this.lvImages.Items[lvImages.SelectedItems[0].Index].Selected = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
           
        private void txtClass1Prefix_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtClass1Prefix.Text == "")
                {
                    lblClass1Destination.Text = "Class 1 Destination";
                    lblClass1Prefix.Text = "Class 1 Prefix";
                    btnClass1.Text = "Class 1 (Shortcut Key 1 / Left)";
                           }
                else
                {
                    lblClass1Destination.Text = txtClass1Prefix.Text + " Destination";
                    lblClass1Prefix.Text = txtClass1Prefix.Text + " Prefix";
                    btnClass1.Text = txtClass1Prefix.Text + " (Shortcut Key 1 / Left)";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtClass2Prefix_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtClass2Prefix.Text == "")
                {
                    lblClass2Destination.Text = "Class 2 Destination";
                    lblClass2Prefix.Text = "Class 2 Prefix";
                    btnClass2.Text = "Class 2 (Shortcut Key 2 / Right)";
                           }
                else
                {
                    lblClass2Destination.Text = txtClass2Prefix.Text + " Destination";
                    lblClass2Prefix.Text = txtClass2Prefix.Text + " Prefix";
                    btnClass2.Text = txtClass2Prefix.Text + " (Shortcut Key 2 / Right)";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExpand_Click(object sender, EventArgs e)
        {
            try
            {
                expanded = !expanded;
                pnlClass1Destination.Visible = !expanded;
                pnlClass1Prefix.Visible = !expanded;
                pnlClass2Destination.Visible = !expanded;
                pnlClass2Prefix.Visible = !expanded;
                pnlLoadImages.Visible = !expanded;
                prgLoad.Visible = !expanded;
                spltImages.Panel1Collapsed = expanded;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnFilterPending_Click(object sender, EventArgs e)
        {
            try
            {
                LoadImages(txtImagesPath.Text, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
