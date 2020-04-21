using System;
using System.Data.Common;

namespace ArtificialIntelligence
{
    public class DFS
    {
        public int NQuens { get; set; }

        public int PlacedQuens { get; set; }

        public int[,] table { get; set; }

        public DFS(int NQuens, int[,] table)
        {
            this.NQuens = NQuens;
            this.table = table;
        }

        public bool isSafe(int column, int row)
        {
            for (int i = 0; i < NQuens; i++)
            {
                if (table[i, column] == 1)
                    return false;

                if (table[row, i] == 1)
                    return false;

                if (row - i >= 0 && column - i >= 0)
                    if (table[row - i, column - i] == 1)
                        return false;

                if(row+i<NQuens && column - i >= 0)
                    if (table[row + i, column - i] == 1)
                        return false;
            }

                return true;
        }

        public bool SolveProblem(int column)
        {
            //Basic case
            if (isGoal)
            {
                PrintBoard();
                return true;
            }
            for (int row = 0; row < NQuens; row++)
            {
                if(isSafe(column, row))
                {
                    if (table[row, column] != -1) //Check if cell is blocked
                    {
                        table[row, column] = 1;
                        PlacedQuens++;
                        if (SolveProblem(column + 1))
                            return true;                        
                        table[row, column] = 0;
                        PlacedQuens--;
                    }
                }
            }
            return false;
        }

        public bool isGoal => NQuens == PlacedQuens;

        public void PrintBoard()
        {
            Console.Write("Zgjidhaj problemit");
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
