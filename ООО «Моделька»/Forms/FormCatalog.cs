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

namespace ООО__Моделька_.Forms
{
    public partial class FormCatalog : Form
    {
        private BindingSource category;

        public FormCatalog()
        {
            InitializeComponent();
        }
        string name;
        public FormCatalog(string name)
        {
            InitializeComponent();
            this.name = name;
        }

        public string search;
        public static Byte[] picName;

        public void UpdateDataDrid()
        {
            dataGridViewData.Columns.Clear();
            dataGridViewData.Rows.Clear();

            using (LegoStoreEntities db = new LegoStoreEntities())
            {

                List<Set> set = db.Set.ToList();
                List<Category> category = db.Category.ToList();

                if (comboBoxMode.SelectedIndex == 0)
                {
                    SetVisibility(true);
                    DataGridViewTextBoxColumn dgvID;
                    dgvID = new DataGridViewTextBoxColumn();
                    dataGridViewData.Columns.Add(dgvID);
                    dgvID.Visible = false;

                    DataGridViewImageColumn dgvPhoto;
                    dgvPhoto = new DataGridViewImageColumn();
                    dataGridViewData.Columns.Add(dgvPhoto);
                    dgvPhoto.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvPhoto.Width = 300;
                    dgvPhoto.ImageLayout = DataGridViewImageCellLayout.Zoom;

                    DataGridViewTextBoxColumn dgvDesc;
                    dgvDesc = new DataGridViewTextBoxColumn();
                    dataGridViewData.Columns.Add(dgvDesc);

                    if(textBoxName.Text.Length != 0)
                        set = set.Where(x => x.SetName.Contains(textBoxName.Text)).ToList();


                    //Сортировка по категориям
                    if (comboBoxCategory.SelectedIndex != 0)
                    set = set.Where(x => (x.SetCategoryID == comboBoxCategory.SelectedIndex)).ToList();
                    

                    //Сортировка по цене
                    if (comboBoxCost.SelectedIndex == 0)
                        set = set.OrderByDescending(x => x.SetPrice).ToList();
                    else if (comboBoxCost.SelectedIndex == 1)
                        set = set.OrderBy(x => x.SetPrice).ToList();

                    //Сортировка по скидке
                    if (comboBoxDiscount.SelectedIndex == 0)
                        set = set.Where(x => x.SetDiscount <= 5 ).ToList();
                    else if (comboBoxDiscount.SelectedIndex == 1)
                        set = set.Where(x => x.SetDiscount > 5 && x.SetDiscount <= 15).ToList();
                    else if (comboBoxDiscount.SelectedIndex == 2)
                        set = set.Where(x => x.SetDiscount > 15 && x.SetDiscount <= 25).ToList();
                    else if (comboBoxDiscount.SelectedIndex == 3)
                        set = set.Where(x => x.SetDiscount > 25).ToList();

                    int i = 0;
                    foreach (var item in set)
                    {
                        dataGridViewData.Rows.Add();
                        dataGridViewData.Rows[i].Cells[0].Value = item.SetID;

                        picName = item.SetPicture;
                        if (picName == null)
                            dataGridViewData.Rows[i].Cells[1].Value = Properties.Resources.LEGO_logo;
                        else
                            dataGridViewData.Rows[i].Cells[1].Value = picName;
                        

                        dataGridViewData.Rows[i].Cells[2].Value =
                            $"Название набора: {item.SetName}" +
                            $"\nАртикль набора: {item.SetID}" +
                            $"\nКатегория: {item.Category.CategoryName}" +
                            $"\nОписание: {item.SetDiscription}" +
                            $"\nСтоимость набора: {item.SetPrice}" + " руб.";

                        if (item.SetDiscount != 0)
                            dataGridViewData.Rows[i].Cells[2].Value += 
                            $"\n Размер скидка: {item.SetDiscount}" + "%"+
                            $"\n Стоимость со скидкой: {item.SetPrice - (item.SetPrice/100*item.SetDiscount)}" + " руб.";
                        i++;
                    }

                    labelCount.Text = "Количество записей: " + set.Count;
                }
                else if (comboBoxMode.SelectedIndex == 1)
                {
                    SetVisibility(false);


                    DataGridViewTextBoxColumn dgvID = new DataGridViewTextBoxColumn();
                    dataGridViewData.Columns.Add(dgvID);
                    dataGridViewData.Columns[0].HeaderText = "ID заказа";

                    DataGridViewTextBoxColumn dgvSets = new DataGridViewTextBoxColumn();
                    dataGridViewData.Columns.Add(dgvSets);
                    dataGridViewData.Columns[1].HeaderText = "Список покупок";

                    DataGridViewTextBoxColumn dgvClients =  new DataGridViewTextBoxColumn();
                    dataGridViewData.Columns.Add(dgvClients);
                    dataGridViewData.Columns[2].HeaderText = "Покупатель";

                    DataGridViewTextBoxColumn dgManadgers = new DataGridViewTextBoxColumn();
                    dataGridViewData.Columns.Add(dgManadgers);
                    dataGridViewData.Columns[3].HeaderText = "Менеджер";

                    DataGridViewTextBoxColumn dgDate = new DataGridViewTextBoxColumn();
                    dataGridViewData.Columns.Add(dgDate);
                    dataGridViewData.Columns[4].HeaderText = "Дата продажи";

                    dataGridViewData.ColumnHeadersVisible = true;
                    DataGridViewCellStyle style = dataGridViewData.ColumnHeadersDefaultCellStyle;
                    style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    List<Order> order = db.Order.ToList();
                    List<Client> client = db.Client.ToList();
                    List<List> list = db.List.ToList();


                    

                    int i = 0;
                    foreach (var item in order)
                    {
                        
                        string orderedSets = "";
                        dataGridViewData.Rows.Add();
                        foreach(var item2 in list.Where(x => x.ListOrderID == item.OrderID).ToList())
                        {
                            orderedSets += " "+item2.ListSet;
                        }

                        dataGridViewData.Rows[i].Cells[0].Value = item.OrderID;
                        dataGridViewData.Rows[i].Cells[1].Value = orderedSets;
                        if (!item.OrderClientID.Equals(null))
                            dataGridViewData.Rows[i].Cells[2].Value = item.Client.ClientFullName;
                        else
                            dataGridViewData.Rows[i].Cells[2].Value = "Не заригистрирован";
                        dataGridViewData.Rows[i].Cells[3].Value = item.Employee.EmployeeFullName;
                        dataGridViewData.Rows[i].Cells[4].Value = item.OrderDate;

                        i++;
                    }

                    labelCount.Text = "Количество записей: " + order.Count;
                }
            }
        }
        private void SetVisibility(bool b)
        {
            labelName.Visible = b;
            labelCategory.Visible = b;
            labelCost.Visible = b;
            labelDiscount.Visible = b;
            textBoxName.Visible = b;
            comboBoxDiscount.Visible = b;
            comboBoxCategory.Visible = b;
            comboBoxCost.Visible = b;
            comboBoxDiscount.Visible = b;
        }

