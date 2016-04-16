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

namespace Girl_Shooters
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D bossHealth;
        int bossLife = 100;
        Texture2D menuBack;
        Texture2D gameOver;
        Texture2D bossBack;
        Texture2D winBack;
        Texture2D bossHit;
        Texture2D bossNormal;
        Texture2D bossMad;
        Boss boss;
        Song lvl1Music;
        Song over;
        SoundEffect firstgun;
        SoundEffect gameEnd;
        SoundEffect enemyDie;
        SoundEffect alienDie;
        Girls girl1;
        Girls girl2;
        Enemies [] enemy;
        Enemies[] alien;
        Bullets[] firstGun;
        Bullets[] secondGun;
        Bullets[] alienShot;
        int bullets = 10;
        bool direction = true;
        bool direction2 = true;
        private TimeSpan ShotUpdate = TimeSpan.Zero;
        public int ShotFreq = 100;
        public bool canFire = true;
        enum GameState { MENU, Level1, Level2, Level3, WON, GAMEOVER };
        GameState currentGameState;
        MovingBackground scrollBack1;
        MovingBackground scrollBack2;
        MovingBackground scrollBack3;
        MovingBackground space1;
        MovingBackground space2;
        MovingBackground space3;
        public Random rnd;
        public int enemySpawn= 10;
        public int alienspawn = 20;
        public int yPos;
        public int ypos2;
        SpriteFont font1;
        SpriteFont font2;
        double timer;
        double timeCounter;
        int gameScore;
        int lives1;
        int lives2;
        int numSpawn = 0;
        int spawnCounter = 0;
        int shown = 0;

        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 40);
            

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
            scrollBack1 = new MovingBackground();
            scrollBack1.Scale = 3.5f;
            scrollBack2 = new MovingBackground();
            scrollBack2.Scale = 3.5f;
            scrollBack3 = new MovingBackground();
            scrollBack3.Scale = 3.0f;
            space1 = new MovingBackground();
            space1.Scale = 3.0f;
            space2 = new MovingBackground();
            space2.Scale = 3.0f;
            space3 = new MovingBackground();
            space3.Scale = 3.0f;
            rnd = new Random();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
             //Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bossHealth = Content.Load<Texture2D>("healthbar");
            font1 = Content.Load<SpriteFont>(@"Font\font1");
            font2 = Content.Load<SpriteFont>(@"Font\font2");
            scrollBack1.LoadContent(this.Content, @"Backgrounds\alienMenu");
            scrollBack1.Position = new Vector2(0, 0);
            scrollBack2.LoadContent(this.Content, @"Backgrounds\scrolling3");
            scrollBack2.Position = new Vector2(scrollBack1.Position.X + scrollBack1.Size.Width, 0);
            scrollBack3.LoadContent(this.Content, @"Backgrounds\scrolling2");
            scrollBack3.Position = new Vector2(scrollBack2.Position.X + scrollBack2.Size.Width, 0);
            space1.LoadContent(this.Content, @"Backgrounds\space0");
            space1.Position = new Vector2(0, 0);
            space2.LoadContent(this.Content, @"Backgrounds\space1");
            space2.Position = new Vector2(space1.Position.X + space1.Size.Width, 0);
            space3.LoadContent(this.Content, @"Backgrounds\space3");
            space3.Position = new Vector2(space2.Position.X + space2.Size.Width, 0);
            currentGameState = GameState.MENU;
            gameOver = Content.Load<Texture2D>(@"Backgrounds\gameEnd");
            bossBack = Content.Load<Texture2D>(@"Backgrounds\boss_background");
            bossHit = Content.Load<Texture2D>(@"Boss\bosshit");
            bossNormal = Content.Load<Texture2D>(@"Boss\boss1");
            bossMad = Content.Load<Texture2D>(@"Boss\bossmad");
            winBack = Content.Load<Texture2D>(@"Backgrounds\win");
            lvl1Music = Content.Load<Song>(@"Backgrounds\Royalty_Free");
            over = Content.Load<Song>(@"Backgrounds\game_over");
            firstgun = Content.Load<SoundEffect>(@"Sound_Effects\first_gun");
            gameEnd = Content.Load<SoundEffect>(@"Sound_Effects\game_over");
            enemyDie = Content.Load<SoundEffect>(@"Sound_Effects\enemydie");
            alienDie = Content.Load<SoundEffect>(@"Sound_Effects\aliendie");
            MediaPlayer.Play(lvl1Music);
            MediaPlayer.IsRepeating = true;
            menuBack = Content.Load<Texture2D>(@"Backgrounds\alienMenu");
            firstGun = new Bullets[bullets];
            for (int i = 0; i < firstGun.Length; i++)
            {
                firstGun[i] = new Bullets(Content.Load<Texture2D>(@"Bullets\bullet1"), graphics);
                firstGun[i].alive = false;
                firstGun[i].visible = false;
                firstGun[i].Scale = .8f;
                firstGun[i].speed = 7;
            }
            secondGun = new Bullets[bullets];
            for (int i = 0; i < secondGun.Length; i++)
            {
                secondGun[i] = new Bullets(Content.Load<Texture2D>(@"Bullets\bullet1"), graphics);
                secondGun[i].alive = false;
                secondGun[i].visible = false;
                secondGun[i].Scale = .8f;
                secondGun[i].speed = 7;
            }
            alienShot = new Bullets[8];
            for (int i = 0; i < alienShot.Length; i++)
            {
                ypos2 = rnd.Next(100, 450);
                alienShot[i] = new Bullets(Content.Load<Texture2D>(@"Boss\alienShot"), graphics);
                alienShot[i].position = new Vector2(475, ypos2);
                alienShot[i].alive = false;
                alienShot[i].visible = false;
                alienShot[i].Scale = .3f;
                alienShot[i].speed = 7;
            }
            boss = new Boss(Content.Load<Texture2D>(@"Boss\boss1"), graphics);
            boss.alive = false;
            boss.visible = false;
            boss.Scale = 1.8f;
            boss.speed = 3;
            boss.position = new Vector2(600, 275);
            girl1 = new Girls(Content.Load<Texture2D>(@"Girls\female1"));
            girl1.SourceRectangle = new Rectangle(0, 0, 32, 48);
            girl1.position = new Vector2(70, 418);
            girl1.framesize = new Point(32, 48);
            girl1.currentframe = new Point(0, 0);
            girl1.sheetsize = new Point(4, 4);
            girl1.Scale = 1.5f;
            girl1.visible = false;
            girl2 = new Girls(Content.Load<Texture2D>(@"Girls\female2"));
            girl2.SourceRectangle = new Rectangle(0, 0, 32, 48);
            girl2.position = new Vector2(25, 418);
            girl2.framesize = new Point(32, 48);
            girl2.currentframe = new Point(0, 0);
            girl2.sheetsize = new Point(4, 4);
            girl2.Scale = 1.5f;
            girl2.visible = false;
            enemy = new Enemies[enemySpawn];
            for (int i = 0; i < enemy.Length; i++)
            {
                yPos = rnd.Next(50, 450);
                enemy[i] = new Enemies(Content.Load<Texture2D>(@"Enemies\enemy1"), graphics);
                enemy[i].SourceRectangle = new Rectangle(0, 0, 32, 48);
                enemy[i].position = new Vector2(850, yPos);
                enemy[i].speed = rnd.Next(3, 10);
                enemy[i].framesize = new Point(32, 48);
                enemy[i].currentframe = new Point(0, 0);
                enemy[i].sheetsize = new Point(4, 4);
                enemy[i].Scale = 1.5f;
                enemy[i].visible = false;
                
            }
            alien = new Enemies[alienspawn];
            for (int i = 0; i < alien.Length; i++)
            {
                yPos = rnd.Next(50, 450);
                alien[i] = new Enemies(Content.Load<Texture2D>(@"Enemies\alien1"), graphics);
                alien[i].SourceRectangle = new Rectangle(0, 0, 32, 48);
                alien[i].position = new Vector2(850, yPos);
                alien[i].speed = rnd.Next(1, 18);
                alien[i].framesize = new Point(32, 48);
                alien[i].currentframe = new Point(0, 0);
                alien[i].sheetsize = new Point(4, 4);
                alien[i].Scale = 1.5f;
                alien[i].visible = false;

            }
            lives1 = 3;
            lives2 = 3;
            
            
  

            // TODO: use this.Content to load your game content here
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
        protected override void Update(GameTime gameTime)
        {
#if!XBOX
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            if(Keyboard.GetState().IsKeyDown(Keys.P))
            {
                lives1 = 1000;
                lives2 = 1000;
            }
            switch (currentGameState)
            {
                case GameState.MENU:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        currentGameState = GameState.Level1;
                        gameScore = 0;
                        numSpawn = 0;
                    break;
                case GameState.Level1:
                    scrollBack1.LoadContent(this.Content, @"Backgrounds\scrolling1");
                    backgroundMotion(gameTime);
                    girl1.visible = true;
                    girl2.visible = true;
                    playerControls(gameTime);
                    foreach (Enemies gameObject in enemy)
                    {
                        if (gameObject.visible && girl1.visible)
                        {
                            if (gameObject.checkCollideWith(girl1))
                            {
                                gameObject.position.X = 850;
                                lives1--;
                            }
                        }
                    }
                    foreach (Enemies gameObject in enemy)
                    {
                        if (gameObject.visible && girl2.visible)
                        {
                            if (gameObject.checkCollideWith(girl2))
                            {
                                gameObject.position.X = 850;
                                lives2--;
                            }
                        }
                    }
                    if (lives1 <= 0)
                    {
                        girl1.visible = false;
                        girl1.position.X = 0;
                        girl1.position.Y = 0;
                    }
                    if (lives2 <= 0)
                    {
                        girl2.visible = false;
                        girl2.position.X = 0;
                        girl2.position.Y = 0;
                    }
                    for (int i = 0; i < firstGun.Length; i++ )
                    {
                        foreach(Enemies gameObject in enemy)
                        {
                            if (gameObject.visible && firstGun[i].visible)
                            {
                                if (firstGun[i].checkCollideWith(gameObject))
                                {
                                    enemyDie.Play();
                                    gameObject.visible = false;
                                    firstGun[i].visible = false;
                                    numSpawn += 1;
                                    gameScore += (numSpawn * 10);
                                }
                            }
                        }
                    }
                    for (int i = 0; i < secondGun.Length; i++)
                    {
                        foreach (Enemies gameObject in enemy)
                        {
                            if (gameObject.visible && secondGun[i].visible)
                            {
                                if (secondGun[i].checkCollideWith(gameObject))
                                {
                                    enemyDie.Play();
                                    gameObject.visible = false;
                                    secondGun[i].visible = false;
                                    numSpawn += 1;
                                    gameScore += 1;
                                }
                            }
                        }
                    }
                    timer += gameTime.ElapsedGameTime.TotalSeconds;
                    timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
                    spawnCounter = (int)timeCounter;
                    if (spawnCounter < 1)
                    {
                        foreach (Enemies gameObject in enemy)
                        {
                            gameObject.visible = true;
                        }
                        shown++;
                    }
                    foreach(Enemies gameObject in enemy)
                    {
                        if(gameObject.visible == true)
                            gameObject.update(gameTime);
                        if (gameObject.position.X < 0)
                            gameObject.position.X = 850;
                    }
                    if (!girl1.visible && !girl2.visible)
                        currentGameState = GameState.GAMEOVER;
                    if(numSpawn >= enemySpawn)
                    {

                        timer = 0;
                        timeCounter = 0;
                        spawnCounter = 0;
                        numSpawn = 0;
                        currentGameState = GameState.Level2;
                    }
                    break;
                case GameState.Level2:
                    backgroundMotionl2(gameTime);
                    girl1.visible = true;
                    girl2.visible = true;
                    playerControls(gameTime);
                    foreach (Enemies gameObject in alien)
                    {
                        if (gameObject.visible == true)
                        {
                            if (gameObject.checkCollideWith(girl1))
                            {
                                gameObject.position.X = 850;
                                lives1--;
                            }
                        }
                    }
                    foreach (Enemies gameObject in alien)
                    {
                        if (gameObject.visible == true)
                        {
                            if (gameObject.checkCollideWith(girl2))
                            {
                                gameObject.position.X = 850;
                                lives2--;
                            }
                        }
                    }
                    if (lives1 <= 0)
                    {
                        girl1.visible = false;
                        girl1.position.X = 0;
                        girl1.position.Y = 0;
                    }
                    if (lives2 <= 0)
                    {
                        girl2.visible = false;
                        girl2.position.X = 0;
                        girl2.position.Y = 0;
                    }
                    for (int i = 0; i < firstGun.Length; i++ )
                    {
                        foreach(Enemies gameObject in alien)
                        {
                            if (gameObject.visible && firstGun[i].visible)
                            {
                                if (firstGun[i].checkCollideWith(gameObject))
                                {
                                    alienDie.Play();
                                    gameObject.visible = false;
                                    firstGun[i].visible = false;
                                    numSpawn += 1;
                                    gameScore += (numSpawn * 10);
                                }
                            }
                        }
                    }
                    for (int i = 0; i < secondGun.Length; i++)
                    {
                        foreach (Enemies gameObject in alien)
                        {
                            if (gameObject.visible && secondGun[i].visible)
                            {
                                if (secondGun[i].checkCollideWith(gameObject))
                                {
                                    alienDie.Play();
                                    gameObject.visible = false;
                                    secondGun[i].visible = false;
                                    numSpawn += 1;
                                    gameScore += (numSpawn * 10);
                                }
                            }
                        }
                    }
                    timer += gameTime.ElapsedGameTime.TotalSeconds;
                    timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
                    spawnCounter = (int)timeCounter;
                    if (spawnCounter < 1)
                    {
                        foreach (Enemies gameObject in alien)
                        {
                            gameObject.visible = true;
                        }
                        shown++;
                    }
                    foreach (Enemies gameObject in alien)
                    {
                        if(gameObject.visible)
                            gameObject.update(gameTime);
                        if (gameObject.position.X < 0)
                            gameObject.position.X = 850;
                    }
                    if (!girl1.visible && !girl2.visible)
                        currentGameState = GameState.GAMEOVER;
                    if(numSpawn >= alienspawn)
                    {
                        timer = 0;
                        timeCounter = 0;
                        spawnCounter = 0;
                        numSpawn = 0;
                        currentGameState = GameState.Level3;
                    }
                    break;
                case GameState.Level3:
                    bossLife = (int)MathHelper.Clamp(bossLife, 0, 100);
                    girl1.visible = true;
                    girl2.visible = true;
                    playerControls(gameTime);
                    boss.visible = true;
                    if (bossLife > 50)
                        boss.item = bossNormal;
                    if (bossLife < 50)
                        boss.item = bossMad;
                    for(int i = 0; i < firstGun.Length; i++ )
                    {
                        if (boss.visible && firstGun[i].visible)
                            {
                                if (firstGun[i].checkCollideWith(boss))
                                {
                                    bossLife -= 1;
                                    boss.item = bossHit;
                                    firstGun[i].visible = false;
                                    numSpawn += 1;
                                    gameScore += (numSpawn * 10);
                                }
                            }
                    }
                    for (int i = 0; i < secondGun.Length; i++)
                    {
                        if (boss.visible && secondGun[i].visible)
                        {
                            if (secondGun[i].checkCollideWith(boss))
                            {
                                bossLife -= 1;
                                boss.item = bossHit;
                                secondGun[i].visible = false;
                                numSpawn += 1;
                                gameScore += (numSpawn * 10);
                            }
                        }
                    }
                    foreach (Bullets gameObject in alienShot)
                    {
                        for (int i = 0; i < firstGun.Length; i++)
                        {
                            if (gameObject.visible && firstGun[i].visible)
                            {
                                if (firstGun[i].checkCollideWith(gameObject))
                                {
                                    ypos2 = rnd.Next(100, 450);
                                    gameObject.position.X = 500;
                                    gameObject.position.Y = ypos2;
                                    firstGun[i].visible = false;
                                    numSpawn += 1;
                                    gameScore += (numSpawn * 10);
                                }
                            }
                        }
                    }
                     foreach (Bullets gameObject in alienShot)
                     {
                         for (int i = 0; i < secondGun.Length; i++)
                         {
                             if (gameObject.visible && secondGun[i].visible)
                             {
                                 if (secondGun[i].checkCollideWith(gameObject))
                                 {
                                     ypos2 = rnd.Next(100, 450);
                                     gameObject.position.X = 500;
                                     gameObject.position.Y = ypos2;
                                     secondGun[i].visible = false;
                                     numSpawn += 1;
                                     gameScore += (numSpawn * 10);
                                 }
                             }
                         }
                     }
                    timer += gameTime.ElapsedGameTime.TotalSeconds;
                    timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
                    spawnCounter = (int)timeCounter;
                    if(spawnCounter>3)
                    {
                        for(int i = 0; i < alienShot.Length; i++)
                        {
                            
                            yPos = rnd.Next(3, 7);
                            alienShot[i].visible = true;
                            alienShot[i].speed = yPos;
                        }
                    }
                    foreach (Bullets gameObject in alienShot)
                    {
                        ypos2 = rnd.Next(100, 450);
                        if (gameObject.visible)
                            gameObject.update(gameTime);
                        if (gameObject.position.X < 0)
                        {
                            gameObject.position = new Vector2(475, ypos2);
                            gameObject.position.X = 850;
                        }
                    }
                    foreach (Bullets gameObject in alienShot)
                    {
                        if (girl1.visible && gameObject.visible)
                        {
                            if (girl1.checkCollideWith(gameObject))
                            {
                                gameObject.visible = false;
                                lives1 -= 1;
                            }
                        }
                    }
                    foreach (Bullets gameObject in alienShot)
                    {
                        if (girl2.visible && gameObject.visible)
                        {
                            if (girl2.checkCollideWith(gameObject))
                            {
                                gameObject.visible = false;
                                lives2 -= 1;
                            }
                        }
                    }
                    if(girl1.checkCollideWith(boss))
                    {
                        lives1--;
                    }
                    if(girl2.checkCollideWith(boss))
                    {
                        lives2--;
                    }
                     if (lives1 <= 0)
                    {
                        girl1.visible = false;
                        girl1.position.X = 0;
                        girl1.position.Y = 0;
                    }
                    if (lives2 <= 0)
                    {
                        girl2.visible = false;
                        girl2.position.X = 0;
                        girl2.position.Y = 0;
                    }
                    
                    if (!girl1.visible && !girl2.visible)
                    {
                        currentGameState = GameState.GAMEOVER;
                    }
                    if (bossLife == 0)
                    {
                        girl1.position = new Vector2(70, 418);
                        girl2.position = new Vector2(25, 418);
                        girl1.Scale = 3;
                        girl2.Scale = 3;
                        currentGameState = GameState.WON;
                    }
                    break;
                case GameState.WON:
                    if (girl1.position.X < 600)
                        girl1.position.X += girl1.speed;
                    if (girl1.position.X == 600)
                    {
                        girl1.currentframe.X = 0;
                        girl1.currentframe.Y = 0;
                    }
                    if (girl2.position.X < 550)
                        girl2.position.X += girl2.speed;
                    if (girl2.position.X == 550)
                    {
                        girl2.currentframe.X = 0;
                        girl2.currentframe.Y = 0;
                    }
                    break;
                case GameState.GAMEOVER:
                    break;
            }

            // TODO: Add your update logic here
#endif
            base.Update(gameTime);
        }
        public void backgroundMotion(GameTime gameTime)
        {
            if (scrollBack1.Position.X < -scrollBack1.Size.Width)
            {
                scrollBack1.Position.X = scrollBack3.Position.X + scrollBack3.Size.Width;
            }

            if (scrollBack2.Position.X < -scrollBack2.Size.Width)
            {
                scrollBack2.Position.X = scrollBack1.Position.X + scrollBack1.Size.Width;
            }

            if (scrollBack3.Position.X < -scrollBack3.Size.Width)
            {
                scrollBack3.Position.X = scrollBack2.Position.X + scrollBack2.Size.Width;
            }
            Vector2 aDirection = new Vector2(-1, 0);
            Vector2 aSpeed = new Vector2(100, 0);
            scrollBack1.Position += aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            scrollBack2.Position += aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            scrollBack3.Position += aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public void backgroundMotionl2(GameTime gameTime)
        {
            if (space1.Position.X < -space1.Size.Width)
            {
                space1.Position.X = space3.Position.X + space3.Size.Width;
            }

            if (space2.Position.X < -space2.Size.Width)
            {
                space2.Position.X = space1.Position.X + space1.Size.Width;
            }

            if (space3.Position.X < -space3.Size.Width)
            {
                space3.Position.X = space2.Position.X + space2.Size.Width;
            }
            Vector2 aDirection = new Vector2(-1, 0);
            Vector2 aSpeed = new Vector2(100, 0);
            space1.Position += aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            space2.Position += aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            space3.Position += aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public void playerControls(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0)
            {
                girl1.currentframe.Y = 1;
                ++girl1.currentframe.X;
                if (girl1.currentframe.X >= girl1.sheetsize.X)
                    girl1.currentframe.X = 0;
                girl1.position.X -= girl1.speed;
                direction = false;
            }
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0)
            {
                girl1.currentframe.Y = 2;
                ++girl1.currentframe.X;
                if (girl1.currentframe.X >= girl1.sheetsize.X)
                    girl1.currentframe.X = 0;
                girl1.position.X += girl1.speed;
                direction = true;
            }
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0)
            {
                girl1.currentframe.Y = 3;
                ++girl1.currentframe.X;
                if (girl1.currentframe.X >= girl1.sheetsize.X)
                    girl1.currentframe.X = 0;
                girl1.position.Y -= girl1.speed;
            }
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0)
            {
                girl1.currentframe.Y = 0;
                ++girl1.currentframe.X;
                if (girl1.currentframe.X >= girl1.sheetsize.X)
                    girl1.currentframe.X = 0;
                girl1.position.Y += girl1.speed;
            }
            //player.position.Y -= GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y * player.speed;
            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
            {
                if (canFire)
                {
                    ShotUpdate = TimeSpan.Zero;
                    fire1();
                    canFire = false;
                }
            }
            if (GamePad.GetState(PlayerIndex.Two).ThumbSticks.Left.X < 0)
            {
                girl2.currentframe.Y = 1;
                ++girl2.currentframe.X;
                if (girl2.currentframe.X >= girl2.sheetsize.X)
                    girl2.currentframe.X = 0;
                girl2.position.X -= girl2.speed;
                direction2 = false;
            }
            if (GamePad.GetState(PlayerIndex.Two).ThumbSticks.Left.X > 0)
            {
                girl2.currentframe.Y = 2;
                ++girl2.currentframe.X;
                if (girl2.currentframe.X >= girl2.sheetsize.X)
                    girl2.currentframe.X = 0;
                girl2.position.X += girl2.speed;
                direction2 = true;
            }
            if (GamePad.GetState(PlayerIndex.Two).ThumbSticks.Left.Y > 0)
            {
                girl2.currentframe.Y = 3;
                ++girl2.currentframe.X;
                if (girl2.currentframe.X >= girl2.sheetsize.X)
                    girl2.currentframe.X = 0;
                girl2.position.Y -= girl2.speed;
            }
            if (GamePad.GetState(PlayerIndex.Two).ThumbSticks.Left.Y < 0)
            {
                girl2.currentframe.Y = 0;
                ++girl2.currentframe.X;
                if (girl2.currentframe.X >= girl2.sheetsize.X)
                    girl2.currentframe.X = 0;
                girl2.position.Y += girl2.speed;
            }
            if (GamePad.GetState(PlayerIndex.Two).Buttons.A == ButtonState.Pressed)
            {
                if (canFire)
                {
                    ShotUpdate = TimeSpan.Zero;
                    fire2();
                    canFire = false;
                }
            }

