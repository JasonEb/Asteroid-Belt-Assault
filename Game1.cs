

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Asteroid_Belt_Assault
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        enum GameStates { TitleScreen, Playing, PlayerDead, GameOver }; //pg 90, declarations
        GameStates gameState = GameStates.TitleScreen; // was playing
        Texture2D titleScreen;
        Texture2D spriteSheet;

        StarField starField; //pg102, viewing the Starfield


        AsteroidManager asteroidManager;

        PlayerManager playerManager;//pg 125 

        EnemyManager enemyManager; //pg 138, drawing enemies

        ExplosionManager explosionManager; //pg 150 

        CollisionManager collisionManager; //pg 156


        //pg 162, sprite font adding and structuring the game

        SpriteFont pericles14;

        private float playerDeathDelayTime = 10f;
        private float playerDeathTimer = 0f;
        private float titleScreenTimer = 0f;
        private float titleScreenDelayTime = 1f;

        private int playerStartingLives = 2;
        private Vector2 playerStartLocation = new Vector2(390, 550);
        private Vector2 scoreLocation = new Vector2(20, 10);
        private Vector2 livesLocation = new Vector2(20, 25);

        Song song;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //pg 90, loading content
            titleScreen = Content.Load<Texture2D>(@"Textures\TitleScreen"); 
            spriteSheet = Content.Load<Texture2D>(@"Textures\spriteSheet");
            // TODO: use this.Content to load your game content here

            //pg103, updating load content to initialize the strfield object
            starField = new StarField(
                this.Window.ClientBounds.Width,//thse pass the size of the screen
                this.Window.ClientBounds.Height,
                200, //this means 200 stars will be created
                new Vector2(0, 30f), //this means each star has a velocity of 30 pixels / s
                spriteSheet,
                new Rectangle(0, 450, 2, 2)); //a single 2 x 2 pixel frame on screen, gotten from location 0, 450 from sprite sheet

            asteroidManager = new AsteroidManager( //pg 110, loading the asteroid manager
                10,
                spriteSheet,
                new Rectangle(0, 0, 50, 50),
                20, 
                this.Window.ClientBounds.Width,
                this.Window.ClientBounds.Height);//pg 110

            playerManager = new PlayerManager( //pg 125 coding 
                spriteSheet,
                new Rectangle(0, 150, 50, 50),
                3,
                new Rectangle(
                    0,
                    0,
                    this.Window.ClientBounds.Width,
                    this.Window.ClientBounds.Height)); //end pg 125

            enemyManager = new EnemyManager(
                spriteSheet,
                new Rectangle(0, 200, 50, 50),
                6,
                playerManager,
                new Rectangle(
                    0,
                    0,
                    this.Window.ClientBounds.Width,
                    this.Window.ClientBounds.Height));//pg 138

            explosionManager = new ExplosionManager(
                spriteSheet,
                new Rectangle(0, 100, 50, 50),
                3,
                new Rectangle(0, 450, 2, 2),
                this.Window.ClientBounds.Width);

            collisionManager = new CollisionManager( //156
                asteroidManager,
                playerManager,
                enemyManager,
                explosionManager);

            song = Content.Load<Song>(@"Music\Playing");

             SoundManager.Initialize(Content); //pg161


            pericles14 = Content.Load<SpriteFont>(@"Fonts\Pericles14"); //pg 162

            MediaPlayer.Play(Content.Load<Song>(@"Music\Playing"));

            MediaPlayer.IsRepeating = true; 
            //MediaPlayer.Play(song);


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 



        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            //pg 90, basic structure for Update()
            switch (gameState)
            {
                case GameStates.TitleScreen:

                    titleScreenTimer +=
                        (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (titleScreenTimer >= titleScreenDelayTime)
                    {
                        if ((Keyboard.GetState().IsKeyDown(Keys.Space)) ||
                            (GamePad.GetState(PlayerIndex.One).Buttons.A ==
                            ButtonState.Pressed))
                        {
                            playerManager.LivesRemaining = playerStartingLives;
                            playerManager.PlayerScore = 0;
                            resetGame();
                            gameState = GameStates.Playing;
                        }
                    }

                    break;

                case GameStates.Playing:
                    starField.Update(gameTime); //pg 103
                    asteroidManager.Update(gameTime); //pg111
                    playerManager.Update(gameTime); //pg125
                    enemyManager.Update(gameTime); //pg138
                    explosionManager.Update(gameTime); //pg150
                    collisionManager.CheckCollisions(); //pg 156


                    if (playerManager.Destroyed) // pg 163
                    {
                        playerDeathTimer = 0f;
                        enemyManager.Active = false;
                        playerManager.LivesRemaining--;
                        if (playerManager.LivesRemaining < 0)
                        {
                            gameState = GameStates.GameOver;
                            MediaPlayer.Play(Content.Load<Song>((@"Music\GameOver")));
                            MediaPlayer.IsRepeating = false;
                        }
                        else
                        {
                            gameState = GameStates.PlayerDead;
                            MediaPlayer.Play(Content.Load<Song>((@"Music\Dead")));
                        }
                    }

                    break;


                case GameStates.PlayerDead: // pg 164
                    playerDeathTimer +=
                        (float)gameTime.ElapsedGameTime.TotalSeconds;

                    starField.Update(gameTime);
                    asteroidManager.Update(gameTime);
                    enemyManager.Update(gameTime);
                    playerManager.PlayerShotManager.Update(gameTime);
                    explosionManager.Update(gameTime);

                    if (playerDeathTimer >= playerDeathDelayTime)
                    {
                        resetGame();
                        gameState = GameStates.Playing;
                        MediaPlayer.Play(song);
                    }

                    SoundManager.PlayLeftSound(0.0f, false);
                    SoundManager.PlayRightSound(0.0f, false);


                    break;

                case GameStates.GameOver:
                    playerDeathTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    starField.Update(gameTime);
                    asteroidManager.Update(gameTime);
                    enemyManager.Update(gameTime);
                    playerManager.PlayerShotManager.Update(gameTime);
                    explosionManager.Update(gameTime);
                    if (playerDeathTimer >= playerDeathDelayTime)
                    {
                        gameState = GameStates.TitleScreen;
                        MediaPlayer.Play(song);
                        MediaPlayer.IsRepeating = true; 
                    }
                    SoundManager.PlayLeftSound(0.0f, false);
                    SoundManager.PlayRightSound(0.0f, false);
                    break; //end pg 164

            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            //pg 91, basic structure for Draw()
            spriteBatch.Begin();

            if (gameState == GameStates.TitleScreen)
            {
                spriteBatch.Draw(titleScreen,
                    new Rectangle(0, 0, this.Window.ClientBounds.Width,
                        this.Window.ClientBounds.Height),
                        Color.White);
            }

            if ((gameState == GameStates.Playing) ||
                (gameState == GameStates.PlayerDead) ||
                (gameState == GameStates.GameOver))
            {
                starField.Draw(spriteBatch);
                asteroidManager.Draw(spriteBatch);
                playerManager.Draw(spriteBatch);
                enemyManager.Draw(spriteBatch);
                explosionManager.Draw(spriteBatch);

                spriteBatch.DrawString(
                    pericles14,
                    "Score: " + playerManager.PlayerScore.ToString(),
                    scoreLocation,
                    Color.White);

                if (playerManager.LivesRemaining >= 0)
                {
                    spriteBatch.DrawString(
                        pericles14,
                        "Ship Remaining: " +
                            playerManager.LivesRemaining.ToString(),
                        livesLocation,
                        Color.White);
                }
            }
       
            if ((gameState == GameStates.GameOver))
            {
                spriteBatch.DrawString(
                    pericles14,
                    "G A M E  O V E R !",
                    new Vector2(this.Window.ClientBounds.Width / 2 -
                        pericles14.MeasureString
                         ("G A M E  O V E R!").X / 2,
                        50),
                    Color.White);
            }
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void resetGame() //pg 163
        {
            playerManager.playerSprite.Location = playerStartLocation;
            foreach (Sprite asteroid in asteroidManager.Asteroids)
            {
                asteroid.Location = new Vector2(-500,-500);
            }
            enemyManager.Enemies.Clear();
            enemyManager.Active = true;
            playerManager.PlayerShotManager.Shots.Clear();
            enemyManager.EnemyShotManager.Shots.Clear();
            playerManager.Destroyed = false;
        }

    }
}
