using System;
using System.Collections.Generic;
using System.Text;

namespace ParticleSystem
{
    public abstract class IImpactPoint
    {
        public float X;
        public float Y;

        public abstract void ImpactParticle(Particle particle);

        public virtual void Render(Graphics g)
        {
            g.FillEllipse(
                    new SolidBrush(Color.Red),
                    X - 5,
                    Y - 5,
                    10,
                    10
                );
        }

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
                g.DrawEllipse(
                       new Pen(Color.Red),
                       X - Power / 2,
                       Y - Power / 2,
                       Power,
                       Power
                   );

                var stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                var text = $"Я гравитон\nc силой {Power}";
                var font = new Font("Verdana", 10);

                var size = g.MeasureString(text, font);

                g.FillRectangle(
                    new SolidBrush(Color.Red),
                    X - size.Width / 2,
                    Y - size.Height / 2,
                    size.Width,
                    size.Height
                );

                g.DrawString(
                    text,
                    font,
                    new SolidBrush(Color.White),
                    X,
                    Y,
                    stringFormat
                );
            }
        }

        public class AntiGravityPoint : IImpactPoint
        {
            public int Power = 100;

            public override void ImpactParticle(Particle particle)
            {
                float gX = X - particle.X;
                float gY = Y - particle.Y;
                float r2 = (float)Math.Max(100, gX * gX + gY * gY);

                particle.SpeedX -= gX * Power / r2;
                particle.SpeedY -= gY * Power / r2;
            }
        }

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
                g.FillEllipse(new SolidBrush(Color.FromArgb(100, Color.Blue)), X - 30, Y - 30, 60, 60);
                g.DrawEllipse(new Pen(Color.Blue, 2), X - 30, Y - 30, 60, 60);

                var text = Counter.ToString();
                var font = new Font("Arial", 16, FontStyle.Bold);
                var stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                g.DrawString(text, font, new SolidBrush(Color.White), X, Y, stringFormat);

                font.Dispose();
            }
        }

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

                var text = $"Радар\n{ParticlesInRadar}";
                var font = new Font("Verdana", 10, FontStyle.Bold);
                var stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                var size = g.MeasureString(text, font);

                g.FillRectangle(new SolidBrush(Color.FromArgb(150, Color.Black)),
                    X - size.Width / 2, Y - size.Height / 2, size.Width, size.Height);

                g.DrawString(text, font, new SolidBrush(RadarColor), X, Y, stringFormat);

                font.Dispose();
            }
        }

        public class RepulsionPoint : IImpactPoint
        {
            public int Radius = 60;
            public int Power = 200;

            public override void ImpactParticle(Particle particle)
            {
                float dx = particle.X - X;
                float dy = particle.Y - Y;
                double dist = Math.Sqrt(dx * dx + dy * dy);

                if (dist + particle.Radius < Radius)
                {
                    float r2 = (float)Math.Max(10, dx * dx + dy * dy);
                    particle.SpeedX += dx * Power / r2;
                    particle.SpeedY += dy * Power / r2;
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

                var text = $"ОТБИВАЛКА";
                var font = new Font("Arial", 8, FontStyle.Bold);
                var stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                g.DrawString(text, font, new SolidBrush(Color.Cyan), X, Y - Radius - 5, stringFormat);
                font.Dispose();
            }
        }
    }
}