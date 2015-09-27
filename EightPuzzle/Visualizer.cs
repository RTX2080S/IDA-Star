using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EightPuzzle
{
    public class Visualizer
    {

        private string state = null;
        private PuzzleMap puzzleMap = null;
        private const int myN = 9;

        public Visualizer(string stateStr)
        {
            state = stateStr;
        }

        public Visualizer(PuzzleMap yourPuzzleMap)
        {
            puzzleMap = yourPuzzleMap;
        }

        public string ToVisual()
        {
            if (state != null)
            {
                string ans = null;
                int[] printOrder = MyTool.ToPrintOrder(state);
                //ans += "The visual image for: " + state;
                ans += String.Format("\n{0}{1}{2}", printOrder[0], printOrder[1], printOrder[2]);
                ans += String.Format("\n{0}{1}{2}", printOrder[3], printOrder[4], printOrder[5]);
                ans += String.Format("\n{0}{1}{2}\n", printOrder[6], printOrder[7], printOrder[8]);
                return ans;
            }

            if (puzzleMap != null)
            {
                string ans = null;
                int[] printOrder = new int[myN];
                printOrder[0] = puzzleMap.data[0, 0];
                printOrder[1] = puzzleMap.data[0, 1];
                printOrder[2] = puzzleMap.data[0, 2];
                printOrder[3] = puzzleMap.data[1, 0];
                printOrder[4] = puzzleMap.data[1, 1];
                printOrder[5] = puzzleMap.data[1, 2];
                printOrder[6] = puzzleMap.data[2, 0];
                printOrder[7] = puzzleMap.data[2, 1];
                printOrder[8] = puzzleMap.data[2, 2];
                //ans += "The visual image for: " + puzzleMap.ToString();
                ans += String.Format("\n{0}{1}{2}", printOrder[0], printOrder[1], printOrder[2]);
                ans += String.Format("\n{0}{1}{2}", printOrder[3], printOrder[4], printOrder[5]);
                ans += String.Format("\n{0}{1}{2}\n", printOrder[6], printOrder[7], printOrder[8]);
                return ans;
            }

            return null;
        }

        public override string ToString()
        {
            return state;
        }

    }
}
