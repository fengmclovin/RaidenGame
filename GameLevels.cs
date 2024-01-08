using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// helper class for global constants such as directory 
    /// </summary>
    class Dir
    {
        public static string dir = @"/Users/fengtian/Projects/Week9/Week9";
    }

   
    // -------------------------------------------------------- Game Level 0 Splash Screen ----------------------------------------------------------------------------------
    class GameLevel_0_Intro : RC_GameStateParent
    {
        RC_RenderableList gta = new RC_RenderableList();
        Texture2D texG;
        RC_Renderable vc = null;

        SoundEffect vicecity;
        SoundEffectInstance vcradio;

        SoundEffect music;
        LimitSound limSound;

        Texture2D cplogo;
        Sprite3 logo;

        Texture2D Particle2;
        Texture2D tex;

        Random random;
        int ticks = 0;
        ParticleSystem p;
        int tix = 0;
        Color screenColour = Color.Tan;

        Vector2 pTarget = new Vector2(0, 0);

        WayPointList wl = null;
        Rectangle rectangle = new Rectangle(0, 0, 800, 600);

        bool showlogo = false;
        public override void LoadContent()
        {
            font1 = Content.Load<SpriteFont>("spritefont1");
            texG = Util.texFromFile(graphicsDevice, Dir.dir + "cpintro.png");
            vc = new ScrollBackGround(texG, new Rectangle(0, 0, 3800, 2700), new Rectangle(0, 0, 800, 600), 1, 1);
            gta.addReuse(vc);

            vicecity = Content.Load<SoundEffect>("starwar");
            vcradio = vicecity.CreateInstance();

            music = Content.Load<SoundEffect>("police");
            limSound = new LimitSound(music, 3);

            cplogo = Content.Load<Texture2D>("Img/cplogo");
            logo = new Sprite3(true, cplogo, 1520, 440);
            logo.setPos(150, 158);
            logo.setWidthHeight(506, 147);

            random = new Random();

            font1 = Content.Load<SpriteFont>("spritefont1");

            Particle2 = Util.texFromFile(graphicsDevice, Dir.dir + "Particle2.png");
            tex = Particle2;

            setSys1();
        }

        public override void Update(GameTime gameTime)
        {
            ticks++;
            getKeyboardAndMouse();

            if (keyState.IsKeyDown(Keys.O) && !prevKeyState.IsKeyDown(Keys.O))
            {
                tex = Particle2;
                p.tex = tex;
                p.reset();
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.P)
                  && prevKeyState.IsKeyUp(Keys.P))
            {
                showlogo = !showlogo;
            }

            if (keyState.IsKeyDown(Keys.U))
            {
                vicecity.Play();
                
            }

            // the s key plays it if its not playing 
            if (keyState.IsKeyDown(Keys.S))
            {
                if (vcradio.State != SoundState.Playing) vcradio.Play();
            };

            // the space key plays it any way but only once
            if (keyState.IsKeyDown(Keys.Space) && prevKeyState.IsKeyUp(Keys.Space))
            {
                vcradio.Stop(); // this seems to take a while to process
                vcradio.Play();
            };

            if (keyState.IsKeyDown(Keys.B))
            {
                vcradio.Stop();
                vcradio.Play();
            };

            if (keyState.IsKeyDown(Keys.M) && prevKeyState.IsKeyUp(Keys.M))
            {
                limSound.playSound();
            }

            // the n key plays it if there is a spare slot limiting it to 3
            if (keyState.IsKeyDown(Keys.P) && prevKeyState.IsKeyUp(Keys.P))
            {
                limSound.playSoundIfOk();
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.N) && !Game1.prevKeyState.IsKeyDown(Keys.N))
            {
                gameStateManager.setLevel(1);
            }

            limSound.Update(gameTime);
            p.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Aqua);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            spriteBatch.DrawString(font1, "level 0 - press n to go to next level", new Vector2(100, 100), Color.Brown);
            gta.Draw(spriteBatch);

            p.Draw(spriteBatch);
            LineBatch.drawCross(spriteBatch, mouse_x, mouse_y, 4, Color.Black, Color.Black);
            LineBatch.drawCrossX(spriteBatch, pTarget.X, pTarget.Y, 4, Color.Gray, Color.Gray);
            if (wl != null) { wl.Draw(spriteBatch, Color.Teal, Color.Red); }
            LineBatch.drawLineRectangle(spriteBatch, rectangle, Color.Gray);

            if (showlogo)
            {

                logo.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        void setSys1()
        {
            p = new ParticleSystem(new Vector2(300, 100), 40, 600, 102);
            p.setMandatory1(tex, new Vector2(6, 6), new Vector2(24, 24), Color.White, new Color(255, 255, 255, 100));
            p.setMandatory2(-1, 1, 1, 3, 0);
            rectangle = new Rectangle(0, 0, 800, 600);
            p.setMandatory3(120, rectangle);
            p.setMandatory4(new Vector2(0, 0.1f), new Vector2(1, 0), new Vector2(0, 0));
            p.randomDelta = new Vector2(0.1f, 0.1f);
            p.Origin = 1;
            p.originRectangle = new Rectangle(0, 0, 800, 10);
            p.activate();
            wl = null;
        }
    }

    // -------------------------------------------------------- Game Level 1 Game Menu ----------------------------------------------------------------------------------
    class GameLevel_1_Menu : RC_GameStateParent
    {
        RC_RenderableList gtamenu = new RC_RenderableList();
        Texture2D tex5;
        RC_Renderable menu = null;

        Texture2D tex6;
        Random RandomClass;
        Sprite3 cpman;

        public override void LoadContent()
        {
            RandomClass = new Random();

            font1 = Content.Load<SpriteFont>("SpriteFont1");
            tex5 = Util.texFromFile(graphicsDevice, Dir.dir + "cpmenu.png");
            tex6 = Util.texFromFile(graphicsDevice, Dir.dir + "cpman.png");

            menu = new ScrollBackGround(tex5, new Rectangle(0, 0, 3300, 1640), new Rectangle(0, 0, 800, 600), 1, 1);
            gtamenu.addReuse(menu);

            cpman = new Sprite3(true, tex6, 50f, (float)(20 + RandomClass.NextDouble() * 500));
            cpman.setWidthHeight(256, 134);
            cpman.setWidthHeightOfTex(768, 402);
            cpman.setDeltaSpeed(new Vector2((float)(0.5f + RandomClass.NextDouble()), 0));
        }

        public override void Update(GameTime gameTime)
        {
            cpman.moveByDeltaXY();
            cpman.animationTick(gameTime);

            if (Game1.currentKeyState.IsKeyDown(Keys.N) && !Game1.prevKeyState.IsKeyDown(Keys.N))
            {
                gameStateManager.setLevel(2);
            }
            if (Game1.currentKeyState.IsKeyDown(Keys.S) && !Game1.prevKeyState.IsKeyDown(Keys.S))
            {
                gameStateManager.setLevel(2);
            }
            if (Game1.currentKeyState.IsKeyDown(Keys.D1) && !Game1.prevKeyState.IsKeyDown(Keys.D1))
            {
                gameStateManager.setLevel(2);
            }
            if (Game1.currentKeyState.IsKeyDown(Keys.D2) && !Game1.prevKeyState.IsKeyDown(Keys.D2))
            {
                gameStateManager.setLevel(3);
            }
            if (Game1.currentKeyState.IsKeyDown(Keys.D3) && !Game1.prevKeyState.IsKeyDown(Keys.D3))
            {
                gameStateManager.setLevel(4);
            }
            if (Game1.currentKeyState.IsKeyDown(Keys.B) && !Game1.prevKeyState.IsKeyDown(Keys.B))
            {
                gameStateManager.setLevel(0);
            }
            if (Game1.currentKeyState.IsKeyDown(Keys.Q) && !Game1.prevKeyState.IsKeyDown(Keys.Q))
            {
                gameStateManager.pushLevel(5);
            }
            if (Game1.currentKeyState.IsKeyDown(Keys.O) && !Game1.prevKeyState.IsKeyDown(Keys.O))
            {
                cpman.setPos(50f, (float)(20 + RandomClass.NextDouble() * 500));
                cpman.active = true;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.BlanchedAlmond);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            menu.Draw(spriteBatch);
            cpman.draw(spriteBatch);

            spriteBatch.End();
        }
    }

    // -------------------------------------------------------- Game Level 2 MODE 1 Easy ----------------------------------------------------------------------------------

    class GameLevel_2_Easy : RC_GameStateParent
    {
        SpriteList slist1 = null;
        SpriteList slist2 = null;
        SpriteList slist3 = null;

        SpriteList booms = null;

        RC_RenderableList rlist1 = null;

        int screenWidth;
        Rectangle screenRect;

        SoundEffect music;
        LimitSound limSound;

        HealthBarAttached myHp;

        bool showbb = false;// show bounding boxes flag (press B)
        bool showgg = false;
        bool showtimer = false;
        bool bossHit = false;

        double a = 2;
        double b = 0;

        double c;
        double i = 0;

        Color myColor;

        int ticks = 0;
        public int score = 0;

        float angle = 0;

        Texture2D background1;
        Texture2D background2;

        Vector2 bg1Position1;
        Vector2 bg2Position2;
        float speed;

        Texture2D spaceship;
        Sprite3 myship;

        Sprite3 missile;
        Sprite3 enemy1;
        Sprite3 enemy2;
        Sprite3 enemy3;
        Sprite3 cloud4;

        Sprite3 ggg;
        Sprite3 boomboom;

        Texture2D tex1;
        Texture2D tex2;
        Texture2D tex3;
        Texture2D tex4;
        Texture2D tex5;
        Texture2D tex6;
        Texture2D tex7;
        Texture2D tex8;
        Texture2D tex9;
        Texture2D tex10 = null;

        Sprite3 ray;
        Sprite3 star;
        Sprite3 blackhole;
        Sprite3 boss1;

        Sprite3 timer;

        Random rgb = new Random();
        Random RandomClass;

        void createExplosion(int x, int y)
        {
            tex10 = Util.texFromFile(graphicsDevice, Dir.dir + "boom.png");
            booms = new SpriteList();
            int xoffset = -2;
            int yoffset = -20;

            boomboom = new Sprite3(true, tex10, x + xoffset, y + yoffset);
            boomboom.setWidthHeight(128, 128);
           
            boomboom.setXframes(4);
            boomboom.setYframes(4);

            Vector2[] anim1 = new Vector2[16];
            anim1[0].X = 0; anim1[0].Y = 0;
            anim1[1].X = 1; anim1[1].Y = 0;
            anim1[2].X = 2; anim1[2].Y = 0;
            anim1[3].X = 3; anim1[3].Y = 0;
            anim1[4].X = 0; anim1[4].Y = 1;
            anim1[5].X = 1; anim1[5].Y = 1;
            anim1[6].X = 2; anim1[6].Y = 1;
            anim1[7].X = 3; anim1[7].Y = 1;
            anim1[8].X = 0; anim1[8].Y = 2;
            anim1[9].X = 1; anim1[9].Y = 2;
            anim1[10].X = 2; anim1[10].Y = 2;
            anim1[11].X = 3; anim1[11].Y = 2;
            anim1[12].X = 0; anim1[12].Y = 3;
            anim1[13].X = 1; anim1[13].Y = 3;
            anim1[14].X = 2; anim1[14].Y = 3;
            anim1[15].X = 3; anim1[15].Y = 3;
            boomboom.setAnimationSequence(anim1, 0, 15, 1);
            boomboom.setAnimFinished(2);
            boomboom.animationStart();

            booms.addSpriteReuse(boomboom);
        }

        public override void LoadContent()
        {
            music = Content.Load<SoundEffect>("explosionSound");
            limSound = new LimitSound(music, 100);

            RandomClass = new Random();
            slist1 = new SpriteList(99);
            slist2 = new SpriteList(99);
            slist3 = new SpriteList(99);
            booms = new SpriteList();

            rlist1 = new RC_RenderableList();

            bg1Position1 = new Vector2(0, 0);
            bg2Position2 = new Vector2(0, 800);

            speed = 0.3f;

            background1 = Content.Load<Texture2D>("Img/space1");
            background2 = Content.Load<Texture2D>("Img/space2");

            spaceship = Content.Load<Texture2D>("Img/spaceship");
            myship = new Sprite3(true, spaceship, 80, 80);
            myship.setPos(350, 450);
            myship.setWidthHeight(200, 200);
            myship.setWidthHeightOfTex(200, 200);
            myship.setHSoffset(new Vector2(40, 40));
            myship.setBB(0, 0, 80, 80);
            myship.setMoveSpeed(100);

            slist2.addSpriteReuse(myship);

            HealthBarAttached myHp = new HealthBarAttached(Color.Aquamarine, Color.Green, Color.Red, 9, true);
            myHp.offset = new Vector2(0, -1); // one pixel above the bounding box
            myHp.gapOfbar = 2;
            myship.hitPoints = 20;
            myship.maxHitPoints = 20;
            myship.attachedRenderable = myHp;

            tex1 = Content.Load<Texture2D>("Img/boss1");
            tex2 = Content.Load<Texture2D>("Img/mis");
            tex3 = Content.Load<Texture2D>("Img/ray");
            tex4 = Content.Load<Texture2D>("Img/star");
            tex5 = Content.Load<Texture2D>("Img/blackhole");
            tex6 = Content.Load<Texture2D>("Img/enemy1");
            tex7 = Content.Load<Texture2D>("Img/enemy2");
            tex8 = Content.Load<Texture2D>("Img/bling");   

            boss1 = new Sprite3(true, tex1, 1f, (float)(4 + RandomClass.NextDouble() * 50));
            boss1.setPos(400, -100);
            boss1.setWidthHeight(80, 80);
            boss1.setWidthHeightOfTex(182, 250);
            boss1.setHSoffset(new Vector2(40, 40));
            boss1.setMoveAngleDegrees(90);
            boss1.setMoveSpeed(0.5f);

            HealthBarAttached bHp1 = new HealthBarAttached(Color.Aquamarine, Color.Green, Color.Red, 9, true);
            bHp1.offset = new Vector2(0, -1); // one pixel above the bounding box
            bHp1.gapOfbar = 2;
            boss1.hitPoints = 20;
            boss1.maxHitPoints = 20;
            boss1.attachedRenderable = bHp1;

            missile = new Sprite3(true, tex2, 1f, (float)(500 + RandomClass.NextDouble() * 50));
            missile.setPos(1000, -100);
            missile.setWidthHeight(100, 100);
            missile.setHSoffset(new Vector2(50, 50));
            missile.setMoveAngleDegrees(135);
            missile.setDisplayAngleDegrees(-270);
            missile.setMoveSpeed(2f);
            missile.setBBFractionOfTexCentered(0.8f);

            slist1.addSpriteReuse(missile);

            star = new Sprite3(true, tex4, 80, 73);
            star.setPos(0, 0);
            star.setWidthHeight(27, 27);
            star.setBBFractionOfTexCentered(0.7f);
            star.setMoveAngleDegrees(RandomClass.Next());
            star.setMoveSpeed(7f);
            slist1.addSpriteReuse(star);

            blackhole = new Sprite3(true, tex5, 600, 300);
            blackhole.setWidthHeight(160, 148);
            blackhole.setBBFractionOfTexCentered(0.7f);
            blackhole.setMoveAngleDegrees(45);
            blackhole.setMoveSpeed(0.3f);
            blackhole.setPos(0, 0);

            ray = new Sprite3(true, tex3, 306, 190);
            ray.setWidthHeight(13, 50);
            ray.setMoveAngleDegrees(90);
            ray.setMoveSpeed(-10f);
            ray.setBBToWH();
            ray.setDisplayAngleDegrees(0);
            ray.setPos(myship.getPosX() - 6, myship.getPosY() - 18);
            ray.setColor(Color.GhostWhite);

            slist2.addSpriteReuse(ray);

            enemy1 = new Sprite3(true, tex6, 3f, (float)(10 + RandomClass.NextDouble() * 50));
            enemy1.setWidthHeight(80, 80);
            enemy1.setWidthHeightOfTex(72, 128);
            enemy1.setBBFractionOfTexCentered(0.5f);
            enemy1.setMoveAngleDegrees(90);
            enemy1.setDisplayAngleDegrees(0);
            enemy1.setMoveSpeed(0.7f);
            enemy1.setPos(300, 0);
            enemy1.setColor(Color.GhostWhite);

            slist1.addSpriteReuse(enemy1);

            enemy2 = new Sprite3(true, tex7, 3f, (float)(100 + RandomClass.NextDouble() * 50));
            enemy2.setWidthHeight(80, 80);
            enemy2.setWidthHeightOfTex(72, 128);
            enemy2.setBBFractionOfTexCentered(0.5f);
            enemy2.setMoveAngleDegrees(90);
            enemy2.setDisplayAngleDegrees(0);
            enemy2.setMoveSpeed(1f);
            enemy2.setPos(200, 0);
            enemy2.setColor(Color.GhostWhite);

            slist1.addSpriteReuse(enemy2);

            enemy3 = new Sprite3(true, tex7, 1f, (float)(4 + RandomClass.NextDouble() * 50));
            enemy3.setWidthHeight(80, 80);
            enemy3.setWidthHeightOfTex(72, 128);
            enemy3.setWidthHeight(80, 80);
            enemy3.setWidthHeightOfTex(72, 128);
            enemy3.setBBFractionOfTexCentered(0.5f);
            enemy3.setMoveAngleDegrees(90);
            enemy3.setDisplayAngleDegrees(0);
            enemy3.setMoveSpeed(1f);
            enemy3.setPos(400, 0);
            enemy3.setColor(Color.GhostWhite);

            slist1.addSpriteReuse(enemy3);

            tex9 = Content.Load<Texture2D>("Img/gg");
            ggg = new Sprite3(true, tex9, 800, 240);
            ggg.setPos(240, 70);
            ggg.setWidthHeight(300, 300);

            AttachedText t = new AttachedText(Color.Red, font1);
            t = new AttachedText(Color.Red, font1, new Vector2(-30, 52), " ");
            star.attachedRenderable = t;

            timer = new Sprite3(true, tex8, 1f, (float)(4 + RandomClass.NextDouble() * 50));
            timer.setPos(10, 10);
            timer.setWidthHeight(135, 135);
            timer.setWidthHeightOfTex(1024, 384);
            timer.setXframes(8);
            timer.setYframes(3);
            timer.setHSoffset(new Vector2(64, 64));
            timer.setBB(38, 38, 58, 58);
            timer.setMoveAngleDegrees(40);
            timer.setMoveSpeed(1);

            Vector2[] anim0 = new Vector2[10];
            anim0[0].X = 0; anim0[0].Y = 1;
            anim0[1].X = 1; anim0[1].Y = 1;
            anim0[2].X = 2; anim0[2].Y = 1;
            anim0[3].X = 3; anim0[3].Y = 1;
            anim0[4].X = 4; anim0[4].Y = 1;
            anim0[5].X = 5; anim0[5].Y = 1;
            anim0[6].X = 6; anim0[6].Y = 1;
            anim0[7].X = 7; anim0[7].Y = 1;
            anim0[8].X = 8; anim0[8].Y = 1;
            anim0[9].X = 1; anim0[8].Y = 1;
            timer.setAnimationSequence(anim0, 0, 9, 6);
            timer.animationStart();

        }

        public override void Update(GameTime gameTime)
        {
            getKeyboardAndMouse();

            booms.animationTick(gameTime);

            blackhole.moveByAngleSpeed();
            missile.moveByAngleSpeed();
            star.moveByAngleSpeed();
            boss1.moveByAngleSpeed();
            timer.moveByAngleSpeed();
            ray.moveByAngleSpeed();
            enemy1.moveByAngleSpeed();
            enemy2.moveByAngleSpeed();
            enemy3.moveByAngleSpeed();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                gameStateManager.setLevel(4);

            KeyboardState state = Keyboard.GetState();

            if (Game1.currentKeyState.IsKeyDown(Keys.Escape))
                gameStateManager.setLevel(0);

            if (bg1Position1.Y > background1.Height)
            {
                bg1Position1.Y = bg2Position2.Y - 800;
            }
            if (bg2Position2.Y > background2.Height)
            {
                bg2Position2.Y = bg1Position1.Y - 800;
            }
            bg1Position1.Y += speed;
            bg2Position2.Y += speed;

            if (Game1.currentKeyState.IsKeyDown(Keys.Up))
            {
                if (myship.getPosY() < 0)
                {
                    myship.setDeltaSpeed(new Vector2(0, 0));
                }
                else
                {
                    myship.setDeltaSpeed(new Vector2(0, -5));
                }
                myship.moveByDeltaXY();
            }
            if (Game1.currentKeyState.IsKeyDown(Keys.Down))
            {
                if (myship.getPosY() > 540)
                {
                    myship.setDeltaSpeed(new Vector2(0, 0));
                }
                else
                {
                    myship.setDeltaSpeed(new Vector2(0, 5));
                }
                myship.moveByDeltaXY();
            }
            if (Game1.currentKeyState.IsKeyDown(Keys.Right))
            {
                if (myship.getPosX() < 0)
                {
                    myship.setDeltaSpeed(new Vector2(0, 0));
                }
                else
                {
                    myship.setDeltaSpeed(new Vector2(5, 0));
                }
                myship.moveByDeltaXY();
            }
            if (Game1.currentKeyState.IsKeyDown(Keys.Left))
            {
                if (myship.getPosX() > 0)
                {
                    myship.setDeltaSpeed(new Vector2(-5, 0));
                }
                myship.moveByDeltaXY();
            }

            Rectangle BoundBoss1 = boss1.getBoundingBoxAA();
            Rectangle BoundShip = myship.getBoundingBoxAA();
            Rectangle BoundHole = blackhole.getBoundingBoxAA();
            Rectangle BoundMis = missile.getBoundingBoxAA();
            Rectangle BoundRay = ray.getBoundingBoxAA();
            Rectangle BoundT = timer.getBoundingBoxAA();
            Rectangle BoundStar = star.getBoundingBoxAA();

            slist1.moveDeltaXY();
            slist1.animationTick(gameTime);
            // collision test
            for (int i = 0; i < slist2.count(); i++)
            {
                Sprite3 s = slist2.getSprite(i);
                if (s == null) continue;
                if (!s.active) continue;
                if (!s.visible) continue;
                int colision = slist1.collisionAA(s);
                if (colision == -1) continue;
                // we get here if we collided
                //make the guy inactive
                Sprite3 c = slist1.getSprite(colision);
                c.setActive(false);
            }
        

            if (Game1.currentKeyState.IsKeyDown(Keys.V)
                && prevKeyState.IsKeyUp(Keys.V))
            {
                showbb = !showbb;
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.G)
                  && prevKeyState.IsKeyUp(Keys.G))
            {
                showgg = !showgg;
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.H) && !Game1.prevKeyState.IsKeyDown(Keys.H))
            {
                myship.hitPoints = myship.hitPoints - 3;
                if (myship.hitPoints <= 0) myship.active = false;
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.J) && !Game1.prevKeyState.IsKeyDown(Keys.J))
            {
                myship.hitPoints = myship.hitPoints + 3;
                if (myship.hitPoints <= 0) myship.active = false;
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.B) && !Game1.prevKeyState.IsKeyDown(Keys.B))
            {
                gameStateManager.setLevel(1);
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.R) && !Game1.prevKeyState.IsKeyDown(Keys.R))
            {
                myship.setPos(50f, (float)(200 + RandomClass.NextDouble() * 500));
                myship.hitPoints = 10;
                myship.active = true;
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.N) && !Game1.prevKeyState.IsKeyDown(Keys.N))
            {
                gameStateManager.setLevel(3);
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.F1) && !Game1.prevKeyState.IsKeyDown(Keys.F1))
            {
                gameStateManager.pushLevel(5);
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                gameStateManager.setLevel(4);

            if (Game1.currentKeyState.IsKeyDown(Keys.Space) && !Game1.prevKeyState.IsKeyDown(Keys.Space))
                ray.setPos((float)(myship.getPosX() - 6), (float)(myship.getPosY() - 18));
                ray.active = true;

            if (Game1.currentKeyState.IsKeyDown(Keys.C) && !Game1.prevKeyState.IsKeyDown(Keys.C))
                star.setPos(boss1.getPosX(), boss1.getPosY());
                star.active = true;

            if (Game1.currentKeyState.IsKeyDown(Keys.Space) && !Game1.prevKeyState.IsKeyDown(Keys.Space))
                limSound.playSound();

            if (BoundStar.Intersects(BoundShip))
            {
                myship.hitPoints = myship.hitPoints - 1;
            }

            if (BoundT.Intersects(BoundShip))
            {
                score = score + 1;              //increase score
            }

            if (!bossHit && BoundRay.Intersects(BoundBoss1))
            {
                bossHit = true;
                boss1.hitPoints -= 2;
                createExplosion((int)boss1.getPosX(), (int)boss1.getPosY());
                score = score + 10;              //increase score
            }

            else
            {
                bossHit = false;    //person has moved outsite hit rectangle, reset
            }

            if (boss1.hitPoints <= 0)
            {
                boss1.setActive(false);
            }

            if (BoundShip.Intersects(missile.getBoundingBoxAA()))

            {
                myColor = new Color(60, 180, 60);

                //myship.hitPoints = (int)(a + b);


                if (BoundBoss1.Intersects(BoundShip))
                {
                    createExplosion((int)boss1.getPosX(), (int)boss1.getPosY());
                    boss1.hitPoints -= 2;
                    if ((a + b) >= 2)
                    {
                        myship.hitPoints = 2;
                    }
                    else
                    {
                        myship.hitPoints = (int)(a + b);
                    }

                }

            }
            else
            {
                ticks = ticks + 1;
                if (ticks % 30 == 0)
                {
                    myColor = new Color(rgb.Next(0, 255), rgb.Next(0, 255), rgb.Next(0, 255));
                }

                myship.hitPoints = (int)(a + b);

                if ((a + b > 0) && (a + b <= 20))
                {
                    a = a - 0.007;
                    myship.hitPoints = (int)(a + b);
                }

                else
                {
                    myship.hitPoints = 20;
                    blackhole.setMoveSpeed(0);
                    myship.active = true;
                    missile.setMoveSpeed(0);
                    boss1.setMoveSpeed(0);
                    slist1.addSpriteReuse(missile);
                    rlist1.addReuse(boss1);

                }


            }

            showtimer = !showtimer;

            if (boss1.collision(myship))
            {

                TextureFade rr = new TextureFade(tex1, new Rectangle((int)myship.getPosX() + 10, (int)myship.getPosY(), 40, 40),
                                                          new Rectangle((int)myship.getPosX() + 10, (int)myship.getPosY() - 150, 40, 40), Color.White, Color.Black, 100);

                boss1.setActive(false);
                missile.setMoveSpeed(4);
                showtimer = !showtimer;


                if (BoundShip.Intersects(BoundMis))
                {
                    myship.hitPoints = (int)(a + b) / 2;
                }
                else
                {
                    myship.hitPoints = (int)(a + b) / 2;
                }

                slist1.addSpriteReuse(missile);
                rlist1.addReuse(rr);

            }


            if (blackhole.collision(myship))
            {
                showgg = !showgg;

                star.setMoveSpeed(1);
                missile.setMoveSpeed(1);
            }
            if (star.collision(myship))
            {
                showgg = !showgg;

                star.setMoveSpeed(1);
                missile.setMoveSpeed(1);

                if (BoundShip.Intersects(missile.getBoundingBoxAA()))
                {
                    myship.hitPoints = 0;
                }
                else
                {
                    myship.hitPoints = 0;
                }
                slist1.addSpriteReuse(missile);




                if (!screenRect.Intersects(BoundHole))
                {
                    blackhole.setPos(550, 145);  
                }

                if (!screenRect.Intersects(boss1.getBoundingBoxAA()))
                {
                    boss1.setPos(350, 410);
                }

                if (!screenRect.Intersects(ray.getBoundingBoxAA()))
                {
                    ray.setPos(750, 65);
                }

                if (!screenRect.Intersects(enemy1.getBoundingBoxAA()))
                {
                    enemy1.setPos(600, 80);
                }

                if (!screenRect.Intersects(enemy2.getBoundingBoxAA()))
                {
                    enemy2.setPos(650, 49);
                }

                limSound.Update(gameTime);
                rlist1.Update(gameTime);

                slist1.addSpriteReuse(myship);
               
                timer.animationTick(gameTime);
                
                base.Update(gameTime);// all components is called by now

            }
        }


        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.CornflowerBlue);// Clear and fill with colour

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            spriteBatch.Draw(background1, bg1Position1, Color.White);
            spriteBatch.Draw(background2, bg2Position2, Color.White);

            missile.Draw(spriteBatch);
            blackhole.Draw(spriteBatch);
           
            timer.Draw(spriteBatch);
            ray.Draw(spriteBatch);
            star.Draw(spriteBatch);
            myship.Draw(spriteBatch);

            enemy1.Draw(spriteBatch);
            enemy2.Draw(spriteBatch);


            if (showbb)
            {

                blackhole.drawBB(spriteBatch, Color.IndianRed);
                boss1.drawBB(spriteBatch, Color.IndianRed);
                enemy3.drawBB(spriteBatch, Color.IndianRed);
                missile.drawBB(spriteBatch, Color.Red);
                myship.drawBB(spriteBatch, Color.Red);
                star.drawBB(spriteBatch, Color.Red);
                ray.drawBB(spriteBatch, Color.Red);
                timer.drawBB(spriteBatch, Color.Red);
                enemy1.drawBB(spriteBatch, Color.Red);
                enemy2.drawBB(spriteBatch, Color.Red);
            }

            if (showgg)
            {

                ggg.Draw(spriteBatch);
               
            }

            spriteBatch.DrawString(font1, "Score:" + Convert.ToString(score), new Vector2(10, 10), Color.White);

            boss1.Draw(spriteBatch);
            booms.Draw(spriteBatch);
            enemy3.Draw(spriteBatch);

            spriteBatch.End();

        }
    }

    // -------------------------------------------------------- Game Level 3 MODE 2 Medium ----------------------------------------------------------------------------------

    class GameLevel_3_Medium : RC_GameStateParent
    {
        SpriteList slist1 = null;
        SpriteList slist2 = null;
        SpriteList slist3 = null;
        
        SpriteList booms = null;

        RC_RenderableList rlist1 = null;

        int screenWidth;
        int ScreenHeight;
        Rectangle screenRect;

        SoundEffect music;
        LimitSound limSound;

        HealthBarAttached myHp;

        bool showbb = false;// show bounding boxes flag (press B)
        bool showgg = false;
        bool showtimer = false;
        bool bossHit = false;

        bool trump = true;

        double a = 2;
        double b = 0;

        double c;
        double i = 0;

        Color myColor;

        int ticks = 0;
        public int score = 0;

        float angle = 0;

        Texture2D background1;
        Texture2D background2;

        Vector2 bg1Position1;
        Vector2 bg2Position2;
        float speed;

        Texture2D spaceship;
        Sprite3 myship;

        Sprite3 missile;
        Sprite3 enemy1;
        Sprite3 enemy2;
        Sprite3 enemy3;

        Sprite3 ggg;
        Sprite3 boomboom;

        Texture2D tex1;
        Texture2D tex11;
        Texture2D tex2;
        Texture2D tex3;
        Texture2D tex4;
        Texture2D tex5;
        Texture2D tex6;
        Texture2D tex7;
        Texture2D tex8;
        Texture2D tex9;
        Texture2D tex10 = null; 

        Sprite3 ray;
        Sprite3 star;
        Sprite3 blackhole;
        Sprite3 boss1;
        Sprite3 boss2;
        Sprite3 timer;

        Random rgb = new Random();
        Random RandomClass;

        void createExplosion(int x, int y)
        {
            tex10 = Util.texFromFile(graphicsDevice, Dir.dir + "boom.png");
            booms = new SpriteList();
            int xoffset = -2;
            int yoffset = -20;

            boomboom = new Sprite3(true, tex10, x + xoffset, y + yoffset);
            boomboom.setWidthHeight(128, 128);   
            boomboom.setXframes(4);
            boomboom.setYframes(4);

            Vector2[] anim1 = new Vector2[16];
            anim1[0].X = 0; anim1[0].Y = 0;
            anim1[1].X = 1; anim1[1].Y = 0;
            anim1[2].X = 2; anim1[2].Y = 0;
            anim1[3].X = 3; anim1[3].Y = 0;
            anim1[4].X = 0; anim1[4].Y = 1;
            anim1[5].X = 1; anim1[5].Y = 1;
            anim1[6].X = 2; anim1[6].Y = 1;
            anim1[7].X = 3; anim1[7].Y = 1;
            anim1[8].X = 0; anim1[8].Y = 2;
            anim1[9].X = 1; anim1[9].Y = 2;
            anim1[10].X = 2; anim1[10].Y = 2;
            anim1[11].X = 3; anim1[11].Y = 2;
            anim1[12].X = 0; anim1[12].Y = 3;
            anim1[13].X = 1; anim1[13].Y = 3;
            anim1[14].X = 2; anim1[14].Y = 3;
            anim1[15].X = 3; anim1[15].Y = 3;
            boomboom.setAnimationSequence(anim1, 0, 15, 1);
            boomboom.setAnimFinished(2);
            boomboom.animationStart();

            booms.addSpriteReuse(boomboom);
        }

        public override void LoadContent()
        {
            music = Content.Load<SoundEffect>("explosionSound");
            limSound = new LimitSound(music, 100);

            RandomClass = new Random();
            slist1 = new SpriteList(99);
            slist2 = new SpriteList(99);
            slist3 = new SpriteList(99);
            rlist1 = new RC_RenderableList();

            booms = new SpriteList();

            bg1Position1 = new Vector2(0, 0);
            bg2Position2 = new Vector2(0, 800);

            speed = 0.3f;

            background1 = Content.Load<Texture2D>("Img/space3");
            background2 = Content.Load<Texture2D>("Img/space4");

            spaceship = Content.Load<Texture2D>("Img/spaceship");
            myship = new Sprite3(true, spaceship, 80, 80);
            myship.setPos(350, 450);
            myship.setWidthHeight(200, 200);
            myship.setWidthHeightOfTex(200, 200);
            myship.setHSoffset(new Vector2(40, 40));
            myship.setBB(0, 0, 80, 80);
            myship.setMoveSpeed(100);

            slist2.addSpriteReuse(myship);

            HealthBarAttached myHp = new HealthBarAttached(Color.Aquamarine, Color.Green, Color.Red, 9, true);
            myHp.offset = new Vector2(0, -1); // one pixel above the bounding box
            myHp.gapOfbar = 2;
            myship.hitPoints = 20;
            myship.maxHitPoints = 20;
            myship.attachedRenderable = myHp;


            trump = true;

            tex1 = Content.Load<Texture2D>("Img/boss1");
            tex11 = Content.Load<Texture2D>("Img/boss2");
            tex2 = Content.Load<Texture2D>("Img/mis");
            tex3 = Content.Load<Texture2D>("Img/ray");
            tex4 = Content.Load<Texture2D>("Img/star");
            tex5 = Content.Load<Texture2D>("Img/blackhole");
            tex6 = Content.Load<Texture2D>("Img/enemy1");
            tex7 = Content.Load<Texture2D>("Img/enemy2");
            tex8 = Content.Load<Texture2D>("Img/bling");

            boss1 = new Sprite3(true, tex1, 1f, (float)(4 + RandomClass.NextDouble() * 50));
            boss1.setPos(400, -100);
            boss1.setWidthHeight(100, 100);
            boss1.setWidthHeightOfTex(182, 250);
            boss1.setHSoffset(new Vector2(40, 40));
            boss1.setMoveAngleDegrees(90);
            boss1.setMoveSpeed(0.5f);

            boss2 = new Sprite3(true, tex11, 1f, (float)(4 + RandomClass.NextDouble() * 50));
            boss2.setPos(250, -100);
            boss2.setWidthHeight(100, 100);
            boss2.setWidthHeightOfTex(182, 250);
            boss2.setHSoffset(new Vector2(40, 40));
            boss2.setMoveAngleDegrees(90);
            boss2.setMoveSpeed(0.5f);

            HealthBarAttached bHp1 = new HealthBarAttached(Color.Aquamarine, Color.Green, Color.Red, 9, true);
            bHp1.offset = new Vector2(0, -1); // one pixel above the bounding box
            bHp1.gapOfbar = 2;
            boss1.hitPoints = 200;
            boss1.maxHitPoints = 200;
            boss1.attachedRenderable = bHp1;

            HealthBarAttached bHp2 = new HealthBarAttached(Color.Aquamarine, Color.Green, Color.Red, 9, true);
            bHp2.offset = new Vector2(0, -1); // one pixel above the bounding box
            bHp2.gapOfbar = 2;
            boss2.hitPoints = 200;
            boss2.maxHitPoints = 200;
            boss2.attachedRenderable = bHp2;

            missile = new Sprite3(true, tex6, 1f, (float)(500 + RandomClass.NextDouble() * 50));
            missile.setPos(600, 0);
            missile.setWidthHeight(100, 100);
            missile.setHSoffset(new Vector2(50, 50));
            missile.setMoveAngleDegrees(90);
            missile.setDisplayAngleDegrees(0);
            missile.setMoveSpeed(1f);
            missile.setBBFractionOfTexCentered(0.8f);

            slist1.addSpriteReuse(missile);

            star = new Sprite3(true, tex4, 80, 73);
            star.setPos(0, 0);
            star.setWidthHeight(27, 27);
            star.setBBFractionOfTexCentered(0.7f);
            star.setMoveAngleDegrees(90);
            star.setMoveSpeed(7f);
            slist1.addSpriteReuse(star);


            blackhole = new Sprite3(true, tex6, 600, 300);
            blackhole.setWidthHeight(160, 148);
            blackhole.setBBFractionOfTexCentered(0.7f);
            blackhole.setMoveAngleDegrees(45);
            blackhole.setMoveSpeed(0.5f);
            blackhole.setPos(500, 0);
            slist1.addSpriteReuse(blackhole);

            ray = new Sprite3(true, tex3, 306, 190);
            ray.setWidthHeight(13, 50);
            ray.setMoveAngleDegrees(90);
            ray.setMoveSpeed(-15f);
            ray.setBBToWH();
            ray.setDisplayAngleDegrees(0);
            ray.setPos(myship.getPosX() - 6, myship.getPosY() - 18);
            ray.setColor(Color.GhostWhite);

            slist2.addSpriteReuse(ray);

            enemy1 = new Sprite3(true, tex6, 3f, (float)(10 + RandomClass.NextDouble() * 50));
            enemy1.setWidthHeight(80, 80);
            enemy1.setWidthHeightOfTex(72, 128);
            enemy1.setBBFractionOfTexCentered(0.7f);
            enemy1.setMoveAngleDegrees(90);
            enemy1.setDisplayAngleDegrees(0);
            enemy1.setMoveSpeed(0.7f);
            enemy1.setPos(100, 0);
            enemy1.setColor(Color.GhostWhite);

            slist1.addSpriteReuse(enemy1);

            enemy2 = new Sprite3(true, tex6, 3f, (float)(100 + RandomClass.NextDouble() * 50));
            enemy2.setWidthHeight(80, 80);
            enemy2.setWidthHeightOfTex(72, 128);
            enemy2.setBBFractionOfTexCentered(0.7f);
            enemy2.setMoveAngleDegrees(90);
            enemy2.setDisplayAngleDegrees(45);
            enemy2.setMoveSpeed(1f);
            enemy2.setPos(200, 0);
            enemy2.setColor(Color.GhostWhite);

            slist1.addSpriteReuse(enemy2);

            enemy3 = new Sprite3(true, tex7, 1f, (float)(4 + RandomClass.NextDouble() * 50));
            enemy3.setWidthHeight(80, 80);
            enemy3.setWidthHeightOfTex(72, 128);
            enemy3.setWidthHeight(80, 80);
            enemy3.setWidthHeightOfTex(72, 128);
            enemy3.setBBFractionOfTexCentered(0.7f);
            enemy3.setMoveAngleDegrees(90);
            enemy3.setDisplayAngleDegrees(45);
            enemy3.setMoveSpeed(1f);
            enemy3.setPos(300, 0);
            enemy3.setColor(Color.GhostWhite);

            slist1.addSpriteReuse(enemy3);

            tex9 = Content.Load<Texture2D>("Img/gg");
            ggg = new Sprite3(true, tex9, 800, 240);
            ggg.setPos(240, 70);
            ggg.setWidthHeight(300, 300);

            tex10 = Content.Load<Texture2D>("Img/boom");
            boomboom = new Sprite3(true, tex10, 800, 450);
            boomboom.setWidthHeight(160, 90);
            boomboom.setPos(350, 390);

            AttachedText t = new AttachedText(Color.Red, font1);
            t = new AttachedText(Color.Red, font1, new Vector2(-30, 52), " ");
            star.attachedRenderable = t;

            timer = new Sprite3(true, tex7, 1f, (float)(4 + RandomClass.NextDouble() * 50));
            timer.setPos(400, 0);
            timer.setWidthHeight(135, 135);
            timer.setWidthHeightOfTex(1024, 384);
            timer.setXframes(8);
            timer.setYframes(3);
            timer.setHSoffset(new Vector2(64, 64));
            timer.setBB(38, 38, 58, 58);
            timer.setMoveAngleDegrees(40);
            timer.setMoveSpeed(1);

        }

        public override void Update(GameTime gameTime)
        {
            getKeyboardAndMouse();

            booms.animationTick(gameTime);
            blackhole.moveByAngleSpeed();
            missile.moveByAngleSpeed();
            star.moveByAngleSpeed();
            boss1.moveByAngleSpeed();
            boss2.moveByAngleSpeed();
            timer.moveByAngleSpeed();
            ray.moveByAngleSpeed();
            enemy1.moveByAngleSpeed();
            enemy2.moveByAngleSpeed();
            enemy3.moveByAngleSpeed();


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                gameStateManager.setLevel(4);

            KeyboardState state = Keyboard.GetState();

            if (Game1.currentKeyState.IsKeyDown(Keys.Escape))
                gameStateManager.setLevel(0);

            //KeyboardState state = Keyboard.GetState();

            if (bg1Position1.Y > background1.Height)
            {
                bg1Position1.Y = bg2Position2.Y - 800;
            }
            if (bg2Position2.Y > background2.Height)
            {
                bg2Position2.Y = bg1Position1.Y - 800;
            }
            bg1Position1.Y += speed;
            bg2Position2.Y += speed;

            if (Game1.currentKeyState.IsKeyDown(Keys.Up))
            {
                if (myship.getPosY() < 0)
                {
                    myship.setDeltaSpeed(new Vector2(0, 0));
                }
                else
                {
                    myship.setDeltaSpeed(new Vector2(0, -5));
                }
                myship.moveByDeltaXY();
            }
            if (Game1.currentKeyState.IsKeyDown(Keys.Down))
            {
                if (myship.getPosY() > 540)
                {
                    myship.setDeltaSpeed(new Vector2(0, 0));
                }
                else
                {
                    myship.setDeltaSpeed(new Vector2(0, 5));
                }
                myship.moveByDeltaXY();
            }
            if (Game1.currentKeyState.IsKeyDown(Keys.Right))
            {
                if (myship.getPosX() < 0)
                {
                    myship.setDeltaSpeed(new Vector2(0, 0));
                }
                else
                {
                    myship.setDeltaSpeed(new Vector2(5, 0));
                }
                myship.moveByDeltaXY();
            }
            if (Game1.currentKeyState.IsKeyDown(Keys.Left))
            {
                if (myship.getPosX() > 0)
                {
                    myship.setDeltaSpeed(new Vector2(-5, 0));
                }

                myship.moveByDeltaXY();
            }


            Rectangle BoundBoss1 = boss1.getBoundingBoxAA();
            Rectangle BoundBoss2 = boss2.getBoundingBoxAA();
            Rectangle BoundShip = myship.getBoundingBoxAA();
            Rectangle BoundHole = blackhole.getBoundingBoxAA();
            Rectangle BoundMis = missile.getBoundingBoxAA();
            Rectangle BoundRay = ray.getBoundingBoxAA();
            Rectangle BoundT = timer.getBoundingBoxAA();
            Rectangle BoundStar = star.getBoundingBoxAA();

            slist1.moveDeltaXY();
            slist1.animationTick(gameTime);
            // collision test
            for (int i = 0; i < slist2.count(); i++)
            {
                Sprite3 s = slist2.getSprite(i);
                if (s == null) continue;
                if (!s.active) continue;
                if (!s.visible) continue;
                int colision = slist1.collisionAA(s);
                if (colision == -1) continue;
                // we get here if we collided
                //make the guy inactive
                Sprite3 c = slist1.getSprite(colision);
                c.setActive(false);
            }


            timer.animationTick(gameTime);

            if (Game1.currentKeyState.IsKeyDown(Keys.V)
                && prevKeyState.IsKeyUp(Keys.V))
            {
                showbb = !showbb;
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.G)
                  && prevKeyState.IsKeyUp(Keys.G))
            {
                showgg = !showgg;
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.H) && !Game1.prevKeyState.IsKeyDown(Keys.H))
            {
                myship.hitPoints = myship.hitPoints - 3;
                if (myship.hitPoints <= 0) myship.active = false;
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.J) && !Game1.prevKeyState.IsKeyDown(Keys.J))
            {
                myship.hitPoints = myship.hitPoints + 3;
                if (myship.hitPoints <= 0) myship.active = false;
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.B) && !Game1.prevKeyState.IsKeyDown(Keys.B))
            {
                gameStateManager.setLevel(1);
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.R) && !Game1.prevKeyState.IsKeyDown(Keys.R))
            {
                myship.setPos(50f, (float)(200 + RandomClass.NextDouble() * 500));
                myship.hitPoints = 10;
                myship.active = true;
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.N) && !Game1.prevKeyState.IsKeyDown(Keys.N))
            {
                gameStateManager.setLevel(4);
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.F1) && !Game1.prevKeyState.IsKeyDown(Keys.F1))
            {
                gameStateManager.pushLevel(5);
            }

          

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                gameStateManager.setLevel(4);

            if (Game1.currentKeyState.IsKeyDown(Keys.Space) && !Game1.prevKeyState.IsKeyDown(Keys.Space))
                ray.setPos((float)(myship.getPosX() - 6), (float)(myship.getPosY() - 18));
            ray.active = true;
            if (Game1.currentKeyState.IsKeyDown(Keys.C) && !Game1.prevKeyState.IsKeyDown(Keys.C))
               
                star.setPos(boss2.getPosX(), boss2.getPosY());
                
            star.active = true;

            if (Game1.currentKeyState.IsKeyDown(Keys.Space) && !Game1.prevKeyState.IsKeyDown(Keys.Space))
                limSound.playSound();

            if (BoundStar.Intersects(BoundShip))
            {
                myship.hitPoints = myship.hitPoints - 1;
            }

            if (BoundT.Intersects(BoundShip))
            {
                score = score + 1;              //increase score
            }

            if (!bossHit && BoundRay.Intersects(BoundBoss1))
            {
                bossHit = true;
                boss1.hitPoints -= 2;
                createExplosion((int)boss1.getPosX(), (int)boss1.getPosY());
               
                score = score + 10;              //increase score
            }

            else
            {
                bossHit = false;    //person has moved outsite hit rectangle, reset
            }

            if (boss1.hitPoints <= 0)
            {
                boss1.setActive(false);
            }

            if (!bossHit && BoundRay.Intersects(BoundBoss2))
            {
                bossHit = true;
                boss2.hitPoints -= 2;//hit occured
                createExplosion((int)boss2.getPosX(), (int)boss2.getPosY());
                score = score + 10;              //increase score
            }

            else
            {
                bossHit = false;    //person has moved outsite hit rectangle, reset
            }

            if (boss2.hitPoints <= 0)
            {
                boss2.setActive(false);
            }

            if (BoundShip.Intersects(missile.getBoundingBoxAA()))

            {
                myColor = new Color(60, 180, 60);

                //myship.hitPoints = (int)(a + b);


                if (BoundBoss1.Intersects(BoundShip))
                {
                    b = b + 0.005;
                    if ((a + b) >= 2)
                    {
                        myship.hitPoints = 2;
                    }
                    else
                    {
                        myship.hitPoints = (int)(a + b);
                    }

                }

            }
            else
            {
                ticks = ticks + 1;
                if (ticks % 30 == 0)
                {
                    myColor = new Color(rgb.Next(0, 255), rgb.Next(0, 255), rgb.Next(0, 255));
                }

                myship.hitPoints = (int)(a + b);

                if ((a + b > 0) && (a + b <= 20))
                {
                    a = a - 0.007;
                    myship.hitPoints = (int)(a + b);
                }

                else
                {
                    myship.hitPoints = 0;
                    blackhole.setMoveSpeed(0);
                    myship.active = true;
                    missile.setMoveSpeed(0);
                    boss1.setMoveSpeed(0);
                    slist1.addSpriteReuse(missile);
                    rlist1.addReuse(boss1);
                }


            }

            showtimer = !showtimer;

            if (boss1.collision(myship))
            {

                TextureFade rr = new TextureFade(tex1, new Rectangle((int)myship.getPosX() + 10, (int)myship.getPosY(), 40, 40),
                                                          new Rectangle((int)myship.getPosX() + 10, (int)myship.getPosY() - 150, 40, 40), Color.White, Color.Black, 100);

                boss1.setActive(false);
                missile.setMoveSpeed(4);
                showtimer = !showtimer;


                if (BoundShip.Intersects(BoundMis))
                {
                    myship.hitPoints = (int)(a + b) / 2;
                }
                else
                {
                    myship.hitPoints = (int)(a + b) / 2;
                }

                slist1.addSpriteReuse(missile);
                rlist1.addReuse(rr);

            }


            if (blackhole.collision(myship))
            {
                showgg = !showgg;

                star.setMoveSpeed(1);
                missile.setMoveSpeed(1);
            }
            if (star.collision(myship))
            {
                showgg = !showgg;

                star.setMoveSpeed(1);
                missile.setMoveSpeed(1);

                if (BoundShip.Intersects(missile.getBoundingBoxAA()))
                {
                    myship.hitPoints = 0;
                }
                else
                {
                    myship.hitPoints = 0;
                }
                slist1.addSpriteReuse(missile);




                if (!screenRect.Intersects(BoundHole))
                {
                    blackhole.setPos(550, 145);
                }

                if (!screenRect.Intersects(boss1.getBoundingBoxAA()))
                {
                    boss1.setPos(350, 410);
                }

                if (!screenRect.Intersects(ray.getBoundingBoxAA()))
                { 
                    ray.setPos(750, 65);         
                }

                if (!screenRect.Intersects(enemy1.getBoundingBoxAA()))
                {
                    enemy1.setPos(600, 80);
                }

                if (!screenRect.Intersects(enemy2.getBoundingBoxAA()))
                {
                    enemy2.setPos(650, 49);
                }

                limSound.Update(gameTime);
                rlist1.Update(gameTime);

                slist1.addSpriteReuse(myship);

                base.Update(gameTime);// all components is called by now

            }
        }


        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.CornflowerBlue);// Clear and fill with colour

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            spriteBatch.Draw(background1, bg1Position1, Color.White);
            spriteBatch.Draw(background2, bg2Position2, Color.White);          

            missile.Draw(spriteBatch);
            blackhole.Draw(spriteBatch);

            timer.Draw(spriteBatch);
            ray.Draw(spriteBatch);
            star.Draw(spriteBatch);
            myship.Draw(spriteBatch);

            enemy1.Draw(spriteBatch);
            enemy2.Draw(spriteBatch);


            if (showbb)
            {

                blackhole.drawBB(spriteBatch, Color.IndianRed);
                boss1.drawBB(spriteBatch, Color.IndianRed);
                boss2.drawBB(spriteBatch, Color.IndianRed);
                enemy3.drawBB(spriteBatch, Color.IndianRed);
                missile.drawBB(spriteBatch, Color.Red);
                myship.drawBB(spriteBatch, Color.Red);
                star.drawBB(spriteBatch, Color.Red);
                ray.drawBB(spriteBatch, Color.Red);
                timer.drawBB(spriteBatch, Color.Red);
                enemy1.drawBB(spriteBatch, Color.Red);
                enemy2.drawBB(spriteBatch, Color.Red);
            }

            if (showgg)
            {

                ggg.Draw(spriteBatch);
                boomboom.Draw(spriteBatch);
            }
 
            spriteBatch.DrawString(font1, "Score:" + Convert.ToString(score), new Vector2(10, 10), Color.White);

            boss1.Draw(spriteBatch);
            boss2.Draw(spriteBatch);
            enemy3.Draw(spriteBatch);
            booms.Draw(spriteBatch);
            spriteBatch.End();

        }
    }

    // -------------------------------------------------------- Game Level 4 MODE 3 Hard ----------------------------------------------------------------------------------

    class GameLevel_4_Hard : RC_GameStateParent
    {
        SpriteList slist1 = null;
        SpriteList slist2 = null;
        SpriteList slist3 = null;
        SpriteList booms  = null;

        RC_RenderableList rlist1 = null;

        int screenWidth;
        int ScreenHeight;
        Rectangle screenRect;

        SoundEffect music;
        LimitSound limSound;

        HealthBar healthBar;
        HealthBarAttached myHp;
        HealthBarAttached bHp1;
        HealthBarAttached bHp2;
        HealthBarAttached bHp3;

        bool showbb = false;// show bounding boxes flag (press B)
        bool showgg = false;
        bool showtimer = false;
        bool bossHit = false;
        bool gameWin = false;
        

        bool trump = true;

        double a = 2;
        double b = 0;

        double c;
        double i = 0;

        Color myColor;

        int ticks = 0;
        public int score = 0;

        float angle = 0;

        Texture2D background1;
        Texture2D background2;

        Vector2 bg1Position1;
        Vector2 bg2Position2;
        float speed;

        Texture2D spaceship;
        Sprite3 myship;

        Sprite3 missile;
        Sprite3 enemy1;
        Sprite3 enemy2;
        Sprite3 enemy3;

        Sprite3 ggg;
        Sprite3 boomboom;

        Texture2D tex1;
        Texture2D tex11;
        Texture2D tex111;
        Texture2D tex2;
        Texture2D tex3;
        Texture2D tex4;
        Texture2D tex5;
        Texture2D tex6;
        Texture2D tex7;
        Texture2D tex8;
        Texture2D tex9;
        Texture2D tex10 = null;
        Texture2D texWin;

        Sprite3 ray;
        Sprite3 star;
        Sprite3 blackhole;
        Sprite3 boss1;
        Sprite3 boss2;
        Sprite3 boss3;

        Sprite3 win;

        Sprite3 timer;

        Random rgb = new Random();
        Random RandomClass;

        void createExplosion(int x, int y)
        {
            tex10 = Util.texFromFile(graphicsDevice, Dir.dir + "boom.png");
            booms = new SpriteList();
            int xoffset = -2;
            int yoffset = -20;

            boomboom = new Sprite3(true, tex10, x + xoffset, y + yoffset);
            boomboom.setWidthHeight(128, 128);
            boomboom.setXframes(4);
            boomboom.setYframes(4);

            Vector2[] anim1 = new Vector2[16];
            anim1[0].X = 0; anim1[0].Y = 0;
            anim1[1].X = 1; anim1[1].Y = 0;
            anim1[2].X = 2; anim1[2].Y = 0;
            anim1[3].X = 3; anim1[3].Y = 0;
            anim1[4].X = 0; anim1[4].Y = 1;
            anim1[5].X = 1; anim1[5].Y = 1;
            anim1[6].X = 2; anim1[6].Y = 1;
            anim1[7].X = 3; anim1[7].Y = 1;
            anim1[8].X = 0; anim1[8].Y = 2;
            anim1[9].X = 1; anim1[9].Y = 2;
            anim1[10].X = 2; anim1[10].Y = 2;
            anim1[11].X = 3; anim1[11].Y = 2;
            anim1[12].X = 0; anim1[12].Y = 3;
            anim1[13].X = 1; anim1[13].Y = 3;
            anim1[14].X = 2; anim1[14].Y = 3;
            anim1[15].X = 3; anim1[15].Y = 3;
            boomboom.setAnimationSequence(anim1, 0, 15, 1);
            boomboom.setAnimFinished(2);
            boomboom.animationStart();

            booms.addSpriteReuse(boomboom);
        }

        public override void LoadContent()
        {
            music = Content.Load<SoundEffect>("explosionSound");
            limSound = new LimitSound(music, 100);

            RandomClass = new Random();
            slist1 = new SpriteList(99);
            slist2 = new SpriteList(99);
            slist3 = new SpriteList(99);
            booms = new SpriteList();
            rlist1 = new RC_RenderableList();

            bg1Position1 = new Vector2(0, 0);
            bg2Position2 = new Vector2(0, 800);

            speed = 0.3f;

            background1 = Content.Load<Texture2D>("Img/space5");
            background2 = Content.Load<Texture2D>("Img/space6");

            spaceship = Content.Load<Texture2D>("Img/spaceship");
            myship = new Sprite3(true, spaceship, 80, 80);
            myship.setPos(350, 450);
            myship.setWidthHeight(200, 200);
            myship.setWidthHeightOfTex(200, 200);
            myship.setHSoffset(new Vector2(40, 40));
            myship.setBB(0, 0, 80, 80);
            myship.setMoveSpeed(100);

            slist3.addSpriteReuse(myship);

            HealthBarAttached myHp = new HealthBarAttached(Color.Aquamarine, Color.Green, Color.Red, 9, true);
            myHp.offset = new Vector2(0, -1); // one pixel above the bounding box
            myHp.gapOfbar = 2;
            myship.hitPoints = 20;
            myship.maxHitPoints = 20;
            myship.attachedRenderable = myHp;  

            trump = true;

            tex1 = Content.Load<Texture2D>("Img/boss1");
            tex11 = Content.Load<Texture2D>("Img/boss2");
            tex111 = Content.Load<Texture2D>("Img/boss3");
            tex2 = Content.Load<Texture2D>("Img/mis");
            tex3 = Content.Load<Texture2D>("Img/ray");
            tex4 = Content.Load<Texture2D>("Img/star");
            tex5 = Content.Load<Texture2D>("Img/blackhole");
            tex6 = Content.Load<Texture2D>("Img/enemy1");
            tex7 = Content.Load<Texture2D>("Img/enemy2");
            tex8 = Content.Load<Texture2D>("Img/bling");

            boss1 = new Sprite3(true, tex1, 1f, (float)(4 + RandomClass.NextDouble() * 50));
            boss1.setPos(350, -100);
            boss1.setWidthHeight(160, 160);
            boss1.setWidthHeightOfTex(182, 250);
            boss1.setHSoffset(new Vector2(40, 40));
            boss1.setMoveAngleDegrees(90);
            boss1.setMoveSpeed(0.5f);

            boss2 = new Sprite3(true, tex11, 1f, (float)(4 + RandomClass.NextDouble() * 50));
            boss2.setPos(150, -100);
            boss2.setWidthHeight(80, 80);
            boss2.setWidthHeightOfTex(182, 250);
            boss2.setHSoffset(new Vector2(40, 40));
            boss2.setMoveAngleDegrees(90);
            boss2.setMoveSpeed(0.5f);

            boss3 = new Sprite3(true, tex111, 1f, (float)(4 + RandomClass.NextDouble() * 50));
            boss3.setPos(650, -100);
            boss3.setWidthHeight(80, 80);
            boss3.setWidthHeightOfTex(182, 250);
            boss3.setHSoffset(new Vector2(40, 40));
            boss3.setMoveAngleDegrees(90);
            boss3.setMoveSpeed(0.5f);

            HealthBarAttached bHp1 = new HealthBarAttached(Color.Aquamarine, Color.Green, Color.Red, 9, true);
            bHp1.offset = new Vector2(0, -1); // one pixel above the bounding box
            bHp1.gapOfbar = 2;
            boss1.hitPoints = 1000;
            boss1.maxHitPoints = 1000;
            boss1.attachedRenderable = bHp1;

            HealthBarAttached bHp2 = new HealthBarAttached(Color.Aquamarine, Color.Green, Color.Red, 9, true);
            bHp2.offset = new Vector2(0, -1); // one pixel above the bounding box
            bHp2.gapOfbar = 2;
            boss2.hitPoints = 500;
            boss2.maxHitPoints = 500;
            boss2.attachedRenderable = bHp2;

            HealthBarAttached bHp3 = new HealthBarAttached(Color.Aquamarine, Color.Green, Color.Red, 9, true);
            bHp3.offset = new Vector2(0, -1); // one pixel above the bounding box
            bHp3.gapOfbar = 2;
            boss3.hitPoints = 500;
            boss3.maxHitPoints = 500;
            boss3.attachedRenderable = bHp3;

            missile = new Sprite3(true, tex2, 1f, (float)(500 + RandomClass.NextDouble() * 50));
            missile.setPos(1000, -100);
            missile.setWidthHeight(100, 100);
            missile.setHSoffset(new Vector2(50, 50));
            missile.setMoveAngleDegrees(135);
            missile.setDisplayAngleDegrees(-270);
            missile.setMoveSpeed(2f);
            missile.setBBFractionOfTexCentered(0.8f);

            slist1.addSpriteReuse(missile);

            star = new Sprite3(true, tex4, 80, 73);
            star.setPos(0, 0);
            star.setWidthHeight(27, 27);
            star.setBBFractionOfTexCentered(0.7f);
            star.setMoveAngleDegrees(90);
            star.setMoveSpeed(7f);
            slist1.addSpriteReuse(star);

            blackhole = new Sprite3(true, tex5, 600, 300);
            blackhole.setWidthHeight(160, 148);
            blackhole.setBBFractionOfTexCentered(0.7f);
            blackhole.setMoveAngleDegrees(45);
            blackhole.setMoveSpeed(0.5f);
            blackhole.setPos(0, 0);

            ray = new Sprite3(true, tex3, 306, 190);
            ray.setWidthHeight(13, 50);
            ray.setMoveAngleDegrees(90);
            ray.setMoveSpeed(-15f);
            ray.setBBToWH();
            ray.setDisplayAngleDegrees(0);
            ray.setPos(myship.getPosX() - 6, myship.getPosY() - 18);
            ray.setColor(Color.GhostWhite);

            slist2.addSpriteReuse(ray);

            enemy1 = new Sprite3(true, tex6, 3f, (float)(10 + RandomClass.NextDouble() * 50));
            enemy1.setWidthHeight(80, 80);
            enemy1.setWidthHeightOfTex(72, 128);
            enemy1.setBBFractionOfTexCentered(0.7f);
            enemy1.setMoveAngleDegrees(45);
            enemy1.setDisplayAngleDegrees(135);
            enemy1.setMoveSpeed(0.7f);
            enemy1.setPos(250, 65);
            enemy1.setColor(Color.GhostWhite);

            slist1.addSpriteReuse(enemy1);

            enemy2 = new Sprite3(true, tex7, 3f, (float)(100 + RandomClass.NextDouble() * 50));
            enemy2.setWidthHeight(80, 80);
            enemy2.setWidthHeightOfTex(72, 128);
            enemy2.setBBFractionOfTexCentered(0.7f);
            enemy2.setMoveAngleDegrees(135);
            enemy2.setDisplayAngleDegrees(45);
            enemy2.setMoveSpeed(1f);
            enemy2.setPos(1000, 100);
            enemy2.setColor(Color.GhostWhite);

            slist1.addSpriteReuse(enemy2);

            enemy3 = new Sprite3(true, tex7, 1f, (float)(4 + RandomClass.NextDouble() * 50));
            enemy3.setWidthHeight(80, 80);
            enemy3.setWidthHeightOfTex(72, 128);
            enemy3.setWidthHeight(80, 80);
            enemy3.setWidthHeightOfTex(72, 128);
            enemy3.setBBFractionOfTexCentered(0.7f);
            enemy3.setMoveAngleDegrees(135);
            enemy3.setDisplayAngleDegrees(45);
            enemy3.setMoveSpeed(1f);
            enemy3.setPos(1200, 0);
            enemy3.setColor(Color.GhostWhite);

            slist1.addSpriteReuse(enemy3);

            tex9 = Content.Load<Texture2D>("Img/gg");
            ggg = new Sprite3(true, tex9, 800, 240);
            ggg.setPos(240, 70);
            ggg.setWidthHeight(300, 300);

            texWin = Content.Load<Texture2D>("Img/cpwin");
            win = new Sprite3(true, texWin, 800, 240);
            win.setPos(245, 175);
            win.setWidthHeight(300, 100);

            AttachedText t = new AttachedText(Color.Red, font1);
            t = new AttachedText(Color.Red, font1, new Vector2(-30, 52), " ");
            star.attachedRenderable = t;

            timer = new Sprite3(true, tex8, 1f, (float)(4 + RandomClass.NextDouble() * 50));
            timer.setPos(10, 10);
            timer.setWidthHeight(135, 135);
            timer.setWidthHeightOfTex(1024, 384);
            timer.setXframes(8);
            timer.setYframes(3);
            timer.setHSoffset(new Vector2(64, 64));
            timer.setBB(38, 38, 58, 58);
            timer.setMoveAngleDegrees(40);
            timer.setMoveSpeed(1);

            Vector2[] anim0 = new Vector2[10];
            anim0[0].X = 0; anim0[0].Y = 1;
            anim0[1].X = 1; anim0[1].Y = 1;
            anim0[2].X = 2; anim0[2].Y = 1;
            anim0[3].X = 3; anim0[3].Y = 1;
            anim0[4].X = 4; anim0[4].Y = 1;
            anim0[5].X = 5; anim0[5].Y = 1;
            anim0[6].X = 6; anim0[6].Y = 1;
            anim0[7].X = 7; anim0[7].Y = 1;
            anim0[8].X = 8; anim0[8].Y = 1;
            anim0[9].X = 1; anim0[8].Y = 1;
            timer.setAnimationSequence(anim0, 0, 9, 60);
            timer.animationStart();

        }

        public override void Update(GameTime gameTime)
        {
            getKeyboardAndMouse();

            booms.animationTick(gameTime);

            blackhole.moveByAngleSpeed();
            missile.moveByAngleSpeed();
            star.moveByAngleSpeed();
            boss1.moveByAngleSpeed();
            boss2.moveByAngleSpeed();
            boss3.moveByAngleSpeed();
            timer.moveByAngleSpeed();
            ray.moveByAngleSpeed();
            enemy1.moveByAngleSpeed();
            enemy2.moveByAngleSpeed();
            enemy3.moveByAngleSpeed();
          
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                gameStateManager.setLevel(4);

            KeyboardState state = Keyboard.GetState();

            if (Game1.currentKeyState.IsKeyDown(Keys.Escape))
                gameStateManager.setLevel(0);

            if (bg1Position1.Y > background1.Height)
            {
                bg1Position1.Y = bg2Position2.Y - 800;
            }
            if (bg2Position2.Y > background2.Height)
            {
                bg2Position2.Y = bg1Position1.Y - 800;
            }
            bg1Position1.Y += speed;
            bg2Position2.Y += speed;

            if (Game1.currentKeyState.IsKeyDown(Keys.Up))
            {
                if (myship.getPosY() < 0)
                {
                    myship.setDeltaSpeed(new Vector2(0, 0));
                }
                else
                {
                    myship.setDeltaSpeed(new Vector2(0, -5));
                }
                myship.moveByDeltaXY();
            }
            if (Game1.currentKeyState.IsKeyDown(Keys.Down))
            {
                if (myship.getPosY() > 540)
                {
                    myship.setDeltaSpeed(new Vector2(0, 0));
                }
                else
                {
                    myship.setDeltaSpeed(new Vector2(0, 5));
                }
                myship.moveByDeltaXY();
            }
            if (Game1.currentKeyState.IsKeyDown(Keys.Right))
            {
                if (myship.getPosX() < 0)
                {
                    myship.setDeltaSpeed(new Vector2(0, 0));
                }
                else
                {
                    myship.setDeltaSpeed(new Vector2(5, 0));
                }
                myship.moveByDeltaXY();
            }
            if (Game1.currentKeyState.IsKeyDown(Keys.Left))
            {
                if (myship.getPosX() > 0)
                {
                    myship.setDeltaSpeed(new Vector2(-5, 0));
                }

                myship.moveByDeltaXY();
            }

            Rectangle BoundBoss1 = boss1.getBoundingBoxAA();
            Rectangle BoundBoss2 = boss2.getBoundingBoxAA();
            Rectangle BoundBoss3 = boss3.getBoundingBoxAA();
            Rectangle BoundShip = myship.getBoundingBoxAA();
            Rectangle BoundHole = blackhole.getBoundingBoxAA();
            Rectangle BoundMis = missile.getBoundingBoxAA();
            Rectangle BoundRay = ray.getBoundingBoxAA();
            Rectangle BoundT = timer.getBoundingBoxAA();
            Rectangle BoundStar = star.getBoundingBoxAA();

            slist1.moveDeltaXY();
            slist1.animationTick(gameTime);
            // collision test
            for (int i = 0; i < slist2.count(); i++)
            {
                Sprite3 s = slist2.getSprite(i);
                if (s == null) continue;
                if (!s.active) continue;
                if (!s.visible) continue;
                int colision = slist1.collisionAA(s);
                if (colision == -1) continue;
                // we get here if we collided
                //make the guy inactive
                Sprite3 c = slist1.getSprite(colision);
                c.setActive(false);
            }

            timer.animationTick(gameTime);

            if (Game1.currentKeyState.IsKeyDown(Keys.V)
                && prevKeyState.IsKeyUp(Keys.V))
            {
                showbb = !showbb;
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.G)
                  && prevKeyState.IsKeyUp(Keys.G))
            {
                showgg = !showgg;
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.W)
                  && prevKeyState.IsKeyUp(Keys.W))
            {
                gameWin = !gameWin;
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.H) && !Game1.prevKeyState.IsKeyDown(Keys.H))
            {
                myship.hitPoints = myship.hitPoints - 3;
                if (myship.hitPoints <= 0) myship.active = false;
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.J) && !Game1.prevKeyState.IsKeyDown(Keys.J))
            {
                myship.hitPoints = myship.hitPoints + 3;
                if (myship.hitPoints <= 0) myship.active = false;
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.B) && !Game1.prevKeyState.IsKeyDown(Keys.B))
            {
                gameStateManager.setLevel(1);
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.R) && !Game1.prevKeyState.IsKeyDown(Keys.R))
            {
                myship.setPos(50f, (float)(200 + RandomClass.NextDouble() * 500));
                myship.hitPoints = 10;
                myship.active = true;
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.N) && !Game1.prevKeyState.IsKeyDown(Keys.N))
            {
                gameStateManager.setLevel(6);
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.F1) && !Game1.prevKeyState.IsKeyDown(Keys.F1))
            {
                gameStateManager.pushLevel(5);
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.F4) && !Game1.prevKeyState.IsKeyDown(Keys.F4))
            {
                gameStateManager.pushLevel(7);
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                gameStateManager.setLevel(4);

            if (Game1.currentKeyState.IsKeyDown(Keys.Space) && !Game1.prevKeyState.IsKeyDown(Keys.Space))
                ray.setPos((float)(myship.getPosX() - 6), (float)(myship.getPosY() - 18));
            //myship.hitPoints = 10;
            ray.active = true;
            if (Game1.currentKeyState.IsKeyDown(Keys.C) && !Game1.prevKeyState.IsKeyDown(Keys.C))
                star.setPos(boss1.getPosX(), boss1.getPosY());
            star.active = true;

            if (Game1.currentKeyState.IsKeyDown(Keys.Space) && !Game1.prevKeyState.IsKeyDown(Keys.Space))
                limSound.playSound();

            if (BoundStar.Intersects(BoundShip))
            {
                myship.hitPoints -= 2;
            }

            if (BoundT.Intersects(BoundShip))
            {
                score = score + 1;              //increase score
            }

            if (!bossHit && BoundRay.Intersects(BoundBoss1))
            {
                bossHit = true;
                boss1.hitPoints -= 2;//hit occured
                createExplosion((int)boss1.getPosX(), (int)boss1.getPosY());
                score = score + 10;              //increase score
            }

            else
            {
                bossHit = false;    //person has moved outsite hit rectangle, reset
            }

            if (boss1.hitPoints <= 0)
            {
                boss1.setActive(false);
            }

            if (!bossHit && BoundRay.Intersects(BoundBoss2))
            {
                bossHit = true;
                boss2.hitPoints -= 2;
                createExplosion((int)boss2.getPosX(), (int)boss2.getPosY());
                score = score + 10;              //increase score
            }

            else
            {
                bossHit = false;    //person has moved outsite hit rectangle, reset
            }

            if (boss2.hitPoints <= 0)
            {
                boss2.setActive(false);
            }

            if (!bossHit && BoundRay.Intersects(BoundBoss3))
            {
                bossHit = true;
                boss3.hitPoints -= 2;//hit occured
                createExplosion((int)boss3.getPosX(), (int)boss3.getPosY());
                score = score + 10;              //increase score
            }

            else
            {
                bossHit = false;    
            }

            if (boss3.hitPoints <= 0)
            {
                boss3.setActive(false);
            }


            if (BoundShip.Intersects(missile.getBoundingBoxAA()))

            {
                myColor = new Color(60, 180, 60);

                if (BoundBoss1.Intersects(BoundShip))
                {
                    b = b + 0.005;
                    if ((a + b) >= 2)
                    {
                        myship.hitPoints = 2;
                    }
                    else
                    {
                        myship.hitPoints = (int)(a + b);
                    }

                }

            }
            else
            {
                ticks = ticks + 1;
                if (ticks % 30 == 0)
                {
                    myColor = new Color(rgb.Next(0, 255), rgb.Next(0, 255), rgb.Next(0, 255));
                }

                myship.hitPoints = (int)(a + b);

                if ((a + b > 0) && (a + b <= 20))
                {
                    a = a - 0.007;
                    myship.hitPoints = (int)(a + b);
                }

                else
                {
                    myship.hitPoints = 0;
                    blackhole.setMoveSpeed(0);
                    myship.active = true;
                    missile.setMoveSpeed(0);
                    boss1.setMoveSpeed(0);
                    slist1.addSpriteReuse(missile);
                    rlist1.addReuse(boss1);

                }


            }

            showtimer = !showtimer;

           
            if (boss1.collision(myship))
            {
           
                TextureFlash rr = new TextureFlash(tex10, new Rectangle((int)myship.getPosX() + 10, (int)myship.getPosY(), 40, 40),
                                                          new Rectangle((int)myship.getPosX() + 10, (int)myship.getPosY() - 150, 40, 40), Color.White, 1, true);

                boss1.setActive(false);
                missile.setMoveSpeed(4);
                showtimer = !showtimer;
                

                if (BoundShip.Intersects(BoundMis))
                {
                    myship.hitPoints = (int)(a + b) / 2;
                }
                else
                {
                    myship.hitPoints = (int)(a + b) / 2;
                }

                slist1.addSpriteReuse(missile);
                rlist1.addReuse(rr);

            }

            if (blackhole.collision(myship))
            {

                showgg = !showgg;
                star.setMoveSpeed(1);
                missile.setMoveSpeed(1);
            }

            if (star.collision(myship))
            {
                showgg = !showgg;

                star.setMoveSpeed(1);
                missile.setMoveSpeed(1);

                if (BoundShip.Intersects(missile.getBoundingBoxAA()))
                {
                    myship.hitPoints = 0;
                }
                else
                {
                    myship.hitPoints = 0;
                }
                slist1.addSpriteReuse(missile);

                slist1.moveDeltaXY();
                slist1.animationTick(gameTime); 
                for (int i = 0; i < slist2.count(); i++)
                {
                    Sprite3 s = slist2.getSprite(i);
                    if (s == null) continue;
                    if (!s.active) continue;
                    if (!s.visible) continue;
                    int colision = slist1.collisionAA(s);
                    if (colision == -1) continue;
                    Sprite3 c = slist1.getSprite(colision);
                    c.setActive(false);
                }

                limSound.Update(gameTime);
              
                rlist1.Update(gameTime);
                slist1.addSpriteReuse(myship);

                
                base.Update(gameTime);// all components is called by now

            }
        }


        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.CornflowerBlue);// Clear and fill with colour

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            spriteBatch.Draw(background1, bg1Position1, Color.White);
            spriteBatch.Draw(background2, bg2Position2, Color.White);

            slist1.Draw(spriteBatch);
            rlist1.Draw(spriteBatch);

            missile.Draw(spriteBatch);
            blackhole.Draw(spriteBatch);
          
            timer.Draw(spriteBatch);
            ray.Draw(spriteBatch);
            star.Draw(spriteBatch);
            myship.Draw(spriteBatch);
            
            enemy1.Draw(spriteBatch);
            enemy2.Draw(spriteBatch);


            if (showbb)
            {

                blackhole.drawBB(spriteBatch, Color.IndianRed);
                boss1.drawBB(spriteBatch, Color.IndianRed);
                boss2.drawBB(spriteBatch, Color.IndianRed);
                boss3.drawBB(spriteBatch, Color.IndianRed);
                enemy3.drawBB(spriteBatch, Color.IndianRed);
                missile.drawBB(spriteBatch, Color.Red);
                myship.drawBB(spriteBatch, Color.Red);
                star.drawBB(spriteBatch, Color.Red);
                ray.drawBB(spriteBatch, Color.Red);
                timer.drawBB(spriteBatch, Color.Red);
                enemy1.drawBB(spriteBatch, Color.Red);
                enemy2.drawBB(spriteBatch, Color.Red);

                boss1.Draw(spriteBatch);
                boss2.Draw(spriteBatch);
                boss3.Draw(spriteBatch);

                enemy3.Draw(spriteBatch);
            }

            if (showgg)
            {
                ggg.Draw(spriteBatch);
            }

            if (gameWin)
            {
                win.Draw(spriteBatch);
            }

          spriteBatch.DrawString(font1, "Score:" + Convert.ToString(score), new Vector2(10, 10), Color.White);

           boss1.Draw(spriteBatch);
           boss2.Draw(spriteBatch);
           boss3.Draw(spriteBatch);
           booms.Draw(spriteBatch);
           spriteBatch.End();

        }
    }

    // -------------------------------------------------------- Game level 5 Pause ----------------------------------------------------------------------------------

    class GameLevel_5_Pause : RC_GameStateParent
    {

        RC_RenderableList ren = new RC_RenderableList();
        Texture2D texG;         
        Texture2D texP; 
        Texture2D texMap;
        Sprite3 map;
        RC_Renderable f2 = null;

        public override void LoadContent()
        {
            font1 = Content.Load<SpriteFont>("SpriteFont1");
            texG = Util.texFromFile(graphicsDevice, Dir.dir + "cppause.png");        
            texP = Util.texFromFile(graphicsDevice, Dir.dir + "cpman.png");  
            texMap = Util.texFromFile(graphicsDevice, Dir.dir + "cpkey.png");
            map = new Sprite3(true, texMap, 800, 600);
            map.setPos(0, 0);
            map.setWidthHeight(800, 500);
        }

        public override void Update(GameTime gameTime)
        {

            if (Game1.currentKeyState.IsKeyDown(Keys.R) && !Game1.prevKeyState.IsKeyDown(Keys.R))
            {
                gameStateManager.popLevel();
            }
            if (Game1.currentKeyState.IsKeyDown(Keys.Space) && !Game1.prevKeyState.IsKeyDown(Keys.Space))
            {
                setUpRendarables();
            }
            if (Game1.currentKeyState.IsKeyDown(Keys.F2) && !Game1.prevKeyState.IsKeyDown(Keys.F2))
            {
                setUpRendarableF2();
            }

            ren.Update(gameTime);
            if (f2 != null) f2.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Aqua);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            map.Draw(spriteBatch);


            if (f2 != null) f2.Draw(spriteBatch);
            spriteBatch.DrawString(font1, " ", new Vector2(100, 100), Color.Brown);
            spriteBatch.DrawString(font1, " ", new Vector2(100, 120), Color.Brown);
            spriteBatch.DrawString(font1, " ", new Vector2(100, 140), Color.Brown);

            ren.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void setUpRendarables()
        {
            RC_Renderable r = new TextRenderableFade("Vice City Map", new Vector2(10, 160), font1, Color.DarkGreen, Color.Transparent, 120);
            ren.addReuse(r);
            r = new TextureFade(texP, new Rectangle(0, 0, 600, 800), new Rectangle(120, 100, 120, 120),
                                Color.White, Color.Transparent, 160);
            ren.addReuse(r);
        }

        public void setUpRendarableF2()
        {
            if (f2 != null) return;
            f2 = new ScrollBackGround(texG, new Rectangle(0, 0, 1920, 1080), new Rectangle(0, 0, 800, 600), 1, 1);
      
        }

    }


    // -------------------------------------------------------- Game level 6 Fail ----------------------------------------------------------------------------------

    class GameLevel_6_Fail : RC_GameStateParent
    {
        RC_RenderableList gtagg = new RC_RenderableList();
        Texture2D texGG;
        RC_Renderable gg = null;

        public override void LoadContent()
        {
            texGG = Util.texFromFile(graphicsDevice, Dir.dir + "cpfail.png");
            gg = new ScrollBackGround(texGG, new Rectangle(0, 0, 1280, 850), new Rectangle(0, 0, 800, 600), 1, 1);
            gtagg.addReuse(gg);

        }

        public override void Update(GameTime gameTime)
        {

            if (Game1.currentKeyState.IsKeyDown(Keys.N) && !Game1.prevKeyState.IsKeyDown(Keys.N))
            {
                gameStateManager.setLevel(8);
            }
            if (Game1.currentKeyState.IsKeyDown(Keys.B) && !Game1.prevKeyState.IsKeyDown(Keys.B))
            {
                gameStateManager.setLevel(1);
            }

        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            spriteBatch.DrawString(font1, "level 4 - press b to go back a level", new Vector2(100, 100), Color.Brown);
            spriteBatch.DrawString(font1, "level 4 - press r to restart guy", new Vector2(100, 120), Color.Brown);
            spriteBatch.DrawString(font1, "level 4 - press n to go to next level", new Vector2(100, 140), Color.Brown);

            gg.Draw(spriteBatch);
            spriteBatch.DrawString(font1, "Score: 3700", new Vector2(360, 255), Color.White);

            spriteBatch.End();
        }
    }

    // -------------------------------------------------------- Game level 7 Win ----------------------------------------------------------------------------------

    class GameLevel_7_Win : RC_GameStateParent
    {
        RC_RenderableList gtagg = new RC_RenderableList();
        Texture2D texGG;
        RC_Renderable gg = null;

        public override void LoadContent()
        {
            texGG = Util.texFromFile(graphicsDevice, Dir.dir + "cpwin.png");
            gg = new ScrollBackGround(texGG, new Rectangle(0, 0, 2590, 1540), new Rectangle(0, 0, 800, 600), 1, 1);
            gtagg.addReuse(gg);
        }

        public override void Update(GameTime gameTime)
        {
           

            if (Game1.currentKeyState.IsKeyDown(Keys.N) && !Game1.prevKeyState.IsKeyDown(Keys.N))
            {
                gameStateManager.setLevel(8);
            }
            if (Game1.currentKeyState.IsKeyDown(Keys.B) && !Game1.prevKeyState.IsKeyDown(Keys.B))
            {
                gameStateManager.setLevel(1);
            }

        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            gg.Draw(spriteBatch);
            spriteBatch.DrawString(font1, " ", new Vector2(360, 255), Color.White);
            
            spriteBatch.End();
        }
    }

    // -------------------------------------------------------- Game level 8 ScoreBoard ----------------------------------------------------------------------------------

    class GameLevel_8_Score : RC_GameStateParent
    {
        RC_RenderableList gtaoutro = new RC_RenderableList();
        Texture2D texF;
        RC_Renderable outro = null;

        Texture2D tex6; // Green Guy
        Random RandomClass;
        Sprite3 greenGuy; //
        Sprite3 greenGuy1; //
        float angle = 0;

        Vector2 pos4dist;

        public override void LoadContent()
        {
            RandomClass = new Random();
            pos4dist = new Vector2(80, 80);
            font1 = Content.Load<SpriteFont>("SpriteFont1");
            tex6 = Util.texFromFile(graphicsDevice, Dir.dir + "GreenHeadT.png"); ;

            texF = Util.texFromFile(graphicsDevice, Dir.dir + "cpoutro.png");
            outro = new ScrollBackGround(texF, new Rectangle(0, 0, 1024, 900), new Rectangle(0, 0, 800, 600), 1, 1);
            gtaoutro.addReuse(outro);

            Vector2[] anim = new Vector2[4];
            anim[0].X = 1; anim[0].Y = 1;
            anim[1].X = 0; anim[1].Y = 1;
            anim[2].X = 1; anim[2].Y = 1;
            anim[3].X = 2; anim[3].Y = 1;

            greenGuy = new Sprite3(true, tex6, 50f, 500f);
            greenGuy.setWidthHeight(24, 32);
            greenGuy.setWidthHeightOfTex(72, 128);
            greenGuy.setXframes(3);
            greenGuy.setYframes(4);
            greenGuy.setBB(0, 0, 24, 32);
            greenGuy.setMoveAngleDegrees(0);
            greenGuy.setMoveSpeed(1.1f);
            greenGuy.setAnimationSequence(anim, 0, 3, 15);
            greenGuy.animationStart();

            greenGuy1 = new Sprite3(true, tex6, 500f, 500f);
            greenGuy1.setWidthHeight(24, 32);
            greenGuy1.setWidthHeightOfTex(72, 128);
            greenGuy1.setXframes(3);
            greenGuy1.setYframes(4);
            greenGuy1.setBB(0, 0, 24, 32);
            greenGuy1.setMoveAngleDegrees(Util.degToRad(180));
            greenGuy1.setMoveSpeed(0.4f);
            greenGuy1.setFlip(SpriteEffects.FlipHorizontally);
            greenGuy1.setAnimationSequence(anim, 0, 3, 15);
            greenGuy1.animationStart();


        }

        public override void Update(GameTime gameTime)
        {
            getKeyboardAndMouse();

            greenGuy.savePosition();
            greenGuy.moveByAngleSpeed();
            greenGuy.animationTick(gameTime);

            if (greenGuy.collision(greenGuy1)) greenGuy.restorePosition();

            greenGuy1.savePosition();
            greenGuy1.moveByAngleSpeed();
            greenGuy1.animationTick(gameTime);

            if (greenGuy1.collision(greenGuy)) greenGuy1.restorePosition();

            if (Game1.currentKeyState.IsKeyDown(Keys.N) && !Game1.prevKeyState.IsKeyDown(Keys.N))
            {
                gameStateManager.setLevel(9);
            }
            if (Game1.currentKeyState.IsKeyDown(Keys.B) && !Game1.prevKeyState.IsKeyDown(Keys.B))
            {
                gameStateManager.setLevel(1);
            }
            if (Game1.currentKeyState.IsKeyDown(Keys.R) && !Game1.prevKeyState.IsKeyDown(Keys.R))
            {
                greenGuy1.setPos(50f + (float)(20 + RandomClass.NextDouble() * 500), 500);
                greenGuy.setPos(50f, 500f);
                angle = 0;
                greenGuy.setMoveAngleRadians(angle);
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.Space))
            {
                angle = angle + 0.03f;
                greenGuy.setMoveAngleRadians(angle);
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.Z) && !Game1.prevKeyState.IsKeyDown(Keys.Z))
            {
                greenGuy.setDisplayAngleRadians(angle);
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.Q) && !Game1.prevKeyState.IsKeyDown(Keys.Q))
            {
                greenGuy.alignDisplayAngle();
            }

            if (Game1.currentKeyState.IsKeyDown(Keys.T) && !Game1.prevKeyState.IsKeyDown(Keys.T))
            {
                greenGuy1.setDisplayAngleRadians(greenGuy1.angleTo(greenGuy));
                greenGuy1.setMoveAngleRadians(greenGuy1.angleTo(greenGuy));
            }

        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Gold);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            greenGuy.draw(spriteBatch);
            greenGuy1.draw(spriteBatch);

           

            greenGuy.drawHS(spriteBatch, Color.Blue);
            greenGuy1.drawHS(spriteBatch, Color.Blue);

            gtaoutro.Draw(spriteBatch);

            spriteBatch.DrawString(font1, "Score Board:", new Vector2(30, 360), Color.White);
            spriteBatch.DrawString(font1, "Level 1: 1000" , new Vector2(30, 380), Color.White);
            spriteBatch.DrawString(font1, "Level 2: 4520 " , new Vector2(30, 400), Color.White);
            spriteBatch.DrawString(font1, "Level 3: 5730 " , new Vector2(30, 420), Color.White);
            spriteBatch.DrawString(font1, "Craft Destroyed: 17 ", new Vector2(30, 440), Color.White);
            spriteBatch.End();
        }
    }


    // -------------------------------------------------------- Game level 9 End Screen ----------------------------------------------------------------------------------

    class GameLevel_9_Outro : RC_GameStateParent
    {
        RC_RenderableList gtagg = new RC_RenderableList();
        Texture2D texGG;
        RC_Renderable gg = null;

        public override void LoadContent()
        {
            texGG = Util.texFromFile(graphicsDevice, Dir.dir + "cpend.png");
            gg = new ScrollBackGround(texGG, new Rectangle(0, 0, 1920, 1240), new Rectangle(0, 0, 800, 600), 1, 1);
            gtagg.addReuse(gg);
        }

        public override void Update(GameTime gameTime)
        {
            if (Game1.currentKeyState.IsKeyDown(Keys.N) && !Game1.prevKeyState.IsKeyDown(Keys.N))
            {
                gameStateManager.setLevel(0);
            }
            if (Game1.currentKeyState.IsKeyDown(Keys.B) && !Game1.prevKeyState.IsKeyDown(Keys.B))
            {
                gameStateManager.setLevel(1);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            gg.Draw(spriteBatch);
            spriteBatch.DrawString(font1, " ", new Vector2(360, 255), Color.White);

            spriteBatch.End();
        }
    }

}

