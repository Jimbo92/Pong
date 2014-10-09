#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Pong
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        enum EGameMode
        {
            MENU,
            PLAY,
            HELP
        };

        enum EPlayStyle
        {
            ONEP,
            TWOP
        };

        enum EDifficulty
        {
            EASY,
            MEDIUM,
            HARD
        };

        EGameMode gamemode = EGameMode.MENU;
        EPlayStyle playstyle = EPlayStyle.ONEP;
        EDifficulty difficulty = EDifficulty.EASY;

        int BestOutOf = 0;
        bool YouWon;
        Sprite WinnerBanner = new Sprite();
        bool BestOfOn;

        //debug
        Texture2D Collisionbox;

        //screensize
        Vector2 screensize;
        bool isfullscreen = false;

        //mouse
        Rectangle mouse_rect = new Rectangle();
        MouseState oldMState = Mouse.GetState();

        //two player
        float paddle_speed;

        //sounds
        SoundEffect paddle_sound;
        SoundEffect rebound_sound;
        SoundEffect goal_sound;

        //Menu
        //play button
        Sprite playbtn = new Sprite();
        Vector2 playbtn_font_pos;
        //multiplayer button
        Sprite multibtn = new Sprite();
        Vector2 multibtn_font_pos;
        //resolution button
        Sprite resbtn = new Sprite();
        Vector2 resbtn_font_pos;
        //difficulty button
        Sprite difbtn = new Sprite();
        Vector2 difbtn_font_pos;
        string Sdifficulty;
        //score button
        Sprite scorebtn = new Sprite();
        Vector2 scorebtn_font_pos;
        //Help button
        Sprite helpbtn = new Sprite();
        Vector2 helpbtn_font_pos;

        //scoreline
        Sprite scoreline = new Sprite();

        //fonts
        SpriteFont font_score;
        SpriteFont font_title;
        SpriteFont font_debug;
        Vector2 font_title_pos;
        Vector2 P1font_score_pos;
        Vector2 P2font_score_pos;
        int P1score = 0;
        int P2score = 0;
        Vector2 debugfont_pos;

        //player 1 paddle
        Sprite P1paddle = new Sprite();

        //player 2 paddle
        Sprite P2paddle = new Sprite();
        //AI
        float Aispeed;
        float Aispeed_previous;
        Vector2 Ai_vel;
        Vector2 Ai_dirto;

        //The ball
        Sprite ball = new Sprite();
        Random ballrand = new Random();
        int ballrand_value;
        bool Bdown;
        bool Bright;
        Vector2 ball_speed;

        //collision check
        bool checkcollision(Rectangle a, Rectangle b)
        {
            if (a.X + a.Width < b.X || a.Y + a.Height < b.Y || a.X > b.X + b.Width || a.Y > b.Y + b.Height)
            {
                return false;
            }

            return true;
        }

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            if (GraphicsDevice.DisplayMode.AspectRatio == 1)
            {
                graphics.PreferredBackBufferHeight = 600;
                graphics.PreferredBackBufferWidth = 800;
                //graphics.PreferredBackBufferHeight = 768;
                //graphics.PreferredBackBufferWidth = 1024;
                graphics.ApplyChanges();
            }


            graphics.IsFullScreen = false;

            graphics.ApplyChanges();

            screensize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            //random generator
            ballrand_value = ballrand.Next(1, 5);


            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //debug 
            debugfont_pos = new Vector2(20, 20);
            Collisionbox = Content.Load<Texture2D>("graphics/collisionbox");

            //sound effects
            paddle_sound = Content.Load<SoundEffect>("sounds/blip1");
            rebound_sound = Content.Load<SoundEffect>("sounds/blip2");
            goal_sound = Content.Load<SoundEffect>("sounds/blip3");

            //menu
            //play btn
            playbtn.SpriteLoadPixel(GraphicsDevice, Content,
                screensize.X / 2, screensize.Y / 2 + 64, 345, 64, Color.DarkGray);
            playbtn.SpriteSetColbox();
            playbtn_font_pos.X = playbtn.GetSpritePos().X;
            playbtn_font_pos.Y = playbtn.GetSpritePos().Y;
            // multiplayer button
            multibtn.SpriteLoadPixel(GraphicsDevice, Content,
                screensize.X / 2, screensize.Y / 2 + 144, 345, 64, Color.DarkGray);
            multibtn.SpriteSetColbox();
            multibtn_font_pos.X = multibtn.GetSpritePos().X;
            multibtn_font_pos.Y = multibtn.GetSpritePos().Y;
            //resolution button
            resbtn.SpriteLoadPixel(GraphicsDevice, Content,
                screensize.X / 2 - 155, screensize.Y / 2 - 48, 32, 32, Color.DarkGray);
            resbtn.SpriteSetColbox();
            resbtn_font_pos.X = resbtn.GetSpritePos().X;
            resbtn_font_pos.Y = resbtn.GetSpritePos().Y;
            //difficulty button
            difbtn.SpriteLoadPixel(GraphicsDevice, Content,
                screensize.X / 2 - 130, screensize.Y / 2, 85, 32, Color.DarkGray);
            difbtn.SpriteSetColbox();
            difbtn_font_pos.X = difbtn.GetSpritePos().X;
            difbtn_font_pos.Y = difbtn.GetSpritePos().Y;
            //score button
            scorebtn.SpriteLoadPixel(GraphicsDevice, Content,
                screensize.X / 2 + 157, screensize.Y / 2 - 48, 32, 32, Color.DarkGray);
            scorebtn.SpriteSetColbox();
            scorebtn_font_pos.X = scorebtn.GetSpritePos().X;
            scorebtn_font_pos.Y = scorebtn.GetSpritePos().Y;
            // help button
            helpbtn.SpriteLoadPixel(GraphicsDevice, Content,
                screensize.X / 2 - 124, screensize.Y / 2 + 208, 96, 32, Color.DarkGray);
            helpbtn.SpriteSetColbox();
            helpbtn_font_pos.X = helpbtn.GetSpritePos().X;
            helpbtn_font_pos.Y = helpbtn.GetSpritePos().Y;

            //Winner Banner
            WinnerBanner.SpriteLoadPixel(GraphicsDevice, Content,
                screensize.X / 2, screensize.Y / 3, 700, 75, Color.DarkGray);

            //scoreline
            scoreline.SpriteLoadContent("graphics/scoreline", Content,
                graphics.PreferredBackBufferWidth / 2,
                graphics.PreferredBackBufferHeight / 2,
                5,
                graphics.PreferredBackBufferHeight);

            //fonts 
            font_score = Content.Load<SpriteFont>("fonts/font_score");
            font_title = Content.Load<SpriteFont>("fonts/font_title");
            font_debug = Content.Load<SpriteFont>("fonts/font_debug");
            font_title_pos = new Vector2(screensize.X / 2 - 115, screensize.Y / 8);
            P1font_score_pos = new Vector2(screensize.X / 4 - 10, screensize.Y / 10);
            P2font_score_pos = new Vector2(screensize.X / 4 * 3, screensize.Y / 10);

            //player 1 paddle load content
            P1paddle.SpriteLoadPixel(GraphicsDevice, Content,
                screensize.X / 12, Mouse.GetState().Y, 16, 64, Color.White);

            //Player 2 paddle load content
            P2paddle.SpriteLoadPixel(GraphicsDevice, Content,
                screensize.X / 9 * 8, screensize.Y / 2, 16, 64, Color.White);

            //the ball load content
            ball.SpriteLoadPixel(GraphicsDevice, Content,
                screensize.X / 2, screensize.Y / 2, 16, 16, Color.White);

        }

        void AIupdate()
        {
            Ai_dirto = ball.GetSpritePos() - P2paddle.GetSpritePos();
            Aispeed = Ai_dirto.Length() * Aispeed_previous;
            Ai_dirto.Normalize();

            Ai_vel = Ai_dirto * Aispeed;
            P2paddle.SpriteSetPos_Y(P2paddle.GetSpritePos().Y + Ai_vel.Y);

            if (difficulty == EDifficulty.EASY)
            {
                if (ball_speed.Y >= 5f)
                {
                    Aispeed_previous = +0.2f;
                }
                else
                    Aispeed_previous = 0.1f;
            }
            if (difficulty == EDifficulty.MEDIUM)
            {
                if (ball_speed.Y >= 8f)
                {
                    Aispeed_previous = +0.2f;
                }
                else
                    Aispeed_previous = 0.15f;
            }
            if (difficulty == EDifficulty.HARD)
            {
                if (ball_speed.Y >= 8f)
                {
                    Aispeed_previous = +0.2f;
                }
                else
                    Aispeed_previous = 0.2f;
            }

        }

        void SinglePlayer()
        {
            P1paddle.SpriteSetPos(screensize.X / 12, Mouse.GetState().Y);

            //New ai code
            AIupdate();
        }

        void TwoPlayer()
        {
            //player 1 controls
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                P1paddle.SpriteSetPos_Y(P1paddle.GetSpritePos().Y - paddle_speed);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                P1paddle.SpriteSetPos_Y(P1paddle.GetSpritePos().Y + paddle_speed);
            }
            if (P1paddle.GetSpritePos().Y < 0)
            {
                P1paddle.SpriteSetPos_Y(P1paddle.GetSpritePos().Y + paddle_speed);
            }
            if (P1paddle.GetSpritePos().Y > screensize.Y)
            {
                P1paddle.SpriteSetPos_Y(P1paddle.GetSpritePos().Y - paddle_speed);
            }

            //player 2 controls
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                P2paddle.SpriteSetPos_Y(P2paddle.GetSpritePos().Y - paddle_speed);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                P2paddle.SpriteSetPos_Y(P2paddle.GetSpritePos().Y + paddle_speed);
            }
            if (P2paddle.GetSpritePos().Y < 0)
            {
                P2paddle.SpriteSetPos_Y(P2paddle.GetSpritePos().Y + paddle_speed);
            }
            if (P2paddle.GetSpritePos().Y > screensize.Y)
            {
                P2paddle.SpriteSetPos_Y(P2paddle.GetSpritePos().Y - paddle_speed);
            }


        }

        void DifficultyEasy()
        {
            Sdifficulty = "Easy";
            ball_speed.X = 5;
            ball_speed.Y = 2;
            paddle_speed = 8f;
            Aispeed_previous = 0.1f;
        }

        void DifficultyMedium()
        {
            Sdifficulty = "Medium";
            ball_speed.X = 8;
            ball_speed.Y = 4;
            paddle_speed = 15f;
            Aispeed_previous = 0.15f;
        }

        void DifficultyHard()
        {
            Sdifficulty = "Hard";
            ball_speed.X = 11;
            ball_speed.Y = 6;
            paddle_speed = 20f;
            Aispeed_previous = 0.2f;
        }

        //Reset the game
        void GameReset()
        {
            //reset scores, ball, paddles
            if (gamemode == EGameMode.MENU)
            {
                P1score = 0;
                P2score = 0;
            }

            if (playstyle == EPlayStyle.ONEP)
            {
                P2paddle.SpriteSetPos_Y(ball.GetSpritePos().Y);
            }

            ball.SpriteSetPos(screensize.X / 2, screensize.Y / 2);
            ballrand_value = ballrand.Next(1, 5);
            YouWon = false;

            switch (difficulty)
            {
                case EDifficulty.EASY: DifficultyEasy(); break;
                case EDifficulty.MEDIUM: DifficultyMedium(); break;
                case EDifficulty.HARD: DifficultyHard(); break;
            }

        }

        void PlayerWinner()
        {

            ball.SpriteSetPos(screensize.X / 2, screensize.Y / 2);

            P2paddle.SpriteSetPos_Y(ball.GetSpritePos().Y);
            P1paddle.SpriteSetPos_Y(ball.GetSpritePos().Y);

            YouWon = true;

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                GameReset();
                P1score = 0;
                P2score = 0;
            }
        }

        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            if (gamemode == EGameMode.MENU)
            {
                //Setup Difficulty
                switch (difficulty)
                {
                    case EDifficulty.EASY: DifficultyEasy(); break;
                    case EDifficulty.MEDIUM: DifficultyMedium(); break;
                    case EDifficulty.HARD: DifficultyHard(); break;
                }

                //mouse
                mouse_rect.X = Mouse.GetState().X;
                mouse_rect.Y = Mouse.GetState().Y;
                IsMouseVisible = true;

                //menu
                //play button
                if (checkcollision(mouse_rect, playbtn.GetColbox()))
                {
                    playbtn.SpriteSetPos(screensize.X / 2 - 2, (screensize.Y / 2 + 64));
                    playbtn_font_pos.X = playbtn.GetSpritePos().X;

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        gamemode = EGameMode.PLAY;
                        playstyle = EPlayStyle.ONEP;
                    }
                }
                else
                {
                    playbtn.SpriteSetPos(screensize.X / 2, screensize.Y / 2 + 64);
                    playbtn_font_pos.X = playbtn.GetSpritePos().X;
                }
                //multiplayer button
                if (checkcollision(mouse_rect, multibtn.GetColbox()))
                {
                    multibtn.SpriteSetPos(screensize.X / 2 - 2, (screensize.Y / 2 + 144));
                    multibtn_font_pos.X = multibtn.GetSpritePos().X;

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        gamemode = EGameMode.PLAY;
                        playstyle = EPlayStyle.TWOP;
                    }
                }
                else
                {
                    multibtn.SpriteSetPos(screensize.X / 2, screensize.Y / 2 + 144);
                    multibtn_font_pos.X = multibtn.GetSpritePos().X;
                }
                //Help button
                if (checkcollision(mouse_rect, helpbtn.GetColbox()))
                {
                    helpbtn.SpriteSetPos(screensize.X / 2 - 126, (screensize.Y / 2 + 208));
                    helpbtn_font_pos.X = helpbtn.GetSpritePos().X;

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        gamemode = EGameMode.HELP;
                    }
                }
                else
                {
                    helpbtn.SpriteSetPos(screensize.X / 2 - 124, screensize.Y / 2 + 208);
                    helpbtn_font_pos.X = helpbtn.GetSpritePos().X;
                }
                //fullscreen button
                if (checkcollision(mouse_rect, resbtn.GetColbox()))
                {
                    resbtn.SpriteSetPos(screensize.X / 2 - 157, screensize.Y / 2 - 48);
                    resbtn_font_pos.X = resbtn.GetSpritePos().X;

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        if (isfullscreen != true)
                        {
                            isfullscreen = true;
                        }
                        else
                        {
                            isfullscreen = false;
                        }
                    }
                }
                else
                {
                    resbtn.SpriteSetPos(screensize.X / 2 - 155, screensize.Y / 2 - 48);
                    resbtn_font_pos.X = resbtn.GetSpritePos().X;
                }
                //Difficulty button
                if (checkcollision(mouse_rect, difbtn.GetColbox()))
                {
                    difbtn.SpriteSetPos(screensize.X / 2 - 132, screensize.Y / 2);
                    difbtn_font_pos.X = difbtn.GetSpritePos().X;

                    if (oldMState.LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        if (difficulty == EDifficulty.EASY)
                            difficulty = EDifficulty.MEDIUM;
                        else if (difficulty == EDifficulty.MEDIUM)
                            difficulty = EDifficulty.HARD;
                        else if (difficulty == EDifficulty.HARD)
                            difficulty = EDifficulty.EASY;
                    }
                    oldMState = Mouse.GetState();
                   
                }
                else
                {
                    difbtn.SpriteSetPos(screensize.X / 2 - 130, screensize.Y / 2);
                    difbtn_font_pos.X = difbtn.GetSpritePos().X;
                }
                //score button
                if (checkcollision(mouse_rect, scorebtn.GetColbox()))
                {
                    scorebtn.SpriteSetPos(screensize.X / 2 + 155, screensize.Y / 2 - 48);
                    scorebtn_font_pos.X = scorebtn.GetSpritePos().X;

                    if (oldMState.LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        BestOutOf++;
                    }
                    else if (oldMState.RightButton == ButtonState.Released && Mouse.GetState().RightButton == ButtonState.Pressed)
                    {
                        BestOutOf--;
                    }
                    oldMState = Mouse.GetState();

                    if (BestOutOf >= 1)
                    {
                        BestOfOn = true;
                    }
                    if (BestOutOf <= 0)
                    {
                        BestOutOf = 0;
                        BestOfOn = false;
                    }
                    if (BestOutOf == 11)
                    {
                        BestOutOf = 0;
                        BestOfOn = false;
                    }
                }
                else
                {
                    scorebtn.SpriteSetPos(screensize.X / 2 + 157, screensize.Y / 2 - 48);
                    scorebtn_font_pos.X = scorebtn.GetSpritePos().X;
                }

                //fullscreen
                if (isfullscreen == true)
                {
                    graphics.IsFullScreen = true;
                    graphics.ApplyChanges();
                }
                else
                {
                    graphics.IsFullScreen = false;
                    graphics.ApplyChanges();
                }
            }
            if (gamemode == EGameMode.HELP)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Back))
                {
                    gamemode = EGameMode.MENU;
                }
            }
            if (gamemode == EGameMode.PLAY)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Back))
                {
                    gamemode = EGameMode.MENU;
                    GameReset();
                }


                //winner counter
                if (BestOfOn == true)
                {
                    if (P1score == BestOutOf)
                    {
                        PlayerWinner();
                    }

                    if (P2score == BestOutOf)
                    {
                        PlayerWinner();
                    }
                }

                //mouse Update
                IsMouseVisible = false;

                if (playstyle == EPlayStyle.TWOP)
                {
                    TwoPlayer();
                }
                else
                {
                    SinglePlayer();
                }

                //Player 1 paddle Rectangle update
                P1paddle.SpriteSetColbox();

                //Player 2 paddle Rectangle update
                P2paddle.SpriteSetColbox();

                //the ball rectangle update
                ball.SpriteSetColbox();

                //collision detection
                if (checkcollision(P1paddle.GetColbox(), ball.GetColbox()))
                {
                    //check pos of edge of paddle for strong angle hit
                    if (P1paddle.GetSpritePos().Y - 32 >= ball.GetSpritePos().Y)
                    {
                        Bdown = false;
                        ball_speed.Y += 2f;
                        ball_speed.X += 1f;
                    }
                    if (P1paddle.GetSpritePos().Y + 32 <= ball.GetSpritePos().Y)
                    {
                        Bdown = true;
                        ball_speed.Y += 2f;
                        ball_speed.X += 1f;
                    }
                    //check pos of edge of paddle for normal angle hit
                    if (P1paddle.GetSpritePos().Y - 16 >= ball.GetSpritePos().Y)
                    {
                        Bdown = false;
                        ball_speed.Y += 1f;
                    }
                    if (P1paddle.GetSpritePos().Y + 16 <= ball.GetSpritePos().Y)
                    {
                        Bdown = true;
                        ball_speed.Y += 1f;
                    }
                    //check pos of edge of paddle for normal hit
                    if (P1paddle.GetSpritePos().Y >= ball.GetSpritePos().Y)
                    {
                        Bdown = false;
                        ball_speed.Y -= 0.5f;
                    }
                    if (P1paddle.GetSpritePos().Y <= ball.GetSpritePos().Y)
                    {
                        Bdown = true;
                        ball_speed.Y -= 0.5f;
                    }

                    Bright = true;
                    ball_speed.X += 0.1f;

                    paddle_sound.Play();
                }

                if (checkcollision(ball.GetColbox(), P2paddle.GetColbox()))
                {
                    //check pos of edge of paddle for strong angle hit
                    if (P2paddle.GetSpritePos().Y - 32 >= ball.GetSpritePos().Y)
                    {
                        Bdown = false;
                        ball_speed.Y += 2f;
                        ball_speed.X += 1f;
                    }
                    if (P2paddle.GetSpritePos().Y + 32 <= ball.GetSpritePos().Y)
                    {
                        Bdown = true;
                        ball_speed.Y += 2f;
                        ball_speed.X += 1f;
                    }
                    //check pos of edge of paddle for normal angle hit
                    if (P2paddle.GetSpritePos().Y - 16 >= ball.GetSpritePos().Y)
                    {
                        Bdown = false;
                        ball_speed.Y += 1f;
                    }
                    if (P2paddle.GetSpritePos().Y + 16 <= ball.GetSpritePos().Y)
                    {
                        Bdown = true;
                        ball_speed.Y += 1f;
                    }
                    //check pos of edge of paddle for normal hit
                    if (P2paddle.GetSpritePos().Y >= ball.GetSpritePos().Y)
                    {
                        Bdown = false;
                        ball_speed.Y -= 0.5f;
                    }
                    if (P2paddle.GetSpritePos().Y <= ball.GetSpritePos().Y)
                    {
                        Bdown = true;
                        ball_speed.Y -= 0.5f;
                    }

                    //Bleft = true;
                    Bright = false;
                    ball_speed.X += 0.1f;

                    paddle_sound.Play();

                }
                //hits Y pos
                if (ball.GetSpritePos().Y < 0)
                {
                    rebound_sound.Play();

                    Bdown = true;

                }
                if (ball.GetSpritePos().Y > screensize.Y - 15)
                {
                    rebound_sound.Play();

                    Bdown = false;

                }
                //hitsX pos with reset
                if (ball.GetSpritePos().X < 0)
                {
                    goal_sound.Play();

                    P2score++;

                    GameReset();
                }
                if (ball.GetSpritePos().X > screensize.X)
                {
                    goal_sound.Play();

                    P1score++;

                    GameReset();
                }

                //random start direction
                if (ballrand_value == 1)
                {
                    Bdown = true;
                    Bright = true;
                    ballrand_value = 0;
                }
                if (ballrand_value == 2)
                {
                    Bdown = true;
                    Bright = false;
                    ballrand_value = 0;
                }
                if (ballrand_value == 3)
                {
                    Bdown = false;
                    Bright = true;
                    ballrand_value = 0;
                }
                if (ballrand_value == 4)
                {
                    Bdown = false;
                    Bright = false;
                    ballrand_value = 0;
                }

                if (Bdown == true)
                {
                    ball.SpriteSetPos_Y(ball.GetSpritePos().Y + ball_speed.Y);
                }
                else
                {
                    ball.SpriteSetPos_Y(ball.GetSpritePos().Y - ball_speed.Y);
                }
                if (Bright == true)
                {
                    ball.SpriteSetPos_X(ball.GetSpritePos().X + ball_speed.X);
                }
                else
                {
                    ball.SpriteSetPos_X(ball.GetSpritePos().X - ball_speed.X);
                }

                // make sure the Y speed doesnt go into the minus
                if (ball_speed.Y <= 0.1f)
                {
                    ball_speed.Y = 1f;
                }


            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (gamemode == EGameMode.MENU)
            {
                playbtn.SpriteDrawPixel(spriteBatch);
                //playbtn.SpriteDrawDebug(spriteBatch);
                multibtn.SpriteDrawPixel(spriteBatch);

                difbtn.SpriteDrawPixel(spriteBatch);
                resbtn.SpriteDrawPixel(spriteBatch);
                scorebtn.SpriteDrawPixel(spriteBatch);
                helpbtn.SpriteDrawPixel(spriteBatch);

                spriteBatch.Begin();

                //title font
                spriteBatch.DrawString(font_title, "PONG", font_title_pos, Color.White);

                //play button

                string singleplayer = "Single Player";
                Vector2 SPfontsize = new Vector2();
                SPfontsize = font_score.MeasureString(singleplayer);
                spriteBatch.DrawString(font_score, singleplayer, playbtn_font_pos, Color.White, 0,
                    SPfontsize / 2,
                    1,
                    SpriteEffects.None,
                    0.0f);
                //multiplayer button
                string twoplayer = "Two Player";
                Vector2 TPfontsize = new Vector2();
                TPfontsize = font_score.MeasureString(twoplayer);
                spriteBatch.DrawString(font_score, twoplayer, multibtn_font_pos, Color.White, 0,
                    TPfontsize / 2,
                    1,
                    SpriteEffects.None,
                    0.0f);
                //Help button
                string HelpString = "Help";
                Vector2 Hfontsize = new Vector2();
                Hfontsize = font_score.MeasureString(HelpString);
                spriteBatch.DrawString(font_score, HelpString, helpbtn_font_pos, Color.White, 0,
                    Hfontsize / 2,
                    0.8f,
                    SpriteEffects.None,
                    0.0f);
                //fullscreen button
                Vector2 resstring_font_pos = new Vector2(screensize.X / 2 - 133, screensize.Y / 2 - 60);
                spriteBatch.DrawString(font_debug, ":Fullscreen", resstring_font_pos, Color.White);
                if (graphics.IsFullScreen == true)
                {
                    string resenable = "X";
                    Vector2 resfontsize = new Vector2();
                    resfontsize = font_score.MeasureString(resenable);
                    spriteBatch.DrawString(font_score, resenable, resbtn_font_pos, Color.White, 0,
                        resfontsize / 2,
                        0.8f,
                        SpriteEffects.None,
                        0.0f);
                }
                //difficulty
                Vector2 Dfontsize = new Vector2();
                Dfontsize = font_score.MeasureString(Sdifficulty);
                spriteBatch.DrawString(font_score, Sdifficulty, difbtn_font_pos, Color.White, 0,
                    Dfontsize / 2,
                    0.5f,
                    SpriteEffects.None,
                    0.0f);

                Vector2 difstring_font_pos = new Vector2(screensize.X / 2 - 80, screensize.Y / 2 - 10);
                spriteBatch.DrawString(font_debug, ":Difficulty", difstring_font_pos, Color.White);
                //score button draw
                Vector2 SBfontsize = new Vector2();
                if (BestOfOn == true)
                {
                    SBfontsize = font_score.MeasureString(BestOutOf.ToString());
                    spriteBatch.DrawString(font_score, BestOutOf.ToString(), scorebtn_font_pos, Color.White, 0,
                        SBfontsize / 2,
                        0.5f,
                        SpriteEffects.None,
                        0.0f);
                }
                else
                {
                    SBfontsize = font_score.MeasureString("-");
                    spriteBatch.DrawString(font_score, "-", scorebtn_font_pos, Color.White, 0,
                        SBfontsize / 2,
                        0.5f,
                        SpriteEffects.None,
                        0.0f);
                }
                Vector2 bestofstring_font_pos = new Vector2(screensize.X / 2 + 45, screensize.Y / 2 - 60);
                spriteBatch.DrawString(font_debug, "Best Of:", bestofstring_font_pos, Color.White);


                spriteBatch.End();
            }
            if (gamemode == EGameMode.HELP)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(font_score, "HELP",
                    new Vector2(screensize.X / 2 - 150, screensize.Y / 2), Color.White);
                spriteBatch.DrawString(font_debug, "Backspace: Return to Menu \nEsc: Quit Game",
                    new Vector2(screensize.X / 2 - 150, screensize.Y / 2 + 50), Color.White);
                spriteBatch.End();
            }
            if (gamemode == EGameMode.PLAY)
            {
                P1paddle.SpriteDrawPixel(spriteBatch);
                //P1paddle.SpriteDrawDebug(spriteBatch);

                P2paddle.SpriteDrawPixel(spriteBatch);
                //P2paddle.SpriteDrawDebug(spriteBatch);

                ball.SpriteDrawPixel(spriteBatch);
                //ball.SpriteDrawDebug(spriteBatch);

                scoreline.SpriteDrawContent(spriteBatch);

                spriteBatch.Begin();

                //draw the scores
                spriteBatch.DrawString(font_score, P1score.ToString(), P1font_score_pos, Color.White);
                spriteBatch.DrawString(font_score, P2score.ToString(), P2font_score_pos, Color.White);

                spriteBatch.End();

                //winning stuff
                string WinString;

                if (BestOutOf == P1score)
                    WinString = "Player 1 Wins!";
                else
                    WinString = "Player 2 Wins!";

                if (YouWon == true)
                {
                    WinnerBanner.SpriteDrawPixel(spriteBatch);

                    spriteBatch.Begin();

                    Vector2 Winfontsize = new Vector2();
                    Winfontsize = font_title.MeasureString(WinString);
                    spriteBatch.DrawString(font_title, WinString,
                        new Vector2(screensize.X / 2, screensize.Y / 3),
                        Color.White,
                        0,
                        Winfontsize / 2,
                        0.8f,
                        SpriteEffects.None,
                        0.0f);

                    spriteBatch.End();
                }
            }

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            P1paddle.SpriteDispose();
            P2paddle.SpriteDispose();
            ball.SpriteDispose();
            playbtn.SpriteDispose();
            multibtn.SpriteDispose();
            resbtn.SpriteDispose();
            difbtn.SpriteDispose();
            WinnerBanner.SpriteDispose();
            scorebtn.SpriteDispose();
            helpbtn.SpriteDispose();
        }
    }
}
