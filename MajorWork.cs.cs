using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

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

        public void WriteSaveFileName(string S) // метод запису даних до об'єкту
        {
            this.SaveFileName = S;
        }

        public void WriteOpenFileName(string S)
        {
            this.OpenFileName = S;
        }

        // Методи
        public void SaveToFile() // Запис даних до файлу
        {
            if (!this.Modify)
                return;
            try
            {
                Stream S;
                if (System.IO.File.Exists(this.SaveFileName)) // існує файл?.уточнити, який File ви маєте на увазі в коді.
                    S = System.IO.File.Open(this.SaveFileName, FileMode.Append); // Відкриття збереженого файлу.уточнити, який File ви маєте на увазі в коді.
                else
                    S = System.IO.File.Open(this.SaveFileName, FileMode.Create); // створення нового файлу.уточнити, який File ви маєте на увазі в коді.

                Buffer D = new Buffer();
                D.Data = this.Data;
                D.Result = Convert.ToString(this.Result);
                D.Key = Key;

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
