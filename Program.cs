using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Program do sortowania liczb przy uzyciu zadaniowo algorytmu merge sort");
            Console.WriteLine("Autorzy: Marek Udziela i Michał Redek\n\n");
            Console.WriteLine("Każda liczba musi być wpisana po przecinku i bez spacji (program nie sprawdza poprawności wpisywanych danych)");
            Console.WriteLine("Przykład: 1,51,88,761,13");
            Console.WriteLine("Algorytm nie ma sensu dla ilosci liczb mniejszej niz 3 ;)");
            Console.WriteLine("Bardzo i to bardzo proszę wpisać liczby:");
            string dane_inpucikowe = Console.ReadLine();
            List<int> parts = new List<int>();
            parts = dane_inpucikowe.Split(',').Select(Int32.Parse).ToList();
            Console.WriteLine("\n\n\nPosortowane liczby: \n");
            Program.ThreadedMergeSort(parts).ForEach(i => Console.Write("{0}\t", i));
            Console.WriteLine("\n\nNacisnij enter aby zakonczyc dzialanie program... \n");
            string dane_inpucikowe2 = Console.ReadLine();
        }

        static List<T> ThreadedMergeSort<T>(List<T> inpucik) where T : IComparable
        {
            //Policzenie ilosci w liscie
            var length = inpucik.Count;
            //Jak lista jest mniejsza niz dwa to nic nie modyfikuj...
            if (length < 2)
                return inpucik;

            List<T> Wschod, Zachod;
            //Program dziala jak lista jest wieksza niz 2, bo po co komu sortowanie dwoch liczb...
            if (length > 2)
            {
                var LeweZadanko = Task<List<T>>.Factory.StartNew(() => { return ThreadedMergeSort(inpucik.GetRange(0, length / 2)); });
                var PraweZadanko = Task<List<T>>.Factory.StartNew(() => { return ThreadedMergeSort(inpucik.GetRange(length / 2, length - length / 2)); });


                LeweZadanko.Wait();
                PraweZadanko.Wait();


                Wschod = LeweZadanko.Result;
                Zachod = PraweZadanko.Result;
            }
            else
            {
                Wschod = ThreadedMergeSort(inpucik.GetRange(0, length / 2));
                Zachod = ThreadedMergeSort(inpucik.GetRange(length / 2, length - length / 2));
            }


            var result = new List<T>();
            for (int WschodIndex = 0, WschodLength = Wschod.Count, ZachodLength = Zachod.Count, ZachodIndex = 0; WschodIndex + ZachodIndex < length;)
            {
                if (ZachodIndex >= ZachodLength || WschodIndex < WschodLength && Wschod[WschodIndex].CompareTo(Zachod[ZachodIndex]) <= 0)
                    result.Add(Wschod[WschodIndex++]);
                else
                    result.Add(Zachod[ZachodIndex++]);
            }



            return result;
        }
    }
}
