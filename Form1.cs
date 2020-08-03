using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2019_11_27_MenschAergerDichNicht
{
    public partial class Form1 : Form
    {
        int Spieleranzahl = 4;
        Game Spiel;
        public Form1()
        {
            InitializeComponent();
        }

        private void Spiel_OnNewGame(object sender, string e)
        {
            Spiel = new Game(Spieleranzahl);
            Spiel.OnUpdate += Spiel_OnUpdate;
            Spiel.OnNextPlayer += Spiel_OnNextPlayer;
            Spiel.OnNewGame += Spiel_OnNewGame;
            Refresh();
            label1.Text = "Grün ist am Zug!";
        }

        private void Spiel_OnUpdate(object sender, string e)
        {
            this.Invoke(new Action(() =>
            {
                Refresh();
            }));
        }

        private void Spiel_OnNextPlayer(object sender, string e)
        {
            this.Invoke(new Action(() =>
            {
                label1.Text = e;
            }));
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Draw(e.Graphics);
        }

        private void Draw(Graphics iGr)
        {
            Field Spielfeld = Spiel.GetField();
            iGr.DrawImage(Spielfeld.Texture, 0, 0, Spielfeld.Groesse.Width, Spielfeld.Groesse.Height);
            Player[] Spieler = Spiel.GetPlayer();
            for (int i = 0; i < Spieler.Length; i++)
            {
                if (Spieler[i] != null)
                    iGr.FillEllipse(new SolidBrush(Spieler[i].Farbe), Spieler[i].AbsolutePosition.X, Spieler[i].AbsolutePosition.Y, Spieler[i].Groesse.Width, Spieler[i].Groesse.Height);
            }
            iGr.DrawImage(Spiel.SpielWuerfel.Texture, Spielfeld.Groesse.Width / 2 - Spiel.SpielWuerfel.Groesse.Width / 2 - 5, Spielfeld.Groesse.Height / 2 - Spiel.SpielWuerfel.Groesse.Height / 2, Spiel.SpielWuerfel.Groesse.Width, Spiel.SpielWuerfel.Groesse.Height);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Spiel.Hit(e.Location);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Spiel.Wuerfeln();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Spiel_OnNewGame(this, "Start Game");
        }
    }
}
