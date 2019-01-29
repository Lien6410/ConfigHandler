using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace Mirle.A33.ConfigHandler
{
    public class ConfigHandler : IConfigHandler
    {
        private const int STRING_BUILDER_SIZE = 256;

        public string FilePath { get; set; }
        public string SectionName { get; set; }
        public string KeyName { get; set; }
        public string DefaultValue { get; set; }


        /// <summary>
        /// 建構子,預設檔案路徑為 D:\CsProject\config.ini
        /// </summary>
        public ConfigHandler()
        {
            try
            {
                FilePath = @"D:\CsProject\ConfigHanlderTest.ini";
                CheckFilePathExist();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// 建構子,填入檔案路徑
        /// </summary>
        /// <param name="FilePath"></param>
        public ConfigHandler(string FilePath)
        {
            try
            {
                this.FilePath = FilePath;
                CheckFilePathExist();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

        // 2.讀取資料
        /// <summary>
        /// 讀取資料並轉型成string
        /// </summary>
        /// <returns></returns>
        public string GetDataToString()
        {
            try
            {
                string result = GetValue();

                return CheckResult(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return DefaultValue;
            }
        }

        /// <summary>
        /// 讀取資料並轉型成string
        /// </summary>
        /// <param name="SectionName"></param>
        /// <param name="KeyName"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public string GetDataToString(string SectionName, string KeyName, string DefaultValue)
        {
            try
            {
                AppDomainSetup(SectionName, KeyName, DefaultValue);

                return GetDataToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return DefaultValue;
            }
        }

        /// <summary>
        /// 填入參數
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="keyName"></param>
        /// <param name="defaultValue"></param>
        private void AppDomainSetup(string sectionName, string keyName, string defaultValue)
        {
            this.SectionName = sectionName;
            this.KeyName = keyName;
            this.DefaultValue = defaultValue;
        }

        /// <summary>
        /// 檢查讀取結果
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private string CheckResult(string result)
        {
            if (result == string.Empty)
            {
                return DefaultValue;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// 透過Windows方法讀取Config內容
        /// </summary>
        /// <returns></returns>
        private string GetValue()
        {
            try
            {
                StringBuilder buffer = new StringBuilder(STRING_BUILDER_SIZE);

                GetIniData(SectionName, KeyName, DefaultValue, buffer, STRING_BUILDER_SIZE, FilePath);

                return buffer.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// 檢查Config檔案路徑是否存在，若否則跳出例外
        /// </summary>
        private void CheckFilePathExist()
        {
            if (!File.Exists(FilePath))
            {
                CheckDirPathExist();

                FilePath = Path.ChangeExtension(FilePath, @".ini");

                File.Create(FilePath);

                Console.WriteLine("Config file is not exist.");
                Console.WriteLine("Generate a file with FilePath.");
            }
        }

        /// <summary>
        /// 確認FilePath所在的資料夾，若無提供則幫補上，若提供的資料夾不存在則產生一個。
        /// </summary>
        private void CheckDirPathExist()
        {
            string dir = Path.GetDirectoryName(FilePath);

            if (dir == string.Empty)
            {
                FilePath = Path.Combine(@"D:\", FilePath);
                Console.WriteLine("FilePath is not full path.");
                Console.WriteLine("Reset FilePath into full path.");
            }
            else
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                    Console.WriteLine("The directory of FilePath is not exist.");
                    Console.WriteLine("Generate the directory in FilePath.");
                }
            }
        }

        /// <summary>
        /// 讀取資料並轉型成bool
        /// </summary>
        /// <param name="SectionName"></param>
        /// <param name="KeyName"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public bool GetDataToBoolean(string SectionName, string KeyName, string DefaultValue)
        {
            try
            {
                var result = GetDataToString(SectionName, KeyName, DefaultValue);
                if (result == "True" || result == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }

        // 3.寫入資料

        /// <summary>
        /// 將string型態的數值寫入資料
        /// </summary>
        /// <param name="SectionName"></param>
        /// <param name="KeyName"></param>
        /// <param name="SetValue"></param>
        public void SetDataFromString(string SectionName, string KeyName, string SetValue)
        {
            try
            {
                SetIniData(SectionName, KeyName, SetValue, FilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return;
            }
        }

        /// <summary>
        /// 將bool型態的數值寫入資料
        /// </summary>
        /// <param name="SectionName"></param>
        /// <param name="KeyName"></param>
        /// <param name="SetValueInBool"></param>
        public void SetDataFromBool(string SectionName, string KeyName, bool SetValueInBool)
        {
            try
            {
                string SetValueInString = SetValueInBool.ToString();
                SetIniData(SectionName, KeyName, SetValueInString, FilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return;
            }
        }


        /// <summary>
        /// 引用windowsAPI/system32內的dll,讀取*.ini內的資料
        /// </summary>
        /// <param name="aSectionName"></param>
        /// <param name="aKeyName"></param>
        /// <param name="aDefault"></param>
        /// <param name="aReturnedString"></param>
        /// <param name="aSize"></param>
        /// <param name="aFileName"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        public static extern int GetIniData(string aSectionName, string aKeyName, string aDefault, StringBuilder aReturnedString, int aSize, string aFileName);

        /// <summary>
        /// 引用windowsAPI/system32內的dll,寫入*.ini內的資料
        /// </summary>
        /// <param name="aSectionName"></param>
        /// <param name="aKeyName"></param>
        /// <param name="aString"></param>
        /// <param name="aFileName"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
        public static extern int SetIniData(string aSectionName, string aKeyName, string aString, string aFileName);

    }
}