#if!XBOX
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                girl1.currentframe.Y = 1;
                ++girl1.currentframe.X;
                if (girl1.currentframe.X >= girl1.sheetsize.X)
                    girl1.currentframe.X = 0;
                girl1.position.X -= girl1.speed;
                direction = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                girl1.currentframe.Y = 2;
                ++girl1.currentframe.X;
                if (girl1.currentframe.X >= girl1.sheetsize.X)
                    girl1.currentframe.X = 0;
                girl1.position.X += girl1.speed;
                direction = true;

            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                girl1.currentframe.Y = 3;
                ++girl1.currentframe.X;
                if (girl1.currentframe.X >= girl1.sheetsize.X)
                    girl1.currentframe.X = 0;
                girl1.position.Y -= girl1.speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                girl1.currentframe.Y = 0;
                ++girl1.currentframe.X;
                if (girl1.currentframe.X >= girl1.sheetsize.X)
                    girl1.currentframe.X = 0;
                girl1.position.Y += girl1.speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.RightAlt))
            {
                if (canFire)
                {
                    ShotUpdate = TimeSpan.Zero;
                    fire1();
                    canFire = false;
                }
            }
            ShotUpdate += gameTime.ElapsedGameTime;
            if (ShotUpdate > TimeSpan.FromMilliseconds(ShotFreq))
            {
                //fire();
                canFire = true;
                ShotUpdate = TimeSpan.Zero;
            }
            updateBullets();
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                girl2.currentframe.Y = 1;
                ++girl2.currentframe.X;
                if (girl2.currentframe.X >= girl2.sheetsize.X)
                    girl2.currentframe.X = 0;
                girl2.position.X -= girl2.speed;
                direction2 = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                girl2.currentframe.Y = 2;
                ++girl2.currentframe.X;
                if (girl2.currentframe.X >= girl2.sheetsize.X)
                    girl2.currentframe.X = 0;
                girl2.position.X += girl2.speed;
                direction2 = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                girl2.currentframe.Y = 3;
                ++girl2.currentframe.X;
                if (girl2.currentframe.X >= girl2.sheetsize.X)
                    girl2.currentframe.X = 0;
                girl2.position.Y -= girl2.speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                girl2.currentframe.Y = 0;
                ++girl2.currentframe.X;
                if (girl2.currentframe.X >= girl2.sheetsize.X)
                    girl2.currentframe.X = 0;
                girl2.position.Y += girl2.speed;
            }
            if(Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                if (canFire)
                {
                    ShotUpdate = TimeSpan.Zero;
                    fire2();
                    canFire = false;
                }
            }
            updateBullets2();
            girl1.position.X = MathHelper.Clamp(girl1.position.X, 0 + (girl1.SourceRectangle.Width / 2)
                        , 800 - (girl1.SourceRectangle.Width / 2));
            girl1.position.Y = MathHelper.Clamp(girl1.position.Y, 0 + (girl1.SourceRectangle.Height / 2)
                , 480 - (girl1.SourceRectangle.Height / 2));
            girl2.position.X = MathHelper.Clamp(girl2.position.X, 0 + (girl2.SourceRectangle.Width / 2)
                        , 800 - (girl2.SourceRectangle.Width / 2));
            girl2.position.Y = MathHelper.Clamp(girl2.position.Y, 0 + (girl2.SourceRectangle.Height / 2)
                , 480 - (girl2.SourceRectangle.Height / 2));
