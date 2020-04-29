using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;

namespace LudoKing_D6_.General
{
    public class Ludo
    {
        public State tempState { get; set; }

        public Ludo(State state)
        {
            tempState = new State
            {
                Min = CopyArray(state.Min),
                Max = CopyArray(state.Max),
                MaxFinished = state.MaxFinished,
                MaxPlaced = state.MaxPlaced,
                MinFinished = state.MinFinished,
                MiniMaxValue = state.MiniMaxValue,
                MinPlaced = state.MinPlaced
            };
        }

        public int[] CopyArray(int[] array)
        {
            int[] newArray = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
                newArray[i] = array[i];
            return newArray;
        }

        public int MaxValue()
        {
            //Check if terminal state
            if (tempState.MaxFinished == 4 || tempState.MinFinished == 4)
                return tempState.MaxFinished == 4 ? 1 : -1;

            int maxValue = int.MinValue;
            int[] possibleDiceVal = new int[6] { 1, 2, 3, 4, 5, 6 };
            foreach (int dice in possibleDiceVal)
            {
                //MAX turn
                if (tempState.MaxPlaced == 0 && dice == 6)
                {
                    tempState.Max[0] = 1;
                    tempState.MaxPlaced++;
                }

                else if (tempState.MaxPlaced == 0 && dice != 6)
                {
                    continue;
                }

                else if (Array.IndexOf(tempState.Max, 1) + dice == 56)
                {
                    tempState.MaxPlaced--;
                    tempState.Max[Array.IndexOf(tempState.Max, 1) + dice] = 0;
                    tempState.MaxFinished++;
                    if (tempState.MaxFinished == 4)
                        return 1;
                }
                else
                {
                    if (Array.IndexOf(tempState.Max, 1) + dice < 56)
                    {
                        int indexOfOne = Array.IndexOf(tempState.Max, 1);
                        tempState.Max[indexOfOne] = 0;
                        tempState.Max[indexOfOne + dice] = 1;
                    }
                }
                return Math.Max(maxValue, MinValue());
            }
            return 0;
        }

        public int MinValue()
        {
            //Check if terminal state
            if (tempState.MaxFinished == 4 || tempState.MinFinished == 4)
                return tempState.MinFinished == 4 ? -1 : 1;

            int minVal = int.MaxValue; //Suppozing that this is infinity for our case. For the worst case this is okay, in other this makes no sence TB.
            int[] possibleDiceVal = new int[6] { 1, 2, 3, 4, 5, 6 };
            foreach (var dice in possibleDiceVal)
            {
                if (tempState.MinPlaced == 0 && dice == 6)
                {
                    tempState.Min[0] = 1;
                    tempState.MinPlaced++;
                }

                else if (tempState.MinPlaced == 0 && dice != 6)
                {
                    continue;
                }
                else if (Array.IndexOf(tempState.Min, 1) + dice == 56)
                {
                    tempState.MinPlaced--;
                    tempState.Min[Array.IndexOf(tempState.Min, 1) + dice] = 0;

                    tempState.MinFinished++;

                    if (tempState.MinFinished == 4)
                        return -1;
                }
                else
                {
                    if (Array.IndexOf(tempState.Min, 1) + dice < 56)
                    {
                        int indexOfOne = Array.IndexOf(tempState.Min, 1);
                        tempState.Min[indexOfOne] = 0;
                        tempState.Min[indexOfOne + dice] = 1;
                    }
                }
                return Math.Min(minVal, MaxValue());
            }
            return 0;
        }


        public List<int[]> FindRowColumn(State state, int nCase)
        {
            List<int[]> gamePositions = new List<int[]>();
            if (nCase == 1)
            {
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 57; j++)
                        if (state.Max[i, j] == 1)
                            gamePositions.Add(new int[] { i, j });
            }
            else
            {
                for (int k = 0; k < 4; k++)
                    for (int t = 0; t < 57; t++)
                        if (state.Min[k, t] == 1)
                            gamePositions.Add(new int[] { k, t });
            }
            return gamePositions;
        }


        public int[] BestMove(State state, int dice, int nCase, int MiniMaxScore)
        {
            List<int[]> gamePositionsMax = FindRowColumn(state, 1);
            List<int[]> gamePositionsMin = FindRowColumn(state, 2);
            if (nCase == 1)
            {
                foreach (var maxPosition in gamePositionsMax)
                {
                    if (MiniMaxScore == 1)
                        return gamePositionsMax[gamePositionsMax.IndexOf(maxPosition)];
                    else
                    {
                        foreach (var minPosition in gamePositionsMin)
                        {
                            for (int i = 26; i < 57; i++)
                            {
                                if (minPosition[i] == 1 &&  maxPosition[i-26]==1)
                                {
                                    state.Min[gamePositionsMin.IndexOf(minPosition), i]=0;
                                    return gamePositionsMax[gamePositionsMax.IndexOf(maxPosition)];
                                }
                            }
                        }
                    }
                }
                return gamePositionsMax[0];
            }
            else
            {
                foreach (var minPosition in gamePositionsMin)
                {
                    if (MiniMaxScore == -1)
                        return gamePositionsMin[gamePositionsMin.IndexOf(minPosition)];
                    else
                    {
                        foreach (var maxPosition in gamePositionsMax)
                        {
                            for (int i = 0; i < 27; i++)
                            {
                                if (maxPosition[i] == 1 && minPosition[i + 26] == 1)
                                {
                                    state.Max[gamePositionsMax.IndexOf(maxPosition), i] = 0;
                                    return gamePositionsMin[gamePositionsMin.IndexOf(minPosition)];
                                }
                            }
                        }
                    }
                }
                return gamePositionsMin[0];
            }
        }
    }
}