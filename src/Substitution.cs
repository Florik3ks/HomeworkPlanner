using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
namespace HomeworkPlanner{
    public class Substitution{
        public DateTime date { get; private set; }
        public string teacher{ get; private set; }
        public string lesson{ get; private set; }
        public string grade{ get; private set; }
        public string room{ get; private set; }
        public string subject{ get; private set; }
        public string substitutionType{ get; private set; }
        public string info{ get; private set; }
        public Substitution(DateTime date, string teacher, string lesson, string grade, string room, string subject, string substitutionType, string info){
            this.date = date;
            this.teacher = teacher;
            this.lesson = lesson;
            this.grade = grade;
            this.room = room;
            this.subject = subject;
            this.substitutionType = substitutionType;
            this.info = info;
        }
        public Dictionary<string, string> GetSubstitutionDictionary(){
            return new Dictionary<string, string>(){
                {"Vertreter", teacher},
                {"Stunde", lesson},
                {"Klasse", grade},
                {"Raum", room},
                {"Fach", subject},
                {"Art", substitutionType},
                {"Info",info}
            };
        }

    }
}