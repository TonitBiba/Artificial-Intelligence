using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Security.Principal;
using System.Text;

namespace Heuristic_D4_
{
    public class AStar
    {
        public int numberOfSteps { get; set; }

        private List<State> OpenList { get; set; } //Nyjet e vizituara por jo te zgjeruara

        private List<State> ClosedList { get; set; } // Nyjet e vizituara dhe te zgjeruara

        private Heuristic heuristic { get; set; } //Heuristiken

        private int[] InitialState { get; set; }

        private int[] GoalState { get; set; }

        private int Size { get; set; }

        public AStar(int[] InitialState, int[] GoalState, Heuristic heuristic, int Size)
        {
            this.InitialState = InitialState;
            this.GoalState = GoalState;
            this.heuristic = heuristic;
            this.Size = Size;

            OpenList = new List<State>();
            ClosedList = new List<State>();
        }

        public ReturnType Solve()
        {
            DateTime startDate = DateTime.Now;
            double heuristic = CalculateHeuristic(InitialState);
            OpenList.Add(new State { Board = InitialState, Cost = 0, Heuristic = heuristic, EvalutionFunction = heuristic }); //Kosto ose g(n) eshte zero per rastin e parë

            while (OpenList.Count > 0) //Perderisa ka elemente qe ende nuk jane zgjeruar vazhdo....
            {
                numberOfSteps++;
                State stateToConsider = OpenList.OrderBy(t => t.EvalutionFunction).FirstOrDefault(); // Na garanton se gjith e merr gjendjen me vleren minimale
                if (IsGoalState(stateToConsider))
                    return new ReturnType { NumberOfSteps = numberOfSteps, RunningTime = (DateTime.Now - startDate).TotalMilliseconds, NumberOfGeneratedNodes = OpenList.Count + ClosedList.Count, Depth = stateToConsider.Cost } ;

                OpenList.Remove(stateToConsider);
                ClosedList.Add(stateToConsider);
                List<State> neighbors = GenerateNeighborhood(stateToConsider);
                foreach (State child in neighbors)
                {
                    if (Exist(child.Board, ClosedList)) // Nese eshte ne listen e nyjeve te vizituara kaloje kete femije.
                        continue;

                    if (Exist(child.Board, OpenList) && child.Cost < stateToConsider.Cost) //Pruning
                        OpenList.Remove(child);

                    if (Exist(child.Board, ClosedList) && stateToConsider.Heuristic + stateToConsider.Cost < child.Heuristic + child.Cost)
                        ClosedList.Remove(child);

                    if (!(Exist(child.Board, OpenList) && Exist(child.Board, ClosedList)))
                        OpenList.Add(new State { Board = child.Board, Cost = child.Cost, Heuristic = child.Heuristic, EvalutionFunction = child.Heuristic + child.Cost });
                }
            }
            return null;
        }

        public void Print(State state)
        {
            Console.WriteLine();
            for (int i = 0; i < state.Board.Length; i++)
                Console.Write(state.Board[i] + " ");
            Console.Write(" G(n)=" + state.Cost + ", H(n)=" + state.Heuristic);
        }

        public List<State> GenerateNeighborhood(State state)
        {
            List<State> states = new List<State>();
            for (int i = 0; i < InitialState.Length; i++)
                if (state.Board[i] == 0)
                {
                    if (i % Size == 0 || i % Size == 1)
                    { //E sigurte levizja ne anen e djathte.
                        int[] newBoard = Clone(state.Board);
                        newBoard[i] = state.Board[i + 1];
                        newBoard[i + 1] = state.Board[i];
                        states.Add(new State { Board = newBoard, Cost = state.Cost + 1, Heuristic = CalculateHeuristic(newBoard) });
                    }

                    if (i % Size > 0)
                    { //E sigurte levizja ne anen e majte.
                        int[] newBoard = Clone(state.Board);
                        newBoard[i] = state.Board[i - 1];
                        newBoard[i - 1] = state.Board[i];
                        states.Add(new State { Board = newBoard, Cost = state.Cost + 1, Heuristic = CalculateHeuristic(newBoard) });
                    }

                    if (i > 2)
                    { //E sigurte levizja larte.
                        int[] newBoard = Clone(state.Board);
                        newBoard[i] = state.Board[i - Size];
                        newBoard[i - Size] = state.Board[i];
                        states.Add(new State { Board = newBoard, Cost = state.Cost + 1, Heuristic = CalculateHeuristic(newBoard) });
                    }

                    if (i < Size * Size - Size)
                    { //E sigurte levizja poshte.
                        int[] newBoard = Clone(state.Board);
                        newBoard[i] = state.Board[i + Size];
                        newBoard[i + Size] = state.Board[i];
                        states.Add(new State { Board = newBoard, Cost = state.Cost + 1, Heuristic = CalculateHeuristic(newBoard) });
                    }
                }
            return states;
        }

