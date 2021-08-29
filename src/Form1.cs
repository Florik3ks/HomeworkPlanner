using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HomeworkPlanner
{
    public partial class Form1 : Form
    {
        bool isEditing;
        Assignment currentlyEditing;
        public Form1()
        {
            InitializeComponent();
            InitializeComponents();
            Homework.LoadAssignments();
            Console.WriteLine(Homework.AssignmentList.Count);
            foreach (Assignment a in Homework.AssignmentList)
            {
                AddAssignmentToList(a);
            }
            isEditing = false;

        }

        public void SaveButtonPressed(object sender, EventArgs e)
        {
            if (!isEditing)
            {
                Assignment a = new Assignment(
                    dateTimePicker.Value,
                    subjectsBox.SelectedItem.ToString(),
                    homeworkText.Text == "" ? homeworkText.PlaceholderText : homeworkText.Text
                );
                Homework.AddAssignment(a);
                AddAssignmentToList(a);

                ResetValues();
            }
            else
            {
                Homework.EditAssignment(currentlyEditing);
                UpdateItem(currentlyEditing);
                ResetValues();
            }
        }
        public void ResetValues(object sender, EventArgs e){
            ResetValues();
        }
        public void ResetValues()
        {
            dateTimePicker.Value = DateTime.Now;
            subjectsBox.SelectedIndex = 0;
            homeworkText.Text = "";
            done.Enabled = false;
            isEditing = false;
            currentlyEditing = null;
        }
        public void OnDoneClick(object sender, EventArgs e){
            currentlyEditing.SetDone();
            Homework.EditAssignment(currentlyEditing);
            UpdateItem(currentlyEditing);
            ResetValues();
        }
        public void UpdateItem(Assignment a)
        {
            homeworkList.Items.RemoveByKey(a.id);
            AddAssignmentToList(a);
        }
        public void AddAssignmentToList(Assignment a)
        {
            #region idk
            // listView.Controls.Add()
            // GroupBox g = new GroupBox();
            // TextBox date = new TextBox();
            // TextBox text = new TextBox();
            // TextBox subject = new TextBox();
            // PictureBox pb = new PictureBox();
            // pb.BackColor = a.Done ? Color.Green : Color.Red;
            // pb.Dock = DockStyle.Top;

            // date.Enabled = false;
            // date.Dock = DockStyle.Top;
            // date.Text = FormateDateToString(a.DueDate);

            // text.Text = a.Message;
            // text.Dock = DockStyle.Top;
            // text.Enabled = false;
            // text.Multiline = true;

            // subject.Text = a.Subject;
            // subject.Enabled = false;
            // subject.Dock = DockStyle.Top;

            // g.Controls.Add(pb);
            // g.Controls.Add(text);
            // g.Controls.Add(subject);
            // g.Controls.Add(date);

            // g.Dock = DockStyle.Top;

            // listView.Columns.Add(g);
            #endregion

            ListView listToAddTo = homeworkList;
            if(a.DueDate < DateTime.Now){
                listToAddTo = pastHomeworkList;
            }

            ListViewItem lwi = new ListViewItem(FormateDateToString(a.DueDate));

            lwi.Name = a.id;
            lwi.UseItemStyleForSubItems = false;
            ListViewItem.ListViewSubItem sub = new();
            sub.BackColor = a.Done ? Color.Green : Color.Red;
            lwi.SubItems.Add(sub);


            ListViewItem.ListViewSubItem sub1 = new();
            sub1.Text = a.Subject;
            lwi.SubItems.Add(sub1);
            ListViewItem.ListViewSubItem sub2 = new();
            sub2.Text = a.Message;
            lwi.SubItems.Add(sub2);
            listToAddTo.Items.Add(lwi);
            Refresh();
        }

        private void OnListSelected(object sender, EventArgs e)
        {
            ListViewItem firstSelectedItem = homeworkList.SelectedItems[0];

            Assignment a = Homework.GetById(firstSelectedItem.Name);
            isEditing = true;
            done.Enabled = true;
            dateTimePicker.Value = a.DueDate;
            subjectsBox.SelectedIndex = subjectsBox.FindStringExact(a.Subject);
            homeworkText.Text = a.Message;
            currentlyEditing = a;

        }

        public string FormateDateToString(DateTime d)
        {
            return d.DayOfWeek.ToString()
            .Replace("Monday", "Montag")
            .Replace("Tuesday", "Dienstag")
            .Replace("Wednesday", "Mittwoch")
            .Replace("Thursday", "Donnerstag")
            .Replace("Friday", "Freitag")
            .Replace("Saturday", "Samstag")
            .Replace("Sunday", "Sonntag")
             + ", " + d.Day.ToString() + "." + d.Month.ToString();
        }

        public void OrderList()
        {
            // List<> SortedList = objListOrder.OrderBy(o=>o.OrderDate).ToList();
        }

    }
}
