using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace VennBot
{
    class Program
    {
        private static readonly TelegramBotClient Bot = new TelegramBotClient("760942295:AAG3bKAaq45TI34KfSiSxgHozwmhsITJsBU");

        static void Main(string[] args) {

            List<string> lines = new List<string>();

            while (true) {
                string inp = Console.ReadLine();
                if (inp == "GO")
                    break;
                lines.Add(inp);
            }
            List<int> omega = lines[0].Split(' ').Select(Int32.Parse).ToList();
            lines.RemoveAt(0);

            Dictionary<char, IEnumerable<int>> sets = new Dictionary<char, IEnumerable<int>>();
            foreach (var l in lines) {
                var el = l.Split(' ');
                sets.Add(el[0][0], el.Skip(1).Select(Int32.Parse).ToList());
            }

            while (true) {
                string formula = Console.ReadLine();
                var res = VennSolver.Solve(formula, omega, sets);
                Console.WriteLine(String.Join(" ", res));
            }





            return;
            Console.WriteLine("Started");
            Bot.OnMessage += Bot_OnMessage;

            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e) {

            List<string> lines = e.Message.Text.Split('\n').ToList();

            List<int> omega = lines[0].Split(' ').Select(Int32.Parse).ToList();
            lines.RemoveAt(0);

            string formula = lines[lines.Count - 1].Replace(" ", "");
            lines.RemoveAt(lines.Count - 1);

            Dictionary<char, List<int>> sets = new Dictionary<char, List<int>>();
            foreach (var l in lines) {
                var el = l.Split(' ');
                sets.Add(el[0][0], el.Skip(1).Select(Int32.Parse).ToList());
            }



        }
    }
}
