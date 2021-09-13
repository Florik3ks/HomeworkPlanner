using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System;

namespace HomeworkPlanner{
    public static class FileManager{
        public const string path = "\\Florian\\HomeworkPlanner\\";
        public static string LoadData(string filename, string ext = ".json")
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + path;
            if (!File.Exists(folderPath + filename + ext))
            {
                return null;
            }
            string jsonString = File.ReadAllText(folderPath + filename + ext);
            return jsonString;
            // Dictionary<string, DummyColorClass> dummyDict = JsonSerializer.Deserialize<Dictionary<string, DummyColorClass>>(jsonString);
            // subjectColors = new Dictionary<string, Color>();
            // foreach (var key in dummyDict.Keys)
            // {
            //     subjectColors[key] = dummyDict[key].GetColor();
            // }
        }
        public static void SaveData(object data, string filename, string ext = ".json")
        {
            
            string jsonString = JsonSerializer.Serialize(data);
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + path;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            File.WriteAllText(folderPath + filename + ext, jsonString);
        }
        public static Dictionary<string, string> fileNames = new Dictionary<string, string>(){
            {"Timetable", "timetable"},
            {"Colors", "colors"},
            {"Homework", "homework"},
            {"Config", "userconfig"},
            {"ext", ".json"}
        };
    }
}