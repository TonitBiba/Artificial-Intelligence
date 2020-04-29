using LudoKing_D6_.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;

namespace LudoKing_D6_.General
{
    public class Ludo
    {
        public IHubContext<LudoHub> _ludoHub { get; set; }

        public State tempState { get; set; }

        public Ludo(State state, IHubContext<LudoHub> _ludoHub)
        {
            this._ludoHub = _ludoHub;

            tempState = new State
            {
                Min = CopyArray(state.Min),
                Max = CopyArray(state.Max),
                MaxFinished = state.MaxFinished,
                MinFinished = state.MinFinished
            };
        }

        public List<int[]> CopyArray(List<int[]> array)
        {
            List<int[]> newArray = new List<int[]>();
            foreach (var item in array)
            {
                int[] tempArray = new int[item.Length];
                for (int i = 0; i < item.Length; i++)
                    tempArray[i] = item[i];
                newArray.Add(tempArray);
            }
            return newArray;
        }

        public int MaxValue(State maxState)
        {
            try
            {
                //Check if terminal state
                if (maxState.MaxFinished == 4 || maxState.MinFinished == 4)
                    return maxState.MaxFinished == 4 ? 1 : -1;

                int maxValue = int.MinValue;
                int[] possibleDiceVal = new int[6] { 1, 2, 3, 4, 5, 6 };
                foreach (int dice in possibleDiceVal)
                {
                    //MAX turn
                    if (maxState.Max.Count == 0 && dice == 6)
                    {
                        maxState.Max.Add(new int[57]);
                        maxState.Max[0][0] = 1;
                    }

                    else if (maxState.Max.Count == 0 && dice != 6)
                        continue;

                    else
                    {
                        foreach (var player in maxState.Max)
                        {
                            int indexOfOne = Array.IndexOf(player, 1);
                            if (indexOfOne > -1)
                            {
                                if (indexOfOne + dice < 56)
                                {
                                    player[indexOfOne] = 0;
                                    player[indexOfOne + dice] = 1;
                                    break;
                                }
                                else if (indexOfOne + dice == 56)
                                {
                                    maxState.Max.RemoveAt(0);
                                    maxState.MaxFinished++;
                                    if (maxState.MaxFinished == 4)
                                        return 1;
                                    break;
                                }
                            }
                            else
                            {
                                maxState.Max.RemoveAt(0);
                                break;
                            }
                        }
                    }
                    return Math.Max(maxValue, MinValue(new State { MaxFinished = maxState.MaxFinished, Max = CopyArray(maxState.Max), Min = CopyArray(maxState.Min), MinFinished = maxState.MinFinished }));
                }
            }catch(Exception ex)
            {

            }
            return 0;
        }

        public int MinValue(State minState)
        {
            try
            {
                //Check if terminal state
                if (minState.MaxFinished == 4 || minState.MinFinished == 4)
                    return minState.MinFinished == 4 ? -1 : 1;

                int minVal = int.MaxValue; //Suppozing that this is infinity for our case. For the worst case this is okay, in other this makes no sence TB.
                int[] possibleDiceVal = new int[6] { 1, 2, 3, 4, 5, 6 };
                foreach (var dice in possibleDiceVal)
                {
                    if (minState.Min.Count == 0 && dice == 6)
                    {
                        minState.Min.Add(new int[57]);
                        minState.Min[0][0] = 1;
                    }

                    else if (minState.Min.Count == 0 && dice != 6)
                        continue;

                    else
                    {
                        foreach (var player in minState.Min)
                        {
                            int indexOfOne = Array.IndexOf(player, 1);
                            if (indexOfOne > -1)
                            {
                                if (indexOfOne + dice < 56)
                                {
                                    player[indexOfOne] = 0;
                                    player[indexOfOne + dice] = 1;
                                    break;
                                }
                                else if (indexOfOne + dice == 56)
                                {
                                    minState.Min.RemoveAt(0);
                                    minState.MinFinished++;
                                    if (minState.MinFinished == 4)
                                        return -1;
                                    break;
                                }
                            }
                            else
                            {
                                minState.Min.RemoveAt(0);
                                break;
                            }
                        }
                    }
                    return Math.Min(minVal, MaxValue(new State { MaxFinished = minState.MaxFinished, MinFinished = minState.MinFinished, Min = CopyArray(minState.Min), Max = CopyArray(minState.Max) }));
                }
            }catch(Exception ex)
            {
                return 0;
            }
            return 0;
        }
    }
}