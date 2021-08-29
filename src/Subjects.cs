using System;
using System.Collections;
using System.Collections.Generic;

namespace HomeworkPlanner
{
    public static class Subjects
    {
        private static Dictionary<string, string> subjects = new Dictionary<string, string>{
        {"Informatik", "INF"},
        {"Englisch", "EN"},
        {"Physik", "PH"},
        {"Deutsch", "DE"},
        {"Mathe", "MA"},
        {"Sozialkunde", "SK"},
        {"Sozialkunde/Erdkunde", "se"},
        {"Erdkunde", "EK"},
        {"Geschichte", "GE"},
        {"Musik", "MU"},
        {"Sport", "SP"},
        {"Biologie", "BI"},
        {"Chemie", "CH"},
        {"Kunst", "BK"},
        {"Französisch", "FR"},
        {"k. Religion", "kr"},
        {"ev. Religion", "er"},
        {"Ethik", "et"},
    };

        public static string[] GetSubjects()
        {
            string[] result = new string[subjects.Keys.Count];
            int i = 0;
            foreach (string key in subjects.Keys)
            {
                result[i] = key;
                i++;
            }
            return result;
        }
    }
}