using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoKing_D6_.General
{
    public class State
    {
        public int[,] Min { get; set; }

        public int[,] Max { get; set; }

        public byte MinPlaced { get; set; }

        public byte MaxPlaced { get; set; }

        public byte MinFinished { get; set; }

        public byte MaxFinished { get; set; }

        public int MiniMaxValue { get; set; }

    }
}
