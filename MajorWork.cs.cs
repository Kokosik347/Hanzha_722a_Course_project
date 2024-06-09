using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Collections;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace Hanzha_722a_Course_project
{
    [Serializable]
    public class Buffer
    {
        public string Data;
        public string Result;
        public int Key;
    }
    public class MajorWork
    {
        private DateTime TimeBegin; // час початку роботи
        private string Data; // вхідні дані
        private string Result; // поле результату
        public bool Modify;
        private int Key; // поле ключа
        private string SaveFileName;
        private string OpenFileName;

        public Stack myStack = new Stack();
        public string[] myArr = new string[100];

        public Queue myQueue = new Queue();
        public string[] smyQueue = new string[100];

        public Buffer D { get; private set; }

        public void ReadFromFile(System.Windows.Forms.DataGridView DG) // зчитування з файлу
        {
            try
            {
                if (!File.Exists(this.OpenFileName))
                {
                    MessageBox.Show("Файлу немає"); // Виведення на екран повідомлення "файлу немає"
                    return;
                }
                Stream S; // створення потоку
                S = File.Open(this.OpenFileName, FileMode.Open); // зчитування даних з файлу Buffer D;
                object O; // буферна змінна для контролю формату
                BinaryFormatter BF = new BinaryFormatter(); // створення об'єкту для форматування
                System.Data.DataTable MT = new System.Data.DataTable();
                System.Data.DataColumn cKey = new

                System.Data.DataColumn("Ключ");// формуємо колонку "Ключ"
                System.Data.DataColumn cInput = new

                System.Data.DataColumn("Вхідні дані");// формуємо колонку "Вхідні
                System.Data.DataColumn cResult = new
                System.Data.DataColumn("Результат");// формуємо колонку "Результат"

                MT.Columns.Add(cKey);// додавання ключа
                MT.Columns.Add(cInput);// додавання вхідних даних
                MT.Columns.Add(cResult);// додавання результату


                while (S.Position < S.Length)
                {
                    O = BF.Deserialize(S); // десеріалізація
                    D = O as Buffer;
                    if (D == null) break;
                    // Виведення даних на екран
                    System.Data.DataRow MR;
                    MR = MT.NewRow();
                    MR["Ключ"] = D.Key; // Занесення в таблицю номер
                    MR["Вхідні дані"] = D.Data; // Занесення в таблицю вхідних
                    MR["Результат"] = D.Result; // Занесення в таблицю
                    MT.Rows.Add(MR);
                }
                                DG.DataSource = MT;
                S.Close(); // закриття
            }
            catch
            {
                MessageBox.Show("Помилка файлу"); // Виведення на екран повідомлення "Помилка файлу"
            }
        } // ReadFromFile закінчився
        public void WriteSaveFileName(string S) // метод запису даних до об'єкту
        {
            this.SaveFileName = S;
        }

        public void WriteOpenFileName(string S)
        {
            this.OpenFileName = S;
        }

        // Методи
        public void Find(string Num) // пошук
        {
            int N;
            try
            {
                N = Convert.ToInt16(Num); // перетворення номера рядка в int16 для відображення
            }
            catch
            {
                MessageBox.Show("помилка пошукового запиту"); // Виведення на екран повідомлення "помилка пошукового запиту"
                return;
            }

            try
            {
                if (!File.Exists(this.OpenFileName))
                {
                    MessageBox.Show("файлу немає"); // Виведення на екран повідомлення "файлу немає"
                    return;
                }
                Stream S; // створення потоку
                S = File.Open(this.OpenFileName, FileMode.Open); // відкриття файлу
                Buffer D;
                object O; // буферна змінна для контролю формату
                BinaryFormatter BF = new BinaryFormatter(); // створення об'єкта для форматування

                while (S.Position < S.Length)
                {
                    O = BF.Deserialize(S);
                    D = O as Buffer;
                    if (D == null) break;
                    if (D.Key == N) // перевірка дорівнює чи номер пошуку номеру рядка в таблиці
                    {
                        string ST;
                        ST = "Запис містить:" + (char)13 + "No" + Num + "Вхідні дані:" + D.Data + "Результат:" + D.Result;

                        MessageBox.Show(ST, "Запис знайдена"); // Виведення на екран повідомлення "запис містить", номер, вхідних даних і результат
                        S.Close();
                        return;
                    }
                }
                S.Close();
                MessageBox.Show("Запис не знайдена"); // Виведення на екран повідомлення "Запис не знайдена"
            }
            catch
            {
                MessageBox.Show("Помилка файлу"); // Виведення на екран повідомлення "Помилка файлу"
            }
        } // Find закінчився


        public void SaveToFile() // Запис даних до файлу
        {
            if (!this.Modify)
                return;
            try
            {
                Stream S;
                if (System.IO.File.Exists(this.SaveFileName)) // існує файл?
                    S = System.IO.File.Open(this.SaveFileName, FileMode.Append); // Відкриття збереженого файлу
                else
                    S = System.IO.File.Open(this.SaveFileName, FileMode.Create); // створення нового файлу

                Buffer D = new Buffer();
                D.Data = this.Data;
                D.Result = Convert.ToString(this.Result);
                D.Key = Key;
                Key++;
                BinaryFormatter BF = new BinaryFormatter();
                BF.Serialize(S, D);
                S.Flush(); // очищення буфера потоку
                S.Close(); // закриття потоку
                this.Modify = false; // Заборона повторного запису
            }
            catch
            {
                Console.WriteLine("Помилка роботи з файлом"); // Виведення на екран повідомлення "Помилка роботи з файлом"
            }
        }
        public bool SaveFileNameExists()
        {
            if (this.SaveFileName == null)
                return false;
            else return true;
        }
        public void NewRec() // новий запис
        {
            this.Data = ""; // "" - ознака порожнього рядка
            this.Result = null; // для рядка - null
            this.Key = 0; // очищення поля ключа
            this.SaveFileName = null; // очищення поля імені файлу для збереження
            this.OpenFileName = null; // очищення поля імені відкритого файлу
            this.Modify = false; // скидання прапорця модифікації
        }
        public void Generator() // метод формування ключового поля
        {
            try
            {
                if (!File.Exists(this.SaveFileName)) // існує файл?
                {
                    Key = 1;
                    return;
                }
                Stream S; // створення потоку
                S = File.Open(this.SaveFileName, FileMode.Open); // Відкриття
                object O; // буферна змінна для контролю формату
                BinaryFormatter BF = new BinaryFormatter(); // створення елемента для форматування
                while (S.Position < S.Length)
                {
                    O = BF.Deserialize(S);
                    D = O as Buffer;
                    if (D == null) break;
                    Key = D.Key;
                }
                Key++;
                S.Close();
            }
            catch
            {
                MessageBox.Show("Помилка файлу"); // Виведення на екран повідомлення "Помилка файлу"
            }
        }
        public void SetTime()
        {
            this.TimeBegin = DateTime.Now;
        }

        public DateTime GetTime()
        {
            return this.TimeBegin;
        }

        public void Write(string D) // метод запису даних в об'єкт.
        {
            this.Data = D;
        }

        public string Read()
        {
            return this.Result; // метод відображення результату
        }

        // У методі Task реалізується завдання: якщо кількість введених цифр більше 5,
        // то результат = true, інакше false.
        public void Task() // метод реалізації програмного завдання
        {
            if (this.Data.Length > 5)
            {
                this.Result = Convert.ToString(true);
            }
            else
            {
                this.Result = Convert.ToString(false);
            }
            this.Modify = true; // Дозвіл запису
        }
    }
}