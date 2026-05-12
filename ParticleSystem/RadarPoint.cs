using System;
using System.Collections.Generic;
using System.Drawing;

namespace ParticleSystem
{
    public class RadarPoint : IImpactPoint
    {
        public int Radius = 80;
        public int ParticlesInRadar = 0;
        public Color RadarColor = Color.Lime;

        public void UpdateCounter(List<Particle> particles)
        {
            ParticlesInRadar = 0;
            foreach (var particle in particles)
            {
                if (particle.Life > 0)
                {
                    float dx = X - particle.X;
                    float dy = Y - particle.Y;
                    double dist = Math.Sqrt(dx * dx + dy * dy);
                    if (dist + particle.Radius < Radius)
                    {
                        ParticlesInRadar++;
                        if (particle is Particle.ParticleColorful colorful)
                        {
                            colorful.IsInRadar = true;
                        }
                    }
                    else
                    {
                        if (particle is Particle.ParticleColorful colorful)
                        {
                            colorful.IsInRadar = false;
                        }
                    }
                }
            }
        }

        public override void ImpactParticle(Particle particle)
        {
          
        }

        public override void Render(Graphics g)
        {

            using (var brush = new SolidBrush(Color.FromArgb(80, RadarColor)))
            using (var pen = new Pen(RadarColor, 2))
            {
                g.FillEllipse(brush, X - Radius, Y - Radius, Radius * 2, Radius * 2);
                g.DrawEllipse(pen, X - Radius, Y - Radius, Radius * 2, Radius * 2);
            }

            var stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            var text = $"Радар\n{ParticlesInRadar}";

            using (var font = new Font("Verdana", 10, FontStyle.Bold))
            {
                var size = g.MeasureString(text, font);

                using (var brush = new SolidBrush(Color.Black))
                {
                    g.FillRectangle(brush, X - size.Width / 2, Y - size.Height / 2, size.Width, size.Height);
                }

                using (var brush = new SolidBrush(RadarColor))
                {
                    g.DrawString(text, font, brush, X, Y, stringFormat);
                }
            }

            stringFormat.Dispose();
        }
    }
}