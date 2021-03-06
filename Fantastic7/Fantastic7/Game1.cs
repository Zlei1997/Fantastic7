﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Timers;

namespace Fantastic7
{
    public class SpriteBatchPlus : SpriteBatch
    {
        private Texture2D _defaultTexture;
        public SpriteBatchPlus(GraphicsDevice graphicsDevice) : base(graphicsDevice){}
        public void setDefaultTexture(Texture2D defaultTexture) { _defaultTexture = defaultTexture; }
        public Texture2D defaultTexture() { return _defaultTexture; }
    }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatchPlus spriteBatch;
        GameState gs;
        Room rm;
        //Texture2D plainText;
        Map currMap;
        const int WIDTH = 1280;
        const int HEIGHT = 720;
        SpriteFont mfont;
        SpriteFont sfont;
        SpriteFont guiFont;
        GGUI mainMenu;
        GGUI pauseMenu;
        int currentTime;
        int goalTime;
        MenuControls MenuControls;
        PlayControls PlayControls;
        EventHandler EventHandler;

        enum GameState
        {
            mainMenu,
            running,
            paused
        };

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = HEIGHT;
            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.IsFullScreen = false;
            graphics.PreferMultiSampling = false;
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
            gs = GameState.mainMenu;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatchPlus(GraphicsDevice);
            Texture2D plainText = new Texture2D(GraphicsDevice, 1, 1);
            plainText.SetData(new[] { Color.White });
            spriteBatch.setDefaultTexture(plainText);
            //currentTime = 0;
            //goalTime = 35;
            Keys[] MenuControlList = { Keys.Escape, Keys.Enter, Keys.W, Keys.S, Keys.Up, Keys.Down };
            MenuControls = new MenuControls(MenuControlList);

            Keys[] playControlList = { Keys.Escape, Keys.W, Keys.A, Keys.S, Keys.D, Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.Space };
            PlayControls = new PlayControls(playControlList);

         



            //Creates Test Room
            /*GSprite[] roomSprites = { new NSprite(new Rectangle(0, 0, WIDTH, HEIGHT), Color.Gray),
                new NSprite(new Rectangle(100, 100, WIDTH - 200, HEIGHT - 200), Color.LightGray)};
            rm = new Room(roomSprites);

            //rm.addObject(new Entity(new Vector2(500, 500), new NSprite(new Rectangle(500, 500, 50, 50), Color.Wheat)));*/


            //Imports Font
            mfont = Content.Load<SpriteFont>("main");
            sfont = Content.Load<SpriteFont>("second");
            guiFont = Content.Load<SpriteFont>("guiFont");
            int mHeight = (int)mfont.MeasureString("M").Y;
            int sHeight = (int)sfont.MeasureString("M").Y;


            //Creates Main Menu
            GSprite[] gs = { new NSprite(new Rectangle(0, 0, WIDTH, HEIGHT), Color.SandyBrown),
                new NSprite(new Rectangle(0, 0, WIDTH, mHeight * 2), Color.SaddleBrown),
                new SSprite("Maze Crawler", mfont, new Vector2(25,mHeight / 2), Color.Azure)};

            MenuOption[] mo = { new MenuOption(new SSprite("Start Game", sfont, new Vector2(50, mHeight * 2 + sHeight), Color.Azure)),
                new MenuOption(new SSprite("Settings", sfont, new Vector2(50, mHeight * 2 + sHeight * 3), Color.Azure)),
                new MenuOption(new SSprite("Quit Game", sfont, new Vector2(50,mHeight * 2 + sHeight * 5), Color.Azure))};

            mainMenu = new GGUI(gs, mo, Color.Azure);



            //Creates Pause Menu
            GSprite[] pgs = { new NSprite(new Rectangle(WIDTH / 4, HEIGHT / 8, WIDTH / 2, HEIGHT / 2), Color.SandyBrown),
                new NSprite(new Rectangle(WIDTH / 4, HEIGHT / 8, WIDTH / 2, mHeight * 2), Color.SaddleBrown),
                new SSprite("Pause Menu", mfont, new Vector2(WIDTH / 2 - mfont.MeasureString("Pause Menu").X / 2, HEIGHT / 8 + mHeight / 2), Color.Azure)};

