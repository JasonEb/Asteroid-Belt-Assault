﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace Asteroid_Belt_Assault
{
    class PlayerManager//pg 119, creating the player
    {
        public Sprite playerSprite;
        private float playerSpeed = 160.0f; //moves at 160 pixels per second 
        private Rectangle playerAreaLimit; // when they leave this rectnagle, position will be readjusted

        public long PlayerScore = 0;
        public int LivesRemaining = 3;
        public bool Destroyed = false;

        private Vector2 gunOffset = new Vector2(25, 10);
        private float shotTimer = 0.0f;
        private float minShotTimer = 0.2f;
        private int playerRadius = 15;
        public ShotManager PlayerShotManager;

        float pan;

        public PlayerManager(
            Texture2D texture,
            Rectangle initialFrame,
            int frameCount,
            Rectangle screenBounds)
        {
            playerSprite = new Sprite(
                new Vector2(500, 500),
                texture,
                initialFrame,
                Vector2.Zero);

            PlayerShotManager = new ShotManager(
                texture,
                new Rectangle(0, 300, 5, 5),
                4,
                2,
                250f,
                screenBounds);

            playerAreaLimit =
                new Rectangle(
                    0,
                    screenBounds.Height / 2,
                    screenBounds.Width,
                    screenBounds.Height / 2);

            for (int x = 1; x < frameCount; x++)
            {
                playerSprite.AddFrame(
                    new Rectangle(
                        initialFrame.X + (initialFrame.Width * x),
                        initialFrame.Y,
                        initialFrame.Width,
                        initialFrame.Height));
            }
            playerSprite.CollisionRadius = playerRadius;

            float pan = 2 * (playerSprite.Source.Location.X / screenBounds.Width) - 1;

            this.pan = pan; 
                

        }// end pg 120 coding

        private void FireShot() //pg 121, handling user input
        {
            if (shotTimer >= minShotTimer)
            {
                PlayerShotManager.FireShot(
                    playerSprite.Location + gunOffset,
                    new Vector2(0, -1),
                    true);
                shotTimer = 0.0f;
            }
        }

        private void HandleKeyboardInput(KeyboardState keyState) //pg 121, handlekeyboardinput helper method
        {
            if (keyState.IsKeyDown(Keys.Up))
            {
                playerSprite.Velocity += new Vector2(0, -1);
            }

            if (keyState.IsKeyDown(Keys.Down))
            {
                playerSprite.Velocity += new Vector2(0, 1);
            }

            if (keyState.IsKeyDown(Keys.Left))
            {
                playerSprite.Velocity += new Vector2(-1, 0);
                //SoundManager.PlayLeftSound(-1.0f, true);
            }

            /*else if (keyState.IsKeyUp(Keys.Left))
            {
                SoundManager.PlayLeftSound(pan, false);
            }*/

            if (keyState.IsKeyDown(Keys.Right))
            {
                playerSprite.Velocity += new Vector2(1, 0);
                //SoundManager.PlayRightSound(1.0f, true);
            }

            /*else if (keyState.IsKeyUp(Keys.Right))
            {
                SoundManager.PlayRightSound(pan, false);
            }*/

            if (keyState.IsKeyDown(Keys.Space))
            {
                FireShot();
            }
        }

        private void HandleGamepadInput(GamePadState gamePadState)
        {
            playerSprite.Velocity +=
                new Vector2(
                    gamePadState.ThumbSticks.Left.X,
                    -gamePadState.ThumbSticks.Left.Y);

            if (gamePadState.Buttons.A == ButtonState.Pressed)
            {
                FireShot();
            }


            if (gamePadState.ThumbSticks.Left.X < 0.0f || gamePadState.ThumbSticks.Left.Y < 0.0f)
                SoundManager.PlayLeftSound(-1.0f, true);
            else if (gamePadState.ThumbSticks.Left.X == 0)
                SoundManager.PlayLeftSound(pan, false);

            if (gamePadState.ThumbSticks.Left.X > 0.0f || gamePadState.ThumbSticks.Left.Y > 0.0f)
                SoundManager.PlayRightSound(-1.0f, true);
            else if (gamePadState.ThumbSticks.Left.X == 0)
                SoundManager.PlayRightSound(pan, false); 




        } //end pg 122 

        private void imposeMovementLimits()//pg 123, updating and darwing the player's ship
        {
            Vector2 location = playerSprite.Location;

            if (location.X < playerAreaLimit.X)
                location.X = playerAreaLimit.X;

            if (location.X >
                (playerAreaLimit.Right - playerSprite.Source.Width))
                location.X =
                        (playerAreaLimit.Right - playerSprite.Source.Width);
            if (location.Y < playerAreaLimit.Y)
                location.Y = playerAreaLimit.Y;

            if (location.Y > playerAreaLimit.Bottom - playerSprite.Source.Height)
            location.Y =
                (playerAreaLimit.Bottom - playerSprite.Source.Height);

            playerSprite.Location = location;
        }

        public void Update(GameTime gameTime)
        {
            PlayerShotManager.Update(gameTime);

 
            if (!Destroyed)
            {
                playerSprite.Velocity = Vector2.Zero;

                shotTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                HandleKeyboardInput(Keyboard.GetState());
                HandleGamepadInput(GamePad.GetState(PlayerIndex.One));

                playerSprite.Velocity.Normalize();
                playerSprite.Velocity *= playerSpeed;

                playerSprite.Update(gameTime);
                imposeMovementLimits();

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerShotManager.Draw(spriteBatch);

            if (!Destroyed)
            {
                playerSprite.Draw(spriteBatch);
            }
        }





    }
}
