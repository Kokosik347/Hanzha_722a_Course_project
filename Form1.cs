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

        ToolStripLabel dateLabel;
        ToolStripLabel timeLabel;
        ToolStripLabel infoLabel;
        Timer timer;

        public Form1()
        {
            InitializeComponent();
            infoLabel = new ToolStripLabel();
            infoLabel.Text = "Поточні дата та час:";
            dateLabel = new ToolStripLabel();
            timeLabel = new ToolStripLabel();
            statusStrip1.Items.Add(infoLabel);
            statusStrip1.Items.Add(dateLabel);
            statusStrip1.Items.Add(timeLabel);
            timer = new Timer() { Interval = 1000 };
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            dateLabel.Text = DateTime.Now.ToLongDateString();
            timeLabel.Text = DateTime.Now.ToLongTimeString();
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

            toolTip1.SetToolTip(bSearch, "Натисніть на кнопку для пошуку"); 
            toolTip1.IsBalloon = true;
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
            A.progressBar1.Hide();
            A.ShowDialog();
        }

        private void зберегтиЯкToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sfdSave.ShowDialog() == DialogResult.OK) // Виклик діалогу збереження файлу
            {
                MajorObject.WriteSaveFileName(sfdSave.FileName); // Запис імені файлу для збереження
                MajorObject.Generator();
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

        private void зберегтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MajorObject.SaveFileNameExists()) // задане ім’я файлу існує?
            {
                MajorObject.WriteOpenFileName(ofdOpen.FileName); // відкриття
                MajorObject.ReadFromFile(dgwOpen); // читання даних з файлу
            }
            else
                зберегтиЯкToolStripMenuItem_Click(sender, e);
        }

        private void новийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MajorObject.NewRec();
            tbInput.Clear();// очистити вміст тексту
            label1.Text = "";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MajorObject.Modify)
                if (MessageBox.Show("Дані не були збережені. Продовжити вихід?", "УВАГА",
                MessageBoxButtons.YesNo) == DialogResult.No)
                    e.Cancel = true; // припинити закриття
        }

        private void bSearch_Click(object sender, EventArgs e)
        {
            MajorObject.Find(tbSearch.Text); //пошук
        }
    }
}