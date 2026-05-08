using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static ParticleSystem.IImpactPoint;
using static ParticleSystem.Particle;

namespace ParticleSystem
{
    public partial class Form1 : Form
    {
        List<Emitter> emitters = new List<Emitter>();
        Emitter emitter;
        GravityPoint point1;
        GravityPoint point2;

        List<CounterPoint> counters = new List<CounterPoint>();

        public Form1()
        {
            InitializeComponent();
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);

            this.emitter = new Emitter
            {
                Direction = 0,
                Spreading = 360,
                SpeedMin = 3,
                SpeedMax = 8,
                ColorFrom = Color.Gold,
                ColorTo = Color.FromArgb(0, Color.Red),
                ParticlesPerTick = 8,
                X = picDisplay.Width / 2,
                Y = picDisplay.Height / 2,
                GravitationY = 0.1f,
                GravitationX = 0
            };

            emitters.Add(this.emitter);

            point1 = new GravityPoint
            {
                X = picDisplay.Width / 2 + 150,
                Y = picDisplay.Height / 2,
                Power = 80
            };
            point2 = new GravityPoint
            {
                X = picDisplay.Width / 2 - 150,
                Y = picDisplay.Height / 2,
                Power = 80
            };

            emitter.impactPoints.Add(point1);
            emitter.impactPoints.Add(point2);

            this.picDisplay.MouseClick += new MouseEventHandler(picDisplay_MouseClick);
        }

        private void picDisplay_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var counter = new CounterPoint
                {
                    X = e.X,
                    Y = e.Y,
                    Counter = 0
                };

                counters.Add(counter);
                emitter.impactPoints.Add(counter);
            }
            else if (e.Button == MouseButtons.Right)
            {
                CounterPoint clickedCounter = null;
                foreach (var counter in counters)
                {
                    float dx = counter.X - e.X;
                    float dy = counter.Y - e.Y;
                    double dist = Math.Sqrt(dx * dx + dy * dy);

                    if (dist < 30)
                    {
                        clickedCounter = counter;
                        break;
                    }
                }

                if (clickedCounter != null)
                {
                    counters.Remove(clickedCounter);
                    emitter.impactPoints.Remove(clickedCounter);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            emitter.UpdateState();

            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.Black);
                emitter.Render(g);
            }

            picDisplay.Invalidate();
        }

        private void picDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            foreach (var emitter in emitters)
            {
                emitter.MousePositionX = e.X;
                emitter.MousePositionY = e.Y;
            }

            point2.X = e.X;
            point2.Y = e.Y;
        }

        private void tbDirection_Scroll(object sender, EventArgs e)
        {
            emitter.Direction = tbDirection.Value;
            lblDirection.Text = $"{tbDirection.Value}°";
        }

        private void tbGraviton_Scroll(object sender, EventArgs e)
        {
            point1.Power = tbGraviton1.Value;
        }

        private void tbGraviton2_Scroll(object sender, EventArgs e)
        {
            point2.Power = tbGraviton2.Value;
        }
    }
}