using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroid_Belt_Assault //pg 92, building the Sprite Class
{
    class Sprite
    {
        public Texture2D Texture;

        protected List<Rectangle> frames = new List<Rectangle>();
        private int frameWidth = 0;
        private int frameHeight = 0;
        private int currentFrame;
        private float frameTime = 0.1f;
        private float timeForCurrentFrame = 0.0f;

        private Color tintColor = Color.White;
        private float rotation = 0.0f;

        public int CollisionRadius = 0;
        public int BoundingXPadding = 0;
        public int BoundingYPadding = 0;

        protected Vector2 location = Vector2.Zero;
        protected Vector2 velocity = Vector2.Zero; //end pg 92 

        //page 93, making the Sprite constructor

        public Sprite(
            Vector2 location,
            Texture2D texture,
            Rectangle initialFrame,
            Vector2 velocity)
        {
            this.location = location;
            Texture = texture;
            this.velocity = velocity;

            frames.Add(initialFrame);
            frameWidth = initialFrame.Width;
            frameHeight = initialFrame.Height;
        } // pg p3. this constructor directly sets the location, texture, and velocity members to the passed parameters. Then sets width and height given by Rectangle

        //adding public properties to Sprite class' members, pg 93
        public Vector2 Location
        {
            get { return location; }
            set { location = value;}
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Color TintColor
        {
            get { return tintColor; }
            set { tintColor = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value % MathHelper.TwoPi; }
        } // end 94 code. 
        //Location, Velocity, and TintColor propers are simple passthroughs for their private members. No extra code or checks needed. 
        //For rotation, look up mod. 

        //pg 94, adding more public properties - animation and drawing properties
        public int Frame
        {
            get { return currentFrame; }
            set
            {
                currentFrame = (int)MathHelper.Clamp(value, 0,
                    frames.Count - 1); //ensure when it is set, the value stored in currentFrame is valid for the frames list of Rectangles. Keeps frames correct. (avoid frame 10 on a 5 frame animation)
            }
        }

        public float FrameTime
        {
            get { return frameTime; }
            set { frameTime = MathHelper.Max(0, value); }
        }

        public Rectangle Source //returns rectangle associated with the current frame form the frames list
        {
            get { return frames[currentFrame]; }
        }

        public Rectangle Destination //builds a new rectangle based on the sprite's current location, with width & height of a frame
        {
            get
            {
                return new Rectangle(
                (int) location.X,
                (int) location.Y,
                frameWidth,
                frameHeight);
            }
        }

        public Vector2 Center //returns center 
        {
            get
            {
                return location +
                    new Vector2(frameWidth / 2, frameHeight / 2);
            }
        } //end pg 95 adding of properties 

        //pg 97, adding collision detection

        public Rectangle BoundingBoxRect //provides a rectangle object based on location, plus the size of the sprite with the padding. Called REct because there already a class
        {
            get
            {
                return new Rectangle(
                    (int)location.X + BoundingXPadding,
                    (int)location.Y + BoundingYPadding,
                    frameWidth - (BoundingXPadding * 2),
                    frameHeight - (BoundingYPadding * 2));
            }
        }

        public bool IsBoxColliding(Rectangle OtherBox) //returns true if two rectangles overlap at any point
        {
            return BoundingBoxRect.Intersects(OtherBox);
        }

        public bool IsCircleColliding(Vector2 otherCenter, float otherRadius) //uses math to determine interesect, by using sum and comparison of the circles' radius and center. 
        {
            if (Vector2.Distance(Center, otherCenter) <
                (CollisionRadius + otherRadius))
                return true;
            else
                return false;
        } //end pg 97 code, collision

        //pg 98, adding animation frames. 
        public void AddFrame(Rectangle frameRectangle)//add frames to the frames list
        {
            frames.Add(frameRectangle);// adds corresponding Rectangle to the frames List. 
        }// end pg 98

        //pg 98, updating the sprite
        public virtual void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            timeForCurrentFrame += elapsed;

            if (timeForCurrentFrame >= FrameTime)
            {
                currentFrame = (currentFrame + 1) % (frames.Count);// shorthand of "add 1 to current frame, divide the total by total frames of animation and return the remainder"
                timeForCurrentFrame = 0.0f; //after current frame has been updated, time for current frame is reset to 0.0f to begin the loop over again
            }

            location += (velocity * elapsed);  //multiplying velocity by ...Total seconds will determine the distanced moved over by a single frame. Ensures smoothness. 
        } // end pg 98 coding 

        //pg 100, drawing the sprite

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                Center,
                Source,
                tintColor,
                rotation,
                new Vector2(frameWidth / 2, frameHeight / 2),
                1.0f,
                SpriteEffects.None,
                0.0f);
        }//end page 100> overload that allows rotation and scaling. 











    }

}
