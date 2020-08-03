using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _2019_11_27_MenschAergerDichNicht
{
    class Player
    {
        internal readonly int Team = 0;
        internal readonly Color Farbe;
        internal int Position = 0;
        internal readonly int StartPosition = 0;
        internal Point AbsolutePosition = new Point(0, 0);
        private Field Spielfeld;
        internal Size Groesse = new Size(50, 50);
        internal readonly int Offset = 0;
        internal int Score = -1;
        internal string GameColor;
        internal event EventHandler<string> OnUpdate;

        public Player(Field iField, int iTeam, int iTeamPosition)
        {
            Spielfeld = iField;
            Team = iTeam;
            Position = -1 * ((iTeam - 1) *  4 + iTeamPosition);
            StartPosition = Position;
            AbsolutePosition = Spielfeld.GetPosition(Position);
            switch (iTeam)
            {
                case 1:
                    Farbe = Color.DarkGreen;
                    GameColor = "Grün";
                    Offset = 0;
                    break;
                case 2:
                    Farbe = Color.DarkBlue;
                    GameColor = "Blau";
                    Offset = 10;
                    break;
                case 3:
                    Farbe = Color.DarkRed;
                    GameColor = "Rot";
                    Offset = 20;
                    break;
                case 4:
                    Farbe = Color.Goldenrod;
                    GameColor = "Gelb";
                    Offset = 30;
                    break;
            }
        }

        internal void Move()
        {
            Step(Position + 1);
        }

        internal void MoveOut()
        {
            Step(Offset);
        }

        private void Step(int iNewPosition)
        {
            Score++;
            if (Score >= 40)
            {
                Position = (Offset / 10) * 4 + (Score - 40) + 1000;
            }
            else
            {
                Position = iNewPosition;
                if (Position >= 40)
                {
                    Position = 0;
                }
            }
            Point NewPos = Spielfeld.GetPosition(Position);
            while (NewPos != AbsolutePosition)
            {
                int Speed = 30;
                if (!(Math.Abs(AbsolutePosition.X - NewPos.X) > Speed))
                {
                    AbsolutePosition.X = NewPos.X;
                }
                if (NewPos.X > AbsolutePosition.X)
                {
                    AbsolutePosition.X += Speed;
                }
                else if (NewPos.X < AbsolutePosition.X)
                {
                    AbsolutePosition.X -= Speed;
                }

                if (!(Math.Abs(AbsolutePosition.Y - NewPos.Y) > Speed))
                {
                    AbsolutePosition.Y = NewPos.Y;
                }
                if (NewPos.Y > AbsolutePosition.Y)
                {
                    AbsolutePosition.Y += Speed;
                }
                else if (NewPos.Y < AbsolutePosition.Y)
                {
                    AbsolutePosition.Y -= Speed;
                }
                OnUpdate?.Invoke(this, "Player Update");
            }
        }

        internal bool InGoal()
        {
            if (Position >= 1000)
            {
                return true;
            }
            return false;
        }

        internal bool InHouse()
        {
            if (Position < 0)
            {
                return true;
            }
            return false;
        }

        internal void Kill()
        {
            Position = StartPosition;
            Score = -1;
            AbsolutePosition = Spielfeld.GetPosition(Position);
        }
    }
}
