using System;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace HomeworkPlanner
{
    public static class Timetable
    {
        public const int timetableBaseHeight = 11;
        public const int timetableBaseWidth = 5;
        public static string[,] timetable;
        private static bool hasTimetableChangedSinceRedraw = false;
        public enum loadingType
        {
            reloadFromPortal, loadFromFile
        };
        public static void GetTimetable(TableLayoutPanel timetablePanel, bool loadFromPortal = false)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + FileManager.path;
            if (loadFromPortal/* || !File.Exists(path + FileManager.fileNames["Timetable"] + FileManager.fileNames["ext"])*/)
            {
                LoadPlan(loadingType.reloadFromPortal, false, Config.portalUsername, Config.portalPassword);
            }
            else
            {
                LoadPlan(loadingType.loadFromFile, false, Config.portalUsername, Config.portalPassword);
            }
            ShowPlan(timetablePanel);
        }
        public static void ShowPlan(TableLayoutPanel timetablePanel)
        {
            if (timetable == null) return;
            if (!hasTimetableChangedSinceRedraw)
            {
                Console.WriteLine("no");
                return;
            }
            hasTimetableChangedSinceRedraw = false;
            if (timetablePanel.InvokeRequired)
            {
                // we aren't on the UI thread. Ask the UI thread to do stuff.
                timetablePanel.Invoke(new Action(() =>
                {
                    timetablePanel.Controls.Clear();
                    timetablePanel.RowCount = timetable.GetLength(1);
                    timetablePanel.ColumnCount = timetable.GetLength(0);
                    if (timetablePanel.ColumnCount > 0 && Config.showTimetableLessonTimes) timetablePanel.ColumnCount++;
                }));
            }
            else
            {
                // we are on the UI thread. We are free to touch things.
                timetablePanel.Controls.Clear();
                timetablePanel.RowCount = timetable.GetLength(1);
                timetablePanel.ColumnCount = timetable.GetLength(0);
                if (timetablePanel.ColumnCount > 0 && Config.showTimetableLessonTimes) timetablePanel.ColumnCount++;
            }


            for (int lesson = 0; lesson < timetable.GetLength(1); lesson++)
            {
                for (int day = -1; day < timetable.GetLength(0); day++)
                {
                    Label label = new Label();
                    label.Dock = DockStyle.Left;
                    label.FlatStyle = FlatStyle.Popup;
                    label.Margin = new Padding(0);

                    // label.BorderStyle = BorderStyle.FixedSingle;
                    // label.Height = (int)(label.Height * 1.275);
                    // label.Width = (int)(label.Height * 2.75);
                    label.Height = timetablePanel.Height / timetablePanel.RowCount;
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    if (day > -1)
                    {
                        label.Width = timetablePanel.Width / timetablePanel.ColumnCount * 2;
                        label.Tag = (day, lesson);
                        label.Click += SetValuesFromLabel;
                        string subject = timetable[day, lesson].Replace(" ", "").Replace("\n", "").Split("_")[0];
                        subject = (subject != "-") ? subject : "FREISTUNDE";
                        label.BackColor = Subjects.GetColorBySubjectAcronym(subject);
                        if (label.BackColor.GetBrightness() < .33f)
                        {
                            label.ForeColor = Color.White;
                        }
                        if (!(timetable[day, lesson] == "-"))
                        {
                            label.Text = subject;
                            label.MouseHover += OnTimeTableLessonHover;
                        }
                    }
                    else if (Config.showTimetableLessonTimes)
                    {
                        label.BackColor = Subjects.GetColorBySubjectAcronym("FREISTUNDE");
                        label.Tag = (-1, -1);
                        label.Width = timetablePanel.Width / timetablePanel.ColumnCount;
                        int hour, minutes;
                        (hour, minutes) = Subjects.GetLessonStartTime(lesson + 1);
                        string h, m;
                        h = hour.ToString().PadLeft(2, '0');
                        m = minutes.ToString().PadLeft(2, '0');
                        // h2 = hour2.ToString().PadLeft(2, '0');
                        // m2 = minutes2.ToString().PadLeft(2, '0');
                        label.Text = $"{h}:{m}";
                    }

                    if (Config.showTimetableLessonTimes || day > -1)
                    {
                        if (timetablePanel.InvokeRequired)
                        {
                            // we aren't on the UI thread. Ask the UI thread to do stuff.
                            timetablePanel.Invoke(new Action(() => Form1.timetablePanel.Controls.Add(label)));
                        }
                        else
                        {
                            // we are on the UI thread. We are free to touch things.
                            timetablePanel.Controls.Add(label);
                        }
                    }
                }
            }
        }
        public static void ResizeLabels(TableLayoutPanel timetablePanel)
        {
            for (int i = 0; i < timetablePanel.Controls.Count; i++)
            {
                timetablePanel.Controls[i].Height = timetablePanel.Height / timetablePanel.RowCount;
                timetablePanel.Controls[i].Width = timetablePanel.Width / timetablePanel.ColumnCount;
            }
        }
        public static void SetValuesFromLabel(object sender, EventArgs e)
        {
            int day, lesson, hour, minutes;
            Label l = (Label)sender;
            (day, lesson) = ((int, int))l.Tag;
            string acronym = timetable[day, lesson].Replace(" ", "").Replace("\n", "").Split("_")[0];
            if (acronym == "-") { return; }
            string subject = Subjects.GetSubjectByAcronym(acronym);
            (hour, minutes) = Subjects.GetLessonStartTime(lesson);

            DayOfWeek dayOfWeek = DayOfWeek.Monday;
            switch (day)
            {
                case 0:
                    dayOfWeek = DayOfWeek.Monday;
                    break;
                case 1:
                    dayOfWeek = DayOfWeek.Tuesday;
                    break;
                case 2:
                    dayOfWeek = DayOfWeek.Wednesday;
                    break;
                case 3:
                    dayOfWeek = DayOfWeek.Thursday;
                    break;
                case 4:
                    dayOfWeek = DayOfWeek.Friday;
                    break;
                case 5:
                    dayOfWeek = DayOfWeek.Saturday;
                    break;
                case 6:
                    dayOfWeek = DayOfWeek.Sunday;
                    break;
            }
            DateTime duedate = new DateTime(DateTime.Today.AddDays(1).Year, DateTime.Today.AddDays(1).Month, DateTime.Today.AddDays(1).Day, hour, minutes, 0);
            duedate = GetNextWeekday(duedate, dayOfWeek);
            Form1.SetValues(duedate, subject);
        }
        public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
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

        public static void OnLoginCredentialsSubmitButtonClick(object sender, EventArgs e)
        {

            Portal.HasPortalLoginDetails = true;
            bool result = LoadPlan(loadingType.reloadFromPortal, true, Form1.username.Text, Form1.password.Text);
            ShowPlan(Form1.timetablePanel);
            ResizeLabels(Form1.timetablePanel);
            if (result && timetable.Length > 0)
            {
                MessageBox.Show("Stundenplan erfolgreich geladen!");
                Config.portalUsername = Form1.username.Text;
                Config.portalPassword = Form1.password.Text;
                Config.SaveConfig();
            }
        }
        public static bool AreTTEqual(string[,] timetable, string[,] otherTimetable)
        {
            // TimetableDummyClass tt = new TimetableDummyClass(timetable);
            // TimetableDummyClass otherTt = new TimetableDummyClass(otherTimetable);
            // return JsonSerializer.Serialize(tt) == JsonSerializer.Serialize(otherTt);
            if (timetable == null || otherTimetable == null) return false;
            if (timetable.GetLength(0) != otherTimetable.GetLength(0) || timetable.GetLength(1) != otherTimetable.GetLength(1)) return false;
            for (int x = 0; x < timetable.GetLength(0); x++)
            {
                for (int y = 0; y < timetable.GetLength(1); y++)
                {
                    if (timetable[x, y] != otherTimetable[x, y]) return false;
                }
            }
            return true;
        }
        public static bool LoadPlan(loadingType lt = loadingType.loadFromFile, bool showMessages = false, string username = "", string password = "")
        {
            string[,] tt;
            // string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + FileManager.path;
            if (lt == loadingType.reloadFromPortal /*|| !File.Exists(path + FileManager.fileNames["Timetable"] + FileManager.fileNames["ext"])*/)
            {
                if (Portal.HasPortalLoginDetails)
                {
                    tt = Portal.GetPlan(username, password, showMessages);
                    if (!AreTTEqual(tt, timetable)) hasTimetableChangedSinceRedraw = true;
                    if (tt.Length > 0)
                    {
                        timetable = tt;
                        SavePlan();
                        return true;
                    }
                    return false;
                }
                // dont overwrite existing timetable if no portal credentials are saved but a timetable.json is available
                if (timetable == null)
                {
                    timetable = new string[0, 0];
                }
                return false;
            }
            string jsonString = FileManager.LoadData(FileManager.fileNames["Timetable"]);
            if (jsonString == null)
            {
                timetable = new string[0, 0];
                return false;
            }
            TimetableDummyClass tbc = JsonSerializer.Deserialize<TimetableDummyClass>(jsonString);
            tt = tbc.GetNormalTimetable();
            if (!AreTTEqual(tt, timetable)) hasTimetableChangedSinceRedraw = true;
            timetable = tt;
            return true;
        }
        public static void SavePlan()
        {
            TimetableDummyClass tdc = new TimetableDummyClass(timetable);
            FileManager.SaveData(tdc, FileManager.fileNames["Timetable"]);
            // string jsonString = JsonSerializer.Serialize(tdc);
            // string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Florian\\HomeworkPlanner\\";
            // if (!Directory.Exists(path))
            // {
            //     Directory.CreateDirectory(path);
            // }
            // File.WriteAllText(path + "timetable.json", jsonString);
        }
    }
    public class TimetableDummyClass
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public string[] Values { get; set; }
        public TimetableDummyClass() { }
        public TimetableDummyClass(string[,] normalTimetable)
        {
            Height = normalTimetable.GetLength(1);
            Width = normalTimetable.GetLength(0);
            Values = new string[Height * Width];
            int i = 0;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Values[i] = normalTimetable[x, y];
                    i++;
                }
            }
        }
        public string[,] GetNormalTimetable()
        {
            string[,] normal = new string[Width, Height];
            int i = 0;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    normal[x, y] = Values[i];
                    i++;
                }
            }
            return normal;
        }
    }
}