using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2019_11_27_MenschAergerDichNicht
{
    class GameManager
    {
        internal Field Spielfeld = new Field();
        internal Player[] Spielfiguren;
        internal event EventHandler<string> OnUpdate;

        public GameManager(int Spieleranzahl)
        {
            Spielfiguren = new Player[Spieleranzahl * 4];
            for (int sa = 0; sa < Spieleranzahl; sa++)
            {
                for (int s = 0; s < 4; s++)
                {
                    Player NewPlayer = new Player(Spielfeld, sa + 1, s + 1);
                    NewPlayer.OnUpdate += NewPlayer_OnUpdate;
                    Spielfiguren[(sa * 4) + s] = NewPlayer;
                }
            }
        }

        private void NewPlayer_OnUpdate(object sender, string e)
        {
            OnUpdate?.Invoke(this, e);
        }

        internal Player isOtherPlayer(int iPosition, Player iPlayer)
        {
            for (int i = 0; i < Spielfiguren.Length; i++)
            {
                if (Spielfiguren[i].Team != iPlayer.Team && Spielfiguren[i].Position == iPosition)
                {
                    return Spielfiguren[i];
                }
            }
            return null;
        }

        internal Player GetPlayerByPosition(Point iPosition)
        {
            for (int i = 0; i < Spielfiguren.Length; i++)
            {
                if (Spielfeld.GetPosition(Spielfiguren[i].Position) == iPosition)
                {
                    return Spielfiguren[i];
                }
            }
            return null;
        }

        internal bool IsValid(Player iPlayer, int iWurf)
        {
            Player Check = GetPlayerByPosition(Spielfeld.GetPosition(iPlayer.Position + iWurf));
            if (iPlayer.InHouse())
            {
                if (iWurf != 6)
                {
                    return false;
                }
                Check = GetPlayerByPosition(Spielfeld.GetPosition(iPlayer.Offset));
            }
            if (iPlayer.InGoal() || iPlayer.Score + iWurf >= 40)
            {
                Check = GetPlayerByPosition(Spielfeld.GetPosition((iPlayer.Offset / 10) * 4 + (iPlayer.Score + iWurf - 40) + 1000));
                List<Player> Checklist = new List<Player>();
                for (int i = 0; i < Spielfiguren.Length; i++)
                {
                    if (Spielfiguren[i].InGoal() && Spielfiguren[i].Team == iPlayer.Team)
                    {
                        Checklist.Add(Spielfiguren[i]);
                    }
                }
                for (int i = 0; i < Checklist.Count; i++)
                {
                    if (Checklist[i].Score > iPlayer.Score &&  Checklist[i].Score != iPlayer.Score && Checklist[i].Score < iPlayer.Score + iWurf)
                    {
                        return false;
                    }
                }
            } 
            if (Check != null)
            {
                if (Check.Team != iPlayer.Team)
                {
                    return ScoreChecker(iPlayer, iWurf);
                }
            }
            else
            {
                return ScoreChecker(iPlayer, iWurf);
            }
            return false;
        }

        private bool ScoreChecker(Player iPlayer, int iWurf)
        {
            if (iPlayer.Score + iWurf < 44)
            {
                return true;
            }
            return false;
        }

        internal Player CheckWinner()
        {
            for (int i = 0; i < Spielfiguren.Length / 4; i++)
            {
                if (AllInGoal(i + 1))
                {
                    return Spielfiguren[i * 4];
                }
            }
            return null;
        }

        private bool AllInGoal(int iTeam)
        {
            for (int i = 0; i < Spielfiguren.Length; i++)
            {
                if (Spielfiguren[i].Team == iTeam)
                {
                    if(!Spielfiguren[i].InGoal())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        internal bool AllInHome(Player iPlayer)
        {
            for (int i = 0; i < Spielfiguren.Length; i++)
            {
                if (Spielfiguren[i].Team == iPlayer.Team)
                {
                    if (!(Spielfiguren[i].InHouse() || Spielfiguren[i].InGoal()))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        internal bool CanMove(int Team, int iWurf)
        {
            List<Player> Spielerliste = new List<Player>();
            for (int i = 0; i < Spielfiguren.Length; i++)
            {
                if (Spielfiguren[i].Team == Team)
                {
                    Spielerliste.Add(Spielfiguren[i]);
                }
            }
            for (int i = 0; i < Spielerliste.Count; i++)
            {
                if (IsValid(Spielerliste[i], iWurf))
                {
                    return true;
                }
            }
            return false;
        }

        internal Player HitPlayer(Point iPoint)
        {
            for (int i = 0; i < Spielfiguren.Length; i++)
            {
                double Abstand = Distance(iPoint, Spielfiguren[i].AbsolutePosition, 1, Spielfiguren[i].Groesse.Width);
                if (Abstand <= Spielfiguren[i].Groesse.Width / 2)
                {
                    return Spielfiguren[i];
                }
            }
            return null;
        }

        private double Distance(Point Point1, Point Point2, int objectgroesse1, int objectgroesse2)
        {
            Point tempPoint1 = new Point(Point1.X + objectgroesse1 / 2, Point1.Y + objectgroesse1 / 2);
            Point tempPoint2 = new Point(Point2.X + objectgroesse2 / 2, Point2.Y + objectgroesse2 / 2);
            double Xsquare = (tempPoint2.X - tempPoint1.X) * (tempPoint2.X - tempPoint1.X);
            double Ysquare = (tempPoint2.Y - tempPoint1.Y) * (tempPoint2.Y - tempPoint1.Y);
            return Math.Sqrt(Xsquare + Ysquare);
        }
    }
}
