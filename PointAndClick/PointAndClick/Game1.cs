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
        private static Rectangle DialogRect;
        private static Vector2 DialogVec;
        private static Texture2D DialogBox;
        private static string DialogText, ParsedText, TypedText;
        private static double TypedTextLength;
        private static int DelayInMilli;
        private static bool IsDoneDrawing;
        private static SpriteSortMode SpriteSorting;
        private static BlendState Blenda;
        private static Nullable<Rectangle> DEATHRECTANGLE;

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
            DialogRect = new Rectangle(0, 275, 800, 200);
            DialogVec = new Vector2(50, 300);
            DialogBox = Content.Load<Texture2D>("dialog");
            DialogText = "This is text";
            ParsedText = parseText(DialogText);
            DelayInMilli = 50;
            IsDoneDrawing = false;
            SpriteSorting = SpriteSortMode.FrontToBack;
            Blenda = BlendState.NonPremultiplied;
            DEATHRECTANGLE = null;
        }

        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            TrackMouse();
            TrackPlayerState();
            TextTyper(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSorting, Blenda);
            DrawBackgroundLayer();
            DrawUILayer();
            DrawCursorLayer();
            DrawTextLayer();
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public static void DrawBackgroundLayer()
        {
            float zDepth = 0;
            Vector2 DummyVec = new Vector2(0, 0);
            if (IsInRoom == true)
            {
                spriteBatch.Draw(DoorwayScene, mainFrame, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
                spriteBatch.Draw(DummyTex, ToRoom, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
            }
            else
            {
                spriteBatch.Draw(sprite, mainFrame, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
                spriteBatch.Draw(DummyTex, BackRoom, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
            }
        }
        public static void DrawUILayer()
        {
            float zDepth = 0.1f;
            Vector2 DummyVec = new Vector2(0, 0);
            spriteBatch.Draw(KeyItem.ItemTex, KeyItem.ItemSize, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
            spriteBatch.Draw(RedXUITex, XUIRect, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
            spriteBatch.Draw(DialogBox, DialogRect, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
        }
        public static void DrawTextLayer()
        {
            float zDepth = 0.2f;
            Vector2 DummyVec = new Vector2(0, 0);
            if (HoverX == true)
            {
                spriteBatch.DrawString(TextFont, "Exit" , HoverText, Color.White, 0, HoverText, 0, SpriteEffects.None, zDepth);
            }
            if (HoverRoom == true)
            {
                spriteBatch.DrawString(TextFont, "Sprite Time", HoverText, Color.White, 0, HoverText, 0, SpriteEffects.None, zDepth);
            }
            spriteBatch.DrawString(TextFont, TypedText, DialogVec, Color.White, 0, DummyVec, 0, SpriteEffects.None, zDepth);
        }
        public static void DrawCursorLayer()
        {
            float zDepth = 0.3f;
            Vector2 DummyVec = new Vector2(0, 0);
            spriteBatch.Draw(CursorImage, cursorPos, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
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
        public String parseText(String text)
        {
            String line = String.Empty;
            String returnString = String.Empty;
            String[] wordArray = text.Split(' ');

            foreach (String word in wordArray)
            {
                if (TextFont.MeasureString(line + word).Length() > DialogBox.Width)
                {
                    returnString = returnString + line + '\n';
                    line = String.Empty;
                }

                line = line + word + ' ';
            }

            return returnString + line;
        }
        public static void TextTyper(GameTime gameTime)
        {
            if (!IsDoneDrawing)
            {
                if (DelayInMilli == 0)
                {
                    TypedText = ParsedText;
                    IsDoneDrawing = true;
                }
                else if (TypedTextLength < ParsedText.Length)
                {
                    TypedTextLength = TypedTextLength + gameTime.ElapsedGameTime.TotalMilliseconds / DelayInMilli;

                    if (TypedTextLength >= ParsedText.Length)
                    {
                        TypedTextLength = ParsedText.Length;
                        IsDoneDrawing = true;
                    }

                    TypedText = ParsedText.Substring(0, (int)TypedTextLength);
                }
            }

        }
    }
}

