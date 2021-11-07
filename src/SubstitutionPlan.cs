using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
namespace HomeworkPlanner
{
    public static class SubstitutionPlan
    {
        private static List<Substitution> substitutions = new List<Substitution>();
        private static Dictionary<string, int> GetWidths(Font font)
        {
            Dictionary<string, int> widths = new Dictionary<string, int>();
            foreach (Substitution s in substitutions)
            {
                foreach (string key in s.GetSubstitutionDictionary().Keys)
                {
                    string text = s.GetSubstitutionDictionary()[key];
                    if (text.Length == 0) text = "-";
                    int width = TextRenderer.MeasureText(text, font).Width;
                    width += TextRenderer.MeasureText(" ", font).Width;
                    if (!widths.ContainsKey(key))
                    {
                        widths.Add(key, width);
                    }
                    widths[key] = Math.Max(widths[key], width);
                }
            }
            return widths;
        }
        public static void ShowPlan()
        {
            foreach (Panel p in new Panel[] { Form1.spPanel1, Form1.spPanel2, Form1.spPanel3 })
            {
                if (p.InvokeRequired)
                {
                    // we aren't on the UI thread. Ask the UI thread to do stuff.
                    p.Invoke(new Action(() =>
                    {
                        p.Controls.Clear();
                    }));
                }
                else
                {
                    // we are on the UI thread. We are free to touch things.
                    p.Controls.Clear();
                }
            }
            // Form1.spPanel1.Controls.Clear();
            // Form1.spPanel2.Controls.Clear();
            // Form1.spPanel3.Controls.Clear();
            // Label vertreter, stunde, klasse, raum, fach, art, info;
            List<Substitution> newSubst = Portal.GetSubstitutionPlan(Config.portalUsername, Config.portalPassword, false);
            if (newSubst.Count != 0 || substitutions == null)
            {
                substitutions = newSubst;
            }

            Font f = new Font(Form1.DefaultFont.FontFamily, Form1.DefaultFont.Size + 5);

            Dictionary<string, int> widths = GetWidths(f);
            int totalWidth = 0;
            bool stop = false;
            do
            {
                float size = f.Size - 0.25f;
                if (size <= 0)
                {
                    size = 0.1f;
                    stop = true;
                }
                f = new Font(f.FontFamily, size);
                widths = GetWidths(f);
                if (widths.Count == 0) return;
                totalWidth = 0;
                foreach (string key in widths.Keys)
                {
                    totalWidth += widths[key];
                }
                if (totalWidth < Form1.spPanel1.Width) widths["Info"] += Form1.spPanel1.Width - totalWidth;
            } while (totalWidth > Form1.spPanel1.Width && !stop);

            foreach (Substitution s in substitutions)
            {
                Panel subs = new Panel();
                subs.Dock = DockStyle.Top;
                subs.BorderStyle = BorderStyle.None;
                subs.BackColor = Color.AliceBlue;
                subs.Height = (int)(Math.Max(30, TextRenderer.MeasureText("|", Form1.DefaultFont).Height) * 1.5f);

                foreach (string key in s.GetSubstitutionDictionary().Keys.Reverse())
                {
                    string value = s.GetSubstitutionDictionary()[key];
                    Label l = new Label();
                    if (value.Length == 0) value = "-";

                    l.Width = widths.ContainsKey(key) ? widths[key] : 30;
                    l.Text = value;
                    l.Font = f;
                    l.BorderStyle = BorderStyle.Fixed3D;
                    l.Dock = DockStyle.Left;
                    l.TextAlign = ContentAlignment.MiddleCenter;
                    subs.Controls.Add(l);
                }

                Panel p = Form1.spPanel1;
                if (s.date.Equals(Portal.GetNextWeekdayAfterDays(1)))
                {
                    p = Form1.spPanel2;
                }
                else if (s.date.Equals(Portal.GetNextWeekdayAfterDays(2)))
                {
                    p = Form1.spPanel3;
                }
                if (p.InvokeRequired)
                {
                    // we aren't on the UI thread. Ask the UI thread to do stuff.
                    p.Invoke(new Action(() =>
                    {
                        p.Controls.Add(subs);
                    }));
                }
                else
                {
                    // we are on the UI thread. We are free to touch things.
                    p.Controls.Add(subs);
                }
                // p.Controls.Add(subs);
            }
        }
    }
}