            MenuOption[] pmo = { new MenuOption(new SSprite("Resume", sfont, new Vector2(WIDTH / 2 - mfont.MeasureString("Pause Menu").X / 4, HEIGHT / 8 + mHeight * 2 + sHeight/2), Color.Azure)),
                new MenuOption(new SSprite("Setting", sfont, new Vector2(WIDTH / 2 - mfont.MeasureString("Pause Menu").X / 4, HEIGHT / 8 + mHeight * 2 + sHeight * 2), Color.Azure)),
                new MenuOption(new SSprite("Quit", sfont, new Vector2(WIDTH / 2 - mfont.MeasureString("Pause Menu").X / 4, HEIGHT / 8 + mHeight * 2 + sHeight * 3.5f), Color.Azure))};

            pauseMenu = new GGUI(pgs, pmo, Color.Azure);


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected void newGame()
        {
            gs = GameState.running;
            currMap = new Map(guiFont);
            currMap.GenerateMap();
            EventHandler = new EventHandler(currMap);
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            

            switch (gs)
            {
                case GameState.mainMenu:


                    MenuControls.update(gameTime);

                    //Poll inputs




                    //currentTime += gameTime.ElapsedGameTime.Milliseconds;
                    //if(currentTime >= goalTime) { 

                    if (MenuControls.getExit()) Exit();
                    if (MenuControls.getSelect()) newGame();
                    if (MenuControls.getNextKey()) mainMenu.nextOption();
                    if (MenuControls.getPrevKey()) mainMenu.previousOption();
                        
                        //End inputs 
                        //currentTime -= goalTime;
                    //}
                    break;
                case GameState.running:

                    PlayControls.update(gameTime);

                    //Poll inputs

                    if (PlayControls.getPause()) gs = GameState.paused;

                    Vector2 playerMovement = new Vector2(0, 0);
                    if (PlayControls.getMoveDown())
                    {
                        playerMovement.Y += currMap.player.movementSpeed;
                        currMap.player.direction = CollisionHandler.Direction.South;
                    }
                    if (PlayControls.getMoveUp())
                    {
                        playerMovement.Y -= currMap.player.movementSpeed;
                        currMap.player.direction = CollisionHandler.Direction.North;
                    }
                    if (PlayControls.getMoveRight())
                    {
                        playerMovement.X += currMap.player.movementSpeed;
                        currMap.player.direction = CollisionHandler.Direction.East;
                    }
                    if (PlayControls.getMoveLeft())
                    {
                        playerMovement.X -= currMap.player.movementSpeed;
                        currMap.player.direction = CollisionHandler.Direction.West;
                    }
                    if (PlayControls.getShootKey())
                    {
                        currMap.player._mainweapon.IsUsing = true;
                    }
                    else currMap.player._mainweapon.IsUsing = false;
                    if (playerMovement.X != 0 || playerMovement.Y != 0)
                    {                      
                        currMap.player.move(playerMovement * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    }


                    //End inputs

                    EventHandler.handle(gameTime);
                    currMap.update(gameTime);
                    
                    break;
                case GameState.paused:


                    MenuControls.update(gameTime);

                    //Poll inputs

                    if (MenuControls.getNextKey()) pauseMenu.nextOption();
                    if (MenuControls.getPrevKey()) pauseMenu.previousOption();
                    if (MenuControls.getSelect()) gs = GameState.mainMenu;
                    if (MenuControls.getExit()) gs = GameState.running;
                    //End inputs
                    break;
                default: break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            //Draws objects depending on the state of the game
            switch (gs)
            {
                case GameState.mainMenu:
                    mainMenu.draw(spriteBatch,1);
                    break;
                case GameState.paused:
                    pauseMenu.draw(spriteBatch, 1);
                    break;
                case GameState.running:
                    currMap.draw(spriteBatch, 1f);
                    break;
                default: break;
            }

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
