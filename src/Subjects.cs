using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System;
namespace HomeworkPlanner
{
    public static class Subjects
    {
        private static Dictionary<string, Color> subjectColors;
        //     private static Dictionary<string, string> subjects = new Dictionary<string, string>{
        //     {"Informatik", "INF"},
        //     {"Englisch", "E"},
        //     {"Physik", "PH"},
        //     {"Deutsch", "D"},
        //     {"Mathe", "M"},
        //     {"Sozialkunde", "SK"},//
        //     {"Sozialkunde/Erdkunde", "SKEK"},
        //     {"Erdkunde", "EK"},//
        //     {"Geschichte", "G"},
        //     {"Musik", "MU"},
        //     {"Sport", "SP"},
        //     {"Biologie", "BIO"},
        //     {"Chemie", "CH"},
        //     {"Kunst", "BK"},//
        //     {"Darstellendes Spiel", "DS"},
        //     {"Französisch", "F"},//
        //     {"k. Religion", "RELRK"},
        //     {"ev. Religion", "RELEV"},
        //     {"Ethik", "ETH"},
        //     {"Philosophie", "PHIL"}
        // };
        private static Dictionary<string, string> subjects = new Dictionary<string, string>{
            {"INF", "Informatik"},
            {"E", "Englisch"},
            {"PH", "Physik"},
            {"D", "Deutsch"},
            {"M", "Mathe"},
            {"SK", "Sozialkunde"},//
            {"SKEK", "Sozialkunde/Erdkunde"},
            {"EK", "Erdkunde"},//
            {"G", "Geschichte"},
            {"MU", "Musik"},
            {"SP", "Sport"},
            {"BIO", "Biologie"},
            {"CH", "Chemie"},
            {"S", "Spanisch"},
            {"L", "Latein"},
            {"BK", "Kunst"},//
            {"DS", "Darstellendes Spiel"},
            {"F", "Französisch"},//
            {"RELRK", "k. Religion"},
            {"RELEV", "ev. Religion"},
            {"ETH", "Ethik"},
            {"PHIL", "Philosophie"},
            {"KL", "Klassenleiterstunde"},
            {"ITG", "Informatisch Technologische Grundlagen"},
            {"NW", "Naturwissenschaften"},
            {"SONSTIGES", "Sonstiges"},
            {"FREISTUNDE","Freistunde"}
        };
        public static Dictionary<string, Color> baseSubjectColors { get; private set; } = new Dictionary<string, Color>{
            {"INF", Color.GreenYellow},
            {"E", Color.Yellow},
            {"PH", Color.Gray},
            {"D", Color.Red},
            {"M", Color.LightBlue},
            {"SK", Color.RosyBrown},//
            {"SKEK", Color.RosyBrown},
            {"EK", Color.RosyBrown},//
            {"G", Color.White},
            {"MU", Color.White},
            {"SP", Color.BurlyWood},
            {"BIO", Color.Green},
            {"CH", Color.LightGreen},
            {"BK", Color.Purple},//
            {"DS", Color.White},
            {"F", Color.Orange},//
            {"L", Color.Cyan},//
            {"RELRK", Color.Lavender},
            {"RELEV", Color.Lavender},
            {"ETH", Color.Lavender},
            {"PHIL", Color.SlateGray},
            {"KL", Color.SlateGray},
            {"ITG", Color.SlateGray},
            {"NW", Color.SeaGreen},
            {"FREISTUNDE", Color.White}
        };
        private static Dictionary<int, ((int, int), (int, int))> lessonStartTimes = new Dictionary<int, ((int, int), (int, int))>(){
            {01 , ((08, 00), (08, 45))},
            {02 , ((08, 45), (09, 30))},
            {03 , ((09, 50), (10, 35))},
            {04 , ((10, 35), (11, 20))},
            {05 , ((11, 40), (12, 25))},
            {06 , ((12, 25), (13, 10))},
            {07 , ((13, 15), (14, 00))},
            {08 , ((14, 00), (14, 45))},
            {09 , ((14, 45), (15, 30))},
            {10 , ((15, 40), (16, 25))},
            {11 , ((16, 25), (17, 10))},

            {-1 , ((17, 10), (08, 00))},
        };
        public static string GetSubjectNameByTimetableString(string ttString)
        {
            string subject = "";
            char[] ar = ttString.Replace("\n", "").Split("_")[0].ToCharArray();
            bool subjectStarted = false;
            for (int i = 0; i < ar.Length; i++)
            {
                if (ar[i] != ' ')
                {
                    subjectStarted = true;
                    subject += ar[i];
                }
                else if (subjectStarted)
                    return subject;
            }
            return subject == "" ? ttString : subject;
        }
        public static (int, int) GetLessonStartTime(int lesson)
        {
            if (lessonStartTimes.ContainsKey(lesson))
            {
                return lessonStartTimes[lesson].Item1;
            }
            return lessonStartTimes[-1].Item1;
        }
        public static (int, int) GetLessonEndTime(int lesson)
        {
            if (lessonStartTimes.ContainsKey(lesson))
            {
                return lessonStartTimes[lesson].Item2;
            }
            return lessonStartTimes[-1].Item2;
        }
        public static String[] GetSubjectsThatTheUserHas()
        {
            List<String> subjects = new List<String>();
            if (Timetable.timetable.GetLength(0) == 0 && Timetable.timetable.GetLength(1) == 0) return GetSubjects();
            string s;
            for (int x = 0; x < Timetable.timetable.GetLength(0); x++)
            {
                for (int y = 0; y < Timetable.timetable.GetLength(1); y++)
                {
                    s = GetSubjectByAcronym(GetSubjectNameByTimetableString(Timetable.timetable[x, y]));
                    if (s == "-") continue;
                    if (!subjects.Contains(s))
                    {
                        subjects.Add(s);
                    }
                }
            }
            subjects.Sort();
            subjects.Add("Sonstiges");
            return subjects.ToArray();
        }
        public static void LoadSubjectColors()
        {
            string jsonString = FileManager.LoadData(FileManager.fileNames["Colors"]);
            if (jsonString == null)
            {
                subjectColors = baseSubjectColors;
                return;
            }
            subjectColors = new Dictionary<string, Color>();
            Dictionary<string, DummyColorClass> dummyDict = JsonSerializer.Deserialize<Dictionary<string, DummyColorClass>>(jsonString);
            foreach (var key in dummyDict.Keys)
            {
                subjectColors[key.ToUpper()] = dummyDict[key].GetColor();
            }
            CheckForWrongOrMissingColorAcronyms();
        }
        // needed for compatibility reasons
        public static void CheckForWrongOrMissingColorAcronyms()
        {
            foreach (var key in baseSubjectColors.Keys)
            {
                if (!subjectColors.ContainsKey(key))
                {
                    subjectColors.Add(key, baseSubjectColors[key]);
                }
            }
            foreach (var key in baseSubjectColors.Keys)
            {
                if (!subjectColors.ContainsKey(key))
                {
                    subjectColors.Remove(key);
                }
            }
        }
        public static void SaveSubjectColors()
        {
            FileManager.SaveData(subjectColors, FileManager.fileNames["Colors"]);
            // string jsonString = JsonSerializer.Serialize(subjectColors);
            // string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Florian\\HomeworkPlanner\\";
            // if (!Directory.Exists(path))
            // {
            //     Directory.CreateDirectory(path);
            // }
            // File.WriteAllText(path + "colors.json", jsonString);
        }
        public static Color GetColorBySubjectAcronym(string acronym)
        {
            if (subjectColors.ContainsKey(acronym.ToUpper()))
            {
                return subjectColors[acronym.ToUpper()];
            }
            return Color.White;
        }
        public static string GetSubjectByAcronym(string acronym)
        {
            if (subjects.ContainsKey(acronym.ToUpper()))
            {
                return subjects[acronym.ToUpper()];
            }
            return acronym;
        }
        public static string GetAcronymBySubject(string subject)
        {
            foreach (var key in subjects.Keys)
            {
                if (subject == subjects[key]) return key;
            }
            return subject;
        }
        public static string[] GetSubjects()
        {
            // string[] result = new string[subjects.Keys.Count];
            // int i = 0;
            // foreach (string key in subjects.Keys)
            // {
            //     result[i] = key;
            //     i++;
            // }
            // return result;

            string[] result = new string[subjects.Keys.Count];
            int i = 0;
            foreach (string key in subjects.Keys)
            {
                result[i] = subjects[key];
                i++;
            }
            return result;
        }
        public static void ColorButtonClick(object sender, System.EventArgs e)
        {
            Control b = (Control)sender;
            ColorDialog dialog = new ColorDialog();
            dialog.ShowHelp = false;
            dialog.Color = b.BackColor;
            // Update the text box color if the user clicks OK 
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog.Color.GetBrightness() < .33f)
                {
                    b.ForeColor = Color.White;
                }
                else
                {
                    b.ForeColor = Color.Black;
                }
                b.BackColor = dialog.Color;
                subjectColors[(string)b.Tag] = dialog.Color;
                SaveSubjectColors();
                Timetable.hasTimetableChangedSinceRedraw = true;
                Timetable.ShowPlan(Form1.timetablePanel);
            }
        }
        public static void ColorListSelectionChanged(object sender, EventArgs e)
        {
            ComboBox box = sender as ComboBox;
            string acronym = GetAcronymBySubject(box.SelectedItem.ToString());
            Form1.colorPictureBox.Tag = acronym;
            Form1.colorPictureBox.BackColor = GetColorBySubjectAcronym(acronym);
        }
        public static void ColorListHoverChanged(object sender, ComboBoxListEx.ListItemSelectionChangedEventArgs e)
        {
            if (e.ItemText.ToString() == "") return;
            string acronym = GetAcronymBySubject(e.ItemText.ToString());
            Form1.colorPictureBox.Tag = acronym;
            Form1.colorPictureBox.BackColor = GetColorBySubjectAcronym(acronym);
        }

    }
    public class DummyColorClass
    {
        public int R { get; set; }
        public int B { get; set; }
        public int G { get; set; }
        public int A { get; set; }
        public bool IsKnownColor { get; set; }
        public bool IsEmpty { get; set; }
        public bool IsNamedColor { get; set; }
        public bool IsSystemColor { get; set; }
        public string Name { get; set; }
        public DummyColorClass() { }
        public Color GetColor()
        {
            return Color.FromArgb(A, R, G, B);
        }
    }
}