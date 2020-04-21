using System;
using System.Collections.Generic;
using System.Text;

namespace ArtificialIntelligence
{
    public class BackTracking
    {
        public int NQuens { get; set; }

        public int[,] table { get; set; }

        public BackTracking(int NQuens, int[,] table)
        {
            this.NQuens = NQuens;
            this.table = table;
        }


        bool isSafe(int[,] board, int row, int col)
        {
            int i, j;
            for (i = 0; i < col; i++)
                if (board[row, i] == 1)
                    return false;

            for (i = row, j = col; i >= 0 &&
                 j >= 0; i--, j--)
                if (board[i, j] == 1)
                    return false;

            for (i = row, j = col; j >= 0 &&
                          i < NQuens; i++, j--)
                if (board[i, j] == 1)
                    return false;

            return true;
        }

        bool solveNQUtil(int[,] board, int col)
        {
            if (col >= NQuens)
                return true;
            for (int i = 0; i < NQuens; i++)
            {
                if (isSafe(board, i, col))
                {
                    if (board[i, col] != -1)
                    {
                        board[i, col] = 1;
                        if (solveNQUtil(board, col + 1) == true)
                            return true;
                        board[i, col] = 0;
                    }
                }
            }
            return false;
        }
        public bool solveNQ()
        {

            if (solveNQUtil(table, 0) == false)
            {
                Console.Write("Solution does not exist");
                return false;
            }
            PrintBoard();
            return true;
        }

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
