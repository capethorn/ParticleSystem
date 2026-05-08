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
    }
}