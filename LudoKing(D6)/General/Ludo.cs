using Microsoft.CodeAnalysis.Options;
using System;

namespace LudoKing_D6_.General
{
    public class Ludo
    {
        public int[] Players { get; set; }

        public int MaxPlaced { get; set; }

        public int MinPlaced { get; set; }

        public Ludo()
        {

        }

        public void Run()
        {
            State state = new State { 
                 MinPlaced =0,
                 MaxPlaced=0,
                 Board = new int[] {0,0,0,0,0,0}
            };
            MaxValue(state);
            while (MaxPlaced!=0 || MinPlaced != 0)
            {
            }

        }

        public float MaxValue(State state)
        {
            //Check if terminal state
            if (MinPlaced == 4|| MaxPlaced == 4)
                return MaxPlaced == 4 ? 1 : 0;

            float maxValue = float.MinValue;
            int[] possibleDiceVal = new int[6]{ 1,2,3,4,5,6};
            foreach (int dice in possibleDiceVal)
            {
                return Math.Max(maxValue, MinValue(state));
            }
            return 0;
        }

        public float MinValue(State state)
        {
            //Check if terminal state
            if (MinPlaced == 4 || MaxPlaced == 4)
                return MinPlaced == 4 ? 1 : 0;

            float minVal = float.MaxValue; //Suppozing that this is infinity for our case. For the worst case this is okay.
            int[] possibleDiceVal = new int[6] { 1, 2, 3, 4, 5, 6 };
            foreach (var element in possibleDiceVal)
            {
                return Math.Min(minVal, MaxValue(state));
            }
            return 0;
        }
    }
}
