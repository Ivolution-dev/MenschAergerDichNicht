using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2019_11_27_MenschAergerDichNicht
{
    class Field
    {
        internal Bitmap Texture = Properties.Resources.Spielfeld;
        internal Size Groesse = new Size(975, 975);
        internal List<Point> HousePoints = new List<Point>();
        internal List<Point> GamePoints = new List<Point>();
        internal List<Point> GoalPoints = new List<Point>();

        public Field()
        {
            FromTo(HousePoints, 0, 2, 0, 2);
            FromTo(HousePoints, 9, 11, 0, 2);
            FromTo(HousePoints, 9, 11, 9, 11);
            FromTo(HousePoints, 0, 2, 9, 11);
            FromTo(GamePoints, 0, 5, 4, 5);
            FromToReverse(GamePoints, 4, 3, 3, 0);
            FromTo(GamePoints, 5, 6, 0, 1);
            FromTo(GamePoints, 6, 7, 0, 4);
            FromTo(GamePoints, 6, 11, 4, 5);
            FromTo(GamePoints, 10, 11, 5, 6);
            FromToReverse(GamePoints, 10, 6, 6, 6);
            FromTo(GamePoints, 6, 7, 6, 11);
            FromTo(GamePoints, 5, 6, 10, 11);
            FromToReverse(GamePoints, 4, 3, 10, 6);
            FromToReverse(GamePoints, 3, -1, 6, 6);
            FromTo(GamePoints, 0, 1, 5, 6);
            FromTo(GoalPoints, 1, 5, 5, 6);
            FromTo(GoalPoints, 5, 6, 1, 5);
            FromToReverse(GoalPoints, 9, 5, 5, 5);
            FromToReverse(GoalPoints, 5, 4, 9, 6);

        }

        private void FromTo(List<Point> PointList, int startx, int endx, int starty, int endy)
        {
            for (int i = startx; i < endx; i++)
            {
                for (int j = starty; j < endy; j++)
                {
                    PointList.Add(new Point((i * 85) + 27, (j * 85) + 38));
                }
            }
        }

        private void FromToReverse(List<Point> PointList, int startx, int endx, int starty, int endy)
        {
            for (int i = startx; i > endx; i--)
            {
                for (int j = starty; j > endy - 1; j--)
                {
                    PointList.Add(new Point((i * 85) + 27, (j * 85) + 38));
                }
            }
        }

        internal Point GetPosition(int iPosition)
        {
            try
            {
                if (iPosition < 0)
                {
                    return HousePoints[(iPosition + 1) * -1];
                }
                else if (iPosition >= 1000)
                {
                    return GoalPoints[(iPosition - 1000)];
                }
                else
                {
                    if (iPosition >= 40)
                    {
                        return GamePoints[iPosition - 40];
                    }
                    return GamePoints[iPosition];
                }
            }
            catch (Exception)
            {
                return new Point(-1, -1);
            }
        }
    }
}
