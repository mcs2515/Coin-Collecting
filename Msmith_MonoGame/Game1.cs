using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Msmith_MonoGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //FIELDS

        //textures
        private SpriteFont textFont;
        private Texture2D playerTexture;
        private Texture2D coinTexture;
        private Texture2D backGround;

        //objects
        private GameState gameState;
        private KeyboardState kbState;
        private KeyboardState previousKbState;
        private Player player; 
        private List<Collectible> collectibles;
        private Random num;
        
        //etc
        private int playerLevel;
        private double timer;

        //enum states of the game
        enum GameState
        {
            Menu,
            Game,
            GameOver,
        }

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

            //set gamestate to menu state
            gameState = GameState.Menu;
            playerLevel = 0;
            timer = 10;
            collectibles = new List<Collectible>();
            player = new Player(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2, 100, 100);
            num = new Random();

            //call nextlevel method
            NextLevel();

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

            // TODO: use this.Content to load your game content here

            //load textures to a variable
            textFont = this.Content.Load<SpriteFont>("Arial14");
            coinTexture = this.Content.Load<Texture2D>("token");
            playerTexture = this.Content.Load<Texture2D>("player");
            backGround = this.Content.Load<Texture2D>("Pixel Islands");

            //set player image to texture
            player.Image = playerTexture;

            //set texture for each collectible
            foreach (Collectible c in collectibles)
            {
                c.Image = coinTexture;
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //set current keypress to previous
            previousKbState = kbState;
            //update the keystate
            kbState = Keyboard.GetState();

            //check the gamestate and do something
            switch (gameState)
            {
                case GameState.Menu:
                    if (SingleKeyPress(Keys.Enter))
                    {
                        //change gamestate to game
                        gameState = GameState.Game;
                        //call reset method
                        Reset();
                    }
                    break;

                case GameState.Game:
                    //countdown timer
                    timer -=gameTime.ElapsedGameTime.TotalSeconds;
                    //check for key press and move player
                    if (kbState.IsKeyDown(Keys.D))
                    {
                        player.X += 8;
                    }
                    if (kbState.IsKeyDown(Keys.S))
                    {
                        player.Y += 8;
                    }
                    if (kbState.IsKeyDown(Keys.A))
                    {
                        player.X -= 8;
                    }
                    if (kbState.IsKeyDown(Keys.W))
                    {
                        player.Y -= 8;
                    }

                    //call the screen wrap method
                    ScreenWrap(player);
                    //check for collisions
                    foreach (Collectible coins in collectibles)
                    {
                        if (coins.CheckCollision(player))
                        {
                            coins.IsActive = false;
                            player.LevelScore++;
                            player.TotalScore++;
                        }
                    }

                    //if timer reaches 0, game over
                    if (timer <= 0)
                    {
                        gameState = GameState.GameOver;
                    }

                    //if playerscorelevel = number of collectibles in list, call next level method
                    if (player.LevelScore == collectibles.Count)
                    {
                        NextLevel();
                    }
                    break;

                case GameState.GameOver:
                    if (SingleKeyPress(Keys.Enter))
                    {
                        //change gamestate to menu
                        gameState = GameState.Menu;
                    }
                    break;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            //draw the background
            spriteBatch.Draw(backGround, new Rectangle(0, 0, 800, 480), Color.White);

            //check the gamestate and do something
            switch (gameState)
            {
                case GameState.Menu:
                    spriteBatch.DrawString(textFont, "Yummy Taiyaki Fishies", new Vector2(325, 100), Color.Black);
                    spriteBatch.DrawString(textFont, "Press 'Enter' to start the game", new Vector2(275, 140), Color.Black);
                    spriteBatch.DrawString(textFont, "Use 'W' 'A' 'S' 'D' to move", new Vector2(295, 180), Color.Black);
                    break;
                case GameState.Game:
                    //draw player
                    spriteBatch.Draw(player.Image, player.ImageRectangle, Color.White);
                    //draw coins
                    foreach (Collectible coins in collectibles)
                    {
                        coins.Draw(spriteBatch);
                    }
                    //draw info to screen
                    spriteBatch.DrawString(textFont, "Timer: " + string.Format("{0:0.00}", timer), new Vector2(5, 10), Color.Black);
                    spriteBatch.DrawString(textFont, "Current Level: " + string.Format("{0:0}", playerLevel), new Vector2(5, 30), Color.Black);
                    spriteBatch.DrawString(textFont, "Level Score: " + string.Format("{0:0}", player.LevelScore), new Vector2(5, 50), Color.Black);
                    break;
                case GameState.GameOver:
                    spriteBatch.DrawString(textFont, "Game Over...", new Vector2(345, 100), Color.Black);
                    spriteBatch.DrawString(textFont, "Current Level: " + string.Format("{0:0}", playerLevel), new Vector2(325, 120), Color.Black);
                    spriteBatch.DrawString(textFont, "Total Score: " + string.Format("{0:0}", player.TotalScore), new Vector2(335, 140), Color.Black);
                    spriteBatch.DrawString(textFont, "Press 'Enter' to return to title", new Vector2(275, 170), Color.Black);
                    string.Format("{0:0.00}", playerLevel);
                    string.Format("{0:0.00}", player.TotalScore);
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }


        //NEXT LEVEL METHOD
        public void NextLevel()
        {
            //increment player's level
            playerLevel++;
            //set timer
            timer = 10;
            //reset player's level score
            player.LevelScore = 0;
            //clear the list of collectibles
            collectibles.Clear();
            //center player to screen
            player.X = GraphicsDevice.Viewport.Width / 2;
            player.Y = GraphicsDevice.Viewport.Height / 2;
            //create more collectibles for current level
            for (int i = 0; i < 3 * playerLevel; i++)
            {
                //create new collectibles and give random coordinates and size
                Collectible coins = new Collectible(num.Next(GraphicsDevice.Viewport.Width-20), num.Next(GraphicsDevice.Viewport.Height-20), 100, 100);
                //set image for coins
                coins.Image = coinTexture;
                //add coin objects to list
                collectibles.Add(coins);
            }         
        }


        //RESET METHOD
        public void Reset()
        {
            //reset player level and total score
            playerLevel = 0;
            player.TotalScore = 0;

            //call the nextlevel method
            NextLevel();
        }


        //SCREEN WRAP METHOD
        public void ScreenWrap(GameObject obj)
        {
            //if objects go off screen, have it appear on oppiste screen 
            if (obj.X > GraphicsDevice.Viewport.Width)
            {
                obj.X = 0;
            }
            else if (obj.X <0)
            {
                obj.X = GraphicsDevice.Viewport.Width;
            }
            else if (obj.Y > GraphicsDevice.Viewport.Height)
            {
                obj.Y = 0;
            }
            else if (obj.Y <0)
            {
                obj.Y = GraphicsDevice.Viewport.Height;
            }
        }

        //SINGLE KEYPRESS METHOD
        public bool SingleKeyPress(Keys key)
        {
            //check to see if this is the first frame that the key was pressed
            if (kbState.IsKeyDown(key) && previousKbState.IsKeyUp(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
