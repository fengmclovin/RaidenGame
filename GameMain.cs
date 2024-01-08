using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using RC_Framework;
using System.IO;

namespace Week9
{
    /// <summary>
    /// This is the main type for your game.

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;// graphics = new GraphicsDeviceManager(this);
        SpriteBatch spriteBatch;// spriteBatch = new SpriteBatch(GraphicsDevice);

        RC_GameStateManager levelManager;
      
        // set up new variables

        public static KeyboardState currentKeyState;     // must use or keystate can be unstable on level change
        public static KeyboardState prevKeyState;

        public static MouseState currentMouseState;  // must use or mousestate can be unstable on level change
        public static MouseState previousMouseState;

        public static float mouse_x = 0;
        public static float mouse_y = 0;

        public static int screenWidth;
        public static int ScreenHeight;
        public static Rectangle screenRect;

        public static string dir = "";// image or media dictionary

        public Game1()//This is Constructor
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;

            TargetElapsedTime = TimeSpan.FromTicks(444444);
        }

        protected override void Initialize()// Put most of the code
        {
            base.Initialize();// all components is called by now 
        }

        protected override void LoadContent()// Load an game asset (sound & image files)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            LineBatch.init(GraphicsDevice);

            Dir.dir = Util.findDirWithFile("GTA.png");

            // levelManager

            levelManager = new RC_GameStateManager();

            levelManager.AddLevel(0, new GameLevel_0_Intro());
            levelManager.getLevel(0).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(0).LoadContent();
            levelManager.setLevel(0);

            levelManager.AddLevel(1, new GameLevel_1_Menu());
            levelManager.getLevel(1).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(1).LoadContent();

            levelManager.AddLevel(2, new GameLevel_2_Easy());
            levelManager.getLevel(2).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(2).LoadContent();

            levelManager.AddLevel(3, new GameLevel_3_Medium());
            levelManager.getLevel(3).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(3).LoadContent();

            levelManager.AddLevel(4, new GameLevel_4_Hard());
            levelManager.getLevel(4).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(4).LoadContent();

            levelManager.AddLevel(5, new GameLevel_5_Pause());
            levelManager.getLevel(5).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(5).LoadContent();

            levelManager.AddLevel(6, new GameLevel_6_Fail());
            levelManager.getLevel(6).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(6).LoadContent();


            levelManager.AddLevel(7, new GameLevel_7_Win());
            levelManager.getLevel(7).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(7).LoadContent();

            levelManager.AddLevel(8, new GameLevel_8_Score());
            levelManager.getLevel(8).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(8).LoadContent();

            levelManager.AddLevel(9, new GameLevel_9_Outro());
            levelManager.getLevel(9).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(9).LoadContent();
            //levelManager.setLevel(9);

            screenRect = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

        }

        protected override void UnloadContent()// Adding cleanup code
        {

        }

        protected override void Update(GameTime gameTime)// Putting frame to frame game logic
                                                         // Controller input & moving around enemies 
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            prevKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            if (currentKeyState.IsKeyDown(Keys.Escape))
                this.Exit();

            levelManager.getCurrentLevel().Update(gameTime);

            base.Update(gameTime);// all components is called by now 
        }

        protected override void Draw(GameTime gameTime)// Rendering, not game logic
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);// Clear and fill with colour

            spriteBatch.Begin();

            spriteBatch.End();

            levelManager.getCurrentLevel().Draw(gameTime);

            base.Draw(gameTime);
        }

        public static Texture2D texFromFile(GraphicsDevice gd, String fName)
        {
            // note needs :using System.IO;
            Stream fs = new FileStream(fName, FileMode.Open);
            Texture2D rc = Texture2D.FromStream(gd, fs);
            fs.Close();
            return rc;
        }
    }
}