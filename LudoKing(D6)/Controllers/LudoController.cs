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
                Min = new int[57, 57],
                Max = new int[57, 57]
            };

            while (!isFinished)
            {
                Thread.Sleep(Speed);
                Ludo ludoMax = new Ludo(state);

                Random random = new Random();
                byte DiceVal = (byte)random.Next(1, 7);

                //MAX turn
                if (state.MaxPlaced == 0 && DiceVal == 6)
                {
                    state.Max[0, 0] = 1;
                    state.MaxPlaced++;
                    await _ludoHub.Clients.All.SendAsync("ReceiveMessage", DiceVal, ".B0", 1);
                }
                else if (state.MaxPlaced > 0 && state.MinPlaced > 0)
                {
                    var calScore = ludoMax.MaxValue();
                    int[] rowColumn = ludoMax.BestMove(state, DiceVal, 1, calScore);
                    await _ludoHub.Clients.All.SendAsync("Delete", ".B" + rowColumn[1], 1);
                    state.Max[rowColumn[0], rowColumn[1]] = 0;
                    state.Max[rowColumn[0], rowColumn[1] + DiceVal] = 1;
                    await _ludoHub.Clients.All.SendAsync("ReceiveMessage", DiceVal, ".B" + rowColumn[1] + DiceVal, 1);

                }

                else if (state.MaxPlaced == 0 && DiceVal != 6){ }

                else if (Array.IndexOf(state.Max, 1) + DiceVal == 56)
                {
                    state.MaxPlaced--;
                    state.Max[0, Array.IndexOf(state.Max, 1) + DiceVal] = 0;
                    await _ludoHub.Clients.All.SendAsync("Delete", ".B" + (Array.IndexOf(state.Max, 1)), 1);
                    state.MaxFinished++;
                    await _ludoHub.Clients.All.SendAsync("ChangeProperty", "#MaxPlaced", state.MaxFinished);
                    if (state.MaxFinished == 4)
                    {
                        await _ludoHub.Clients.All.SendAsync("Winner", "Maxi");
                        return Json(true);
                    }
                }
                else
                {
                    if (Array.IndexOf(state.Max, 1) + DiceVal <= 56)
                    {
                        int indexOfOne = Array.IndexOf(state.Max, 1);
                        await _ludoHub.Clients.All.SendAsync("Delete", ".B" + indexOfOne, 1);
                        state.Max[indexOfOne] = 0;
                        state.Max[indexOfOne + DiceVal] = 1;
                        await _ludoHub.Clients.All.SendAsync("ReceiveMessage", DiceVal, ".B" + Array.IndexOf(state.Max, 1), 1);
                    }
                }

                Thread.Sleep(Speed);
                Ludo ludoMin = new Ludo(state);

                DiceVal = (byte)random.Next(1, 7);

                //MIN turn
                if (state.MinPlaced == 0 && DiceVal == 6)
                {
                    state.Min[0] = 1;
                    state.MinPlaced++;
                    await _ludoHub.Clients.All.SendAsync("ReceiveMessage", DiceVal, ".G0", 2);
                }
                else if (state.MaxPlaced > 0 && state.MinPlaced > 0)
                {
                    var calScore = ludoMax.MinValue();
                    if (Array.IndexOf(state.Min, 1) + DiceVal <= 56)
                    {
                        int indexOfOne = Array.IndexOf(state.Min, 1);
                        await _ludoHub.Clients.All.SendAsync("Delete", ".G" + indexOfOne, 1);
                        state.Min[indexOfOne] = 0;
                        state.Min[indexOfOne + DiceVal] = 1;
                        await _ludoHub.Clients.All.SendAsync("ReceiveMessage", DiceVal, ".G" + Array.IndexOf(state.Min, 1), 2);
                    }
                }

                else if (state.MinPlaced == 0 && DiceVal != 6) { }
                else if (Array.IndexOf(state.Min, 1) + DiceVal == 56)
                {
                    state.MinPlaced--;
                    state.Min[Array.IndexOf(state.Min, 1) + DiceVal] = 0;
                    await _ludoHub.Clients.All.SendAsync("Delete", ".G" + (Array.IndexOf(state.Min, 1)));

                    state.MinFinished++;
                    await _ludoHub.Clients.All.SendAsync("ChangeProperty", "#MinPlaced", state.MinFinished);

                    if (state.MinFinished == 4)
                    {
                        await _ludoHub.Clients.All.SendAsync("Winner", "Mini");
                        return Json(true);
                    }
                }

                else
                {
                    if (Array.IndexOf(state.Min, 1) + DiceVal < 56)
                    {
                        int indexOfOne = Array.IndexOf(state.Min, 1);
                        await _ludoHub.Clients.All.SendAsync("Delete", ".G" + indexOfOne);
                        state.Min[indexOfOne] = 0;
                        state.Min[indexOfOne + DiceVal] = 1;
                        await _ludoHub.Clients.All.SendAsync("ReceiveMessage", DiceVal, ".G" + Array.IndexOf(state.Min, 1), 2);
                    }
                }
            }
            return Json(true);
        }
    }
}