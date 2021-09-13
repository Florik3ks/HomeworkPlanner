using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace HomeworkPlanner
{
    public partial class Form1 : Form
    {

        private static bool isEditing;
        private static Assignment currentlyEditing;
        public Form1()
        {
            Shown += OnShown;
            Subjects.LoadSubjectColors();
            InitializeComponent();
            InitializeComponents();
            isEditing = false;

        }
        private void OnShown(object sender, EventArgs e)
        {
            Config.LoadConfig();
            // get local timetable on startup
            Timetable.GetTimetable(timetablePanel, false);
            TimetableResize(null, null);
            // load assignments
            Homework.LoadAssignments();
            for (int i = Homework.AssignmentList.Count - 1; i >= 0; i--)
            {
                AddAssignmentToList(Homework.AssignmentList[i], false);
            }
            Refresh();
            // update timetable from portal
            BackgroundWorker bgw = new BackgroundWorker();
            bgw.DoWork += LoadTimetable;
            bgw.RunWorkerCompleted += LoadTimetableCompleted;
            bgw.RunWorkerAsync();
        }
        private void LoadTimetable(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Timetable.GetTimetable(timetablePanel, true);
        }
        private void LoadTimetableCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            TimetableResize(null, null);
        }

        public void SaveButtonPressed(object sender, EventArgs e)
        {
            if (!isEditing) // save new assignment
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
            else // edit currently selected assignment
            {
                currentlyEditing.DueDate = dateTimePicker.Value;
                currentlyEditing.Message = homeworkText.Text == "" ? homeworkText.PlaceholderText : homeworkText.Text;
                currentlyEditing.Subject = subjectsBox.SelectedItem.ToString();
                Homework.EditAssignment(currentlyEditing);
                UpdateItem(currentlyEditing);
                ResetValues();
            }
        }
        public void ResetValues(object sender, EventArgs e)
        {
            ResetValues();
        }


        public static void SetValues(DateTime duedate, string subject)
        {
            ResetValues();
            dateTimePicker.Value = duedate;
            subjectsBox.SelectedIndex = subjectsBox.FindStringExact(subject);
        }
        public static void ResetValues()
        {
            dateTimePicker.Value = DateTime.Now;
            subjectsBox.SelectedIndex = 0;
            homeworkText.Text = "";
            done.Enabled = false;
            isEditing = false;
            currentlyEditing = null;
        }
        public void OnDoneClick(object sender, EventArgs e)
        {
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
        public void AddAssignmentToList(Assignment a, bool refresh = true)
        {

            ListView listToAddTo = homeworkList;
            if (a.DueDate < DateTime.Now)
            {
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
            if (refresh) Refresh();
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
