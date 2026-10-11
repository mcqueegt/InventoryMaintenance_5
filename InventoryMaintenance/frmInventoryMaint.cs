namespace InventoryMaintenance
{
    public partial class frmInventoryMaint : Form
    {
        public frmInventoryMaint()
        {
            InitializeComponent();
        }

        private List<InventoryItem> items = null!;

        private void frmInventoryMaint_Load(object sender, EventArgs e)
        {
            items = InventoryDB.GetItems();
            LoadComboBox();
            FillItemListBox();
        }

        private void LoadComboBox()
        {
            cboFilterBy.DataSource = new string[] {
                "All", "Under $10", "$10 to $50", "Over $50"
            };
        }
        //Gavin McQueen
        private void FillItemListBox()
        {
            lstItems.Items.Clear();

            string filter = cboFilterBy.SelectedValue?.ToString() ?? "";
            IEnumerable<InventoryItem> filteredItems = null!;

            // add items to the filteredItems collection based on FilterBy value'
            if (filter == "All")
            {
                filteredItems = items
                                    .OrderBy(item => item.Description);
            }
            else if (filter == "Under $10")
            {
                filteredItems = items.Where(item => item.Price < 10)
                                    .OrderBy(item => item.Description);
            }
            else if (filter == "$10 to $50")
            {
                filteredItems = items.Where(item => item.Price >= 10 && item.Price <= 50)
                                    .OrderBy(item => item.Description);
            }
            else if (filter == "Over $50")
            {
                filteredItems = items.Where(item => item.Price > 50)
                                    .OrderBy(item => item.Description);
            }

            // change code to loop the filteredItems collection
            foreach (InventoryItem item in filteredItems)
            {
                lstItems.Items.Add(item.GetDisplayText());
            }
        }
        //Gavin McQueen
        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmNewItem newItemForm = new();
            InventoryItem item = newItemForm.GetNewItem();
            if (item != null)
            {
                items.Add(item);
                InventoryDB.SaveItems(items);
                FillItemListBox();
            }
        }
        //Gavin McQueen
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int i = lstItems.SelectedIndex;

            if (i == -1)
            {
                MessageBox.Show("Please select an item to delete.", "No item selected");
            }
            else
            {
                InventoryItem item = items.FirstOrDefault(i => i.GetDisplayText() == lstItems.SelectedItem.ToString())!;

                string message = $"Are you sure you want to delete {item.Description}?";
                DialogResult result =
                    MessageBox.Show(message, "Confirm Delete",
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    items.Remove(item);
                    InventoryDB.SaveItems(items);
                    FillItemListBox();
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillItemListBox();
        }
    }
}