using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GDIPlusSample
{
    public class CanvasControl : Control
    {
        public CanvasControl()
        {
            ResizeRedraw = true;
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;

            g.Clear(Color.Black);
        }

        private bool firstDraw = true;
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var rect = ClientRectangle;

            g.FillEllipse(Brushes.Green, rect);
        }

        private void Sample1(PaintEventArgs e)
        {
            var g = e.Graphics;
            var rect = ClientRectangle;
            var clipRect = e.ClipRectangle;

            // 선
            g.DrawLine(Pens.Red, 50, 50, 100, 100);

            // 면
            g.DrawRectangle(Pens.Green, 10, 10, 30, 30);

            // 원
            g.DrawEllipse(Pens.Gray, 100, 100, 50, 50);

            if (firstDraw == false)
            {
                g.FillRectangle(Brushes.Gray, rect);
            }

            firstDraw = false;
        }
    }
}
