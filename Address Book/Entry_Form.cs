using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Address_Book
{
    public partial class Entry_Form : Form
    {
        private ListView list_view;

        public Entry_Form(ListView listview)
        {
            list_view = listview;
            InitializeComponent();
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            // Check all fields
            string[] field_data = new string[14]
            {
                "id",
                First_Name_Box.Text.Trim(),
                Surname_Box.Text.Trim(),
                DOB_Picker.Value.ToString("dd/MM/yyyy"),
                Address_Line_1_Box.Text.Trim(),
                Address_Line_2_Box.Text.Trim(),
                Address_Line_3_Box.Text.Trim(),
                Town_City_Box.Text.Trim(),
                Country_Box.Text.Trim(),
                Postcode_Box.Text.Trim(),
                Email_Box.Text.Trim(),
                Telephone_Home_Box.Text.Trim(),
                Telephone_Mobile_Box.Text.Trim(),
                Telephone_Work_Box.Text.Trim()
            };

            bool pass = true;
            foreach (string data in field_data)
            {
                if (string.IsNullOrEmpty(data))
                {
                    pass = false;
                    break;
                }
            }

            if (pass)
            {
                this.Close();
                this.Dispose();

                // Add new person to .csv file

                // Get new ID
                string file_path = "C:\\Users\\jbrya\\source\\repos\\Address Book\\Address Book\\Data.csv";
                field_data[0] = File.ReadAllLines(file_path).Length.ToString();

                File.AppendAllText(file_path, string.Join(',', field_data));

                list_view.Items.Add(new ListViewItem(field_data));

                MessageBox.Show("Successfully added the new entry to the address book.", "Success", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("One or more fields are invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}