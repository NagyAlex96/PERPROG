using System;
using System.Threading;
using System.Threading.Tasks;

namespace _02_Ora_Async_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Kiir();
            Action a = async () => { Console.WriteLine(await AsyncAnswer()); };

            Console.WriteLine("Először ez!");
            Console.ReadKey();
        }

        static async void Kiir()
        {
            Console.WriteLine(await AsyncAnswer());
        }

        static string Answer()
        {
            Thread.Sleep(3000);

            return "Hello world!";
        }

        static async Task<string> AsyncAnswer()
        {
            await Task.Delay(3000);
            return "Hello world!";
        }
    }
}
