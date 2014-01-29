using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Asteroid_Belt_Assault
{
    class StarField //pg 100
    {
        private List<Sprite> stars = new List<Sprite>();
        private int screenWidth = 800;
        private int screenHeight = 600;
        private Random rand = new Random();
        private Color[] colors = {Color.White, Color.Yellow,
                                     Color.Wheat, Color.WhiteSmoke,
                                     Color.SlateGray };

        public StarField( //constructor for starfield class
            int screenWidth,
            int screenHeight,
            int starCount,
            Vector2 starVelocity,
            Texture2D texture,
            Rectangle frameRectangle)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            for (int x = 0; x < starCount; x++) //cycles through, adds random stars with random colors
            {
                stars.Add(new Sprite(
                    new Vector2(rand.Next(0, screenWidth),
                        rand.Next(0, screenHeight)),
                        texture,
                        frameRectangle,
                        starVelocity));
                Color starColor = colors[rand.Next(0, colors.Count())];
                starColor *= (float)(rand.Next(30, 80) / 100f);
                stars[stars.Count() - 1].TintColor = starColor;
            }
        }//end pg 101


        public void Update(GameTime gameTime)// pg 102, updating and drawing StarField
        {
            foreach (Sprite star in stars) //processes each item in stars list, running the update method 
            {
                star.Update(gameTime);
                if (star.Location.Y > screenHeight)
                {
                    star.Location = new Vector2(
                        rand.Next(0, screenWidth), 0);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) //passes along the spriteBatch object to each stars in list and tells them to draw themselves.
        {
            foreach (Sprite star in stars)
            {
                star.Draw(spriteBatch);
            }
        }
    }

       
}
