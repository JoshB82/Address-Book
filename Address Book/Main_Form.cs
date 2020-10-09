using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Address_Book
{
    public partial class Main_Form : Form
    {
        private List<Person> people = new List<Person>();

        public Main_Form()
        {
            InitializeComponent();

            // Read CSV list
            Properties.Settings.Default.file_path = "C:\\Users\\jbrya\\source\\repos\\Address Book\\Address Book\\Data.csv";
            string[] csv_data = File.ReadAllLines(Properties.Settings.Default.file_path);
            
            // Populate the list view (starts at 1 to ignore header)
            for (int i = 1; i < csv_data.Length; i++)
            {
                string[] line_content = csv_data[i].Split(',');
                people.Add(new Person(
                    int.Parse(line_content[0]),
                    line_content[1],
                    line_content[2],
                    line_content[3],
                    $"{line_content[4]},\r\n{line_content[5]},\r\n{line_content[6]},\r\n{line_content[7]},\r\n{line_content[8]},\r\n{line_content[9]}",
                    line_content[10],
                    line_content[11],
                    line_content[12],
                    line_content[13]
                ));

                AllDataList.Items.Add(new ListViewItem(line_content));
            }
        }

        private void AllDataList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (AllDataList.SelectedItems.Count == 0) return;

            // Get matching person
            int required_ID = Int32.Parse(AllDataList.SelectedItems[0].SubItems[0].Text);
            Person match = people.Single(x => x.ID == required_ID);

            // Extract required data and calculate age
            string name = $"{match.First_Name} {match.Surname}";

            int birth_year = Int32.Parse(match.DOB.Substring(6, 4));
            int birth_month = Int32.Parse(match.DOB.Substring(3, 2));
            int birth_day = Int32.Parse(match.DOB.Substring(0, 2));

            int now_year = DateTime.Now.Year;

            int birth_day_since_year_start = Number_Of_Days_Since_Now_Year_Start(new DateTime(now_year, birth_month, birth_day));
            int now_day_since_year_start = Number_Of_Days_Since_Now_Year_Start(DateTime.Now);

            int age = now_year - birth_year;
            if (birth_day_since_year_start - now_day_since_year_start > 0) age--;

            string dob = $"{match.DOB} (Age: {age})";
            string address = match.Address;
            string email = match.Email;
            string telephone_home = match.Telephone_Home;
            string telephone_mobile = match.Telephone_Mobile;
            string telephone_work = match.Telephone_Work;

            // Update fields
            details_text.Text =
                $"Name: {name}\r\n" +
                $"DOB: {dob}\r\n\r\n" +
                $"Address:\r\n" +
                $"{address}\r\n\r\n" +
                $"Email {email}\r\n\r\n" +
                $"Telephone:\r\n" +
                $"[Home] {telephone_home}\r\n" +
                $"[Mobile] {telephone_mobile}\r\n" +
                $"[Work] {telephone_work}";
        }

        private int Number_Of_Days_Since_Now_Year_Start(DateTime dt) => (int)Math.Floor((dt - new DateTime(DateTime.Now.Year, 1, 1)).TotalDays);

        private void Add_New_Entry_Menu_Button_Click(object sender, EventArgs e)
        {
            using (Form entry_form = new Entry_Form(AllDataList))
            {
                entry_form.ShowDialog();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Remove_Entry_Menu_Button_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to remove this entry?", "Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Remove from internal list
                people.RemoveAt(AllDataList.SelectedItems[0].Index);

                // Remove from csv file
                List<string> new_lines = new List<string>();

                new_lines.AddRange(File.ReadAllLines(Properties.Settings.Default.file_path));
                new_lines.RemoveAt(AllDataList.SelectedItems[0].Index + 1);
                File.WriteAllLines(Properties.Settings.Default.file_path, new_lines);

                // Remove from displayed list
                AllDataList.Items.RemoveAt(AllDataList.SelectedItems[0].Index);
            }
        }
    }

    public class Person
    {
        public int ID { get; set; }
        public string First_Name { get; set; }
        public string Surname { get; set; }
        public string DOB { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }
        public string Telephone_Home { get; set; }
        public string Telephone_Mobile { get; set; }
        public string Telephone_Work { get; set; }

        public Person
        (
            int id,
            string first_name,
            string surname,
            string dob,
            string address,
            string email,
            string telephone_home,
            string telephone_mobile,
            string telephone_work
        )
        {
            ID = id;
            First_Name = first_name;
            Surname = surname;
            DOB = dob;
            Address = address;
            Email = email;
            Telephone_Home = telephone_home;
            Telephone_Mobile = telephone_mobile;
            Telephone_Work = telephone_work;
        }
    }
}