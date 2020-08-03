using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2019_11_27_MenschAergerDichNicht
{
    class Game
    {
        private GameManager Spiel;
        private int Spieleranzahl;
        internal Wuerfel SpielWuerfel = new Wuerfel();
        private TaskCompletionSource<int> OnWuerfeln = new TaskCompletionSource<int>();
        private TaskCompletionSource<Player> OnClickPlayer = new TaskCompletionSource<Player>();
        internal event EventHandler<string> OnUpdate;
        internal event EventHandler<string> OnNextPlayer;
        internal event EventHandler<string> OnNewGame;

        public Game(int iSpieleranzahl)
        {
            SpielWuerfel.OnUpdate += SpielWuerfel_OnUpdate;
            Spieleranzahl = iSpieleranzahl;
            Spiel = new GameManager(iSpieleranzahl);
            Spiel.OnUpdate += Spiel_OnUpdate;
            StartGame();
        }

        private void Spiel_OnUpdate(object sender, string e)
        {
            OnUpdate?.Invoke(this, e);
        }

        private void SpielWuerfel_OnUpdate(object sender, string e)
        {
            OnUpdate?.Invoke(this, "WuerfelUpdate");
        }

        private async void StartGame()
        {
            Player Winner = null;
            while (Winner == null)
            {
                for (int i = 0; i < Spieleranzahl; i++)
                {
                    Winner = Spiel.CheckWinner();
                    if (Winner != null)
                    {
                        EndGame(Winner);
                        return;
                    }
                    OnNextPlayer?.Invoke(this, Spiel.Spielfiguren[i * 4].GameColor + " ist am Zug!");
                    int Wurf = await OnWuerfeln.Task;
                    if (Spiel.AllInHome(Spiel.Spielfiguren[i * 4]) && !Spiel.CanMove(i + 1, Wurf))
                    {
                        for (int w = 0; w < 2; w++)
                        {
                            OnWuerfeln = new TaskCompletionSource<int>();
                            Wurf = await OnWuerfeln.Task;
                            if (Spiel.CanMove(i + 1, Wurf))
                                break;
                        }
                    }
                    while (Spiel.CanMove(i + 1, Wurf))
                    {
                        OnClickPlayer = new TaskCompletionSource<Player>();
                        Player CheckValid = await OnClickPlayer.Task;
                        if (CheckValid != null)
                        {
                            if (CheckValid.Team == i + 1 && Spiel.IsValid(CheckValid, Wurf))
                            {
                                if (CheckValid.InHouse() && Wurf == 6)
                                {
                                    CheckValid.MoveOut();
                                }
                                else
                                {
                                    for (int m = 0; m < Wurf; m++)
                                    {
                                        CheckValid.Move();
                                    }
                                }
                                Player KillPlayer = Spiel.isOtherPlayer(CheckValid.Position, CheckValid);
                                if (KillPlayer != null)
                                {
                                    KillPlayer.Kill();
                                }
                                OnUpdate?.Invoke(this, i.ToString());
                                break;
                            }
                        }
                    }
                    if (Wurf == 6)
                    {
                        i--;
                    }
                    OnWuerfeln = new TaskCompletionSource<int>();
                }
            }
            EndGame(Winner);
        }

        private void EndGame(Player iWinner)
        {
            MessageBox.Show("Herzlichen Glückwunsch " + iWinner.GameColor + " hat gewonnen!");
            OnNewGame?.Invoke(this, "New Game");
        }

        internal void Hit(Point iPoint)
        {
            if (iPoint.X > Spiel.Spielfeld.Groesse.Width / 2 - SpielWuerfel.Groesse.Width / 2 - 5 && iPoint.X < Spiel.Spielfeld.Groesse.Width / 2 - SpielWuerfel.Groesse.Width / 2 - 5 + SpielWuerfel.Groesse.Width && iPoint.Y > Spiel.Spielfeld.Groesse.Height / 2 - SpielWuerfel.Groesse.Height / 2 && iPoint.Y < Spiel.Spielfeld.Groesse.Height / 2 - SpielWuerfel.Groesse.Height / 2 + SpielWuerfel.Groesse.Height)
            {
                Wuerfeln();
            }
            else
            {
                if (!OnClickPlayer.Task.IsCompleted)
                {
                    OnClickPlayer.SetResult(Spiel.HitPlayer(iPoint));
                }
            }
        }

        internal void Wuerfeln()
        {
            if (!OnWuerfeln.Task.IsCompleted)
            {
                int Wurf = SpielWuerfel.Wuerfeln();
                OnWuerfeln.SetResult(Wurf);
            }
        }

        internal Field GetField()
        {
            return Spiel.Spielfeld;
        }

        internal Player[] GetPlayer()
        {
            return Spiel.Spielfiguren;
        }
    }
}
