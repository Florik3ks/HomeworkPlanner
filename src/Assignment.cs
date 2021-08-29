using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HomeworkPlanner{
    public class Assignment{
        public string id { get; set; }
        private DateTime dueDate;
        private string subject;
        private string message;
        private bool done;
        public DateTime DueDate { get => dueDate; set => dueDate = value; }
        public string Subject { get => subject; set => subject = value;}
        public string Message { get => message; set => message = value;}
        public bool Done { get => done; set => done = value;}
        public Assignment(DateTime dueDate, string subject, string message){
            done = false;
            this.dueDate = dueDate;
            this.subject = subject;
            this.message = message;
        }

        public Assignment(){

        }
        // [JsonConstructor]
        // public Assignment(string id, DateTime DueDate, string Subject, string Message, bool Done) =>
        //     (this.id, dueDate, subject, message, done) = (id, DueDate, Subject, Message, Done);
    
        public void SetDone(){
            done = true;
        }
    }
}