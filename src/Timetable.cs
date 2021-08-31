using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using HtmlAgilityPack;
using System.Windows.Forms;

namespace HomeworkPlanner
{
    public static class Timetable
    {
        public const int timetableHeight = 11;
        public const int timetableWidth = 5;
        public static string[,] timetable;
        public static void GetTimetable(TableLayoutPanel timetablePanel)
        {
            LoadPlan();
            ShowPlan(timetablePanel);
        }
        public static void ShowPlan(TableLayoutPanel timetablePanel)
        {
            timetablePanel.Controls.Clear();
            Console.WriteLine("a");
            for (int lesson = 0; lesson < timetable.GetLength(1); lesson++)
            {
                for (int day = 0; day < timetable.GetLength(0); day++)
                {
                    Label label = new Label();
                    label.Margin = new Padding(0);
                    label.Dock = DockStyle.Left;
                    label.BorderStyle = BorderStyle.FixedSingle;
                    label.Height = (int)(label.Height * 1.275);
                    label.Width = (int)(label.Height * 2.75);
                    label.FlatStyle = FlatStyle.Popup;
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.Tag = (day, lesson);
                    string subject = timetable[day, lesson].Replace(" ", "").Replace("\n", "").Split("_")[0];
                    label.BackColor = Subjects.GetColorBySubjectAcronym(subject);
                    if (!(timetable[day, lesson] == "-"))
                    {
                        label.Text = subject;
                        label.MouseHover += OnTimeTableLessonHover;
                    }
                    timetablePanel.Controls.Add(label);
                }
            }
        }
        public static void OnTimeTableLessonHover(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            Label label = (Label)sender;
            int day, lesson;
            (day, lesson) = ((int, int))label.Tag;
            string subject = Subjects.GetSubjectByAcronym(timetable[day, lesson].Replace(" ", "").Replace("\n", "").Split("_")[0]);
            string[] baseText = timetable[day, lesson].Replace(" ", "").Replace("\n\n", "").Trim().Split("_");
            baseText[0] = subject;
            toolTip.SetToolTip(label, String.Join("_", baseText));
        }

        public static void OnLoginCredentialsSubmitButtonClick(object sender, EventArgs e){

            Portal.HasPortalLoginDetails = true;
            LoadPlan(true, Form1.username.Text, Form1.password.Text);
            ShowPlan(Form1.timetablePanel);
            if(timetable.Length > 0){
                MessageBox.Show("Stundenplan erfolgreich geladen!");
            }
        }
        public static void LoadPlan(bool reloadFromPortal = false, string username = "", string password = "")
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Florian\\HomeworkPlanner\\";
            if (reloadFromPortal || !File.Exists(path + "timetable.json"))
            {
                if (Portal.HasPortalLoginDetails)
                {
                    timetable = Portal.GetPlan(username, password);
                    if (timetable.Length > 0)
                    {
                        SavePlan();
                    }
                    return;
                }
                timetable = new string[0, 0];
                return;
            }
            string jsonString = File.ReadAllText(path + "timetable.json");
            TimetableDummyClass tbc = JsonSerializer.Deserialize<TimetableDummyClass>(jsonString);
            timetable = tbc.GetNormalTimetable();
        }
        public static void SavePlan()
        {
            TimetableDummyClass tdc = new TimetableDummyClass(timetable);

            string jsonString = JsonSerializer.Serialize(tdc);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Florian\\HomeworkPlanner\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.WriteAllText(path + "timetable.json", jsonString);
        }
    }
    public class TimetableDummyClass{
        public int Height{ get; set; }
        public int Width{ get; set; }
        public string[] Values{ get; set; }
        public TimetableDummyClass(){}
        public TimetableDummyClass(string[,] normalTimetable){
            Height = normalTimetable.GetLength(1);
            Width = normalTimetable.GetLength(0);
            Values = new string[Height * Width];
            int i = 0;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Values[i] = normalTimetable[x,y];
                    i++;
                }
            }
        }
        public string[,] GetNormalTimetable(){
            string[,] normal = new string[Width, Height];
            int i = 0;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    normal[x,y] = Values[i];
                    i++;
                }
            }
            return normal;
        }
    }
}