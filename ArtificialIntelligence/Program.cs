using Microsoft.VisualBasic;
using System;

namespace ArtificialIntelligence
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Shtypni numrin e mbretereshave:");
            string numberOfQueensStr = Console.ReadLine();
            int numberOfQueens = int.Parse(numberOfQueensStr);

            int[,] table = new int[numberOfQueens, numberOfQueens];

            Console.WriteLine("Shtypni fushat e bllokuara në formatin x,y. Shtypni q per te perfunduar.");
            while (true)
            {
                string bllockedField = Console.ReadLine();
                if (bllockedField == "q")
                    break;
                var blockedCoordinates = bllockedField.Split(",");
                table[int.Parse(blockedCoordinates[0]), int.Parse(blockedCoordinates[1])] = -1;
            }

            PrintTable("Gjendja fillestare", table);

            string inputCharacter;
            do
            {
                CleanTable(numberOfQueens, table);

                Console.WriteLine("**********************************************************");
                Console.WriteLine("Zgjedhi algoritmin me te cilin doni te zgjidhni problemin:\n" +
                    "1. DFS;\n" +
                    "2.Iterative DFS\n" +
                    "3.Backtrack DFS\n" +
                    "q për të përfunduar.");
                inputCharacter = Console.ReadLine();
                switch (inputCharacter)
                {
                    case "1":
                        DFS dFS = new DFS(numberOfQueens, table);
                        var solution = dFS.SolveProblem(0);
                        if (!solution)
                            Console.WriteLine("Nuk eshte gjetur zgjidhje me parametrat e dhënë");
                        break;
                    case "2":
                        ItDFS itDFS = new ItDFS(numberOfQueens, table);
                        if (!itDFS.FindSolution(1))
                            Console.WriteLine("Nuk eshte gjetur zgjidhje me parametrat e dhënë");
                        break;
                    case "3":
                        BackTracking backTracking = new BackTracking(numberOfQueens, table);
                        if (!backTracking.solveNQ())
                            Console.WriteLine("Nuk eshte gjetur zgjidhje me parametrat e dhënë");
                        break;
                }
            } while (inputCharacter != "q");
        }

        public static void CleanTable(int numberOfQueens, int[,] table)
        {
            for (int i = 0; i < numberOfQueens; i++)
                for (int j = 0; j < numberOfQueens; j++)
                    if (table[i, j] == 1)
                        table[i, j] = 0;
        }

        public static void PrintTable(string text, int[,] table)
        {
            Console.Write(text);
            for (int i = 0; i < table.GetLength(0); i++)
            {
                Console.WriteLine();
                for (int j = 0; j < table.GetLength(0); j++)
                {
                    Console.Write(table[i, j] + " ");
                }
            }
            Console.WriteLine();
        }
    }
}
