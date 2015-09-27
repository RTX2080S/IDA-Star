using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EightPuzzle
{
    public class PuzzleMap
    {
        private const int myN = 9;
        private string state = null;

        public int[,] data = new int[3, 3];

        public PuzzleMap()
        {
            state = "Empty";
        }

        public PuzzleMap(string stateStr)
        {
            state = stateStr;
            int[] dataSource = MyTool.ToPrintOrder(state);
            data[0, 0] = dataSource[0];
            data[0, 1] = dataSource[1];
            data[0, 2] = dataSource[2];
            data[1, 0] = dataSource[3];
            data[1, 1] = dataSource[4];
            data[1, 2] = dataSource[5];
            data[2, 0] = dataSource[6];
            data[2, 1] = dataSource[7];
            data[2, 2] = dataSource[8];
        }

        public PuzzleMap Clone()
        {
            try
            {
                PuzzleMap ans = new PuzzleMap();
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                        ans.data[i, j] = data[i, j];
                return ans;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
            
        }

        private bool xyInRange(int x, int y)
        {
            if ((x > 2)
                || (y > 2)
                || (x < 0)
                || (y < 0))
                return false;
            return true;
        }

        private int getInc(int u)
        {
            return (u);
            //if (u > 0)
            //    return (1);
            //return 0;
        }

        public int GetHeuristicValue()
        {
            int h = 0;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    int u = 0;
                    switch (data[i, j])
                    {
                        case 0:
                            h += 0;
                            break;
                        case 1:
                            u = Math.Abs(i - 0) + Math.Abs(j - 0);
                            h += getInc(u);
                            break;
                        case 2:
                            u = Math.Abs(i - 0) + Math.Abs(j - 1);
                            h += getInc(u);
                            break;
                        case 3:
                            u = Math.Abs(i - 0) + Math.Abs(j - 2);
                            h += getInc(u);
                            break;
                        case 4:
                            u = Math.Abs(i - 1) + Math.Abs(j - 0);
                            h += getInc(u);
                            break;
                        case 5:
                            u = Math.Abs(i - 1) + Math.Abs(j - 1);
                            h += getInc(u);
                            break;
                        case 6:
                            u = Math.Abs(i - 1) + Math.Abs(j - 2);
                            h += getInc(u);
                            break;
                        case 7:
                            u = Math.Abs(i - 2) + Math.Abs(j - 0);
                            h += getInc(u);
                            break;
                        case 8:
                            u = Math.Abs(i - 2) + Math.Abs(j - 1);
                            h += getInc(u);
                            break;
                        default:
                            break;
                    }
                }
            return h;
        }

        public void MoveTile(int x, int y, int dx, int dy)
        {
            int nx = x + dx;
            int ny = y + dy;
            int tmp = data[x, y];
            data[x, y] = data[nx, ny];
            data[nx, ny] = tmp;
        }

        public bool AbleToMove(int x, int y, int dx, int dy)
        {
            if (!(((dx != 0) && (dy == 0)) || ((dx == 0) && (dy != 0))))
                return false;
            if (!xyInRange(x + dx, y + dy))
                return false;
            if (data[x, y] == 0)
                return false;
            if (data[x + dx, y + dy] > 0)
                return false;
            return true;
        }

        public bool Equals(PuzzleMap pm2)
        {
            if (pm2 == null)
                return false;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (data[i, j] != pm2.data[i, j])
                        return false;
            return true;
        }

        public override string ToString()
        {
            return state;
        }

        public string ToVisual()
        {
            string ans = null;
            int[] printOrder = new int[myN];
            printOrder[0] = data[0, 0];
            printOrder[1] = data[0, 1];
            printOrder[2] = data[0, 2];
            printOrder[3] = data[1, 0];
            printOrder[4] = data[1, 1];
            printOrder[5] = data[1, 2];
            printOrder[6] = data[2, 0];
            printOrder[7] = data[2, 1];
            printOrder[8] = data[2, 2];
            //ans += "The visual image for: " + puzzleMap.ToString();
            ans += String.Format("\n{0}{1}{2}", printOrder[0], printOrder[1], printOrder[2]);
            ans += String.Format("\n{0}{1}{2}", printOrder[3], printOrder[4], printOrder[5]);
            ans += String.Format("\n{0}{1}{2}\n", printOrder[6], printOrder[7], printOrder[8]);
            return ans;
        }
    }
}
