using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TextAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Напишите полный путь к файлу.");
            Console.WriteLine(@"(C:\каталог\название_файла.txt)");
            Console.WriteLine("");

            while (true)
            {
                try
                {
                    string path = Console.ReadLine();
                    if (Path.GetExtension(path) == ".txt")
                    {
                        AnalyzeTextAsync(path);
                    }
                    else
                    {
                        throw new FileNotFoundException();
                    }

                }
                catch (FileNotFoundException fileNotFound)
                {
                    Console.WriteLine("Текстовый файл не найден.");
                }
            }

            Console.Read();
        }

        private static async Task AnalyzeTextAsync(string path)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var result = await Task.Run(() => GetMostFrequentTriplets(path));

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Число повторений: " + result[i].Item1 + " Триплет: '" + result[i].Item2 + "'");
            }

            stopWatch.Stop();
            Console.WriteLine("Время выполнения в миллисекундах: " + stopWatch.Elapsed.TotalMilliseconds.ToString());
        }

        public static List<Tuple<int, string>> GetMostFrequentTriplets(string path)
        {
            List<Tuple<int, string>> tripletAnalysResult = new List<Tuple<int, string>>();
            using (StreamReader streamReader = new StreamReader(path))
            {
                string textFromFile = streamReader.ReadToEnd();
                for (int i = 0; i < textFromFile.Length - 3; i++)
                {
                    string triplet = textFromFile[i].ToString() + textFromFile[i + 1].ToString() + textFromFile[i + 2].ToString();

                    if (tripletAnalysResult.Exists(x => x.Item2 == triplet) == false)
                    {
                        tripletAnalysResult.Add(Tuple.Create(1, triplet));
                    }
                    else
                    {
                        int tripletIndex = tripletAnalysResult.FindIndex(x => x.Item2 == triplet);
                        tripletAnalysResult[tripletIndex] = Tuple.Create(tripletAnalysResult[tripletIndex].Item1 + 1, tripletAnalysResult[tripletIndex].Item2);
                    }
                }
                tripletAnalysResult.Sort((x, y) => y.Item1.CompareTo(x.Item1));
            }
            return tripletAnalysResult;
        }
    }
}
