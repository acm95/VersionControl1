using _8gyak.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8gyak
{
    public class Present : Toy
    {
        public SolidBrush PresentColor { get; private set; }

        public Present(Color color)
        {
            PresentColor = new SolidBrush(color);
        }
        protected override void DrawImage(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Blue), 0, 0, Width, Height);
        }
    }
}
