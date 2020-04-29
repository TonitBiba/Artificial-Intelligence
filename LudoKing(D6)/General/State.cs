using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoKing_D6_.General
{
    public class State
    {
        public List<int[]> Min { get; set; }

        public List<int[]> Max { get; set; }

        public byte MinFinished { get; set; }

        public byte MaxFinished { get; set; }

    }
}
