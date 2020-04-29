using LudoKing_D6_.General;
using LudoKing_D6_.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LudoKing_D6_.Controllers
{
    public class LudoController : Controller
    {

        public IHubContext<LudoHub> _ludoHub { get; set; }

        public LudoController(IHubContext<LudoHub> _ludoHub)
        {
            this._ludoHub = _ludoHub;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> Play()
        {
            return Json(true);
        }


        //1 MAX
        //-1 MIN
        public async Task<JsonResult> Test(int Speed)
        {
            bool isFinished = false;
            //Initial state
            State state = new State
            {
                Min = new List<int[]>(),
                Max = new List<int[]>()
            };

            while (!isFinished)
            {
                Thread.Sleep(Speed);
                Ludo ludoMax = new Ludo(state, _ludoHub);

                Random random = new Random();
                byte DiceVal = (byte)random.Next(1, 7);

                //MAX turn
                if (state.Max.Count == 0 && DiceVal == 6)
                {
                    state.Max.Add(new int[57]);
                    state.Max[0][0] = 1;
                    await _ludoHub.Clients.All.SendAsync("ReceiveMessage", DiceVal, ".B0", 1);
                }
                else if (state.Max.Count > 0 && state.Min.Count > 0)
                {
                    var calScore = ludoMax.MaxValue(new State { MaxFinished = state.MaxFinished, MinFinished = state.MinFinished, Max = ludoMax.CopyArray(state.Max), Min = ludoMax.CopyArray(state.Min) });
                    foreach (var player in state.Max)
                    {
                        int indexOfOne = Array.IndexOf(player, 1);
                        if (indexOfOne > -1)
                        {
                            if (indexOfOne + DiceVal < 56)
                            {
                                await _ludoHub.Clients.All.SendAsync("Delete", ".B" + indexOfOne, 1);
                                player[indexOfOne] = 0;
                                player[indexOfOne + DiceVal] = 1;
                                await _ludoHub.Clients.All.SendAsync("ReceiveMessage", DiceVal, ".B" + Array.IndexOf(player, 1), 1);
                                break;
                            }
                            else if (indexOfOne + DiceVal == 56)
                            {
                                await _ludoHub.Clients.All.SendAsync("Delete", ".B" + (Array.IndexOf(state.Max[0], 1)), 1);
                                state.MaxFinished++;
                                await _ludoHub.Clients.All.SendAsync("ChangeProperty", "#MaxPlaced", state.MaxFinished);
                                state.Max.Remove(state.Max[0]);
                                if (state.MaxFinished == 4)
                                {
                                    await _ludoHub.Clients.All.SendAsync("Winner", "Maxi");
                                    return Json(true);
                                }
                                break;
                            }
                        }
                        else
                        {
                            state.Max.RemoveAt(0);
                            break;
                        }
                    }
                }

                else if (state.Max.Count == 0 && DiceVal != 6) { }

                else if (Array.IndexOf(state.Max[0], 1) + DiceVal == 56)
                {
                    await _ludoHub.Clients.All.SendAsync("Delete", ".B" + (Array.IndexOf(state.Max[0], 1)), 1);

                    state.MaxFinished++;

                    await _ludoHub.Clients.All.SendAsync("ChangeProperty", "#MaxPlaced", state.MaxFinished);

                    state.Max.Remove(state.Max[0]);
                    if (state.MaxFinished == 4)
                    {
                        await _ludoHub.Clients.All.SendAsync("Winner", "Maxi");
                        return Json(true);
                    }
                }
                else
                {
                    foreach (var player in state.Max)
                    {
                        int indexOfOne = Array.IndexOf(player, 1);
                        if (indexOfOne > -1)
                        {
                            if (indexOfOne + DiceVal < 56)
                            {
                                await _ludoHub.Clients.All.SendAsync("Delete", ".B" + indexOfOne, 1);
                                player[indexOfOne] = 0;
                                player[indexOfOne + DiceVal] = 1;
                                await _ludoHub.Clients.All.SendAsync("ReceiveMessage", DiceVal, ".B" + Array.IndexOf(player, 1), 1);
                                break;
                            }
                        }
                        else
                        {
                            state.Max.RemoveAt(0);
                            break;
                        }
                    }
                }

                Thread.Sleep(Speed);
                Ludo ludoMin = new Ludo(state, _ludoHub);

                DiceVal = (byte)random.Next(1, 7);


                //MIN turn
                if (state.Min.Count == 0 && DiceVal == 6)
                {
                    state.Min.Add(new int[57]);
                    state.Min[0][0] = 1;
                    await _ludoHub.Clients.All.SendAsync("ReceiveMessage", DiceVal, ".G0", 2);
                }
                else if (state.Max.Count > 0 && state.Min.Count > 0)
                {
                    var calScore = ludoMax.MinValue(new State { MaxFinished = state.MaxFinished, MinFinished = state.MinFinished, Max = ludoMax.CopyArray(state.Max), Min = ludoMax.CopyArray(state.Min) });
                    foreach (var player in state.Min)
                    {
                        int indexOfOne = Array.IndexOf(player, 1);
                        if (indexOfOne > -1)
                        {
                            if (indexOfOne + DiceVal < 56)
                            {
                                await _ludoHub.Clients.All.SendAsync("Delete", ".G" + indexOfOne, 2);
                                player[indexOfOne] = 0;
                                player[indexOfOne + DiceVal] = 1;
                                await _ludoHub.Clients.All.SendAsync("ReceiveMessage", DiceVal, ".G" + Array.IndexOf(player, 1), 2);
                                break;
                            }else if(indexOfOne+DiceVal == 56)
                            {
                                await _ludoHub.Clients.All.SendAsync("Delete", ".G" + (Array.IndexOf(player, 1)), 2);
                                state.MinFinished++;
                                await _ludoHub.Clients.All.SendAsync("ChangeProperty", "#MinPlaced", state.MinFinished);
                                state.Min.Remove(player);
                                if (state.MinFinished == 4)
                                {
                                    await _ludoHub.Clients.All.SendAsync("Winner", "Mini");
                                    return Json(true);
                                }
                                break;
                            }
                        }
                        else
                        {
                            state.Min.RemoveAt(0);
                            break;
                        }
                    }
                }

                else if (state.Min.Count == 0 && DiceVal != 6) { }

                else if (Array.IndexOf(state.Min[0], 1) + DiceVal == 56)
                {
                    await _ludoHub.Clients.All.SendAsync("Delete", ".G" + (Array.IndexOf(state.Min[0], 1)), 2);
                    state.MinFinished++;
                    await _ludoHub.Clients.All.SendAsync("ChangeProperty", "#MinPlaced", state.MinFinished);
                    state.Min.Remove(state.Min[0]);
                    if (state.MinFinished == 4)
                    {
                        await _ludoHub.Clients.All.SendAsync("Winner", "Mini");
                        return Json(true);
                    }
                }

                else
                {
                    foreach (var player in state.Min)
                    {
                        int indexOfOne = Array.IndexOf(player, 1);
                        if (indexOfOne > -1)
                        {
                            if (indexOfOne + DiceVal < 56)
                            {
                                await _ludoHub.Clients.All.SendAsync("Delete", ".G" + indexOfOne, 2);
                                player[indexOfOne] = 0;
                                player[indexOfOne + DiceVal] = 1;
                                await _ludoHub.Clients.All.SendAsync("ReceiveMessage", DiceVal, ".G" + Array.IndexOf(player, 1), 2);
                                break;
                            }
                        }
                        else
                        {
                            state.Min.RemoveAt(0);
                            break;
                        }
                    }
                }
            }
            return Json(true);
        }

        //public async Task<int[]> BestMove(State state, int dice, int nCase, int MiniMaxScore)
        //{
        //    try
        //    {
        //        if (nCase == 1)
        //        {
        //            foreach (var maxPlayer in state.Max)
        //            {
        //                if (MiniMaxScore == 1)
        //                {
        //                    int maxPlayerIndex = Array.IndexOf(maxPlayer, 1);
        //                    if (maxPlayerIndex > -1)
        //                    {
        //                        if (maxPlayerIndex + dice < 56)
        //                        {
        //                            await _ludoHub.Clients.All.SendAsync("Delete", ".B" + maxPlayerIndex, 2);
        //                            maxPlayer[maxPlayerIndex] = 0;
        //                            maxPlayer[maxPlayerIndex + dice] = 1;
        //                            await _ludoHub.Clients.All.SendAsync("ReceiveMessage", dice, ".B" + (maxPlayerIndex + dice), 1);
        //                            return maxPlayer;
        //                        }
        //                        else if (maxPlayerIndex + dice == 56)
        //                        {
        //                            await _ludoHub.Clients.All.SendAsync("Delete", ".B" + maxPlayerIndex, 1);
        //                            state.MinFinished++;
        //                            state.Min.Remove(maxPlayer);
        //                            return maxPlayer;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        state.Max.RemoveAt(0);
        //                        break;
        //                    }
        //                }

        //                else
        //                {
        //                    foreach (var minPlayer in state.Min)
        //                    {
        //                        int indexOfMin = Array.IndexOf(minPlayer, 1);
        //                        int indexOfMax = Array.IndexOf(maxPlayer, 1);
        //                        if (indexOfMin < 57 && indexOfMax < 57 && indexOfMin>-1 && indexOfMax>-1)
        //                        {
        //                            if (indexOfMin <= 50 && indexOfMax + 26 + dice < 50)
        //                            {
        //                                //if (indexOfMin - 26 == indexOfMax + dice)
        //                                //{
        //                                //    await _ludoHub.Clients.All.SendAsync("Delete", ".G" + indexOfMin, 2);
        //                                //    await _ludoHub.Clients.All.SendAsync("Delete", ".B" + indexOfMax, 2);
        //                                //    await _ludoHub.Clients.All.SendAsync("ReceiveMessage", dice, ".B" + (indexOfMax + dice), 1);
        //                                //    state.Min.Remove(minPlayer);
        //                                //    return maxPlayer;
        //                                //}
        //                                //else
        //                                //{
        //                                    state.Max[state.Max.IndexOf(maxPlayer)][indexOfMax] = 0;
        //                                    state.Max[state.Max.IndexOf(maxPlayer)][indexOfMax + dice] = 1;
        //                                    await _ludoHub.Clients.All.SendAsync("Delete", ".B" + indexOfMax, 2);
        //                                    await _ludoHub.Clients.All.SendAsync("ReceiveMessage", dice, ".B" + (indexOfMax + dice), 1);
        //                                    return maxPlayer;
        //                                //}
        //                            }
        //                            else
        //                            {
        //                                state.Max[state.Max.IndexOf(maxPlayer)][indexOfMax] = 0;
        //                                state.Max[state.Max.IndexOf(maxPlayer)][indexOfMax + dice] = 1;
        //                                await _ludoHub.Clients.All.SendAsync("Delete", ".B" + indexOfMax, 2);
        //                                await _ludoHub.Clients.All.SendAsync("ReceiveMessage", dice, ".B" + (indexOfMax + dice), 1);
        //                                return maxPlayer;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            return state.Max[0];
        //        }
        //        else
        //        {
        //            foreach (var minPlayer in state.Min)
        //            {
        //                if (MiniMaxScore == -1)
        //                {
        //                    int minIndex = Array.IndexOf(minPlayer, 1);
        //                    if (minIndex > -1)
        //                    {
        //                        if (minIndex + dice < 56)
        //                        {
        //                            await _ludoHub.Clients.All.SendAsync("Delete", ".G" + (minIndex), 2);
        //                            minPlayer[minIndex] = 0;
        //                            await _ludoHub.Clients.All.SendAsync("ReceiveMessage", dice, ".G" + (minIndex + dice), 2);
        //                            return minPlayer;
        //                        }
        //                        else if (minIndex + dice == 56)
        //                        {
        //                            await _ludoHub.Clients.All.SendAsync("Delete", ".G" + minPlayer, 2);
        //                            state.MinFinished++;
        //                            state.Min.Remove(minPlayer);
        //                            return minPlayer;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        state.Min.RemoveAt(0);
        //                        break;
        //                    }
        //                }
        //                else
        //                {
        //                    foreach (var maxPlayer in state.Max)
        //                    {
        //                        int indexOfMin = Array.IndexOf(minPlayer, 1);
        //                        int indexOfMax = Array.IndexOf(maxPlayer, 1);
        //                        if (indexOfMin < 57 && indexOfMax < 57 && indexOfMin > -1 && indexOfMax > -1)
        //                        {
        //                            if (indexOfMax <= 50 && indexOfMin + 26 + dice < 50)
        //                            {
        //                                if (indexOfMax - 26 == indexOfMin + dice)
        //                                {
        //                                    await _ludoHub.Clients.All.SendAsync("Delete", ".G" + indexOfMin, 2);
        //                                    await _ludoHub.Clients.All.SendAsync("Delete", ".B" + indexOfMax, 2);
        //                                    await _ludoHub.Clients.All.SendAsync("ReceiveMessage", dice, ".G" + (indexOfMin + dice), 2);
        //                                    state.Max.Remove(maxPlayer);
        //                                    return maxPlayer;
        //                                }
        //                                else
        //                                {
        //                                    state.Min[state.Min.IndexOf(minPlayer)][indexOfMin] = 0;
        //                                    state.Min[state.Min.IndexOf(minPlayer)][indexOfMin + dice] = 1;
        //                                    await _ludoHub.Clients.All.SendAsync("Delete", ".G" + indexOfMin, 2);
        //                                    await _ludoHub.Clients.All.SendAsync("ReceiveMessage", dice, ".G" + (indexOfMin + dice), 2);
        //                                    return maxPlayer;
        //                            }
        //                        }
        //                            else
        //                            {
        //                                state.Min[state.Min.IndexOf(minPlayer)][indexOfMin] = 0;
        //                                state.Min[state.Min.IndexOf(minPlayer)][indexOfMin + dice] = 1;
        //                                await _ludoHub.Clients.All.SendAsync("Delete", ".G" + indexOfMin, 2);
        //                                await _ludoHub.Clients.All.SendAsync("ReceiveMessage", dice, ".G" + (indexOfMin + dice), 2);
        //                                return maxPlayer;
        //                            }
        //                        }else if ()
        //                        {

        //                        }
        //                    }
        //                }
        //            }
        //            return state.Min[0];
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
    }
}