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

namespace PointAndClick
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        private static Texture2D sprite;
        private static Texture2D CursorImage;
        private static Rectangle cursorPos;
        private static Vector2 cursorVec;
        private static Rectangle mainFrame;
        private static Texture2D DoorwayScene;
        private static Texture2D Doorway;
        private static Rectangle ToRoom;
        private static Texture2D DummyTex;
        private static Rectangle BackRoom;
        private static Rectangle XUIRect;
        private static Texture2D RedXUITex;
        private static bool IsInRoom = true;
        private static SpriteFont TextFont;
        private static bool HoverX = false;
        private static Vector2 HoverText;
        private static bool HoverRoom;
        private static Item KeyItem = new Item();
        private static Player PlayerOne = new Player();

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
            CursorImage = Content.Load<Texture2D>("Cursor");
            sprite = this.Content.Load<Texture2D>("SpriteCan");
            DoorwayScene = Content.Load<Texture2D>("doorway");
            Doorway = Content.Load<Texture2D>("doorway1");
            RedXUITex = Content.Load<Texture2D>("x");
            TextFont = Content.Load<SpriteFont>("Text");
            XUIRect = new Rectangle(0, 0, 50, 50);
            DummyTex = new Texture2D(GraphicsDevice, 1, 1);
            mainFrame = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            ToRoom = new Rectangle(275, -20, 230, 425);
            BackRoom = new Rectangle(0, 0, 230, 425);
            KeyItem.ItemTex = Content.Load<Texture2D>("key");
            KeyItem.ItemSize = new Rectangle(200, -30, 100, 100);



            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            //Keeps track of the position of the mouse (in a Point, vector, and as a rect)
            var mouseState = Mouse.GetState();
            cursorPos = new Rectangle(mouseState.X, mouseState.Y, 48, 48);
            cursorVec = new Vector2(mouseState.X, mouseState.Y);
            var CursorPoint = new Point(mouseState.X, mouseState.Y);
            HoverText = new Vector2(mouseState.X + 25, mouseState.Y - 15);
            //Keeps track of player state. 1= start, 11= win state. 
            switch (PlayerOne.PlayerState)
            {
                case 1:

                break;
                    //Etc. 
            }
            if (cursorPos.Intersects(ToRoom))
            {
                if (IsInRoom == true)
                {
                    HoverRoom = true;
                }
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    IsInRoom = false;
                }
            }
            else
            {
                HoverRoom = false;
            }
            if(cursorPos.Intersects(BackRoom))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    IsInRoom = true;
                }
            }
            if (cursorPos.Intersects(XUIRect))
            {
                HoverX = true;
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    Exit();
                }
            }
            else
            {
                HoverX = false;
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            DrawBackgroundLayer();
            DrawUILayer();
            DrawCursorLayer();
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public static void DrawBackgroundLayer()
        {
            if (IsInRoom == true)
            {
                spriteBatch.Draw(DoorwayScene, mainFrame, Color.White);
                spriteBatch.Draw(DummyTex, ToRoom, Color.White);
            }
            else
            {
                spriteBatch.Draw(sprite, mainFrame, Color.White);
                spriteBatch.Draw(DummyTex, BackRoom, Color.White);
            }
        }
        public static void DrawUILayer()
        {
            spriteBatch.Draw(KeyItem.ItemTex, KeyItem.ItemSize, Color.White);
            spriteBatch.Draw(RedXUITex, XUIRect, Color.White);
            if (HoverX == true)
            {
                spriteBatch.DrawString(TextFont, "Exit", HoverText, Color.White);
            }
            if (HoverRoom == true)
            {
                spriteBatch.DrawString(TextFont, "Sprite Time", HoverText, Color.White);
            }
        }
        public static void DrawCursorLayer()
        {
            spriteBatch.Draw(CursorImage, cursorPos, Color.White);
        }

    }
}

