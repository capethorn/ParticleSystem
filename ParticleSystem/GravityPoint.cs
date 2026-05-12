using System;
using System.Drawing;

namespace ParticleSystem
{
    public class GravityPoint : IImpactPoint
    {
        public int Power = 100;

        public override void ImpactParticle(Particle particle)
        {
            float gX = X - particle.X;
            float gY = Y - particle.Y;

            double r = Math.Sqrt(gX * gX + gY * gY);

            if (r + particle.Radius < Power / 2)
            {
                float r2 = (float)Math.Max(100, gX * gX + gY * gY);
                particle.SpeedX += gX * Power / r2;
                particle.SpeedY += gY * Power / r2;
            }
        }

        public override void Render(Graphics g)
        {
          
            using (var pen = new Pen(Color.Red))
            {
                g.DrawEllipse(pen, X - Power / 2, Y - Power / 2, Power, Power);
            }

            var stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            var text = $"Я гравитон\nc силой {Power}";

            using (var font = new Font("Verdana", 10))
            {
                var size = g.MeasureString(text, font);

             
                using (var brush = new SolidBrush(Color.Red))
                {
                    g.FillRectangle(brush, X - size.Width / 2, Y - size.Height / 2, size.Width, size.Height);
                }

                using (var brush = new SolidBrush(Color.White))
                {
                    g.DrawString(text, font, brush, X, Y, stringFormat);
                }
            }

            stringFormat.Dispose();
        }
    }
}