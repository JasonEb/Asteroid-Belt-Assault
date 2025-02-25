﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Asteroid_Belt_Assault
{
    class ExplosionManager //pg 146 code, making Explosions
    {
        //declarations
        private Texture2D texture;
        private List<Rectangle> pieceRectangles = new List<Rectangle>();
        private Rectangle pointRectangle;
        float screenLength;

        private int minPieceCount = 3;
        private int maxPieceCount = 6;
        private int minPointCount = 20;
        private int maxPointCount = 30;

        private int durationCount = 90; //each particle will live for 90 seconds
        private float explosionMaxSpeed = 30f; //speed

        private float pieceSpeedScale = 30f; //these control how fast the particles move away
        private int pointSpeedMin = 15;
        private int pointSpeedMax = 30;

        private Color initialColor = new Color(1.0f, 0.3f, 0f) * 0.5f;
        private Color finalColor = new Color(0f, 0f, 0f, 0f);

        Random rand = new Random();

        private List<Particle> ExplosionParticles = new List<Particle>();

        //adding a constructor
        public ExplosionManager(
            Texture2D texture,
            Rectangle initialFrame,
            int pieceCount,
            Rectangle pointRectangle,
            float screenLength)
        {
            this.texture = texture;
            for (int x = 0; x < pieceCount; x++)
            {
                pieceRectangles.Add(new Rectangle(
                    initialFrame.X + (initialFrame.Width * x),
                    initialFrame.Y,
                    initialFrame.Width,
                    initialFrame.Height));
            }
            this.pointRectangle = pointRectangle;
            this.screenLength = screenLength;
        }//end pg 147

        public Vector2 randomDirection(float scale)//pg 147, creating explosions
        {
            Vector2 direction;
            do
            {
                direction = new Vector2(
                    rand.Next(0, 101) - 50,
                    rand.Next(0, 101) - 50);
            } while (direction.Length() == 0);
            direction.Normalize();
            direction *= pieceSpeedScale;

            return direction;
        }

        public void AddExplosion(Vector2 location, Vector2 momentum)
        {
            Vector2 pieceLocation = location -
                new Vector2(pieceRectangles[0].Width / 2, //this is to keep it centered
                    pieceRectangles[0].Height / 2); //on explosion

            int pieces = rand.Next(minPieceCount, maxPieceCount + 1); //how many pieces
            for (int x = 0; x < pieces; x++)
            {
                ExplosionParticles.Add(new Particle(
                    pieceLocation,
                    texture,
                    pieceRectangles[rand.Next(0, pieceRectangles.Count)],
                    randomDirection(pieceSpeedScale) + momentum, //momentum creates drift effect
                    Vector2.Zero,
                    explosionMaxSpeed,
                    durationCount,
                    initialColor,
                    finalColor));
            }

            int points = rand.Next(minPointCount, maxPointCount + 1);
            for (int x = 0; x < points; x++)
            {
                ExplosionParticles.Add(new Particle(
                    location,
                    texture,
                    pointRectangle,
                    randomDirection((float)rand.Next(
                        pointSpeedMin, pointSpeedMax)) + momentum,
                    Vector2.Zero,
                    explosionMaxSpeed,
                    durationCount,
                    initialColor,
                    finalColor)); //end pg 148 code
            }

            float pan = 2 * (location.X / screenLength) - 1;

            SoundManager.PlayExplosion(pan); //pg 161, sound use
        }
          
        public void Update(GameTime gameTime) //pg 149 code, updating and drawing explosions
        {
            for (int x = ExplosionParticles.Count - 1; x >= 0; x--)
            {
                if (ExplosionParticles[x].IsActive)
                {
                    ExplosionParticles[x].Update(gameTime);
                }
                else
                {
                    ExplosionParticles.RemoveAt(x);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle particle in ExplosionParticles)
            {
                particle.Draw(spriteBatch);
            }
        }

     



    }
}
