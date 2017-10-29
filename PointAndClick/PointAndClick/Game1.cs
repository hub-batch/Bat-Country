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
        private static Rectangle ToRoom;
        private static Texture2D DummyTex;
        private static Rectangle BackRoom;
        private static Rectangle XUIRect;
        private static Texture2D RedXUITex;
        private static bool IsAtDoorstep = true;
        private static SpriteFont TextFont;
        private static bool HoverX = false;
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
        private Texture2D ContinueArrow;
        private Rectangle ContinueArrowRect;
        private bool IsInDialog;
        private Actor SisActor = new Actor();
        private bool IsInCar, IsInRoom, HasTalkedToSis, IsAtPond, HasWater, ThrowWaterOnKid;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            PlayerOne = new Player();
            KeyItem = new Item();
            PlayerOne.PlayerState = 0;
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            CursorImage = Content.Load<Texture2D>("Cursor");
            sprite = this.Content.Load<Texture2D>("SpriteCan");
            DoorwayScene = Content.Load<Texture2D>("doorway");
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
            ContinueArrow = Content.Load<Texture2D>("SpriteCan");
            ContinueArrowRect = new Rectangle(675, 350, 100, 100);
            DialogRect = new Rectangle(0, 275, 800, 200);
            DialogVec = new Vector2(50, 300);
            DialogBox = Content.Load<Texture2D>("dialog");
            DialogText = "This is dummy text";
            ParsedText = parseText(DialogText);
            DelayInMilli = 50;
            IsDoneDrawing = false;
            SpriteSorting = SpriteSortMode.FrontToBack;
            Blenda = BlendState.NonPremultiplied;
            DEATHRECTANGLE = null;
            SisActor.ActorID = 1;
            SisActor.ActorName = "Sarah";
            SisActor.ActorDialog = new List<string>
            {
                //0
                "Oh...Hello. You must be the man mom called.",
                //1
                "She's at the store right now...",
                //2
                "He's under the bed. He's been acting weird for days. You've got to help him.",
                //3
                "There's a pond out back you can get water from...",
                //4
                "I can go get salt.",
                //5
                "Okay, here's the salt!"
            };
            PlayerOne.PlayerDialog = new List<string>
            {
                //0
                "Uh...Hi?",
                //1
                "Where's your mother?",
                //2
                "Where's the aflicted?",
                //3
                "I can help him. I've been locked out of my car, though. Have to improvise some holy water.",
                //4
                "I need salt, too.",
                //5
                "Hey, I've got the water."
            };
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }
        protected override void Update(GameTime gameTime)
        {
            TrackMouse(gameTime);
            TrackPlayerState();
            TextTyper(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin(SpriteSorting, Blenda);
            DrawBackgroundLayer();
            DrawUILayer();
            DrawTextLayer();
            DrawCursorLayer();
            spriteBatch.End();
            base.Draw(gameTime);
        }
        public void DrawBackgroundLayer()
        {
            float zDepth = 0;
            Vector2 DummyVec = new Vector2(0, 0);
            if (IsInCar == true)
            {
                spriteBatch.Draw(sprite, mainFrame, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
                spriteBatch.Draw(DummyTex, BackRoom, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
            }
            if (IsAtDoorstep == true)
            {
                spriteBatch.Draw(DoorwayScene, mainFrame, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
                spriteBatch.Draw(DummyTex, ToRoom, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
            }
            if (IsInRoom == true)
            {
                spriteBatch.Draw(sprite, mainFrame, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
            }
            if (HasTalkedToSis == true)
            {
                spriteBatch.Draw(sprite, mainFrame, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
                spriteBatch.Draw(DummyTex, BackRoom, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
            }
            if (IsAtPond == true)
            {
                spriteBatch.Draw(sprite, mainFrame, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
                spriteBatch.Draw(DummyTex, BackRoom, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
            }
            if (HasWater == true)
            {
                spriteBatch.Draw(sprite, mainFrame, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
                spriteBatch.Draw(DummyTex, BackRoom, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
            }
            else
            {
                spriteBatch.Draw(sprite, mainFrame, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
                spriteBatch.Draw(DummyTex, BackRoom, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
            }
        }
        public void DrawUILayer()
        {
            float zDepth = 0.1f;
            Vector2 DummyVec = new Vector2(0, 0);
            spriteBatch.Draw(KeyItem.ItemTex, KeyItem.ItemSize, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
            spriteBatch.Draw(RedXUITex, XUIRect, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
            if (IsInDialog == true)
            {
                spriteBatch.Draw(DialogBox, DialogRect, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
                spriteBatch.Draw(ContinueArrow, ContinueArrowRect, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
            }


        }
        public void DrawTextLayer()
        {
            var mouseState = Mouse.GetState();
            cursorPos = new Rectangle(mouseState.X, mouseState.Y, 48, 48);
            Vector2 TextCursorVec = new Vector2(mouseState.X - 20, mouseState.Y - 20);
            float zDepth = 0.2f;
            Vector2 DummyVec = new Vector2(0, 0);
            if (HoverX == true)
            {
                spriteBatch.DrawString(TextFont, "Exit" , TextCursorVec, Color.Black, 0, DummyVec, 1, SpriteEffects.None, zDepth);
            }
            if (HoverRoom == true)
            {
                spriteBatch.DrawString(TextFont, "Sprite Time", TextCursorVec, Color.Black, 0, DummyVec, 1, SpriteEffects.None, zDepth);
            }
            if (IsInDialog == true)
            {
                spriteBatch.DrawString(TextFont, TypedText, DialogVec, Color.Black, 0, DummyVec, 1, SpriteEffects.None, zDepth);
            }

        }
        public void DrawCursorLayer()
        {
            float zDepth = 0.3f;
            Vector2 DummyVec = new Vector2(0, 0);
            spriteBatch.Draw(CursorImage, cursorPos, DEATHRECTANGLE, Color.White, 0, DummyVec, SpriteEffects.None, zDepth);
        }
        public string Dialog(Actor TalkingActor)
        {
            while (IsInDialog == true)
            {
                if (TalkingActor.IsTalking == true && PlayerOne.IsTalking == false)
                {
                    switch (TalkingActor.DialogResponse)
                    {
                        case 0:
                            ParsedText = parseText(TalkingActor.ActorDialog[0]);
                            return ParsedText;
                        case 1:
                            ParsedText = parseText(TalkingActor.ActorDialog[1]);
                            return ParsedText;
                        case 2:
                            ParsedText = parseText(TalkingActor.ActorDialog[2]);
                            return ParsedText;
                        case 3:
                            ParsedText = parseText(TalkingActor.ActorDialog[3]);
                            return ParsedText;
                        case 4:
                            ParsedText = parseText(TalkingActor.ActorDialog[4]);
                            return ParsedText;
                        case 5:
                            ParsedText = parseText(TalkingActor.ActorDialog[5]);
                            return ParsedText;
                        default:
                            ParsedText = parseText(DialogText);
                            return ParsedText;
                    }
                }
                if (PlayerOne.IsTalking == true && TalkingActor.IsTalking == false)
                {
                    switch (PlayerOne.DialogChoice)
                    {
                        case 0:
                            ParsedText = parseText(PlayerOne.PlayerDialog[0]);
                            return ParsedText;
                        case 1:
                            ParsedText = parseText(PlayerOne.PlayerDialog[1]);
                            return ParsedText;
                        case 2:
                            ParsedText = parseText(PlayerOne.PlayerDialog[2]);
                            return ParsedText;
                        case 3:
                            ParsedText = parseText(PlayerOne.PlayerDialog[3]);
                            return ParsedText;
                        case 4:
                            ParsedText = parseText(PlayerOne.PlayerDialog[4]);
                            return ParsedText;
                        case 5:
                            ParsedText = parseText(PlayerOne.PlayerDialog[5]);
                            return ParsedText;
                        default:
                            ParsedText = parseText(DialogText);
                            return ParsedText;
                    }
                }
            }
            return ParsedText;
        }
        public void TrackMouse(GameTime gameTime)
        {
            //Keeps track of the position of the mouse (in a Point, vector, and as a rect)
            var mouseState = Mouse.GetState();
            cursorPos = new Rectangle(mouseState.X, mouseState.Y, 48, 48);
            cursorVec = new Vector2(mouseState.X, mouseState.Y);
            var CursorPoint = new Point(mouseState.X, mouseState.Y);
            if (cursorPos.Intersects(ContinueArrowRect))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    IsInDialog = false;
                }
            }
            if (cursorPos.Intersects(ToRoom))
            {
                if (IsAtDoorstep == true)
                {
                    HoverRoom = true;
                }
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    //FootstepsSound.Play();
                    IsAtDoorstep = false;
                    IsInRoom = true;
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
                    IsAtDoorstep = true;
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
                case 0:
                    IsInCar = true;
                    break;
                case 1:
                    IsAtDoorstep = true;
                    break;
                case 2:
                    IsInRoom = true;
                    break;
                case 3:
                    HasTalkedToSis = true;
                    break;
                case 4:
                    IsAtPond = true;
                    break;
                case 5:
                    HasWater = true;
                    break;
                case 6:
                    ThrowWaterOnKid = true;
                    break;
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
        public void TextTyper(GameTime gameTime)
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

