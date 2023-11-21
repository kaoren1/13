using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PR8;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Timers;

namespace PR8
{
    class Data
    {
        public string Name {  get; }
        public int SM { get; }
        public int SS { get; }
        public Data(string name, int sm, int ss)
        {
            this.Name = name;
            this.SM = sm;
            this.SS = ss;
        }
    }
    internal class Program
    {
        public static Data data {  get; set; }
        public static void Main()
        {
            Console.WriteLine("Введите имя таблицы: ");
            string name = Console.ReadLine();
            Console.Clear();
            string text = "Она несла желтые цветы! Нехороший цвет. Она повернула с Тверской в переулок и тут обернулась. Ну,\nТверскую вы знаете? По Тверской шли тысячи людей, но я вам ручаюсь, что увидела она меня одного и\nпоглядела не то что тревожно, а даже как будто болезненно. И меня поразила не столько ее красота,\nсколько необыкновенное, никем не виданное одиночество в глазах! Повинуясь этому желтому знаку, я тоже \nсвернул в переулок и пошел по ее следам. Мы шли по кривому, скучному переулку безмолвно, я по одной \nстороне, а она по другой. И не было, вообразите, в переулке ни души. Я мучился, потому что мне \nпоказалось, что с нею необходимо говорить, и тревожился, что я не вымолвлю ни одного слова, а она уйдет,\nи я никогда ее более не увижу...";
            Console.WriteLine(text);
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("Нажмите Enter, чтобы начать тестирование");
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.Enter)
                Vvod.Vvod1(name);
        }
    }
    class Vvod
    {
        public static bool stopWatch = false;
        public static bool stop = false;
        public static int a = 0;
        public static void Vvod1(string o)
        {
            //таймер
            Thread timerThread = new Thread(new ThreadStart(Vvod2));
            timerThread.Start();
            //ввод
            Console.WriteLine("Введите текст и нажмите Enter:");
            string inputText = "";
            int cursorLeft = Console.CursorLeft;
            int cursorTop = Console.CursorTop;
            while (a == 0)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    a = 1;
                    stop = true;
                }
                Console.SetCursorPosition(cursorLeft + 1, cursorTop + 1);
                inputText += keyInfo.KeyChar;
                Console.Write(inputText);
            } 
            int len = inputText.Length - 1;
            int SM = len;
            int SS = len/60;
            Tabl.Result(o, SM, SS);
        }
        public static void Vvod2()
        {
            for (int i = 60; i >= 0; i--)
            {
                if (stop)
                {
                    stopWatch = true;
                    break;
                }
                Console.SetCursorPosition(0, 11);
                Console.WriteLine("Оставшееся время: " + i);
                if (i == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Время закончилось. Ввод текста запрещен. Нажмите любую клавишу для продолжения");
                    break;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
    static class Tabl
    {
        public static void Result(string p, int p1, int p2)
        {
            Console.Clear();
            //запись
            string path = "Таблица рекордов.json";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = new StreamWriter(path, false, Encoding.Unicode)) { }
            }
            Data data = new Data(p, p1, p2);
            string json = JsonConvert.SerializeObject(data);
            File.AppendAllText(path, json + Environment.NewLine, Encoding.Unicode);
            //вывод
            using (StreamReader sr = new StreamReader(path, Encoding.Unicode))
            {
                Console.WriteLine("Таблица рекордов\n");
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    JObject obj = JObject.Parse(line);
                    Console.WriteLine("Имя: " + obj["Name"]);
                    Console.WriteLine("Символы в минуту: " + obj["SM"]);
                    Console.WriteLine("Символы в секунду: " + obj["SS"] + "\n");
                }
            }
            Console.WriteLine("Для повторного прохождения теста нажмите Enter, для выхода Escape");
            ConsoleKeyInfo r = Console.ReadKey();
            if (r.Key == ConsoleKey.Enter)
            {
                Console.Clear();
                Program.Main();
            }
            else if (r.Key == ConsoleKey.Escape)
            {
                Console.WriteLine("Завершение программы");
                Thread.Sleep(1000);
                Console.Clear();
                Environment.Exit(0);
            }
        }
    }
}