#endif
        }
        public void spawn(int a)
        {
            enemy[a].visible = true;
        }
        public void fire1()
        {
            foreach (Bullets gameObject in firstGun)
            {
                if (!gameObject.alive)
                {
                    if (direction)
                    {
                        //Right
                        gameObject.moveInDirection.X = gameObject.speed;
                        gameObject.moveInDirection.Y = 0;
                    }
                    else
                    {
                        //Left
                        gameObject.spriteEffects = SpriteEffects.FlipHorizontally;
                        gameObject.moveInDirection.X = -gameObject.speed;
                        gameObject.moveInDirection.Y = 0;
                    }
                    gameObject.position = girl1.position;
                    gameObject.alive = true;
                    gameObject.visible = true;
                    firstgun.Play();
                    return;
                }

            }
        }
        public void fire2()
        {
            foreach (Bullets gameObject in secondGun)
            {
                if (!gameObject.alive)
                {
                    if (direction2)
                    {
                        //Right
                        gameObject.moveInDirection.X = gameObject.speed;
                        gameObject.moveInDirection.Y = 0;
                    }
                    else
                    {
                        //Left
                        gameObject.spriteEffects = SpriteEffects.FlipHorizontally;
                        gameObject.moveInDirection.X = -gameObject.speed;
                        gameObject.moveInDirection.Y = 0;
                    }
                    gameObject.position = girl2.position;
                    gameObject.alive = true;
                    gameObject.visible = true;
                    firstgun.Play();
                    return;
                }

            }

        }
        private void updateBullets()
        {
            foreach (Bullets gameObject in firstGun)
            {
                if (gameObject.alive)
                {
                    gameObject.position += gameObject.moveInDirection;
                    if (gameObject.position.X < 0 || gameObject.position.X > 800)
                    {
                        gameObject.alive = false;
                        gameObject.visible = false;
                    }
                }
            }
        }
        private void updateBullets2()
        {
            foreach (Bullets gameObject in secondGun)
            {
                if (gameObject.alive)
                {
                    gameObject.position += gameObject.moveInDirection;
                    if (gameObject.position.X < 0 || gameObject.position.X > 800)
                    {
                        gameObject.alive = false;
                        gameObject.visible = false;
                    }
                }
            }
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            
            switch(currentGameState)
            {
                case GameState.MENU:
                    spriteBatch.Draw(menuBack, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height),
                        null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                    //spriteBatch.DrawString(font1, "Press Enter to Start", new Vector2(300, 300), Color.White);
                    break;
                case GameState.Level1:
                    scrollBack1.Draw(this.spriteBatch);
                    scrollBack2.Draw(this.spriteBatch);
                    scrollBack3.Draw(this.spriteBatch);
                    foreach (Enemies gameObject in enemy)
                    {
                        gameObject.draw(spriteBatch);
                    }
                    spriteBatch.DrawString(font1, "Score: " + gameScore, new Vector2(10, 10), Color.White);
                    spriteBatch.DrawString(font1, "Time: " + (int)timer, new Vector2(700, 10), Color.White);
                    spriteBatch.DrawString(font1, "P1 Lives: " + lives1, new Vector2(10, 450), Color.White);
                    spriteBatch.DrawString(font1, "P2 Lives: " + lives2, new Vector2(150, 450), Color.White);
                    break;
                case GameState.Level2:
                    space1.Draw(this.spriteBatch);
                    space2.Draw(this.spriteBatch);
                    space3.Draw(this.spriteBatch);
                    foreach (Enemies gameObject in alien)
                    {
                        gameObject.draw(spriteBatch);
                    }
                    spriteBatch.DrawString(font1, "Score: " + gameScore, new Vector2(10, 10), Color.White);
                    spriteBatch.DrawString(font1, "Time: " + (int)timer, new Vector2(700, 10), Color.White);
                    spriteBatch.DrawString(font1, "P1 Lives: " + lives1, new Vector2(10, 450), Color.White);
                    spriteBatch.DrawString(font1, "P2 Lives: " + lives2, new Vector2(150, 450), Color.White);
                    break;
                case GameState.Level3:
                    spriteBatch.Draw(bossBack, new Rectangle(0, 0, (Window.ClientBounds.Width+50), (Window.ClientBounds.Height+50)),
                        null, Color.White, 0, new Vector2(10,10), SpriteEffects.None, 0);
                    spriteBatch.Draw(bossHealth, new Rectangle(this.Window.ClientBounds.Width / 2 - bossHealth.Width / 2,
                        30, bossHealth.Width, 44), new Rectangle(0, 45, bossHealth.Width, 44), Color.Gray);
                    spriteBatch.Draw(bossHealth, new Rectangle(this.Window.ClientBounds.Width / 2 - bossHealth.Width / 2,
                        30, (int)(bossHealth.Width * ((double)bossLife / 100)), 44),
                        new Rectangle(0, 45, bossHealth.Width, 44), Color.Red);
                    spriteBatch.Draw(bossHealth, new Rectangle(this.Window.ClientBounds.Width / 2 - bossHealth.Width / 2,
                        30, bossHealth.Width, 44), new Rectangle(0, 0, bossHealth.Width, 44), Color.White);
                    foreach (Bullets gameObject in alienShot)
                    {
                        gameObject.draw(spriteBatch);
                    }

                    boss.draw(spriteBatch);
                    spriteBatch.DrawString(font1, "Score: " + gameScore, new Vector2(10, 10), Color.White);
                    spriteBatch.DrawString(font1, "Time: " + (int)timer, new Vector2(700, 10), Color.White);
                    spriteBatch.DrawString(font1, "P1 Lives: " + lives1, new Vector2(10, 450), Color.White);
                    spriteBatch.DrawString(font1, "P2 Lives: " + lives2, new Vector2(150, 450), Color.White);
                    break;
                case GameState.WON:
                    spriteBatch.Draw(winBack, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height),
                        null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                    spriteBatch.DrawString(font2, "YOU WON", new Vector2(300, 150), Color.Red);
                    spriteBatch.DrawString(font2, "Score: " + gameScore, new Vector2(250, 200), Color.Red);
                    break;
                case GameState.GAMEOVER:
                    spriteBatch.Draw(gameOver, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height),
                        null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                    break;


            }
            foreach (Bullets gameObject in firstGun)
            {
                gameObject.draw(spriteBatch);
            }
            foreach (Bullets gameObject in secondGun)
            {
                gameObject.draw(spriteBatch);
            }
            
            girl1.draw(spriteBatch);
            girl2.draw(spriteBatch);
            // TODO: Add your drawing code here
            
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
