using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoKing_D6_.General
{
    public class State
    {
        public int[] Board { get; set; }

        public byte MinPlaced { get; set; }

        public byte MaxPlaced { get; set; }
    }
}
