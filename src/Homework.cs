using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace HomeworkPlanner
{
    public static class Homework
    {

        private static List<Assignment> assignmentList = new();
        public static List<Assignment> AssignmentList { get => assignmentList; }
        private static Dictionary<string, Assignment> pairs = new();
        public static void AddAssignment(Assignment a)
        {
            assignmentList.Add(a);
            a.id = Guid.NewGuid().ToString("N");
            pairs.Add(a.id, a);
            SaveAssignments();
        }
        public static Assignment GetById(string id)
        {
            return pairs[id];
        }
        public static void EditAssignment(Assignment a)
        {
            assignmentList.Remove(pairs[a.id]);
            pairs[a.id] = a;
            assignmentList.Add(a);
            SaveAssignments();
        }


        public static void SaveAssignments()
        {
            string jsonString = JsonSerializer.Serialize(assignmentList);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Florian\\HomeworkPlanner\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.WriteAllText(path + "homework.json", jsonString);
        }
        public static void LoadAssignments()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Florian\\HomeworkPlanner\\";
            if (!File.Exists(path + "homework.json"))
            {
                return;
            }
            string jsonString = File.ReadAllText(path + "homework.json");
            assignmentList = JsonSerializer.Deserialize<List<Assignment>>(jsonString);
            foreach (Assignment a in assignmentList)
            {
                pairs.Add(a.id, a);
            }
        }
    }


}