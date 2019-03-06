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
            Console.WriteLine("Started");
            Bot.OnMessage += Bot_OnMessage;
            Bot.OnMessageEdited += Bot_OnMessage;

            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
            return;
            //List<string> lines = new List<string>();

            //while (true) {
            //    string inp = Console.ReadLine();
            //    if (inp == "GO")
            //        break;
            //    lines.Add(inp);
            //}
            //List<int> omega = lines[0].Split(' ').Select(Int32.Parse).ToList();
            //lines.RemoveAt(0);

            //Dictionary<char, IEnumerable<int>> sets = new Dictionary<char, IEnumerable<int>>();
            //foreach (var l in lines) {
            //    var el = l.Split(' ');
            //    sets.Add(el[0][0], el.Skip(1).Select(Int32.Parse).ToList());
            //}

            //while (true) {
            //    string formula = Console.ReadLine();
            //    var res = VennSolver.Solve(formula, omega, sets);
            //    Console.WriteLine(String.Join(" ", res));
            //}

        }

        private static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e) {
            string res = "";
            if (e.Message.Text == "/start" || e.Message.Text == "/help") {
                res = @"Inserire i dati nel seguente modo:
1 riga: omega,
n righe: i vari insiemi,
ultima riga: l'equazione o l'espressione

Tutti gli insiemi dovranno contenere solo numeri.
Gli insiemi avranno il nome di formato da un char.
Per inserire omega nell'equazione usare O (o maiuscola) e per l'insieme vuoto usare o (o minuscola).
Le operazioni sono le seguenti:
+ -> unione
- -> intersezione
* -> quella col triangolo
^ -> complementare

Non c'è nessun tipo di controllo se sbagliate ad inserire non vi diro perchè, perchè tutto il programma è in un try catch perchè fare i controlli è faticoso

Per cambiare solo l'espressione finale bastera modificare il messaggio

Esempio:
1 2 3 4 5 6 7 8 9 10
A 1 2 3
B 3 4 5
C 9 10
(A+B^)-C";
            } else {

                try {

                    List<string> lines = e.Message.Text.Split('\n').ToList();

                    List<int> omega = lines[0].Split(' ').Select(Int32.Parse).ToList();
                    lines.RemoveAt(0);

                    string formula = lines[lines.Count - 1].Replace(" ", "");
                    lines.RemoveAt(lines.Count - 1);

                    Dictionary<char, IEnumerable<int>> sets = new Dictionary<char, IEnumerable<int>>();
                    foreach (var l in lines) {
                        var el = l.Split(' ');
                        sets.Add(el[0][0], el.Skip(1).Select(Int32.Parse).ToList());
                    }

                    res = VennSolver.Solve(formula, omega, sets);

                } catch (Exception) {
                    res = "Mi sono rotto :/";
                }


            }
            Bot.SendTextMessageAsync(e.Message.Chat.Id, res);
            Console.WriteLine(e.Message.From);
            Console.WriteLine(e.Message.Text);
            Console.WriteLine();


        }
    }
}
