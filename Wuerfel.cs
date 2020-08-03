using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2019_11_27_MenschAergerDichNicht
{
    class Wuerfel
    {
        public Bitmap Texture = Properties.Resources._1;
        Random rnd = new Random();
        public event EventHandler<string> OnUpdate;
        List<Bitmap> Textures = new List<Bitmap>();
        internal Size Groesse = new Size(90, 80);

        public Wuerfel()
        {
            Textures.Add(Properties.Resources._1);
            Textures.Add(Properties.Resources._2);
            Textures.Add(Properties.Resources._3);
            Textures.Add(Properties.Resources._4);
            Textures.Add(Properties.Resources._5);
            Textures.Add(Properties.Resources._6);
        }

        public int Wuerfeln()
        {
            int Zahl = 0;
            int Delay = 25;
            for (int i = 0; i < 10; i++)
            {
                Zahl = rnd.Next(1, 7);
                Texture = Textures[Zahl - 1];
                OnUpdate?.Invoke(this, "NewWuerfel");
                Delay += 1;
                Task.Delay(Delay);
            }
            return Zahl;
        }
    }
}
