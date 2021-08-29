using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HomeworkPlanner
{
    partial class Form1
    {
        public Button done;
        public ComboBox subjectsBox;
        public TextBox homeworkText;
        public DateTimePicker dateTimePicker;
        public ListView homeworkList;
        public ListView pastHomeworkList;
        private int formBaseWidth = 800;
        private int formBaseHeight = 450;
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
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(formBaseWidth, formBaseHeight);
            this.Text = formName;
            this.MinimumSize = new Size(formBaseWidth, formBaseHeight);
            // this.MaximumSize = new Size(formBaseWidth, formBaseHeight);
        }

        #endregion


        private void InitializeComponents()
        {
            SplitContainer splitContainer = new SplitContainer();
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
            // dateTimePicker.ShowUpDown = true;


            Button saveButton = new Button();
            saveButton.Text = "Speichern";
            saveButton.Dock = DockStyle.Top;
            saveButton.Size = new Size(80, 24);
            saveButton.Click += SaveButtonPressed;

            done = new Button();
            done.Dock = DockStyle.Top;
            done.Text = "Erledigt!";
            done.Enabled = false;
            done.Click += OnDoneClick;

            Button clear = new Button();
            clear.Text = "Neue Aufgabe";
            clear.Dock = DockStyle.Top;
            clear.Click += ResetValues;

            splitContainer.Panel1.Controls.Add(clear);
            splitContainer.Panel1.Controls.Add(done);
            splitContainer.Panel1.Controls.Add(saveButton);
            splitContainer.Panel1.Controls.Add(dateTimePicker);
            splitContainer.Panel1.Controls.Add(homeworkText);
            splitContainer.Panel1.Controls.Add(subjectsBox);

            ////////////

            homeworkList = new ListView();
            homeworkList.Dock = DockStyle.Fill;
            homeworkList.Scrollable = true;
            homeworkList.View = View.Details;
            homeworkList.Click += OnListSelected;
            homeworkList.Columns.Add("Datum", 100, HorizontalAlignment.Center);
            homeworkList.Columns.Add("Erledigt", 100, HorizontalAlignment.Center);
            homeworkList.Columns.Add("Fach", 100, HorizontalAlignment.Center);
            homeworkList.Columns.Add("Nachricht", -2, HorizontalAlignment.Center);
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
            pastHomeworkList.Columns.Add("Datum", 100, HorizontalAlignment.Center);
            pastHomeworkList.Columns.Add("Erledigt", 100, HorizontalAlignment.Center);
            pastHomeworkList.Columns.Add("Fach", 100, HorizontalAlignment.Center);
            pastHomeworkList.Columns.Add("Nachricht", -2, HorizontalAlignment.Center);
            pastHomeworkList.LabelEdit = false;
            pastHomeworkList.LabelWrap = true;
            pastHomeworkList.FullRowSelect = true;
            ListSorter ls = new ListSorter();
            ls.reverse = true;
            pastHomeworkList.ListViewItemSorter = ls;
            splitContainer.Panel2.Controls.Add(pastHomeworkList);

            Controls.Add(splitContainer);
        }
    }
}

