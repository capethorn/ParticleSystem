using System;
using System.Collections.Generic;
using System.Text;

namespace ParticleSystem
{
    public class TopEmitter : Emitter
    {
        public int Width; 

        public override void ResetParticle(Particle particle)
        {
            base.ResetParticle(particle); 

           
            particle.X = Particle.rand.Next(Width); 
            particle.Y = 0;   

            particle.SpeedY = 1; 
            particle.SpeedX = Particle.rand.Next(-2, 2); 
        }
    }
}
