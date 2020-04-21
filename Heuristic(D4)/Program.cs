using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml.Serialization;

//Solving N puzzle problem with A* algorithm and comparing the solution among different selection of heuristics.
//Implemented heuristics:
//1. Misplaced tiles;
//2.Manhattan distance;

namespace Heuristic_D4_
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] InitialSeed = { 1, 2, 3, 4, 5, 6, 7, 8, 0 };
            int[] GoalState = { 1, 2, 3, 4, 5, 6, 7, 8, 0 };
            var xmlSer = new XmlSerializer(typeof(FileStructureXml));
            FileStream fs;

            int size = 3;
            AStar aStar;
            //foreach (string file in Directory.EnumerateFiles(@"C:\Users\tonit\Desktop\Rezultatet e testimit", "2*.xml"))
            //{
            //    Thread manHattanTh = new Thread(t =>
            //    {
            //        try
            //        {
            //            //int[] initialSeed = ShufflePuzzle(InitialSeed);
            //            fs = new FileStream(file, FileMode.Open, FileAccess.Read);
            //            FileStructureXml obj = (FileStructureXml)xmlSer.Deserialize(fs);
            //            aStar = new AStar(obj.InitialState, GoalState, Heuristic.ManhattanDistance, size);
            //            var manhattanSolve = aStar.Solve();
            //            var xmlToSave = new FileStructureXml { algorithm = 1, Heuristic = "Manhattan Distance", Depth = manhattanSolve.Depth, InitialState = obj.InitialState, NumberOfGeneratedNodes = manhattanSolve.NumberOfGeneratedNodes, NumberOfSteps = manhattanSolve.NumberOfSteps, RunningTime = manhattanSolve.RunningTime };
            //            fs = new FileStream(@"C:\Users\tonit\Desktop\Rezultatet e testimit\1" + DateTimeOffset.Now.ToUnixTimeMilliseconds() + ".xml", FileMode.Create, FileAccess.Write);
            //            xmlSer.Serialize(fs, xmlToSave);
            //        }
            //        catch
            //        {

            //        }
            //    });
            //    manHattanTh.Start();
            //}
            foreach (string file in Directory.EnumerateFiles(@"C:\Users\tonit\Desktop\Rezultatet e testimit", "1*.xml"))
            {
                Thread euclideanTh = new Thread(t =>
                    {
                        try
                        {
                            //int[] initialSeed = ShufflePuzzle(InitialSeed);
                            fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                            FileStructureXml obj = (FileStructureXml)xmlSer.Deserialize(fs);
                            aStar = new AStar(obj.InitialState, GoalState, Heuristic.EuclideanDistance, size);
                            var euclideanDistanceSolve = aStar.Solve();
                            var xmlToSave = new FileStructureXml { algorithm = 2, Heuristic = "Euclidean Distance", Depth = euclideanDistanceSolve.Depth, InitialState = obj.InitialState, NumberOfGeneratedNodes = euclideanDistanceSolve.NumberOfGeneratedNodes, NumberOfSteps = euclideanDistanceSolve.NumberOfSteps, RunningTime = euclideanDistanceSolve.RunningTime };
                            fs = new FileStream(@"C:\Users\tonit\Desktop\Rezultatet e testimit\2" + DateTimeOffset.Now.ToUnixTimeMilliseconds() + ".xml", FileMode.Create, FileAccess.Write);
                            xmlSer.Serialize(fs, xmlToSave);
                        }
                        catch
                        {

                        }

                    });
                euclideanTh.Start();
            }
            //foreach (string file in Directory.EnumerateFiles(@"C:\Users\tonit\Desktop\Rezultatet e testimit", "2*.xml"))
            //{
            //    Thread egasTh = new Thread(t =>
            //    {
            //        try
            //        {
            //            //int[] initialSeed = ShufflePuzzle(InitialSeed);
            //            fs = new FileStream(file, FileMode.Open, FileAccess.Read);
            //            FileStructureXml obj = (FileStructureXml)xmlSer.Deserialize(fs);
            //            aStar = new AStar(obj.InitialState, GoalState, Heuristic.Gaschnig, size);
            //            var gaschnigSolve = aStar.Solve();
            //            var xmlToSave = new FileStructureXml { algorithm = 3, Heuristic = "Gaschig", Depth = gaschnigSolve.Depth, InitialState = obj.InitialState, NumberOfGeneratedNodes = gaschnigSolve.NumberOfGeneratedNodes, NumberOfSteps = gaschnigSolve.NumberOfSteps, RunningTime = gaschnigSolve.RunningTime };
            //            fs = new FileStream(@"C:\Users\tonit\Desktop\Rezultatet e testimit\3" + DateTimeOffset.Now.ToUnixTimeMilliseconds() + ".xml", FileMode.Create, FileAccess.Write);
            //            xmlSer.Serialize(fs, xmlToSave);
            //        }
            //        catch
            //        {

            //        }
            //    });
            //    egasTh.Start();
            //}

            //foreach (string file in Directory.EnumerateFiles(@"C:\Users\tonit\Desktop\Rezultatet e testimit", "2*.xml"))
            //{
            //    Thread misTh = new Thread(t =>
            //    {
            //        try
            //        {
            //            fs = new FileStream(file, FileMode.Open, FileAccess.Read);
            //            FileStructureXml obj = (FileStructureXml)xmlSer.Deserialize(fs);

            //            //int[] initialSeed = ShufflePuzzle(InitialSeed);
            //            aStar = new AStar(obj.InitialState, GoalState, Heuristic.MisplacedTiles, size);
            //            var misplacedSolve = aStar.Solve();
            //            var xmlToSave = new FileStructureXml { algorithm = 4, Heuristic = "Misplacced tiles (Hamming)", Depth = misplacedSolve.Depth, InitialState = obj.InitialState, NumberOfGeneratedNodes = misplacedSolve.NumberOfGeneratedNodes, NumberOfSteps = misplacedSolve.NumberOfSteps, RunningTime = misplacedSolve.RunningTime };
            //            fs = new FileStream(@"C:\Users\tonit\Desktop\Rezultatet e testimit\4" + DateTimeOffset.Now.ToUnixTimeMilliseconds() + ".xml", FileMode.Create, FileAccess.Write);
            //            xmlSer.Serialize(fs, xmlToSave);
            //        }
            //        catch
            //        {

            //        }
            //    });
            //    misTh.Start();
            //}

            //foreach (string file in Directory.EnumerateFiles(@"C:\Users\tonit\Desktop\Rezultatet e testimit", "2*.xml"))
            //{
            //    Thread outOf = new Thread(t =>
            //    {
            //        try
            //        {
            //            fs = new FileStream(file, FileMode.Open, FileAccess.Read);
            //            FileStructureXml obj = (FileStructureXml)xmlSer.Deserialize(fs);
            //            //int[] initialSeed = ShufflePuzzle(InitialSeed);
            //            aStar = new AStar(obj.InitialState, GoalState, Heuristic.OutOfRowAndColumn, size);
            //            var outOfRowAndColumnSolve = aStar.Solve();
            //            var xmlToSave = new FileStructureXml { algorithm = 5, Heuristic = "Out of Row and Column", Depth = outOfRowAndColumnSolve.Depth, InitialState = obj.InitialState, NumberOfGeneratedNodes = outOfRowAndColumnSolve.NumberOfGeneratedNodes, NumberOfSteps = outOfRowAndColumnSolve.NumberOfSteps, RunningTime = outOfRowAndColumnSolve.RunningTime };
            //            fs = new FileStream(@"C:\Users\tonit\Desktop\Rezultatet e testimit\5" + DateTimeOffset.Now.ToUnixTimeMilliseconds() + ".xml", FileMode.Create, FileAccess.Write);
            //            xmlSer.Serialize(fs, xmlToSave);
            //        }
            //        catch
            //        {

            //        }
            //    });
            //    outOf.Start();

            //}
        }
        public static int[] ShufflePuzzle(int[] puzzle)
        {
            int[] newSeed = new int[puzzle.Length];

            for (int i = 0; i < puzzle.Length; i++)
            {
                newSeed[i] = puzzle[i];
            }

            Random random = new Random();
            for (int i = 0; i < newSeed.Length; i++)
            {
                int temp = newSeed[i];
                int randomIndex = random.Next(i, newSeed.Length);
                newSeed[i] = newSeed[randomIndex];
                newSeed[randomIndex] = temp;
            }
            return newSeed;
        }

        public static void printArray(int[] array)
        {
            Console.WriteLine();
            for (int i = 0; i < array.Length; i++)
                Console.Write(array[i] + ", ");
        }
    }
}
