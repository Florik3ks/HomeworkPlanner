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
        //     {"Französisch", "FR"},//
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
            {"BK", "Kunst"},//
            {"DS", "Darstellendes Spiel"},
            {"FR", "Französisch"},//
            {"RELRK", "k. Religion"},
            {"RELEV", "ev. Religion"},
            {"ETH", "Ethik"},
            {"PHIL", "Philosophie"},
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
            {"FR", Color.Orange},//
            {"RELRK", Color.Lavender},
            {"RELEV", Color.Lavender},
            {"ETH", Color.Lavender},
            {"PHIL", Color.SlateGray},
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
            // string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Florian\\HomeworkPlanner\\";
            // if (!File.Exists(path + "colors.json"))
            // {
            //     subjectColors = baseSubjectColors;
            //     return;
            // }
            // string jsonString = File.ReadAllText(path + "colors.json");
            // Dictionary<string, DummyColorClass> dummyDict = JsonSerializer.Deserialize<Dictionary<string, DummyColorClass>>(jsonString);
            // subjectColors = new Dictionary<string, Color>();
            // foreach (var key in dummyDict.Keys)
            // {
            //     subjectColors[key] = dummyDict[key].GetColor();
            // }
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
            Button b = (Button)sender;
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
                Timetable.ShowPlan(Form1.timetablePanel);
            }
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