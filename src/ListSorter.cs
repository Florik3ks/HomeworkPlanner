using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HomeworkPlanner
{
    public class ListSorter : IComparer
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public bool reverse = false;
        public int Compare(object x, object y)
        {
            Assignment itemA = Homework.GetById(((ListViewItem)x).Name);
            Assignment itemB = Homework.GetById(((ListViewItem)y).Name);
            // if(itemA.DueDate < DateTime.Now){
            //     return 1;
            // }
            if (itemA.DueDate < itemB.DueDate)
                return !reverse ? -1 : 1;

            if (itemA.DueDate > itemB.DueDate)
                return !reverse ? 1 : -1;

            if (itemA.DueDate == itemB.DueDate)
                return 0;

            return 0;

        }
    }
}