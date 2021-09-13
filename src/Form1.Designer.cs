using System;
using System.Drawing;
using System.Windows.Forms;

namespace HomeworkPlanner
{
    partial class Form1
    {
        public static Button done;
        public static ComboBox subjectsBox;
        public static TextBox homeworkText;
        public static DateTimePicker dateTimePicker;
        public ListView homeworkList;
        public ListView pastHomeworkList;
        public SplitContainer splitContainer;
        public static TableLayoutPanel timetablePanel;
        public static TextBox username;
        public static TextBox password;
        private int formBaseWidth = (int)(800 * 1.5);
        private int formBaseHeight = (int)(450 * 1.5);
        private string formName = "Hausaufgabenplaner";
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code




        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            // this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(formBaseWidth, formBaseHeight);
            this.Text = formName;
            this.MinimumSize = new Size(formBaseWidth, formBaseHeight);
            // this.MaximumSize = new Size(formBaseWidth, formBaseHeight);
            // AutoScaleMode = AutoScaleMode.Dpi;
        }

        #endregion


        private void InitializeComponents()
        {
            splitContainer = new SplitContainer();
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Name = "splitContainerIDK";
            splitContainer.BorderStyle = BorderStyle.FixedSingle;


            // subject box
            subjectsBox = new ComboBox();
            subjectsBox.DataSource = Subjects.GetSubjects();
            subjectsBox.DropDownStyle = ComboBoxStyle.DropDownList;
            subjectsBox.Dock = DockStyle.Top;


            homeworkText = new TextBox();
            homeworkText.PlaceholderText = "Hausaufgabe";
            homeworkText.Multiline = true;
            homeworkText.MinimumSize = new Size(0, 100);
            homeworkText.Dock = DockStyle.Top;


            dateTimePicker = new DateTimePicker();
            dateTimePicker.Dock = DockStyle.Left;
            dateTimePicker.MinDate = DateTime.Now;
            dateTimePicker.Size = new Size(150, 50);
            dateTimePicker.CustomFormat = "dddd d.M";
            dateTimePicker.Format = DateTimePickerFormat.Custom;


            Button saveButton = new Button();
            saveButton.FlatStyle = FlatStyle.Flat;
            saveButton.FlatAppearance.BorderSize = 0;
            saveButton.Text = "Speichern";
            saveButton.Dock = DockStyle.Top;
            saveButton.Size = new Size(80, 30);
            saveButton.Click += SaveButtonPressed;


            done = new Button();
            done.Dock = DockStyle.Top;
            done.Text = "Erledigt!";
            done.Size = new Size(80, 30);
            done.Enabled = false;
            done.Click += OnDoneClick;


            Button clear = new Button();
            clear.FlatStyle = FlatStyle.Flat;
            clear.FlatAppearance.BorderSize = 0;
            clear.Text = "Neue Aufgabe";
            clear.Dock = DockStyle.Top;
            clear.Size = new Size(80, 30);
            clear.Click += ResetValues;


            Panel p = new Panel();
            p.Controls.Add(clear);
            p.Controls.Add(done);
            p.Controls.Add(saveButton);
            p.BackColor = Color.LightGray;
            p.Dock = DockStyle.Top;
            p.Size = new Size(0, 90);
            splitContainer.Panel1.Controls.Add(p);


            splitContainer.Panel1.Controls.Add(dateTimePicker);
            splitContainer.Panel1.Controls.Add(homeworkText);
            splitContainer.Panel1.Controls.Add(subjectsBox);

            ////////////

            homeworkList = new ListView();
            homeworkList.Dock = DockStyle.Fill;
            homeworkList.Scrollable = true;
            homeworkList.View = View.Details;
            homeworkList.Click += OnListSelected;
            homeworkList.Columns.Add("Datum", 125, HorizontalAlignment.Center);
            homeworkList.Columns.Add("Erledigt", 100, HorizontalAlignment.Center);
            homeworkList.Columns.Add("Fach", 120, HorizontalAlignment.Center);
            homeworkList.Columns.Add("Nachricht", -2, HorizontalAlignment.Left);
            homeworkList.FullRowSelect = true;
            homeworkList.LabelEdit = false;
            homeworkList.LabelWrap = true;
            homeworkList.ListViewItemSorter = new ListSorter();

            splitContainer.Panel2.Controls.Add(homeworkList);


            pastHomeworkList = new ListView();
            pastHomeworkList.BackColor = Color.LightGray;
            pastHomeworkList.Dock = DockStyle.Bottom;
            pastHomeworkList.Size = new Size(0, 150);
            pastHomeworkList.Scrollable = true;
            pastHomeworkList.View = View.Details;
            // pastHomeworkList.Click += OnListSelected;
            pastHomeworkList.Columns.Add("Datum", 125, HorizontalAlignment.Center);
            pastHomeworkList.Columns.Add("Erledigt", 100, HorizontalAlignment.Center);
            pastHomeworkList.Columns.Add("Fach", 120, HorizontalAlignment.Center);
            pastHomeworkList.Columns.Add("Nachricht", -2, HorizontalAlignment.Left);
            pastHomeworkList.LabelEdit = false;
            pastHomeworkList.LabelWrap = true;
            pastHomeworkList.FullRowSelect = true;


            ListSorter ls = new ListSorter();
            ls.reverse = true;
            pastHomeworkList.ListViewItemSorter = ls;
            splitContainer.Panel2.Controls.Add(pastHomeworkList);
            TabPage tp1 = new TabPage();
            tp1.Text = "Hausaufgaben";
            tp1.Controls.Add(splitContainer);
            TabPage tp2 = new TabPage();
            tp2.Text = "Einstellungen";

            Panel p2 = new Panel();

            p2.Dock = DockStyle.Left;
            p2.Width = 300;
            p2.Height = Subjects.baseSubjectColors.Keys.Count * 30;
            foreach (string key in Subjects.baseSubjectColors.Keys)
            {
                Button colorButton = new Button();
                colorButton.Text = Subjects.GetSubjectByAcronym(key);
                colorButton.BackColor = Subjects.GetColorBySubjectAcronym(key);
                if (colorButton.BackColor.GetBrightness() < .33f)
                {
                    colorButton.ForeColor = Color.White;
                }
                colorButton.FlatStyle = FlatStyle.Flat;
                colorButton.FlatAppearance.BorderSize = 0;
                colorButton.Tag = key;
                colorButton.Height = 30;
                colorButton.Click += Subjects.ColorButtonClick;
                colorButton.Dock = DockStyle.Top;
                p2.Controls.Add(colorButton);
            }

            Panel p3 = new Panel();
            p3.Dock = DockStyle.Left;
            p3.Width = 300;
            p3.Height = 150;

            username = new TextBox();
            username.Dock = DockStyle.Top;
            username.PlaceholderText = "Portal Nutzername";
            username.Height = 50;
            username.TabIndex = 1;

            password = new TextBox();
            password.Dock = DockStyle.Top;
            password.PlaceholderText = "Portal Passwort";
            password.Height = 50;
            password.UseSystemPasswordChar = true;
            password.TabIndex = 2;
            password.KeyDown += OnPasswordKeyDown;

            Button portalSubmitButton = new Button();
            portalSubmitButton.Dock = DockStyle.Top;
            portalSubmitButton.FlatStyle = FlatStyle.Flat;
            portalSubmitButton.Text = "Stundenplan speichern / aktualisieren";
            portalSubmitButton.FlatAppearance.BorderSize = 1;
            portalSubmitButton.Height = 50;
            portalSubmitButton.Click += Timetable.OnLoginCredentialsSubmitButtonClick;
            portalSubmitButton.TabIndex = 3;

            p3.Controls.Add(portalSubmitButton);
            p3.Controls.Add(password);
            p3.Controls.Add(username);


            tp2.Controls.Add(p3);
            tp2.Controls.Add(p2);

            timetablePanel = new TableLayoutPanel();
            timetablePanel.BackColor = Subjects.GetColorBySubjectAcronym("Freistunde");
            // timetablePanel.ColumnCount = Timetable.timetableWidth;
            // timetablePanel.RowCount = Timetable.timetableHeight;
            timetablePanel.Dock = DockStyle.Bottom;
            timetablePanel.Height = (int)(splitContainer.Panel1.Height / 2);
            splitContainer.Panel1.Controls.Add(timetablePanel);
            // splitContainer.Panel1.Resize += OnPanel1Resize;
            // tp2.Controls.Add(timeTablePanel);
            TabControl tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;
            tabControl.Controls.Add(tp1);
            tabControl.Controls.Add(tp2);
            Controls.Add(tabControl);
            splitContainer.Panel1.Resize += TimetableResize;
            // ResizeEnd += TimetableResize;

        }
        private void TimetableResize(object sender, EventArgs e)
        {
            Console.WriteLine("a");
            timetablePanel.Height = (int)(splitContainer.Panel1.Height / 2);
            timetablePanel.Width = splitContainer.Panel1.Width;
            Timetable.ResizeLabels(timetablePanel);
        }
        protected override void WndProc(ref Message m)
        {
            FormWindowState org = this.WindowState;
            base.WndProc(ref m);
            if (this.WindowState != org)
                this.OnFormWindowStateChanged(EventArgs.Empty);
        }

        protected virtual void OnFormWindowStateChanged(EventArgs e)
        {
            TimetableResize(null, null);
        }
        private void OnPasswordKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Timetable.OnLoginCredentialsSubmitButtonClick(sender, e);
        }
    }
}

