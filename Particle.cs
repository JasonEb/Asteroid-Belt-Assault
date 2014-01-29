using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroid_Belt_Assault //CH05!! Pg 142
{
    class Particle : Sprite //extends to the Sprite class, it's a child. Has features of parent in addition to:
    {
        private Vector2 acceleration; //applied to the sprite's felocity during each update cycle
        private float maxSpeed; //limit
        private int initialDuration;
        private int remainingDuration;
        private Color initialColor;
        private Color finalColor;

        public int ElapsedDuration //pg142, adding properties to access info about members
        {
            get
            {
                return initialDuration - remainingDuration;
            }
        }

        public float DurationProgress
        {
            get
            {
                return (float)ElapsedDuration /
                    (float)initialDuration;
            }
        }

        public bool IsActive //returns false if remainingDuration has reached zero
        {
            get
            {
                return (remainingDuration > 0);
            }
        }

        public Particle( //adding constructor 
            Vector2 location,
            Texture2D texture,
            Rectangle initialFrame,
            Vector2 velocity,
            Vector2 acceleration,
            float maxSpeed,
            int duration,
            Color initialColor,
            Color finalColor)
            : base(location, texture, initialFrame, velocity)
        {
            initialDuration = duration;
            remainingDuration = duration;
            this.acceleration = acceleration;
            this.initialColor = initialColor;
            this.maxSpeed = maxSpeed;
            this.finalColor = finalColor; //end pg 143 code
        }

        //pg 144, updating and drawing particles
        public override void Update(GameTime gameTime)
        {
            if (IsActive) //i guess that means if true)
            {
                velocity += acceleration;
                if (velocity.Length() > maxSpeed) //if velocity is faster than max
                {
                    velocity.Normalize();
                    velocity *= maxSpeed; //i guess normalizes to one, than becomes maxspeed?
                }
                TintColor = Color.Lerp( //what's Lerp? 
                    initialColor, // Lerp returns a color between two colors privded as parameters, 
                    finalColor, //that is scaled towards one color and ranges from zero to one (zero being one color and one being the other)
                    DurationProgress); //Lerp could be used for anything else with such parameters.
                remainingDuration--;
                base.Update(gameTime); //i don't see this anywhere else
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive) //while particle class is active
            {
                base.Draw(spriteBatch);
            }
        } //end 144
        


    }




}