        private void FormCatalog_Load(object sender, EventArgs e)
        {


            comboBoxMode.SelectedIndex = 0;
            using (LegoStoreEntities db = new LegoStoreEntities())
            {


                List<Category> categories = db.Category.ToList();
                category = new BindingSource() { DataSource = categories };
                comboBoxCategory.DataSource = category;
                comboBoxCategory.DisplayMember = "CategoryName";
                var newCat = new Category() { CategoryName = "Все" };
                comboBoxCategory.ValueMember = "CategoryID";
                category.Insert(0, newCat);
                comboBoxCategory.SelectedIndex = 0;

                dataGridViewData.Rows.Clear();
                UpdateDataDrid();
            }

            
            
            switch (Helper.idRole)
            {
                case -1:
                    Text = "Каталог (гость)";
                    return;

                case 1:
                    comboBoxMode.Visible=true;
                    tableLayoutPanelButtons.Visible = true;
                    Text = "Каталог (администратор)";
                    return;
                case 2:
                    comboBoxMode.Visible=true;
                    this.Text = "Каталог (менеджер)";
                    return;
                case 3:
                    labelWelcome.Visible = true;
                    labelClientName.Visible = true;
                    labelClientName.Text = name;
                    this.Text = "Каталог (клиент)";
                    return;
            }

        }

        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDataDrid();
        }

        private void comboBoxData_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDataDrid(); 
        }

        private void comboBoxCost_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDataDrid();
        }

        private void comboBoxDiscount_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDataDrid();
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            UpdateDataDrid();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            int index = dataGridViewData.CurrentRow.Index;

            if (index >= 0)
            {
                FormEdit formEdit = new FormEdit(dataGridViewData.Rows[index].Cells[0].Value.ToString());
                Hide();
                formEdit.ShowDialog();
                Show();
                UpdateDataDrid();
            }
            else
            {
                MessageBox.Show("Выбереите Товар");
            }
        }

        private void buttonAddNew_Click(object sender, EventArgs e)
        {
            FormEdit formEdit = new FormEdit();
            Hide();
            formEdit.ShowDialog();
            Show();
            UpdateDataDrid();

        }
    }
}
