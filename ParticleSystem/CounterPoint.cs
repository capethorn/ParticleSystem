using System;
using System.Drawing;

namespace ParticleSystem
{
    public class CounterPoint : IImpactPoint
    {
        public int Counter = 0;

        public override void ImpactParticle(Particle particle)
        {
            float gX = X - particle.X;
            float gY = Y - particle.Y;
            double r = Math.Sqrt(gX * gX + gY * gY);

            if (r + particle.Radius < 30)
            {
                particle.Life = 0;
                Counter++;
            }
        }

        public override void Render(Graphics g)
        {
            using (var brush = new SolidBrush(Color.FromArgb(100, Color.Blue)))
            {
                g.FillEllipse(brush, X - 30, Y - 30, 60, 60);
            }

            using (var pen = new Pen(Color.Blue, 2))
            {
                g.DrawEllipse(pen, X - 30, Y - 30, 60, 60);
            }

            var text = Counter.ToString();

            using (var font = new Font("Arial", 16, FontStyle.Bold))
            {
                var stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                var size = g.MeasureString(text, font);


                using (var brush = new SolidBrush(Color.Black))
                {
                    g.FillRectangle(brush, X - size.Width / 2, Y - size.Height / 2, size.Width, size.Height);
                }

                using (var brush = new SolidBrush(Color.White))
                {
                    g.DrawString(text, font, brush, X, Y, stringFormat);
                }

                stringFormat.Dispose();
            }
        }
    }
}