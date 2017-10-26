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
        private static Item KeyItem;
        private static Player PlayerOne;
        private static SoundEffect FootstepsSound;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            PlayerOne = new Player();
            KeyItem = new Item();
            base.Initialize();
        }
        protected override void LoadContent()
        {
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
            FootstepsSound = Content.Load<SoundEffect>("footsteps");
        }

        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            TrackMouse();
            TrackPlayerState();
            base.Update(gameTime);
        }
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
        public void TrackMouse()
        {
            //Keeps track of the position of the mouse (in a Point, vector, and as a rect)
            var mouseState = Mouse.GetState();
            cursorPos = new Rectangle(mouseState.X, mouseState.Y, 48, 48);
            cursorVec = new Vector2(mouseState.X, mouseState.Y);
            var CursorPoint = new Point(mouseState.X, mouseState.Y);
            HoverText = new Vector2(mouseState.X + 25, mouseState.Y - 15);
            if (cursorPos.Intersects(ToRoom))
            {
                if (IsInRoom == true)
                {
                    HoverRoom = true;
                }
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    FootstepsSound.Play();
                    IsInRoom = false;
                }
            }
            else
            {
                HoverRoom = false;
            }
            if (cursorPos.Intersects(BackRoom))
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
        }
        public void TrackPlayerState()
        {
            switch (PlayerOne.PlayerState)
            {
                case 1:
                    break;
                    //Etc. 
            }
        }

    }
}

