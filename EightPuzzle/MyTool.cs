using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EightPuzzle
{
    public static class MyTool
    {
        private const int myN = 9;

        public static int[] ToPrintOrder(string state)
        {
            const int indexZero = 48;
            if (String.IsNullOrEmpty(state)) return null;
            if (state.Length < myN) return null;
            char[] stateCharArray = state.ToArray();
            int[] stateIntArray = new int[myN];

            for (int i = 0; i < myN; i++)
            {
                char c = stateCharArray[i];
                stateIntArray[i] = (int)c - indexZero;
            }

            int[] printOrder = new int[myN];
            for (int i = 0; i < myN; i++)
                printOrder[i] = retIndex(stateIntArray, i);

            return printOrder;
        }



        private static int retIndex(int[] targetArray, int v)
        {
            for (int i = 0; i < targetArray.Length; i++)
                if (targetArray[i] == v) return i;
            return (0);
        }
    }
}
