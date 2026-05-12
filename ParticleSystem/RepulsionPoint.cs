using System;
using System.Drawing;

namespace ParticleSystem
{
    public class RepulsionPoint : IImpactPoint
    {
        public int Radius = 60;
        public int Power = 300;

        public override void ImpactParticle(Particle particle)
        {
            float dx = particle.X - X;
            float dy = particle.Y - Y;
            float dist = (float)Math.Sqrt(dx * dx + dy * dy);

            float border = Radius - particle.Radius;

            if (dist + particle.Radius <= Radius)
            {
                float nx = dx / dist;
                float ny = dy / dist;

                particle.X = X + nx * border;
                particle.Y = Y + ny * border;

                float speed = particle.SpeedX * nx + particle.SpeedY * ny;
                particle.SpeedX -= speed * nx * (Power / 50f);
                particle.SpeedY -= speed * ny * (Power / 50f);

                particle.SpeedX += nx * (Power / 20f);
                particle.SpeedY += ny * (Power / 20f);
            }
        }

        public override void Render(Graphics g)
        {
            using (var brush = new SolidBrush(Color.FromArgb(80, Color.Cyan)))
            using (var pen = new Pen(Color.Cyan, 2))
            {
                g.FillEllipse(brush, X - Radius, Y - Radius, Radius * 2, Radius * 2);
                g.DrawEllipse(pen, X - Radius, Y - Radius, Radius * 2, Radius * 2);
            }

            var stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            var text = $"ОТБИВАЛКА";

            using (var font = new Font("Arial", 8, FontStyle.Bold))
            {
                var size = g.MeasureString(text, font);

                using (var brush = new SolidBrush(Color.Black))
                {
                    g.FillRectangle(brush, X - size.Width / 2, Y - Radius - size.Height / 2, size.Width, size.Height);
                }

                using (var brush = new SolidBrush(Color.Cyan))
                {
                    g.DrawString(text, font, brush, X, Y - Radius - 5, stringFormat);
                }
            }

            stringFormat.Dispose();
        }
    }
}