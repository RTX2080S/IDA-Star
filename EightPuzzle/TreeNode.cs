using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EightPuzzle
{
    public class TreeNode
    {
        public PuzzleMap puzzleMap;
        public List<PuzzleMap> SolutionPath = new List<PuzzleMap>();
        public int NodeDepth = 0;

        public int fValue()
        {
            return (puzzleMap.GetHeuristicValue() + NodeDepth);
        }

        public List<PuzzleMap> CloneSolutionPath()
        {
            List<PuzzleMap> newStack = new List<PuzzleMap>();
            foreach (PuzzleMap pm in SolutionPath)
            {
                newStack.Add(pm);
            }
            return newStack;
        }

    }
}
