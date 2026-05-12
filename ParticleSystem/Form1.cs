using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ParticleSystem
{
    public partial class Form1 : Form
    {
        Emitter emitter;
        GravityPoint point1;
        GravityPoint point2;
        List<CounterPoint> counters = new List<CounterPoint>();
        RadarPoint radar;
        RepulsionPoint repulsion;

        public Form1()
        {
            InitializeComponent();
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);

            timer1.Enabled = true;
            timer1.Interval = 50;

            this.emitter = new Emitter
            {
                Direction = 0,
                Spreading = 50,
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


            point1 = new GravityPoint
            {
                X = picDisplay.Width / 2 + 150,
                Y = picDisplay.Height / 2,
                Power = 0
            };

            point2 = new GravityPoint
            {
                X = picDisplay.Width / 2 - 150,
                Y = picDisplay.Height / 2,
                Power = 0
            };

            radar = new RadarPoint
            {
                X = picDisplay.Width / 2,
                Y = picDisplay.Height / 2,
                Radius = 80,
                RadarColor = Color.Lime
            };

            repulsion = new RepulsionPoint
            {
                X = picDisplay.Width / 2,
                Y = picDisplay.Height / 2,
                Radius = 60,
                Power = 200
            };

            emitter.impactPoints.Add(point1);
            emitter.impactPoints.Add(point2);
            emitter.impactPoints.Add(radar);
            emitter.impactPoints.Add(repulsion);

            this.picDisplay.MouseClick += picDisplay_MouseClick;
            this.picDisplay.MouseWheel += picDisplay_MouseWheel;
            this.picDisplay.MouseMove += picDisplay_MouseMove;

                tbDirection.Scroll += tbDirection_Scroll;
                tbGraviton1.Scroll += tbGraviton_Scroll;
                tbGraviton2.Scroll += tbGraviton2_Scroll;
        }

        private void picDisplay_MouseWheel(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                int newRadius = radar.Radius - (e.Delta / 120 * 10);
                radar.Radius = Math.Clamp(newRadius, 30, 200);
            }
            else
            {
                int newRadius = repulsion.Radius - (e.Delta / 120 * 10);
                repulsion.Radius = Math.Clamp(newRadius, 30, 150);
            }
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
            repulsion.X = e.X;
            repulsion.Y = e.Y;
        }

        private void tbDirection_Scroll(object sender, EventArgs e)
        {

             emitter.Direction = tbDirection.Value;
           
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