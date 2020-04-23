using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LudoKing_D6_.General;
using LudoKing_D6_.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

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

        public async Task<JsonResult> Test()
        {
            while (true)
            {
                Random random = new Random();
                int DiceVal = random.Next(1, 6);
                Ludo ludo = new Ludo();
                ludo.Run();

                await _ludoHub.Clients.All.SendAsync("ReceiveMessage", DiceVal);
                Thread.Sleep(10);
            }

            return Json(true);
        }
    }
}