        public double CalculateHeuristic(int[] board)
        {
            double calculatedHeuristic = 0;
            switch (heuristic)
            {
                case Heuristic.MisplacedTiles:
                    calculatedHeuristic = MisplacedTiles(board);
                    break;
                case Heuristic.ManhattanDistance:
                    calculatedHeuristic = ManhattanDistance(board);
                    break;
                case Heuristic.OutOfRowAndColumn:
                    calculatedHeuristic = OutOfRowAndColumn(board);
                    break;
                case Heuristic.EuclideanDistance:
                    calculatedHeuristic = EuclideanDistance(board);
                    break;
                case Heuristic.Gaschnig:
                    calculatedHeuristic = Gaschnig(board);
                    break;
            }
            return calculatedHeuristic;
        }

        public double MisplacedTiles(int[] board)
        {
            double misplacedTiles = 0;
            for (int i = 0; i < InitialState.Length; i++)
                if (board[i] != GoalState[i])
                    misplacedTiles++;
            return misplacedTiles;
        }

        public double ManhattanDistance(int[] board)
        {
            double calculatedDistance = 0;
            for (int i = 0; i < board.Length; i++)
                calculatedDistance += double.Parse(Math.Abs(Array.IndexOf(board, board[i]) - Array.IndexOf(GoalState, board[i])).ToString());
            return calculatedDistance;
        }

        public double OutOfRowAndColumn(int[] board)
        {
            double calculateHeuristic = 0;
            for (int i = 0; i < board.Length; i++)
            {
                if (Array.IndexOf(board, board[i]) % Size != Array.IndexOf(GoalState, board[i])) //Nuk eshte ne kolonen e njejte
                    calculateHeuristic++;

                if (i < Size)
                    if (Array.IndexOf(GoalState, board[i]) >= Size)
                        calculateHeuristic++;
                    else if (i < Size * Size - Size)
                    {
                        if (Array.IndexOf(GoalState, board[i]) >= Size * Size - Size || Array.IndexOf(GoalState, board[i]) < Size)
                            calculateHeuristic++;
                    }
                    else
                        if (Array.IndexOf(GoalState, board[i]) < Size * Size - Size)
                        calculateHeuristic++;
            }
            return calculateHeuristic;
        }

        public double EuclideanDistance(int[] board)
        {
            double calculateDistance = 0;

            for (int i = 0; i < board.Length; i++)
            {
                var coordinates = getCoordinates(board, board[i]);
                var coordinatesGoalState = getCoordinates(GoalState, board[i]);
                calculateDistance += Math.Sqrt(Math.Pow(coordinatesGoalState[0] - coordinates[0], 2) + Math.Pow(coordinatesGoalState[1] - coordinates[1], 2));
            }
            return calculateDistance;
        }

        public double Gaschnig(int[] board)
        {
            double numberOfMoves = 0;

            State stateToCheck = new State { Board = Clone(board) };

            while (!IsGoalState(stateToCheck))
            {
                int zeroIndex = Array.IndexOf(stateToCheck.Board, 0);
                if (GoalState[zeroIndex] != 0)
                {
                    int sv = GoalState[zeroIndex];
                    int ci = Array.IndexOf(stateToCheck.Board, sv);
                    (stateToCheck.Board[ci], stateToCheck.Board[zeroIndex]) = (stateToCheck.Board[zeroIndex], stateToCheck.Board[ci]);
                }
                else
                {
                    for (int i = 0; i < Size * Size; i++)
                    {
                        if (GoalState[i] != stateToCheck.Board[i])
                        {
                            (stateToCheck.Board[i], stateToCheck.Board[zeroIndex]) = (stateToCheck.Board[zeroIndex], stateToCheck.Board[i]);
                            break;
                        }
                    }
                }
                numberOfMoves++;
            }
            return numberOfMoves;
        }

        public int[] getCoordinates(int[] board, int value)
        {
            int row = 0, column = 0;
            column = Array.IndexOf(board, value) % Size;
            row = Array.IndexOf(board, value) / Size;
            return new int[] { row, column };
        }

        public bool IsGoalState(State state)
        {
            for (int i = 0; i < InitialState.Length; i++)
                if (GoalState[i] != state.Board[i])
                    return false;
            return true;
        }

        public int[] Clone(int[] board)
        {
            int[] newBoard = new int[InitialState.Length];
            for (int i = 0; i < InitialState.Length; i++)
                newBoard[i] = board[i];
            return newBoard;
        }

        public bool Exist(int[] board, List<State> states)
        {
            int numberOfCompares;
            foreach (var item in states)
            {
                numberOfCompares = 0;
                for (int i = 0; i < InitialState.Length; i++)
                    if (board[i] == item.Board[i])
                        numberOfCompares++;
                if (numberOfCompares == InitialState.Length)
                    return true;
            }
            return false;
        }
    }

    public class State
    {
        public int[] Board { get; set; }

        public double Cost { get; set; }

        public double Heuristic { get; set; }

        public double EvalutionFunction { get; set; }
    }

    public class ReturnType
    {
        public int NumberOfGeneratedNodes { get; set; }

        public int NumberOfSteps { get; set; }

        public double RunningTime { get; set; }

        public double Depth { get; set; }
    }

    public enum Heuristic
    {
        MisplacedTiles = 1,
        ManhattanDistance = 2,
        OutOfRowAndColumn = 3,
        EuclideanDistance = 4,
        Gaschnig = 5
    }
}