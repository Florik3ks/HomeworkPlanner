using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace HomeworkPlanner
{
    public static class Config
    {
        public static string portalUsername = "";
        public static string portalPassword = "";
        public static bool showTimetableLessonTimes = false;
        public static void SaveConfig()
        {
            string u = DataProtectionApiWrapper.Encrypt(portalUsername);
            string p = DataProtectionApiWrapper.Encrypt(portalPassword);
            ConfigContainer cc = new ConfigContainer(u, p, showTimetableLessonTimes);
            FileManager.SaveData(cc, FileManager.fileNames["Config"]);
        }
        public static void LoadConfig()
        {
            string jsonString = FileManager.LoadData(FileManager.fileNames["Config"]);
            if (jsonString == null) return;
            ConfigContainer cc = JsonSerializer.Deserialize<ConfigContainer>(jsonString);
            portalUsername = DataProtectionApiWrapper.Decrypt(cc.portalUsername);
            portalPassword = DataProtectionApiWrapper.Decrypt(cc.PortalPassword);
            showTimetableLessonTimes = cc.ShowTimetableLessonTimes;
            if (portalUsername != "" && portalPassword != "" && portalUsername != null && portalPassword != null)
            {
                Portal.HasPortalLoginDetails = true;
                Form1.username.Text = portalUsername;
            }
        }
    }
    public class ConfigContainer
    {
        public string portalUsername{ get; set; }
        public string PortalPassword{ get; set; }
        public bool ShowTimetableLessonTimes{ get; set; }
        public ConfigContainer() { }
        public ConfigContainer(string u, string p, bool s)
        {
            portalUsername = u;
            PortalPassword = p;
            ShowTimetableLessonTimes = s;
        }
    }

}