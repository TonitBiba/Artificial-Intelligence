using System;
using System.Collections.Generic;
using System.Text;

namespace ArtificialIntelligence
{
    public class ItDFS
    {
        public int NQuens { get; set; }

        public int PlacedQuens { get; set; }

        public int[,] table { get; set; }

        public ItDFS(int NQuens, int[,] table)
        {
            this.NQuens = NQuens;
            this.table = table;
        }

        public bool isSafe(int column, int row)
        {
            for (int i = 0; 
                i < NQuens; i++)
            {
                if (table[i, column] == 1)
                    return false;

                if (table[row, i] == 1)
                    return false;

                if (row - i >= 0 && column - i >= 0)
                    if (table[row - i, column - i] == 1)
                        return false;

                if (row + i < NQuens && column - i >= 0)
                    if (table[row + i, column - i] == 1)
                        return false;
            }

            return true;
        }

        public bool FindSolution(int depth)
        {
            if(!SolveProblem(0, depth))
                if (!FindSolution(depth+1))
                    return false;
            return true;
        }

        public bool SolveProblem(int column, int depth)
        {
            if (isGoal)
            {
                PrintBoard();
                return true;
            }
            for (int row = 0; row < NQuens; row++)
            {
                if(row<depth)
                if (isSafe(column, row))
                    if (table[row, column] != -1) //Check if cell is blocked
                    {
                        table[row, column] = 1;
                        PlacedQuens++;
                        if (SolveProblem(column + 1, depth))
                            return true;
                        table[row, column] = 0;
                        PlacedQuens--;
                    }
            }
            return false;
        }

        public bool isGoal => NQuens == PlacedQuens;

        public void PrintBoard()
        {
            Console.Write("Zgjidhja problemit");
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
