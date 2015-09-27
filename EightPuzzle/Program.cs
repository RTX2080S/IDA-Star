using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EightPuzzle
{
    class Program
    {
        const int myN = 9;
        const string finalStateStr = "801234567";
        static PuzzleMap finalState = null;

        static string inputDir = @"A:\Miscellany\767\NewCodeResources\Probs10-25DavidTsai\";
        static string outputDir = @"A:\Miscellany\767\Output\767_output_{0}.txt";

        static int fileNum = 12;
        static int problemId = 0;

        static string InputFileName = null;
        static string prefix = null;
        static string suffix = "). ";
        static List<PuzzleMap> Problems = new List<PuzzleMap>();
        static List<TreeNode> SearchSpace = new List<TreeNode>();

        //The switch to control whether output to disk
        static bool diskProtectMode = true;

        // used to analyse the number of nodes generated
        static List<string> InitialStateStr = new List<string>();
        static List<int> NodesGenerated = new List<int>();
        static int CurrentNodeGenerated = 0;
        static List<int> NodesExpanded = new List<int>();
        static int CurrentNodeExpanded = 0;
        static string reportDir = @"A:\Miscellany\767\Output\";

        // The switch to control whether Loop Elimination is enabled
        static bool EnableLoopElimination = true;
        static bool EnableGrandfatherPruning = true;

        static void Main(string[] args)
        {
            Initialize();

            int totalProblems = Problems.Count;
            Console.WriteLine("Now begin searching {0} problems...", totalProblems);

            int outputPos = Console.CursorTop;
            foreach (PuzzleMap iPuzzleMap in Problems)
            {
                SearchSpace.Clear();
                IDAStarSearch(iPuzzleMap);

                Console.SetCursorPosition(0, outputPos);
                Console.WriteLine("{0} out of {1} finished.", problemId, totalProblems);

                // record the nodes generated for current problem

                NodesGenerated.Add(CurrentNodeGenerated);
                NodesExpanded.Add(CurrentNodeExpanded);
                CurrentNodeGenerated = 0;
                CurrentNodeExpanded = 0;
            }

            GeneratedReport();

            Console.WriteLine();
            Console.WriteLine("Finished! ");
            Console.ReadLine();
        }

        static void ScanParams()
        {
        start:
            try
            {
                Console.WriteLine("Input depth (10~25): ");
                string nStr = Console.ReadLine();
                int n = int.Parse(nStr);
                if ((n < 10) || (n > 25)) 
                    goto start;
                fileNum = n;
                Console.WriteLine("Enable Loop Elimination (Default YES, enter N to turn off)? ");
                string resp = Console.ReadLine();
                if (!string.IsNullOrEmpty(resp))
                    EnableLoopElimination = !(resp.ToUpper() == "N");
                Console.WriteLine("Generate detailed report (Default NO, enter Y to turn on)? ");
                resp = Console.ReadLine();
                if (!string.IsNullOrEmpty(resp))
                    diskProtectMode = !(resp.ToUpper() == "Y");

                // output a summary
                Console.WriteLine("Depth = {0}", fileNum);
                Console.WriteLine("LoopElimination = {0}", EnableLoopElimination);
                Console.WriteLine("Generated detailed report = {0}", !diskProtectMode);
                Console.WriteLine();
            }
            catch (Exception)
            {
                goto start;
            }
        }

        static void Initialize()
        {
            ScanParams();

            InputFileName = fileNum.ToString() + ".pl";
            prefix = "problems(" + fileNum.ToString() + ", ";

            InitDataSource(inputDir + InputFileName);
        }

        static void IDAStarSearch(PuzzleMap iPuzzleMap)
        {
            SearchSpace.Add(new TreeNode()
            {
                puzzleMap = iPuzzleMap,
                NodeDepth = 0,
                SolutionPath = new List<PuzzleMap>() { iPuzzleMap }
            });

            int nodeIndex = 0;
            TreeNode myNode = SearchSpace[nodeIndex];

            while (true)
            {
                myNode = SearchSpace[nodeIndex];

                // should be (myNode.NodeDepth > depthLimit)
                if (myNode.NodeDepth > fileNum)
                {
                    //SearchSpace.Add(myNode);
                    break;
                }

                if (myNode.puzzleMap.Equals(finalState))
                {
                    WriteOutputFile(myNode);
                    break;
                }

                // begin expanding this node
                myNode.NodeDepth += 1;
                PuzzleMap childNodePuzzleMap = myNode.puzzleMap.Clone();
                SearchSpace.RemoveAt(nodeIndex);

                // increase the nodes-expanded counter
                CurrentNodeExpanded += 1;

                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                        for (int x = -1; x < 2; x++)
                            for (int y = -1; y < 2; y++)
                            {
                                childNodePuzzleMap = myNode.puzzleMap.Clone();
                                if (myNode.puzzleMap.AbleToMove(i, j, x, y))
                                {
                                    // new children nodes are generated here                                    
                                    childNodePuzzleMap.MoveTile(i, j, x, y);
                                    if (GrandfatherPruningCheckOK(myNode.SolutionPath, childNodePuzzleMap)
                                        && LoopEliminationCheckOK(myNode.SolutionPath, childNodePuzzleMap))
                                    {
                                        TreeNode newChildNode = new TreeNode()
                                        {
                                            puzzleMap = childNodePuzzleMap,
                                            NodeDepth = myNode.NodeDepth,
                                            SolutionPath = myNode.CloneSolutionPath()
                                        };
                                        newChildNode.SolutionPath.Add(childNodePuzzleMap);
                                        SearchSpace.Add(newChildNode);

                                        // increase the nodes-generated counter
                                        CurrentNodeGenerated += 1;
                                    }
                                }
                            }

                int minimumValueIndex = 0;
                int minimumValue = int.MaxValue;
                for (int k = 0; k < SearchSpace.Count; k++)
                {
                    if (SearchSpace[k].fValue() < minimumValue)
                    {
                        minimumValueIndex = k;
                        minimumValue = SearchSpace[k].fValue();
                    }
                }

                nodeIndex = minimumValueIndex;
            };
        }

        static bool GrandfatherPruningCheckOK(List<PuzzleMap> thePath, PuzzleMap newNode)
        {
            if (!EnableGrandfatherPruning)
                return true;
            int total = thePath.Count;
            if (total == 0)
                return true;
            if (total == 1)
                return (!thePath[0].Equals(newNode));
            if (thePath[total - 1].Equals(newNode) || thePath[total - 2].Equals(newNode))
                return false;
            return true;
        }

        static bool LoopEliminationCheckOK(List<PuzzleMap> thePath, PuzzleMap newNode)
        {
            if (!EnableLoopElimination)
                return true;
            foreach (PuzzleMap node in thePath)
            {
                if (node.Equals(newNode))
                    return false;
            }
            return true;
        }

        static void GeneratedReport()
        {
            try
            {
                string reportFileName = EnableLoopElimination ?
                    "767_Report_{0}_LE.txt"
                    : "767_Report_{0}.txt";
                string outputpath = String.Format(reportDir + reportFileName, fileNum);
                using (StreamWriter sw = new StreamWriter(outputpath, false))
                {
                    sw.WriteLine("InitialState NodesGenerated NodesExpanded");
                    for (int i = 0; i < InitialStateStr.Count; i++)
                    {
                        sw.WriteLine(String.Format("{0,3}{1,12}{2,12}",
                            InitialStateStr[i], NodesGenerated[i], NodesExpanded[i]));
                    }
                    sw.WriteLine();
                    sw.WriteLine("NodesGenerated Average = {0}", NodesGenerated.Average());
                    sw.WriteLine("NodesExpanded Average = {0}", NodesExpanded.Average());
                }
                
                Console.WriteLine("Reports generated at {0}", outputpath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void WriteOutputFile(TreeNode myNode)
        {
            try
            {
                problemId += 1;
                if (diskProtectMode)
                    return;
                string outputpath = String.Format(outputDir, problemId);
                using (StreamWriter sw = new StreamWriter(outputpath, false))
                {
                    sw.WriteLine("Steps = " + (myNode.NodeDepth).ToString());
                    sw.WriteLine("Nodes Generated = " + CurrentNodeGenerated.ToString());
                    sw.WriteLine("Nodes Expanded = " + CurrentNodeExpanded.ToString());
                    foreach (PuzzleMap item in myNode.SolutionPath)
                    {
                        sw.WriteLine(item.ToVisual());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //Console.WriteLine("Found solution {0}! ", outputNum);
            }
        }

        static void InitDataSource(string filePath)
        {
            try
            {
                finalState = new PuzzleMap(finalStateStr);

                using (StreamReader sr = new StreamReader(filePath))
                {
                    Problems.Clear();

                    string tmpLine = null;
                    while (!sr.EndOfStream)
                    {
                        tmpLine = sr.ReadLine();

                        if (tmpLine.StartsWith(prefix))
                        {
                            string stateStr = tmpLine.Substring(prefix.Length,
                                tmpLine.Length - prefix.Length - suffix.Length);
                            Problems.Add(new PuzzleMap(stateStr));

                            // used to generated the output file
                            InitialStateStr.Add(stateStr);
                        }
                    }
                }

                Console.WriteLine("File loaded: \n" + filePath + "\n"
                    + "Total elements: " + Problems.Count.ToString()
                    + "\n");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
