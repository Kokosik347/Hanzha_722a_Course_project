using Hanzha_722a_Course_project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace Hanzha_722a_Course_project
{
    public partial class Form1 : Form
    {
        private bool Mode; // Режим дозволу / заборони введення даних 
        private MajorWork MajorObject; // Створення об'єкта класу MajorWork

        public Form1()
        {
            InitializeComponent();
        }

        private void tClock_Tick(object sender, EventArgs e)
        {
            tClock.Stop();
            MessageBox.Show("Минуло 25 секунд", "Увага"); // Виведення повідомлення "Минуло 25 секунд" на екран
            tClock.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            About A = new About(); // створення форми About
            A.tAbout.Start();
            A.ShowDialog(); // відображення діалогового вікна About
            MajorObject = new MajorWork();
            this.Mode = true;
            MajorObject = new MajorWork();
            MajorObject.SetTime();
            MajorObject.Modify = false;// заборона запису
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            if (Mode)
            {
                tbInput.Enabled = true; // Режим дозволу введення
                tbInput.Focus();
                tClock.Start();
                bStart.Text = "Стоп"; // зміна тексту на кнопці на "Стоп"
                this.Mode = false;
                пускToolStripMenuItem.Text = "Стоп";
            }
            else
            {
                tbInput.Enabled = false; // Режим заборони введення
                tClock.Stop();
                bStart.Text = "Пуск"; // зміна тексту на кнопці на "Пуск"
                this.Mode = true;
                MajorObject.Write(tbInput.Text); // Запис даних у об'єкт
                MajorObject.Task(); // Обробка даних
                label1.Text = MajorObject.Read(); // Відображення результату
                пускToolStripMenuItem.Text = "Старт";
            }
        }

        private void tbInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            tClock.Stop();
            tClock.Start();

            if ((e.KeyChar >= '0') && (e.KeyChar <= '9') || (e.KeyChar == (char)8))
            {
                return;
            }
            else
            {
                tClock.Stop();
                MessageBox.Show("Неправильний символ", "Помилка");
                tClock.Start();
                e.KeyChar = (char)0;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            string s;
            s = (System.DateTime.Now - MajorObject.GetTime()).ToString();
            MessageBox.Show(s, "Час роботи програми");
        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void проНакопиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] disks = System.IO.Directory.GetLogicalDrives(); // Строковий масив з логічніх дисків
            string disk = "";
            for (int i = 0; i < disks.Length; i++)
            {
                try
                {
                    System.IO.DriveInfo D = new System.IO.DriveInfo(disks[i]);
                    if (D.IsReady)
                    {
                        double totalSizeGB = D.TotalSize / 1073741824.0; // Переведення байтів в гігабайти
                        double totalFreeSpaceGB = D.TotalFreeSpace / 1073741824.0; // Переведення байтів в гігабайти
                        disk += $"{D.Name} - Загальний об'єм: {totalSizeGB:F2} ГБ - Вільне місце: {totalFreeSpaceGB:F2} ГБ" + Environment.NewLine;
                        // змінній присвоюється ім’я диска, загальна кількість місця і вільне місце на диску
                    }
                    else
                    {
                        disk += $"{disks[i]} - не готовий" + Environment.NewLine;
                    }
                }
                catch
                {
                    disk += $"{disks[i]} - помилка" + Environment.NewLine;
                }
            }
            MessageBox.Show(disk, "Накопичувачі");
        }

        private void вихідToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void проПрограмуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About A = new About();
            A.ShowDialog();
        }

        private void зберегтиЯкToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sfdSave.ShowDialog() == DialogResult.OK) // Виклик діалогового вікна збереження
            {
                MajorObject.WriteSaveFileName(sfdSave.FileName); // написання імені файлу
                MajorObject.SaveToFile(); // метод збереження в файл 
            }
        }
            private void відкритиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ofdOpen.ShowDialog() == DialogResult.OK) // Виклик діалогового вікна відкриття файлу
            {
                MessageBox.Show(ofdOpen.FileName);
            }
        }
    }
}