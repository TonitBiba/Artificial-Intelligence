using System;
using System.Collections.Generic;
using System.Text;

namespace Heuristic_D4_
{
    public class FileStructureXml
    {
        public int[] InitialState { get; set; }

        public int algorithm { get; set; }

        public string Heuristic { get; set; }

        public double RunningTime { get; set; }

        public int NumberOfGeneratedNodes { get; set; }

        public int NumberOfSteps { get; set; }

        public double Depth { get; set; }
    }
}
