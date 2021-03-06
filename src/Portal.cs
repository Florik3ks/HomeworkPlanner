using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using HtmlAgilityPack;

namespace HomeworkPlanner
{
    public static class Portal
    {
        public static bool HasPortalLoginDetails = false;
        private const string url = "https://portal.gymnasium-oberstadt.de/";
        private const string stdplanUrl = "https://portal.gymnasium-oberstadt.de/stdplan.php";
        private const string substitutionPlanUrl = "https://portal.gymnasium-oberstadt.de/vertretung.php";
        private static HttpClient client;
        public async static Task<string> GetCookie(string username, string password)
        {
            var cookieJar = new CookieContainer();
            var handler = new HttpClientHandler
            {
                CookieContainer = cookieJar,
                UseCookies = true,
                UseDefaultCredentials = false
            };
            client = new HttpClient(handler);
            var values = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password }
            };

            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage res = await client.PostAsync(url, content).ConfigureAwait(false);
            res.EnsureSuccessStatusCode();

            Uri uri = new Uri(url);
            var responseCookies = cookieJar.GetCookies(uri);
            string cookieName = "";
            string cookieValue = "";
            foreach (Cookie cookie in responseCookies)
            {
                cookieName = cookie.Name;
                cookieValue = cookie.Value;
            }
            return cookieName + "=" + cookieValue;
        }
        public static string[,] GetPlan(string username, string password, bool showErrors = false)
        {
            string html = GetHtmlFromUrl(stdplanUrl, username, password, showErrors);
            if(html == "")return new string[0, 0];
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            HtmlNode table = doc.DocumentNode.SelectSingleNode("//table");
            int height = table.SelectNodes("tr").Count;//, Timetable.timetableWidth;
            int width = Timetable.timetableBaseWidth;
            // if (height != 0)
            // {
            // width = table.SelectNodes("tr")[0].SelectNodes("th|td").Count;// Timetable.timetableHeight;
            // }
            string[,] timetable = new string[width, height];
            int day = 0;
            int lesson = 0;

            foreach (HtmlNode row in table.SelectNodes("tr"))
            {
                day = 0;
                foreach (HtmlNode cell in row.SelectNodes("th|td"))
                {
                    if (cell.InnerText.Length > 0 && int.TryParse(cell.InnerText.Replace(" ", "").Replace("\n", "").ToCharArray()[0].ToString(), out _)) continue;
                    if (cell.InnerText.Replace(" ", "").Replace("\n", "") == "-")
                    {
                        timetable[day, lesson] = cell.InnerText.Replace("\n", "").Replace(" ", "");//.PadRight(7);
                    }
                    else //(cell.InnerText.Contains("_"))
                    {
                        timetable[day, lesson] = cell.InnerText;//.Split("_")[0].PadRight(7);
                    }
                    day++;
                }
                lesson++;
            }
            return timetable;
        }
        public static string GetHtmlFromUrl(string url, string username, string password, bool showErrors=false){
            string cookie;
            try
            {
                cookie = GetCookie(username, password).Result;
            }
            catch
            {
                if (showErrors)
                {
                    System.Windows.Forms.MessageBox.Show("Fehler bei der Verbindung!", "Fehler", System.Windows.Forms.MessageBoxButtons.OK);
                }
                return "";
            }
            var request2 = (HttpWebRequest)WebRequest.Create(url);
            request2.AllowAutoRedirect = true;
            request2.UseDefaultCredentials = true;
            request2.PreAuthenticate = true;
            request2.Credentials = CredentialCache.DefaultCredentials;
            request2.Headers.Add(HttpRequestHeader.Cookie, cookie);
            request2.Method = "GET";
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request2.GetResponse();
            }
            catch
            {
                if (showErrors)
                {
                    System.Windows.Forms.MessageBox.Show("Deine Anmeldedaten sind ung??ltig!", "Fehler", System.Windows.Forms.MessageBoxButtons.OK);
                }
                return "";
            }
            Stream receiveStream = response.GetResponseStream();

            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            string html = readStream.ReadToEnd();
            response.Close();
            readStream.Close();
            return html;
        }
        public static List<Substitution> GetSubstitutionPlan(string username, string password, bool showErrors = false)
        {
            string html = GetHtmlFromUrl(substitutionPlanUrl, username, password, showErrors);
            if (html == "") return new List<Substitution>();
            // string path = @"C:\Users\flori\Documents\Code\HomeworkPlanner\subPlan.txt";
            // html = File.ReadAllText(path);
            // debug
            List<Substitution> substitutions = new List<Substitution>();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            int rowNum = 0;
            int cellNum = 0;
            int tableNum = 0;
            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
            {
                rowNum = 0;
                foreach (HtmlNode row in table.SelectNodes("tr"))
                {
                    cellNum = 0;
                    Dictionary<int, string> cellContents = new Dictionary<int, string>();
                    foreach (HtmlNode cell in row.SelectNodes("th|td"))
                    {
                        // (rowNum > 0)
                        // {
                            cellContents.Add(cellNum, cell.InnerText);
                        // }
                        cellNum++;
                    }
                    if (cellContents.Count == 7)
                    {
                        DateTime d = GetNextWeekdayAfterDays(tableNum);
                        Substitution s = new Substitution(d,
                            cellContents[0], cellContents[1], cellContents[2],
                            cellContents[3], cellContents[4], cellContents[5], cellContents[6]
                            );
                        substitutions.Add(s);
                    }
                    rowNum++;
                }
                tableNum++;
            }
            substitutions.Reverse();
            return substitutions;
        }
        public static DateTime GetNextWeekdayAfterDays(int days)
        {
            DateTime d = DateTime.Today;
            for (int i = 0; i < days; i++)
            {
                while (d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday)
                {
                    d = d.AddDays(1);
                }
                d = d.AddDays(1);
            }
            while (d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday)
            {
                d = d.AddDays(1);
            }
            return d;
        }
    }
}