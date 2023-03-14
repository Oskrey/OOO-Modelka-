using ООО__Моделька_.Classes;
using ООО__Моделька_.Forms;
using ООО__Моделька_.Entity;
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

namespace ООО__Моделька_.Forms
{
    public partial class FormEdit : Form
    {
        public FormEdit()
        {
            InitializeComponent();
        }
        string id;
        public FormEdit(string id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (textBoxSetID.Text.Length != 0 && textBoxSetName.Text.Length != 0 && textBoxSetDescription.Text.Length != 0 && textBoxPrice.Text.Length != 0 && textBoxMaxDiscount.Text.Length != 0 && textBoxDiscount.Text.Length != 0)
            {
                using (LegoStoreEntities db = new LegoStoreEntities())
                {
                    if (id != null)
                    {

                        var set = db.Set.Where(x => x.SetID == id).FirstOrDefault();
                        set.SetName = textBoxSetName.Text;
                        set.SetDiscription = textBoxSetDescription.Text;
                        set.SetPrice = Convert.ToDouble(textBoxPrice.Text);
                        set.SetCategoryID = comboBoxCategory.SelectedIndex + 1;
                        set.SetMaxDiscount = Convert.ToInt32(textBoxMaxDiscount.Text);
                        set.SetDiscount = Convert.ToInt32(textBoxDiscount.Text);
                        MemoryStream memoryStream = new MemoryStream();
                        pictureBoxSetImage.Image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        set.SetPicture = memoryStream.ToArray();
                        db.SaveChanges();
                        MessageBox.Show("Данные обновлены успешно");
                        Close();

                    }
                    else
                    {
                        Set set = new Set();
                        {
                            set.SetID = textBoxSetID.Text;
                            set.SetCategoryID = comboBoxCategory.SelectedIndex+1;
                            set.SetName = textBoxSetName.Text;
                            set.SetDiscription = textBoxSetDescription.Text;
                            set.SetPrice = Convert.ToDouble(textBoxPrice.Text);
                            set.SetMaxDiscount = Convert.ToInt32(textBoxMaxDiscount.Text);
                            set.SetDiscount = Convert.ToInt32(textBoxDiscount.Text);
                            MemoryStream memoryStream = new MemoryStream();
                            pictureBoxSetImage.Image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                            set.SetPicture = memoryStream.ToArray();
                        };
                        db.Set.Add(set);
                        db.SaveChanges();
                        MessageBox.Show("Данные добавлены успешно");
                        Close();

                    }
                }
            }
            else MessageBox.Show("Введите полную информацию");

        }

        private void FormEdit_Load(object sender, EventArgs e)
        {
            using (LegoStoreEntities db = new LegoStoreEntities())
            {
                BindingSource category;
                List<Category> categories = db.Category.ToList();
                category = new BindingSource() { DataSource = categories };
                comboBoxCategory.DataSource = category;
                comboBoxCategory.DisplayMember = "CategoryName";
                
                comboBoxCategory.SelectedIndex = 0;
            }
            if (id != null)
            {
                using (LegoStoreEntities db = new LegoStoreEntities())
                {

                    

                    List<Set> set = db.Set.ToList();
                    set = set.Where(x => x.SetID.Contains(id)).ToList();

                    foreach (var item in set)
                    {
                        MemoryStream ms = new MemoryStream(item.SetPicture);
                        pictureBoxSetImage.Image = Image.FromStream(ms);
                        textBoxSetID.Text = id;
                        textBoxSetName.Text = item.SetName;
                        textBoxSetDescription.Text = item.SetDiscription;
                        textBoxPrice.Text = item.SetPrice.ToString();
                        textBoxMaxDiscount.Text = item.SetMaxDiscount.ToString();
                        textBoxDiscount.Text = item.SetDiscount.ToString();
                        comboBoxCategory.SelectedIndex = item.SetCategoryID-1;


                    }

                }
            }
            else textBoxSetID.Enabled = true;
        }

        private void buttonAddPicture_Click(object sender, EventArgs e)
        {
            openFileDialogPic.ShowDialog();
            if(openFileDialogPic.FileName != null)
            { pictureBoxSetImage.Image = Image.FromFile(openFileDialogPic.FileName); }
        }

        private void buttonDeletePicture_Click(object sender, EventArgs e)
        {
            pictureBoxSetImage.Image = Properties.Resources.LEGO_logo;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
