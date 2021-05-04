using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using System;
using PolyOne.Utility;
using PolyOne.Collision;
using PolyOne.Input;
using PolyOne.ParticleSystem;
using PolyOne.Animation;

using TheRunner.RunCamera;
using TheRunner.RunnerTimes;


using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace TheRunner
{
    enum AnimationState
    {
      WallClimbRight = 1,
      WallClimbLeft = 2,
      WallOver = 3,
      WallSlideLeft = 4,
      WallSlideRight = 5,
      Crawl = 6,
      CrawlStop = 7,
      SlowRun = 8,
      FastRun = 9,
      SlopeFastRun = 10,
      SlopeSlowRun = 11,
      Idle = 12,
      Jump = 13,
      Slide = 14,
      WallJumpLeft = 15, 
      WallJumpRight = 16,
      Fall = 17,
      None = 18
    }



    public class Player
    {
        bool jumpParticle;
        bool jumpLeftParticle;
        bool jumpRightParticle;

        TileCollision previousCollision;

       List<Particle> particleEngines = new List<Particle>();

        private AnimationState aniState;
        private AnimationState previousAniState;

        private const int slideAdjustment = 11;

        bool runButton = false;
        private bool runningStat = false;

        private string levelName;

        private bool upAgainstTileLeft;
        private bool upAgainstTileRight;

        // Wall over 
        private bool wallover;
        private bool walloverTakeaway;
        private float walloverTimer;
        private const float wallOverTime = 50.1f;

        // check against wall 
        LiangBarsky checkAgainstTileLineLeft;
        LiangBarsky checkAgainstTileLineRight;

        RunnerCamera camera = new RunnerCamera();
      
        Texture2D block;

        Texture2D playerBox;
        Vector2 playerOrigin;

        private AnimationData idleAnimation;
        private AnimationData jumpAnimation;
        private AnimationData run0Animation;
        private AnimationData run1Animation;
        private AnimationData runDownSlopeAnimationFast;
        private AnimationData wallRunAnimation;
        private AnimationData wallSlideAnimation;
        private AnimationData floorSlideAnimation;
        private AnimationData crawlAnimation;
        private AnimationData crawlStopAnimation;
        private AnimationData wallOverAnimation;
        private AnimationData fallAnimation;
        private AnimationPlayer sprite;

        private SpriteEffects flip = SpriteEffects.None;

        private Rectangle localBounds;
        private Rectangle idleBounds;
        private Rectangle jumpBounds;
        private Rectangle run0Bounds;
        private Rectangle runDownSlopeBoundFast;
        private Rectangle run1Bounds;
        private Rectangle wallRunBounds;
        private Rectangle wallSlideBoundsLeft;
        private Rectangle wallSlideBoundsRight;
        private Rectangle floorSlideBounds;
        private Rectangle crawlBounds;
        private Rectangle crawlStopBounds;
        private Rectangle wallOverBounds;
        private Rectangle fallBounds;
        private Rectangle fallNormalBounds;
        private Rectangle fallWallBounds;

        Vector2 previousPosition;
       

        Stopwatch stopWatch = new Stopwatch();
        float countdown = 3;
        float countRun = 1;

        //death reset values
        private Vector2 spawnPoint;
        private Vector2 previousSpawnPoint; 

        private Vector2 lastLedgePoint;
        private float fallDepth;

        //timer variables;
        float milliseconds;
        float seconds;
        float minutes;

        SpriteFont font;
        SpriteFont debugFont;

        int slideFrameCount;
        int wallSlideRightCount;
        int wallSlideLeftCount;

        private string animationPlaying;

        List<LiangBarsky> lines = new List<LiangBarsky>();

        // Gravity variables
        private const float gravityMax = 15.0f;
        private const float gravityAccel = 0.5f;

        // Movement variables 
        private const float playerAccerlation = 0.28f;
        private const float playerDeceleration = 0.8f;
        private const float playerMaxSpeed = 8.5f; // previous value was 6.4f
        private const float playerSuperSpeed = 12.25f; // previous value was 10.3f
        private const float playerFriction = 0.16f;


        //Player variables slope
        private const float playerGoDownSlope = 3.25f;
        private const float playerSpeedUpSlope = 12.0f; // previous speed up slope 7.0f;
        private const float playerSpeedDownSlope = 14.25f;
        private TileCollision preSlopeTypeSpeed;
        private bool speedSlopeBool;

        // player sliding variables
        private const float playerSlideFrictionSuper = 0.037f;
        private const float playerSlideFriction = 0.037f;
        private const float playerSlideFrictionUpSlope = 0.8f;
        private bool isSliding;
        private bool isSlidingDown;
        private TileCollision preSlopeType;
  


        //player crawling variables
        private bool isCrawling;
        private float underTileVariable;


        // Jump variables
        private const float normalJump = -9.5f;
        private const float smallJump = -7.5f;
        private bool jumping;
        private float jumpVariable;

        private float jumpTimer;

        // Jump grace period
        private const float graceTime = 83.5f;
        private float graceTimer;

        private const float slopeGraceTime = 83.5f;
        private float slopeGraceTimer;

        // Wall Jump variables
        private bool onRightWall;
        private bool onLeftWall;

        private float rightTimer;
        private float leftTimer;

        private float onWallTime = 33.4f;
        
        // Normal wall variables
        private bool normalRightWall;
        private bool normalLeftWall;

        private const float stickTimer = 700.0f;

        private float rightStickTimer;
        private float leftStickTimer;

        private const float wallJumpTime = 66.8f;

        private float wallJumpTimer;
        private bool initateJumpTimer;

        private bool timerStart;

        private const float jumpAccer = 50.1f;
        private float jumpAccerTimerRight = 0;
        private float jumpAccerTimerLeft = 0;

        private const float wallFriction = 0.6f;

        private bool stopMovement;

        private bool startCountDown;

        private bool falling;

        //Line checks for wall  jump
        LiangBarsky checkCharRight1;
        LiangBarsky checkCharRight2;
        LiangBarsky checkCharRight3;

        LiangBarsky checkCharLeft1;
        LiangBarsky checkCharLeft2;
        LiangBarsky checkCharLeft3;

      
        //Line check for sliding 

        private bool isUnderTile;

        private TileCollision previousTile;
        private LiangBarsky checkCharRoof1;
        private LiangBarsky checkCharRoof2;
        private LiangBarsky checkCharRoof3;

        private Vector2 movement;

        private InputState input = new InputState();

        ContentManager content;

        private float previousBottom;
        private float previousTop;
      
        private bool isHeadOnRoof;

        //wall climbing variable
        private const float wallClimbSpeed = -7.0f; // previous number -3.5f
        private const float floorCrawl = 3.5f;
        private float climbChecker;
        private bool climbLimt;

        private string count;

        private float playerRotation;

        private TileCollision previousTileSlope;

        Random example = new Random();


        public bool IsOnGround
        {
            get { return isOnGround; }
        }

        private bool isOnGround;
        private bool isOnSlope;

        private bool stopCameraMovement;

        public Vector2 PlayerPosition
        {
            get { return playerPosition; }
        }
        Vector2 playerPosition;

        public Vector2 PlayerVelocity
        {
            get { return playerVelocity; }
        }
        private Vector2 playerVelocity;


        public Rectangle PlayerRectangle
        {
            get
            {
                int left = (int)Math.Round(playerPosition.X - sprite.Origin.X) + localBounds.X;
                int top = (int)Math.Round(playerPosition.Y - sprite.Origin.Y - 1.0f) + localBounds.Y;

                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
            }
        }

        public bool DebugInformation
        {
            get { return debugInformation; }
        }
        private bool debugInformation;

        public bool StopTimer
        {
            get { return stopTimer; }
            set { stopTimer = value; }
        }
        private bool stopTimer;

        public HighTimeData PlayerData
        {
            get { return data; }
        }
        HighTimeData data = new HighTimeData();

        CollisionMask collisionMask = new CollisionMask();

        public Player(ContentManager content, Vector2 position, RunnerCamera camera, string levelName)
        {
            HighTime.TimeList.Clear();
            this.content = content;
            this.playerPosition = new Vector2(position.X, position.Y + 4);
            this.camera = camera;
            this.levelName = levelName;
            camera.Position = camera.Waypoints.Peek();

            LoadContent();
        }

        public void LoadContent()
        {
            playerRotation = 0.0f;

            collisionMask.Initialize(content);

            sprite = new AnimationPlayer();

            playerBox = content.Load<Texture2D>("Player");
            playerOrigin = new Vector2(playerBox.Width / 2, playerBox.Height / 2);

            block = content.Load<Texture2D>("square");

            font = content.Load<SpriteFont>("MenuAssets/gamefont");
            debugFont = content.Load<SpriteFont>("debugfont");

            AnimationPlayer player = new AnimationPlayer();

            idleAnimation = new AnimationData(content.Load<Texture2D>("Player/Idle"), 120, 48, true);
            jumpAnimation = new AnimationData(content.Load<Texture2D>("Player/Jump"), 60, 48, false);
            run0Animation = new AnimationData(content.Load<Texture2D>("Player/Run0"), 70, 48, true);
            run1Animation = new AnimationData(content.Load<Texture2D>("Player/Run1"), 84, 48, true);
            runDownSlopeAnimationFast = new AnimationData(content.Load<Texture2D>("Player/Run0"), 48, 48, true);
            wallRunAnimation = new AnimationData(content.Load<Texture2D>("Player/Wallrun"), 130, 48, false);
            wallSlideAnimation = new AnimationData(content.Load<Texture2D>("Player/WallSlide"), 100, 48, true);
            floorSlideAnimation = new AnimationData(content.Load<Texture2D>("Player/Slide"), 100, 48, false);
            crawlAnimation = new AnimationData(content.Load<Texture2D>("Player/Crawl"), 100, 48, true);
            crawlStopAnimation = new AnimationData(content.Load<Texture2D>("Player/CrawlStop"), 100, 48, true);
            wallOverAnimation = new AnimationData(content.Load<Texture2D>("Player/Wallover"), 200, 48, true);
            fallAnimation = new AnimationData(content.Load<Texture2D>("Player/Fall"), 100, 48, false);

            sprite.PlayAnimation(idleAnimation);

            int width = (int)(idleAnimation.FrameWidth * 0.5 - slideAdjustment);
            int left = (idleAnimation.FrameWidth - width) / 2;
            int height = (int)(idleAnimation.FrameHeight * 0.75);
            int top = idleAnimation.FrameHeight - height;

            idleBounds = new Rectangle(left, top, width, height);



            width = (int)(jumpAnimation.FrameWidth * 0.5 - slideAdjustment);
            left = (jumpAnimation.FrameWidth - width) / 2;
            height = (int)(run1Animation.FrameHeight * 0.75);
            top = jumpAnimation.FrameHeight - height;

            jumpBounds = new Rectangle(left, top, width, height);

            width = (int)(run0Animation.FrameWidth * 0.5 - 10);
            left = (run0Animation.FrameWidth - width) / 2;
            height = (int)(run1Animation.FrameHeight * 0.75);
            top = run0Animation.FrameHeight - height;

            run0Bounds = new Rectangle(left, top, width, height);

            width = (int)(runDownSlopeAnimationFast.FrameWidth * 0.5 - 10);
            left = (runDownSlopeAnimationFast.FrameWidth - width) / 2;
            height = (int)(runDownSlopeAnimationFast.FrameHeight * 0.75);
            top = runDownSlopeAnimationFast.FrameHeight - height;

            runDownSlopeBoundFast = new Rectangle(left, top, width, height);


            width = (int)(run1Animation.FrameWidth * 0.5 - slideAdjustment);
            left = (run1Animation.FrameWidth - width) / 2;
            height = (int)(run1Animation.FrameHeight * 0.75);
            top = run1Animation.FrameHeight - height;

            run1Bounds = new Rectangle(left, top, width, height);

            width = (int)(wallRunAnimation.FrameWidth * 0.5 - slideAdjustment);
            left = (wallRunAnimation.FrameWidth - width) / 2;
            height = (int)(run1Animation.FrameHeight * 0.75);
            top = wallRunAnimation.FrameHeight - height;

            wallRunBounds = new Rectangle(left, top, width, height);

            width = (int)(wallSlideAnimation.FrameWidth * 0.5 - slideAdjustment);
            left = (wallSlideAnimation.FrameWidth - width) / 2;
            height = (int)(run1Animation.FrameHeight * 0.5);
            top = wallSlideAnimation.FrameHeight - height;

            wallSlideBoundsLeft = new Rectangle(left, top, width, height);

            width = (int)(wallSlideAnimation.FrameWidth * 0.5 - slideAdjustment);
            left = (wallSlideAnimation.FrameWidth - width) / 2;
            height = (int)(wallSlideAnimation.FrameHeight);
            top = wallSlideAnimation.FrameHeight - height;

            wallSlideBoundsRight = new Rectangle(left, top, width, height);

            width = (int)(floorSlideAnimation.FrameWidth * 0.5 - slideAdjustment);
            left = (floorSlideAnimation.FrameWidth - width) / 2;
            height = (int)(floorSlideAnimation.FrameHeight * 0.5);
            top = floorSlideAnimation.FrameHeight - height;

            floorSlideBounds = new Rectangle(left, top, width, height);

           width = (int)(crawlAnimation.FrameWidth * 0.5);
           left = (crawlAnimation.FrameWidth - width) / 2;
           height = (int)(crawlAnimation.FrameHeight * 0.5);
           top = crawlAnimation.FrameHeight - height;

           crawlBounds = new Rectangle(left, top, width, height);

           width = (int)(crawlStopAnimation.FrameWidth * 0.5);
           left = (crawlStopAnimation.FrameWidth - width) / 2;
           height = (int)(crawlStopAnimation.FrameHeight * 0.5f);
           top = crawlStopAnimation.FrameHeight - height;

           crawlStopBounds = new Rectangle(left, top, width, height);

           width = (int)(wallOverAnimation.FrameWidth * 0.5 - slideAdjustment);
           left = (wallOverAnimation.FrameWidth - width) / 2;
           height = (int)(wallOverAnimation.FrameHeight * 0.3);
           top = wallOverAnimation.FrameHeight - height;

           wallOverBounds = new Rectangle(left, top, width, height);

           width = (int)(fallAnimation.FrameWidth * 0.55 - slideAdjustment);
           left = (fallAnimation.FrameWidth - width) / 2;
           height = (int)(fallAnimation.FrameHeight * 0.5);
           top = fallAnimation.FrameHeight - height;

           fallWallBounds = new Rectangle(left, top, width, height);

           fallBounds = fallWallBounds;

           width = (int)(fallAnimation.FrameWidth * 0.8 - slideAdjustment);
           left = (fallAnimation.FrameWidth - width) / 2;
           height = (int)(fallAnimation.FrameHeight * 0.5);
           top = fallAnimation.FrameHeight - height;

           fallNormalBounds = new Rectangle(left, top, width, height);

            localBounds = idleBounds;

           camera.CameraTrap = new Rectangle((int)PlayerRectangle.Right,
           PlayerRectangle.Bottom - 230, 200, 200);
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = gameTime.ElapsedGameTime.Milliseconds;

            if (initateJumpTimer == true && jumpTimer < 112) {
                jumpTimer += elapsed;
            }
            else{
                jumpTimer = 0;
                initateJumpTimer = false;
            }

            CountDown(gameTime);

            if (KeepOnSlope() == true) {
                graceTimer = graceTime;
            }
            else if (isOnGround == true && jumpTimer <= 0)
            {
                initateJumpTimer = false;
                jumpTimer = 0;
                playerVelocity.Y = 0;
                leftStickTimer = 0;
                rightStickTimer = 0;
                graceTimer = graceTime;
            }

            if (graceTimer > 0)
                graceTimer -= elapsed;

            if (graceTimer < 0)
                graceTimer = 0;
            
            if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.Q) == true &&
                input.LastKeyboardStates[0].IsKeyUp(Keys.Q) == true && debugInformation == false)
            {
                debugInformation = true;
            }
            else if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.Q) == true &&
              input.LastKeyboardStates[0].IsKeyUp(Keys.Q) == true && debugInformation == true)
            {
                debugInformation = false;
            }


            
            GameStartUp(gameTime);

            Vector2 previousVeclocity = playerVelocity;
            falling = false;

            if (isHeadOnRoof == true)
            {
                playerVelocity.Y = 0;
            }

            previousPosition = playerPosition;           

            input.Update();

            float deadZoneMagnitude = 0.25f;
            if (input.LeftStick(PlayerIndex.One).Length() < deadZoneMagnitude && 
               input.CurrentInputType == CurrentInput.GamePadCurrent)
            {
                movement = Vector2.Zero;
            }
            else
            {
                movement = input.LeftStick(PlayerIndex.One);

                if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.Right) == true || 
                    input.CurrentKeyboardStates[0].IsKeyDown(Keys.D) == true) {
                    movement.X = 1.0f;
                }
                if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.Left) == true || 
                         input.CurrentKeyboardStates[0].IsKeyDown(Keys.A) == true)
                {
                    movement.X = -1.0f;
                }
                if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.Up) == true ||
                        input.CurrentKeyboardStates[0].IsKeyDown(Keys.W) == true)
                {
                    movement.Y = 1.0f;
                }
                if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.Down) == true ||
                       input.CurrentKeyboardStates[0].IsKeyDown(Keys.S) == true)
                {
                    movement.Y = -1.0f;
                }
            }

            if (input.CurrentGamePadStates[0].IsConnected == true)
            {
                if (input.CurrentGamePadStates[0].DPad.Right == ButtonState.Pressed)
                {
                    movement.X = 1.0f;
                }
                if (input.CurrentGamePadStates[0].DPad.Left == ButtonState.Pressed)
                {
                    movement.X = -1.0f;
                }
                if (input.CurrentGamePadStates[0].DPad.Up == ButtonState.Pressed)
                {
                    movement.Y = 1.0f;
                }
                if (input.CurrentGamePadStates[0].DPad.Down == ButtonState.Pressed)
                {
                    movement.Y = -1.0f;
                }
            }

            if (playerVelocity.Y > 0.6f && rightTimer <= 0 && leftTimer <= 0 && isOnSlope == false 
                && isOnGround == false && isUnderTile == false)
            {
                aniState = AnimationState.Fall;
            }

            if (stopMovement == false)
            {
                if (walloverTakeaway == false) {
                    Movement(gameTime);
                }
              

                if (isUnderTile == false)
                    Jumping(gameTime);


                if (isOnSlope == false) {
                    playerVelocity.Y = MathHelper.Clamp(
                    playerVelocity.Y + gravityAccel, -gravityMax, gravityMax);
                }

                if (isOnGround == true) {
                    Sliding(gameTime);
                }
                ClimbWall();


                //if (Keyboard.GetState().IsKeyDown(Keys.W) == true)
                //{
                //    playerPosition.Y -= 2;
                //}

                //if (Keyboard.GetState().IsKeyDown(Keys.S) == true)
                //{
                //    playerPosition.Y += 2;
                //}
            }


            if (playerVelocity.Y < -previousVeclocity.Y && Math.Abs(playerVelocity.Y) > 1.0f)
            {
                falling = false;
            }
            else if (playerVelocity.Y > previousVeclocity.Y &&
               Math.Abs(playerVelocity.Y) > 2.0f || playerVelocity.Y >= gravityMax)
            {
                falling = true;
                initateJumpTimer = false;
                jumpTimer = 0;
            }

         
            if (onLeftWall == true && aniState != AnimationState.WallClimbLeft)
            {
                if (graceTimer <= 0 && previousAniState != AnimationState.WallOver && 
                    aniState != AnimationState.WallOver)  {
                    aniState = AnimationState.WallSlideLeft;
                }
            }
           
            if (onRightWall == true && aniState != AnimationState.WallClimbRight)
            {
                if (graceTimer <= 0 && previousAniState != AnimationState.WallOver && 
                    aniState != AnimationState.WallOver) {
                    aniState = AnimationState.WallSlideRight;
                }
            }

            playerVelocity = wallJump(gameTime, playerVelocity);


            if (wallover == false) {
                KillVelocity();
            }

            playerPosition += playerVelocity;
            playerPosition = new Vector2((float)Math.Round(playerPosition.X), (float)Math.Round(playerPosition.Y));


            if (particleEngines.Count > 0)
            {
                foreach (Particle particleEngine in particleEngines)
                {
                    particleEngine.Update();
                }  
            }
           
            HandleCollisions(gameTime);
            HandleWallCollision(gameTime);
            PlayerAnimation(gameTime);

            PlayerRotationOnSlope(gameTime);
        }

        private bool runningState()
        {
            if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.Q) && 
                input.LastKeyboardStates[0].IsKeyUp(Keys.Q) && runningStat == false)
            {
                runningStat = true;
            }
            else if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.Q) && 
                input.LastKeyboardStates[0].IsKeyUp(Keys.Q) && runningStat == true)
            {
                runningStat = false;
            }

            return runningStat;
        }

        private bool checkRunButton()
        {
            if (input.CurrentGamePadStates[0].Triggers.Right >= 0.5f &&
                input.LastGamePadStates[0].Triggers.Right < 0.5f &&
               input.CurrentInputType == CurrentInput.GamePadCurrent && 
                runButton == false)
            {
                runButton = true;
            }
            else if (input.CurrentGamePadStates[0].Triggers.Right >= 0.5f &&
                    input.LastGamePadStates[0].Triggers.Right < 0.5f &&
                   input.CurrentInputType == CurrentInput.GamePadCurrent &&
                    runButton == true)
            {
                runButton = false;
            }
            else if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.Z) && 
                    input.LastKeyboardStates[0].IsKeyUp(Keys.Z) && runButton == false)
            {
                runButton = true;
            }
            else if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.Z) &&
                   input.LastKeyboardStates[0].IsKeyUp(Keys.Z) && runButton == true)
            {
                runButton = false;
            }

            return runButton;
        }

        private void GameStartUp(GameTime gameTime)
        {
            if (input.IsButtonAPressed(PlayerIndex.One) == true && camera.Waypoints.Count > 0)
            {
                camera.Waypoints.Clear();
                camera.Position = playerPosition;
                camera.LockToTarget(playerVelocity, PlayerRectangle, 1280, 720);
            }

            if (camera.Waypoints.Count > 0)
            {
               stopMovement = true;
               camera.LinearNodeMovement(gameTime);
               camera.LockToTarget(playerVelocity, PlayerRectangle, 1280, 720);
            }
            else if(countdown <= 0)
            {
                stopMovement = false;

                if (stopCameraMovement == false) {
                    camera.LockToTarget(playerVelocity, PlayerRectangle, 1280, 720);
                }

                if(stopTimer == false)
                {
                    milliseconds += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (milliseconds >= 1000.0f)
                    {
                        seconds++;
                        milliseconds = 0;
                    }

                    if (seconds >= 60.0f)
                    {
                        minutes++;
                        seconds = 0;
                    }
                }
            }
            else if(startCountDown == false)
            {
                stopMovement = true;
                startCountDown = true;
            }
        }

        /// <summary>
        /// Called when this player reaches the level's exit.
        /// </summary>
        public void OnReachedExit()
        {
            stopWatch.Stop();
            stopMovement = true;

            string elapsedTime = String.Format("{0:00}:{1:00}.{2:000}",
            minutes, seconds, milliseconds);
            data.PlayerTime = elapsedTime;
 
       }

        private void Movement(GameTime gameTime)
        {
            if (isSliding == false)
            {
                if (movement.X < 0)
                {
                    if (playerVelocity.X > 0)
                    {
                        playerVelocity.X -= playerDeceleration;
                    }
                    else
                    {
                        playerVelocity.X -= playerAccerlation;
                    }
                }
                else if (movement.X > 0)
                {
                    if (playerVelocity.X < 0)
                    {
                        playerVelocity.X += playerDeceleration;
                    }
                    else
                    {
                        playerVelocity.X += playerAccerlation;
                    }
                }
                else
                {
                    playerVelocity.X -= playerFriction * Math.Sign(playerVelocity.X);
                }
            }

            Rectangle bounds = PlayerRectangle;
            Point tileInfo = TileInformation.GetTile(playerPosition.X, playerPosition.Y);
            TileCollision slopeType = CollisionRetrieve.GetCollision(tileInfo.X, tileInfo.Y);

            if (slopeType == TileCollision.Passable) {
                slopeType = preSlopeTypeSpeed;
            }

            if (Math.Abs(playerVelocity.X) <= playerMaxSpeed) {
                speedSlopeBool = false;
            }

            if (slopeType == TileCollision.LeftSlope && playerVelocity.X > 0.2f ||
                 slopeType == TileCollision.RightSlope && playerVelocity.X < -0.2f) {
                     speedSlopeBool = true;
            }

           
            if (slopeType == TileCollision.LeftSlope && playerVelocity.X < -0.2f ||
                slopeType == TileCollision.RightSlope && playerVelocity.X > 0.2f)
            {
                playerVelocity.X = MathHelper.Clamp(playerVelocity.X,
                  -playerSpeedUpSlope, playerSpeedUpSlope);
            }
            else if (input.RightTrigger(PlayerIndex.One) >= 0.5f &&
                  input.CurrentInputType == CurrentInput.GamePadCurrent)
            {
                playerVelocity.X = MathHelper.Clamp(playerVelocity.X,
               -playerMaxSpeed, playerMaxSpeed);
            }
            else if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.Z))
            {
                playerVelocity.X = MathHelper.Clamp(playerVelocity.X,
               -playerMaxSpeed, playerMaxSpeed);
            }
            else if (speedSlopeBool == true)
            {
                playerVelocity.X = MathHelper.Clamp(playerVelocity.X,
                                 -playerSpeedDownSlope, playerSpeedDownSlope);
            }
            else
            {
                playerVelocity.X = MathHelper.Clamp(playerVelocity.X,
                                  -playerSuperSpeed, playerSuperSpeed);
            }
            
              if (isOnGround == true && Math.Abs(playerVelocity.X) <= 0.2)
              {
                  aniState = AnimationState.Idle;
              }
              else if (isOnGround == true && Math.Abs(playerVelocity.X) < playerSuperSpeed)
              {
                  aniState = AnimationState.SlowRun;
              }
              else if ((isOnGround) && Math.Abs(playerVelocity.X) >= playerSpeedDownSlope)
              {
                  aniState = AnimationState.SlopeFastRun;
              }
              else if (isOnGround == true && Math.Abs(playerVelocity.X) >= playerMaxSpeed)
              {
                  aniState = AnimationState.FastRun;
              }
   
              preSlopeTypeSpeed = slopeType;
      }

        private void KillVelocity()
        {
            if (movement.X < 0 && onRightWall && falling == true)
            {
                playerVelocity.Y *= wallFriction;
            }
            else if (movement.X > 0 && onLeftWall && falling == true)
            {
                playerVelocity.Y *= wallFriction;
            }

            if ((movement.X >= 0 && normalLeftWall == true && isSliding == false))
            {
                playerVelocity.X = 0;
            }
            else if ((movement.X <= 0 && normalRightWall == true && isSliding == false))
            {
                playerVelocity.X = 0;
            }

            if ((movement.X >= 0 && onLeftWall == true && isSliding == false))
            {
                playerVelocity.X = 0;
            }
            else if ((movement.X <= 0 && onRightWall == true && isSliding == false))
            {
                playerVelocity.X = 0;
            }

            if (onLeftWall == false && onRightWall == false)
            {
                if ((movement.X >= 0 && upAgainstTileLeft == true && isSliding == false))
                {
                    playerVelocity.X = 0;
                }
                else if ((movement.X <= 0 && upAgainstTileRight == true && isSliding == false))
                {
                    playerVelocity.X = 0;
                }
            }
        }

        private void Sliding(GameTime gameTime)
        {
            float elapsed = gameTime.ElapsedGameTime.Milliseconds;

            isCrawling = false;

            if (isOnGround == true) {
                isSlidingDown = false;
            }

            if (underTileVariable < 0)
                underTileVariable = 0;

            if (isUnderTile == true)
            {
                underTileVariable = 83.5f;
            }

            if (underTileVariable > 0)
            {
                underTileVariable -= elapsed;
            }


            if (input.IsButtonBDown(PlayerIndex.One) == true || movement.Y > -0.5f &&
                input.CurrentInputType == CurrentInput.GamePadCurrent)
            {
                isSliding = true;
            }
            else if (movement.Y < -0.5f &&
               input.CurrentInputType == CurrentInput.GamePadCurrent)
            {
                isSliding = true;
            }
            else if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.S) == true &&
                     input.CurrentInputType == CurrentInput.KeyBoardCurrent)
            {
                isSliding = true;
            }
            else if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.Down) == true &&
                     input.CurrentInputType == CurrentInput.KeyBoardCurrent)
            {
                isSliding = true;
            }

            if (input.IsButtonBReleased(PlayerIndex.One) == true && movement.Y > -0.5f &&
               input.CurrentInputType == CurrentInput.GamePadCurrent)
            {
                isSliding = false;
            }
            else if (input.CurrentKeyboardStates[0].IsKeyUp(Keys.Down) == true &&
                    input.CurrentKeyboardStates[0].IsKeyUp(Keys.S) == true &&
                    input.CurrentInputType == CurrentInput.KeyBoardCurrent)
            {
                isSliding = false;
            }

            Point something1;
            something1 = TileInformation.GetTile(playerPosition.X, playerPosition.Y);


            TileCollision nextTile = TileCollision.Impassable;

            if (playerVelocity.X > 4.0f)
            {
                nextTile = CollisionRetrieve.GetCollision(something1.X + 1, something1.Y - 2);
            }
            else if (playerVelocity.X < -4.0f)
            {
                nextTile = CollisionRetrieve.GetCollision(something1.X - 1, something1.Y - 2);
            }


            if (underTileVariable > 0 && nextTile == TileCollision.Passable)
            {
                isSliding = true;
            }


            if (isSliding == true && isOnSlope == true)
            {
                aniState = AnimationState.Slide;

                  Rectangle bounds = PlayerRectangle;
                  Point tileInfo = TileInformation.GetTile(playerPosition.X, playerPosition.Y);
                  TileCollision slopeType = CollisionRetrieve.GetCollision(tileInfo.X, tileInfo.Y);

                  if (slopeType == TileCollision.Passable)
                      slopeType = preSlopeType;

                  if (slopeType == TileCollision.LeftSlope   || slopeType == TileCollision.RightSlope) {
                      isSlidingDown = true;
                  }

                  if (isSlidingDown == true && slopeType == TileCollision.RightSlope && 
                      Math.Abs(playerVelocity.X) <= 0.35f) {
                      playerVelocity.X = -playerGoDownSlope;
                  }
                  if (isSlidingDown == true && slopeType == TileCollision.LeftSlope &&
                       Math.Abs(playerVelocity.X) <= 0.35f)
                  {
                      playerVelocity.X = playerGoDownSlope;
                  } 


                  if (playerVelocity.X > 0.2f && slopeType == TileCollision.RightSlope) {
                      playerVelocity.X -= playerSlideFrictionUpSlope * Math.Sign(playerVelocity.X);
                  }
                  else if (playerVelocity.X < -0.2f && slopeType == TileCollision.LeftSlope) {
                      playerVelocity.X -= playerSlideFrictionUpSlope * Math.Sign(playerVelocity.X);
                  }
                  else {
                      playerVelocity.X -= playerSlideFrictionSuper * Math.Sign(playerVelocity.X);
                  }

                  preSlopeType = slopeType;
            }
            else if (isSliding == true && isOnGround == true)
            {
                aniState = AnimationState.Slide;

                if (playerVelocity.X > playerMaxSpeed)
                {
                    playerVelocity.X -= playerSlideFrictionSuper * Math.Sign(playerVelocity.X);
                }
                else
                {
                    playerVelocity.X -= playerSlideFriction * Math.Sign(playerVelocity.X);
                }
            }
            else if (underTileVariable > 0 && isOnGround == true)
            {
                isCrawling = true;

                aniState = AnimationState.Crawl;

                 if (movement.X < 0)
                 {
                     playerVelocity.X = -floorCrawl;
                 }
                 else if (movement.X > 0)
                 {
                     playerVelocity.X = floorCrawl;
                 }

                 if (Math.Abs(playerVelocity.X) <= 0.2f) {
                     aniState = AnimationState.CrawlStop;
                 }
            }
        }

        private void Jumping(GameTime gameTime)
        {
            float elapsed = gameTime.ElapsedGameTime.Milliseconds;

            if (input.IsButtonAPressed(PlayerIndex.One) == true && 
               input.CurrentInputType == CurrentInput.GamePadCurrent) 
            {
                jumpVariable = 83.1f;
            }
            else if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.Space) == true &&
                     input.LastKeyboardStates[0].IsKeyUp(Keys.Space) == true)
            {
                jumpVariable = 83.1f;
            }

            if (input.IsButtonAReleased(PlayerIndex.One) == true &&
               input.CurrentInputType == CurrentInput.GamePadCurrent)
            {
                    jumpVariable = 0;
            }
            else if (input.CurrentKeyboardStates[0].IsKeyUp(Keys.Space) == true && 
                     input.CurrentInputType == CurrentInput.KeyBoardCurrent)
            {
                jumpVariable = 0;
            }

            if (jumpVariable > 0)
            {
                jumpVariable -= elapsed;
            }

            if (jumpVariable < 0)
                jumpVariable = 0;


            if (jumpVariable > 0 && jumping == false && isSliding == false)
            {
                if (graceTimer > 0)
                {
                    jumping = true;
                    playerVelocity.Y = normalJump;
                    aniState = AnimationState.Jump;
                    initateJumpTimer = true;
                }
            }
           
            if (jumpVariable == 0 && jumping == true && isSliding == false)
            {
                if (playerVelocity.Y < smallJump)
                {
                    playerVelocity.Y = smallJump;
                }

                jumping = false;
                jumpVariable = 0;
            }
        }

        private void CountDown(GameTime gameTime)
        {
            if (startCountDown == true)
            {
                count = "1";

                countdown -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (countdown > 1)
                {
                    count = Math.Round(countdown).ToString();
                }

                if (countdown <= 0)
                {
                    count = "Run !";
                    countRun -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                if (countRun <= 0)
                {
                    startCountDown = false;
                }
            }   
        }

        private Vector2 wallJump(GameTime gameTime, Vector2 velocity)
        {
            float elapsed = gameTime.ElapsedGameTime.Milliseconds;

            Vector2 vectorJump = new Vector2(0.0f, -14.0f);

            if (graceTimer <= 0)
            {
                if (movement.X > 0 && onRightWall == true && timerStart == false
                && isOnGround == false)
                {
                    timerStart = true;
                    rightStickTimer = stickTimer;
                }

                if (movement.X < 0 && onLeftWall == true && timerStart == false
                    && isOnGround == false)
                {
                    timerStart = true;
                    leftStickTimer = stickTimer;
                }
            }

            if (input.IsButtonAPressed(PlayerIndex.One) == true && 
               input.CurrentInputType == CurrentInput.GamePadCurrent) {
                jumping = false;
                wallJumpTimer = wallJumpTime;
            }
            else if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.Space) == true) {
                if(input.LastKeyboardStates[0].IsKeyUp(Keys.Space) == true)
                {
                      jumping = false;
                      wallJumpTimer = wallJumpTime;
                }
            }

            if (wallJumpTimer > 0)
                wallJumpTimer -= elapsed;

            if (rightStickTimer > 0)
            {
                rightStickTimer -= elapsed;
                velocity.X = 0;
            }

            if (leftStickTimer > 0)
            {
                leftStickTimer -= elapsed;
                velocity.X = 0;
            }


            if (graceTimer > 0)
            {
                timerStart = false;
                wallJumpTimer = 0;
                leftStickTimer = 0;
                rightStickTimer = 0;
            }

            if (wallJumpTimer > 0 && rightStickTimer > 0)
            {
                wallJumpTimer = 0;
                rightStickTimer = 0;
                jumpAccerTimerRight = jumpAccer;
                aniState = AnimationState.WallJumpRight;

                timerStart = false;
                velocity.Y = vectorJump.Y;
            }

            if (wallJumpTimer > 0 && leftStickTimer > 0)
            {
                wallJumpTimer = 0;
                leftStickTimer = 0;
                jumpAccerTimerLeft = jumpAccer;
                aniState = AnimationState.WallJumpLeft;

                timerStart = false;
                velocity.Y = vectorJump.Y;
            }


            if (jumpAccerTimerRight > 0)
            {
                jumpAccerTimerLeft = 0;
                jumpAccerTimerRight -= elapsed;
                velocity.X += 8.0f;
            }

            if (jumpAccerTimerLeft > 0)
            {
                jumpAccerTimerRight = 0;
                jumpAccerTimerLeft -= elapsed;
                velocity.X -= 8.0f;
            }

            return velocity;
        }

        public void Reset(Vector2 position)
        {
            playerPosition = new Vector2(position.X, position.Y + 37);
            playerVelocity = Vector2.Zero;

            camera.Position += camera.Position - playerPosition * 0.5f;

            stopCameraMovement = false;

            camera.CameraTrap = new Rectangle((int)PlayerRectangle.Right,
                PlayerRectangle.Bottom - 230, 200, 200);
        }

        public void FallDeath(float width, float height)
        {
            if (isOnGround == true && stopCameraMovement == false || onLeftWall == true 
                || onRightWall == true)
            {
                fallDepth = 0;
                stopCameraMovement = false;
            }
            else if (isOnGround == false)
            {
                fallDepth += Math.Sign(playerVelocity.Y);
            }

            if (fallDepth > 40)
            {
                stopCameraMovement = true;
  
                if (PlayerPosition.Y > camera.Position.Y + 820) {
                    Reset(spawnPoint);
                    fallDepth = 0;
                }
                else if (isOnGround == true)
                {
                    Reset(previousSpawnPoint);
                    fallDepth = 0;
                }       
            }
        }

        private void ClimbWall()
        {

            if (movement.X < 0 && onLeftWall == true && isOnGround == false && leftStickTimer <= 0) {
                return;
            }

            if (movement.X > 0 && onRightWall == true && isOnGround == false && rightStickTimer <= 0) {
                return;
            }

            wallover = false;

            if (wallover == false && isOnGround == true)
                walloverTakeaway = false;

            if (isOnGround == true)
            {
                climbLimt = true;
                climbChecker = 0;
            }

            if (jumpAccerTimerRight > 0 || jumpAccerTimerLeft > 0)
            {
                climbLimt = true;
                climbChecker = 0;
            }

            if (movement.Y > 0.5f && onRightWall == true && climbLimt == true)
            {
                climbChecker += wallClimbSpeed;
                playerVelocity.Y = wallClimbSpeed;

                Point something1;
                something1 = TileInformation.GetTile(playerPosition.X - 20, playerPosition.Y - 65);
                TileCollision topTile = CollisionRetrieve.GetCollision(something1.X - 1, something1.Y);

                if (climbLimt == true){ 
                    aniState = AnimationState.WallClimbRight;
                }

                if (topTile == TileCollision.Passable && onRightWall == true)
                {
                    playerVelocity.X = 0f;

                    wallover = true;
                    walloverTakeaway = true;

                    aniState = AnimationState.WallOver;
                    playerVelocity.Y -=  1.5f;
                    playerVelocity.X += -1.0f;
                }
            }
            else if (movement.Y > 0.5f && onLeftWall == true && climbLimt == true)
            { 
               climbChecker += wallClimbSpeed;
                playerVelocity.Y = wallClimbSpeed;

                Point something1;
                something1 = TileInformation.GetTile(playerPosition.X - 20, playerPosition.Y - 40);
                TileCollision topTile = CollisionRetrieve.GetCollision(something1.X + 1, something1.Y);

                if (climbLimt == true) {
                    aniState = AnimationState.WallClimbLeft;
                }
              
                if (topTile == TileCollision.Passable && onLeftWall == true)
                {
                    playerVelocity.X = 0f;

                    wallover = true;
                    walloverTakeaway = true;

                    aniState = AnimationState.WallOver;
                    playerVelocity.Y -= 1.5f;
                    playerVelocity.X += 1.0f;
                }
            }

            if (climbChecker < -180.0f)
            {
                climbChecker = 0;
                climbLimt = false;

                if (onLeftWall == true && 
                    previousAniState != AnimationState.WallOver && 
                    aniState != AnimationState.WallOver)
                {
                    aniState = AnimationState.WallSlideLeft;
                }
                else if (onRightWall == true &&
                      previousAniState != AnimationState.WallOver && 
                      aniState != AnimationState.WallOver)
                {
                    aniState = AnimationState.WallSlideRight;
                }
            }

            if (movement.Y < 0.5f )
            {
                if (onLeftWall == true && IsOnGround == false && 
                    previousAniState != AnimationState.WallOver) {
                    aniState = AnimationState.WallSlideLeft;
                }
                else if (onRightWall == true && IsOnGround == false && 
                    previousAniState != AnimationState.WallOver)
                {
                    aniState = AnimationState.WallSlideRight;
                }
            }

            if (Math.Abs(climbChecker) > 0.0f && IsOnGround == false 
                && onLeftWall == false && onRightWall == false && climbLimt == false) {
                    aniState = AnimationState.Fall;
            }
        }

        private void PlayerAnimation(GameTime gameTime)
        {
            float elapsed = gameTime.ElapsedGameTime.Milliseconds;

            if (leftTimer < 0)
                leftTimer = 0;

            if (rightTimer < 0)
                rightTimer = 0;

            if (walloverTimer < 0)
                walloverTimer = 0;

            if (wallover == true)
                walloverTimer = wallOverTime;

            if (onLeftWall == true)
                leftTimer = onWallTime;

            if (onRightWall == true)
                rightTimer = onWallTime;

            if (walloverTimer > 0)
                walloverTimer -= elapsed;

            if (leftTimer > 0)
                leftTimer -= elapsed;

            if (rightTimer > 0)
                rightTimer -= elapsed;

            if (isOnGround == true || isOnSlope == true) {
                jumpParticle = true;
            }

            if (onLeftWall == true)
                jumpLeftParticle = true;

            if (onRightWall == true)
                jumpRightParticle = true;

            Point currentTilePoint;
            TileCollision currentTile;

            currentTilePoint = TileInformation.GetTile(playerPosition.X, playerPosition.Y + 5);
            currentTile = CollisionRetrieve.GetCollision(currentTilePoint.X, currentTilePoint.Y);

            if (currentTile == TileCollision.Passable) {
                currentTile = previousTile;
            }


            if (aniState == AnimationState.Idle)
            {
                animationPlaying = "idle";
                localBounds = idleBounds;
                sprite.PlayAnimation(idleAnimation);
            }

            if (aniState == AnimationState.SlowRun)
            {
                animationPlaying = "slowRun";
                localBounds = run1Bounds;
                sprite.PlayAnimation(run1Animation);

                if (sprite.FrameIndex == 2 && sprite.SameFrame == false ||
                    sprite.FrameIndex == 5 && sprite.SameFrame == false)
                {
                    AnimationData sprites = new AnimationData(content.Load<Texture2D>("Player/Particles/footpuff"), 120, 16, true);
                    AnimationPlayer player = new AnimationPlayer();

                    if (playerVelocity.X > 0.2f && currentTile == TileCollision.RightSlope)
                    {
                        particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X - 25, playerPosition.Y + 30),
                        Vector2.Zero, -MathHelper.PiOver4, 0f, Color.White, SpriteEffects.None, 1.0f, 30));
                    }
                    else if (playerVelocity.X < -0.2f && currentTile == TileCollision.RightSlope)
                    {
                        particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X + 25, playerPosition.Y - 15),
                       Vector2.Zero, -MathHelper.PiOver4, 0f, Color.White, SpriteEffects.FlipHorizontally, 1.0f, 30));
                    }
                    else if (playerVelocity.X > 0.2f && currentTile == TileCollision.LeftSlope)
                    {
                        particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X - 25, playerPosition.Y - 15),
                        Vector2.Zero, MathHelper.PiOver4, 0f, Color.White, SpriteEffects.None, 1.0f, 30));
                    }
                    else if (playerVelocity.X < -0.2f && currentTile == TileCollision.LeftSlope)
                    {
                        particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X + 25, playerPosition.Y + 30),
                       Vector2.Zero, MathHelper.PiOver4, 0f, Color.White, SpriteEffects.FlipHorizontally, 1.0f, 30));
                    }
                     else if (playerVelocity.X > 2.0f)
                    {
                        particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X - 10, playerPosition.Y - 1),
                        Vector2.Zero, 0f, 0f, Color.White, SpriteEffects.None, 1.0f, 30));
                    }
                    else if (playerVelocity.X < -2.0f)
                    {
                        particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X + 10, playerPosition.Y - 1),
                       Vector2.Zero, 0f, 0f, Color.White, SpriteEffects.FlipHorizontally, 1.0f, 30));
                    }
                }
            }

            if (aniState == AnimationState.FastRun)
            {
                animationPlaying = "fastRun";
                localBounds = run0Bounds;
                sprite.PlayAnimation(run0Animation);

                if (sprite.FrameIndex == 2 && sprite.SameFrame == false ||
                    sprite.FrameIndex == 5 && sprite.SameFrame == false)
                {
                    AnimationData sprites = new AnimationData(content.Load<Texture2D>("Player/Particles/runpuff"), 170, 16, true);
                    AnimationPlayer player = new AnimationPlayer();

                    if (playerVelocity.X > 0.2f)
                    {
                        particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X - 25, playerPosition.Y - 1),
                        Vector2.Zero, 0f, 0f, Color.White,SpriteEffects.None,1.0f, 30));
                    }
                    else if (playerVelocity.X < -0.2f)
                    {
                        particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X + 25, playerPosition.Y - 1),
                       Vector2.Zero, 0f, 0f, Color.White, SpriteEffects.FlipHorizontally,1.0f, 30));
                    }
                }
            }

            if (aniState == AnimationState.SlopeFastRun)
            {
                animationPlaying = "slopeFastRun";
                localBounds = runDownSlopeBoundFast;
                sprite.PlayAnimation(runDownSlopeAnimationFast);

                if (sprite.FrameIndex == 2 && sprite.SameFrame == false ||
                    sprite.FrameIndex == 5 && sprite.SameFrame == false)
                {
                    AnimationData sprites = new AnimationData(content.Load<Texture2D>("Player/Particles/runpuff"), 170, 16, true);
                    AnimationPlayer player = new AnimationPlayer();

                    if (playerVelocity.X > 0.2f && currentTile == TileCollision.RightSlope)
                    {
                        particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X - 25, playerPosition.Y + 40),
                        Vector2.Zero, -MathHelper.PiOver4, 0f, Color.White, SpriteEffects.None, 1.0f, 30));
                    }
                    else if (playerVelocity.X < -0.2f && currentTile == TileCollision.RightSlope)
                    {
                        particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X + 25, playerPosition.Y - 20),
                       Vector2.Zero, -MathHelper.PiOver4, 0f, Color.White, SpriteEffects.FlipHorizontally, 1.0f, 30));
                    }
                    else if (playerVelocity.X > 0.2f && currentTile == TileCollision.LeftSlope)
                    {
                        particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X - 25, playerPosition.Y - 20),
                        Vector2.Zero, MathHelper.PiOver4, 0f, Color.White, SpriteEffects.None, 1.0f, 30));
                    }
                    else if (playerVelocity.X < -0.2f && currentTile == TileCollision.LeftSlope)
                    {
                        particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X + 25, playerPosition.Y + 40),
                       Vector2.Zero, MathHelper.PiOver4, 0f, Color.White, SpriteEffects.FlipHorizontally, 1.0f, 30));
                    }
                    else if (playerVelocity.X > 0.2f)
                    {
                        particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X - 25, playerPosition.Y - 2),
                        Vector2.Zero, 0f, 0f, Color.White, SpriteEffects.None, 1.0f, 30));
                    }
                    else if (playerVelocity.X < -0.2f)
                    {
                        particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X + 25, playerPosition.Y - 2),
                       Vector2.Zero, 0f, 0f, Color.White, SpriteEffects.FlipHorizontally, 1.0f, 30));
                    }
                }
            }

            if (aniState == AnimationState.Slide)
            {
                animationPlaying = "floorSlide";
                localBounds = floorSlideBounds;
                sprite.PlayAnimation(floorSlideAnimation);

                slideFrameCount++;

                if (slideFrameCount > 10)
                {
                    AnimationData sprites = new AnimationData(content.Load<Texture2D>("Player/Particles/slidepuff"), 120, 32, true);
                    AnimationPlayer player = new AnimationPlayer();


                    if (Math.Abs(playerVelocity.X) > 0.3f && currentTile == TileCollision.RightSlope)
                    {
                        particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X, playerPosition.Y + 1),
                      Vector2.Zero, -MathHelper.PiOver4, 0f, Color.White, SpriteEffects.None, 1.0f, 30));
                    }
                    else if (Math.Abs(playerVelocity.X) > 0.3f && currentTile == TileCollision.LeftSlope)
                    {
                        particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X, playerPosition.Y + 1),
                      Vector2.Zero, MathHelper.PiOver4, 0f, Color.White, SpriteEffects.None, 1.0f, 30));
                    }
                    else if (playerVelocity.X > 0.3f)
                    {
                        particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X, playerPosition.Y - 1),
                        Vector2.Zero, 0f, 0f, Color.White, SpriteEffects.None, 1.0f, 30));
                    }
                    else if (playerVelocity.X < -0.3f) {
                        particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X, playerPosition.Y - 1),
                        Vector2.Zero, 0f, 0f, Color.White, SpriteEffects.FlipHorizontally, 1.0f, 30));
                    }

                    slideFrameCount = 0;
                }
            }

            if (aniState == AnimationState.Fall)
            {
                animationPlaying = "fall";
                localBounds = fallBounds;
                sprite.PlayAnimation(fallAnimation);
            }

            if (aniState == AnimationState.Jump)
            {
                animationPlaying = "jump";
                localBounds = jumpBounds;
                sprite.PlayAnimation(jumpAnimation);


                if (sprite.FrameIndex == 0 && jumpParticle == true)
                {
                    jumpParticle = false;

                    AnimationData sprites = new AnimationData(content.Load<Texture2D>("Player/Particles/jumppuff"), 120, 32, true);
                    AnimationPlayer player = new AnimationPlayer();

                    Point tileDetection = TileInformation.GetTile(playerPosition.X, playerPosition.Y);
                    TileCollision bottomTile = CollisionRetrieve.GetCollision(tileDetection.X, tileDetection.Y);

                    for (int i = 0; i < 3; i++)
                    {
                        tileDetection = TileInformation.GetTile(playerPosition.X, playerPosition.Y + (i * 32));
                        bottomTile = CollisionRetrieve.GetCollision(tileDetection.X, tileDetection.Y);

                        if (bottomTile != TileCollision.Passable)
                            break;
                    }


                    if (bottomTile == TileCollision.LeftSlope)
                    {
                        particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X, playerPosition.Y + 3),
                                            Vector2.Zero, MathHelper.PiOver4, 0f, Color.White, SpriteEffects.None, 1.0f, 30));
                    }
                    else if (bottomTile == TileCollision.RightSlope) 
                    {
                        particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X, playerPosition.Y + 3),
                                            Vector2.Zero, -MathHelper.PiOver4, 0f, Color.White, SpriteEffects.None, 1.0f, 30));
                    }
                    else if (bottomTile == TileCollision.Impassable || bottomTile == TileCollision.NormalTile)
                    {
                        particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X, playerPosition.Y + 9),
                        Vector2.Zero, 0f, 0f, Color.White, SpriteEffects.None, 1.0f, 30));
                    }
                    else
                    {
                        particleEngines.Add(new Particle(sprites, player, new Vector2(lastLedgePoint.X, lastLedgePoint.Y + 3),
                        Vector2.Zero, 0f, 0f, Color.White, SpriteEffects.None, 1.0f, 30));
                    }
                }
            }

            if (aniState == AnimationState.WallClimbLeft)
            {
                animationPlaying = "wallClimbLeft";
                localBounds = wallRunBounds;
                sprite.PlayAnimation(wallRunAnimation);
            }

            if (aniState == AnimationState.WallClimbRight)
            {
                animationPlaying = "wallClimbRight";
                localBounds = wallRunBounds;
                sprite.PlayAnimation(wallRunAnimation);
            }

            if (aniState == AnimationState.WallSlideLeft)
            {
                animationPlaying = "wallSlideLeft";
                localBounds = wallSlideBoundsLeft;
                sprite.PlayAnimation(wallSlideAnimation);

                wallSlideLeftCount++;

                AnimationData sprites = new AnimationData(content.Load<Texture2D>("Player/Particles/wallslidepuff"), 120, 16, true);
                AnimationPlayer player = new AnimationPlayer();

                if (wallSlideLeftCount > 10 && playerVelocity.Y > 3.0f)
                {
                    particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X, playerPosition.Y - 50),
                    Vector2.Zero, 0f, 0f, Color.White, SpriteEffects.None, 1.0f, 45));

                    wallSlideLeftCount = 0;
                }
            }

            if (aniState == AnimationState.WallSlideRight)
            {
                animationPlaying = "wallSlideRight";
                localBounds = wallSlideBoundsRight;
                sprite.PlayAnimation(wallSlideAnimation);

                wallSlideRightCount++;

                AnimationData sprites = new AnimationData(content.Load<Texture2D>("Player/Particles/wallslidepuff"), 120, 16, true);
                AnimationPlayer player = new AnimationPlayer();

                if (wallSlideRightCount > 10 && playerVelocity.Y > 3.0f) {
                   particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X, playerPosition.Y - 50),
                   Vector2.Zero, 0f, 0f, Color.White, SpriteEffects.FlipHorizontally, 1.0f, 45));

                   wallSlideRightCount = 0;
                }
            }

            if (aniState == AnimationState.WallJumpLeft)
            {
                animationPlaying = "wallJumpLeft";
                localBounds = jumpBounds;
                sprite.PlayAnimation(jumpAnimation);

                if (jumpLeftParticle == true && sprite.FrameIndex == 0)
                {
                    jumpLeftParticle = false;

                    AnimationData sprites = new AnimationData(content.Load<Texture2D>("Player/Particles/jumppuff"), 170, 32, true);
                    AnimationPlayer player = new AnimationPlayer();

                    particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X - 20, playerPosition.Y),
                      Vector2.Zero, 1.57f, 0f, Color.White, SpriteEffects.FlipVertically, 1.0f, 30));
                }
            }

            if (aniState == AnimationState.WallJumpRight)
            {
                animationPlaying = "wallJumpRight";
                localBounds = jumpBounds;
                sprite.PlayAnimation(jumpAnimation);

                if (jumpRightParticle == true && sprite.FrameIndex == 0)
                {
                    jumpRightParticle = false;

                    AnimationData sprites = new AnimationData(content.Load<Texture2D>("Player/Particles/jumppuff"), 170, 32, true);
                    AnimationPlayer player = new AnimationPlayer();

                    particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X - 18, playerPosition.Y),
                      Vector2.Zero, 1.57f, 0f, Color.White, SpriteEffects.None, 1.0f, 30));
                }
            }

            if (aniState == AnimationState.WallOver)
            {
                animationPlaying = "wallOver";
                localBounds = wallOverBounds;
                sprite.PlayAnimation(wallOverAnimation);
            }

            if (aniState == AnimationState.Crawl)
            {
                animationPlaying = "crawl";
                localBounds = crawlBounds;
                sprite.PlayAnimation(crawlAnimation);
            }

            if (aniState == AnimationState.CrawlStop)
            {
                animationPlaying = "crawlStop";
                localBounds = crawlStopBounds;
                sprite.PlayAnimation(crawlStopAnimation);
            }

            if (aniState == AnimationState.Idle && previousAniState == AnimationState.Fall ||
                aniState == AnimationState.FastRun && previousAniState == AnimationState.Fall ||
                 aniState == AnimationState.SlowRun && previousAniState == AnimationState.Fall)
            {
                AnimationData sprites = new AnimationData(content.Load<Texture2D>("Player/Particles/footpuff"), 120, 16, true);
                AnimationPlayer player = new AnimationPlayer();

                if (flip == SpriteEffects.None)
                {
                    particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X - 20, playerPosition.Y - 1),
                    Vector2.Zero, 0f, 0f, Color.White, SpriteEffects.None, 1.0f, 30));
                }
                else if (flip == SpriteEffects.FlipHorizontally)
                {
                    particleEngines.Add(new Particle(sprites, player, new Vector2(playerPosition.X + 20, playerPosition.Y - 1),
                   Vector2.Zero, 0f, 0f, Color.White, SpriteEffects.None, 1.0f, 30));
                }
            }

            previousTile = currentTile;
            previousAniState = aniState;
        }

        private void HandleWallCollision(GameTime gameTime)
        {
            upAgainstTileRight = false;
            upAgainstTileLeft = false;

            onLeftWall = false;
            onRightWall = false;

            normalRightWall = false;
            normalLeftWall = false;

            isUnderTile = false;

            Rectangle bounds = PlayerRectangle;
            Point something1 = TileInformation.GetTile(playerPosition.X, playerPosition.Y);

            TileCollision collisionRight = CollisionRetrieve.GetCollision(something1.X - 1, something1.Y - 1);
            TileCollision collisionLeft = CollisionRetrieve.GetCollision(something1.X + 1, something1.Y - 1);


            TileCollision againstTileRight = CollisionRetrieve.GetCollision(something1.X - 1, something1.Y - 2);
            TileCollision againstTileLeft = CollisionRetrieve.GetCollision(something1.X + 1, something1.Y - 2);

            TileCollision collisionTop = CollisionRetrieve.GetCollision(something1.X, something1.Y - 2);
            TileCollision collisionTop2 = CollisionRetrieve.GetCollision(something1.X, something1.Y - 2);

            if (collisionRight == TileCollision.Impassable || collisionRight == TileCollision.NormalTile)
            {
                Rectangle tileBounds = CollisionRetrieve.GetBounds(something1.X - 1, something1.Y);
                Vector2 depth = RectangleExtensions.GetIntersectionDepth(bounds, tileBounds);

                checkCharRight1 = new LiangBarsky(new Vector2(tileBounds.Right, tileBounds.Center.Y), new Vector2(tileBounds.Right + 1, tileBounds.Center.Y));

                lines.Add(checkCharRight1);

                if (collisionRight == TileCollision.Impassable)
                {
                    onRightWall = checkCharRight1.Intersect(bounds);
                }

                if (collisionRight == TileCollision.NormalTile)
                {
                    normalRightWall = checkCharRight1.Intersect(bounds);
                }

                checkCharRight2 = new LiangBarsky(new Vector2(tileBounds.Right, tileBounds.Center.Y), new Vector2(tileBounds.Right + 1, tileBounds.Center.Y + 20));

                lines.Add(checkCharRight2);

                if (collisionRight == TileCollision.Impassable && onRightWall == false)
                {
                    onRightWall = checkCharRight2.Intersect(bounds);
                }

                if (collisionRight == TileCollision.NormalTile && normalRightWall == false)
                {
                    normalRightWall = checkCharRight2.Intersect(bounds);
                }

                checkCharRight3 = new LiangBarsky(new Vector2(tileBounds.Right, tileBounds.Center.Y), new Vector2(tileBounds.Right + 1, tileBounds.Center.Y - 20));

                lines.Add(checkCharRight3);

                if (collisionRight == TileCollision.Impassable && onRightWall == false)
                {
                    onRightWall = checkCharRight3.Intersect(bounds);
                }

                if (collisionRight == TileCollision.NormalTile && normalRightWall == false)
                {
                    normalRightWall = checkCharRight3.Intersect(bounds);
                }
            }

            if (collisionLeft == TileCollision.Impassable || collisionLeft == TileCollision.NormalTile)
            {
                Rectangle tileBounds = CollisionRetrieve.GetBounds(something1.X + 1, something1.Y);

                checkCharLeft1 = new LiangBarsky(new Vector2(tileBounds.Left, tileBounds.Center.Y), new Vector2(tileBounds.Left - 1, tileBounds.Center.Y));

                lines.Add(checkCharLeft1);

                if (collisionLeft == TileCollision.Impassable)
                {
                    onLeftWall = checkCharLeft1.Intersect(bounds);
                }

                if (collisionLeft == TileCollision.NormalTile)
                {
                    normalLeftWall = checkCharLeft1.Intersect(bounds);
                }

                checkCharLeft2 = new LiangBarsky(new Vector2(tileBounds.Left, tileBounds.Center.Y), new Vector2(tileBounds.Left - 1, tileBounds.Center.Y + 20));

                if (collisionLeft == TileCollision.Impassable && onLeftWall == false)
                {
                    onLeftWall = checkCharLeft2.Intersect(bounds);
                }

                if (collisionLeft == TileCollision.NormalTile && normalLeftWall == false)
                {
                    normalLeftWall = checkCharLeft2.Intersect(bounds);
                }

                checkCharLeft3 = new LiangBarsky(new Vector2(tileBounds.Left, tileBounds.Center.Y), new Vector2(tileBounds.Left - 1, tileBounds.Center.Y - 20));

                if (collisionLeft == TileCollision.Impassable && onLeftWall == false)
                {
                    onLeftWall = checkCharLeft3.Intersect(bounds);
                }

                if (collisionLeft == TileCollision.NormalTile && normalLeftWall == false)
                {
                    normalLeftWall = checkCharLeft3.Intersect(bounds);
                }
            }

            if (collisionTop == TileCollision.Impassable || collisionTop == TileCollision.NormalTile)
            {
                Rectangle tileBounds = CollisionRetrieve.GetBounds(something1.X, something1.Y - 2);

                checkCharRoof1 = new LiangBarsky(new Vector2(tileBounds.Center.X, tileBounds.Bottom), new Vector2(tileBounds.Center.X, tileBounds.Bottom + 15));
                isUnderTile = checkCharRoof1.Intersect(bounds);


                if (isUnderTile == false) {
                    checkCharRoof2 = new LiangBarsky(new Vector2(tileBounds.Center.X - 16, tileBounds.Bottom), new Vector2(tileBounds.Center.X - 16, tileBounds.Bottom + 15));
                    isUnderTile = checkCharRoof2.Intersect(bounds);
                }


                if (isUnderTile == false) {
                    checkCharRoof3 = new LiangBarsky(new Vector2(tileBounds.Center.X + 16, tileBounds.Bottom), new Vector2(tileBounds.Center.X + 16, tileBounds.Bottom + 15));
                    isUnderTile = checkCharRoof3.Intersect(bounds);
                }
             
            }
            else if (collisionTop2 == TileCollision.Impassable || collisionTop2 == TileCollision.NormalTile)
            {
                Rectangle tileBounds = CollisionRetrieve.GetBounds(something1.X, something1.Y - 2);

                checkCharRoof1 = new LiangBarsky(new Vector2(tileBounds.Center.X + 16, tileBounds.Bottom), new Vector2(tileBounds.Center.X, tileBounds.Bottom + 15));
                isUnderTile = checkCharRoof1.Intersect(bounds);
            }


            if (againstTileRight == TileCollision.Impassable || againstTileRight == TileCollision.NormalTile)
            {
                Rectangle tileBounds = CollisionRetrieve.GetBounds(something1.X - 1, something1.Y - 2);

                checkAgainstTileLineRight = new LiangBarsky(new Vector2(tileBounds.Right - 32, tileBounds.Center.Y + 10), new Vector2(tileBounds.Right + 1, tileBounds.Center.Y + 10));
                upAgainstTileRight = checkAgainstTileLineRight.Intersect(bounds);                   
            }

            if (againstTileLeft == TileCollision.Impassable || againstTileLeft == TileCollision.NormalTile)
            {
                Rectangle tileBounds = CollisionRetrieve.GetBounds(something1.X + 1, something1.Y - 2);

                checkAgainstTileLineLeft = new LiangBarsky(new Vector2(tileBounds.Left + 32, tileBounds.Center.Y + 10), new Vector2(tileBounds.Left - 1, tileBounds.Center.Y + 10));
                upAgainstTileLeft = checkAgainstTileLineLeft.Intersect(bounds);
            }
        }

        private void HandleCollisions(GameTime gameTime)
        {
            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle bounds = PlayerRectangle;
            int leftTile = (int)Math.Floor((float)bounds.Left / Tile.Width);
            int rightTile = (int)Math.Ceiling(((float)bounds.Right / Tile.Width)) - 1;
            int topTile = (int)Math.Floor((float)bounds.Top / Tile.Height);
            int bottomTile = (int)Math.Ceiling(((float)bounds.Bottom / Tile.Height)) - 1;

            TileCollision collision = CollisionRetrieve.GetCollision(0, 0);

            // Reset flag to search for ground collision.
            isOnGround = false;
            isOnSlope = false;
            isHeadOnRoof = false;

            // For each potentially colliding tile,
            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    // If this tile is collidable,
                    collision = CollisionRetrieve.GetCollision(x, y);

                    if (collision != TileCollision.Passable)
                    {
                        // Determine collision depth (with direction) and magnitude.
                        Rectangle tileBounds = CollisionRetrieve.GetBounds(x, y);
                        Vector2 depth = RectangleExtensions.GetIntersectionDepth(bounds, tileBounds);

                        Vector2 playerSensor = new Vector2(bounds.Center.X, bounds.Bottom); 

                            if (depth != Vector2.Zero)
                            {
                                float absDepthX = Math.Abs(depth.X);
                                float absDepthY = Math.Abs(depth.Y);

                                if (collision == TileCollision.RightSlope)
                                {
                                    int qX = (int)playerSensor.X - tileBounds.Left;
                                    int qY = (int)playerSensor.Y - tileBounds.Top;

                                    if (jumpTimer > 0) {
                                        return;
                                    }

                                    if (qX + qY > Tile.Width)
                                    {
                                        isOnGround = true;
                                        isOnSlope = true;
                                        playerPosition.Y = tileBounds.Bottom - qX + 1;
                                    }
                                }
                                else if (collision == TileCollision.LeftSlope)
                                {
                                    int qX = (int)playerSensor.X - tileBounds.Left;
                                    int qY = (int)playerSensor.Y - tileBounds.Top;

                                    if (jumpTimer > 0) {
                                        return;
                                    }

                                    if ((qX - qY < 0 && qX > 0 && qY > 0))
                                    {
                                        isOnGround = true;
                                        isOnSlope = true;
                                        playerPosition.Y = tileBounds.Top + qX + 1;
                                    }
                                }
                                // Resolve the collision along the shallow axis.
                                else if (absDepthY < absDepthX)
                                {
                                    // If we crossed the top of a tile, we are on the ground.
                                    if (previousBottom <= tileBounds.Top)
                                    {
                                        isOnGround = true;

                                        camera.MoveTrapUp(tileBounds.Top);

                                        if (collision == TileCollision.NormalTile)
                                        {
                                            previousSpawnPoint = spawnPoint;
                                            spawnPoint = new Vector2(tileBounds.X + 16, tileBounds.Y - 32);
                                        }

                                        lastLedgePoint = new Vector2(tileBounds.X + 16, tileBounds.Y);
                                    }

                                    if (previousTop >= tileBounds.Bottom)
                                    {
                                        isHeadOnRoof = true;
                                    }

                                    // Ignore platforms, unless we are on the ground.
                                    if (collision == TileCollision.Impassable || collision == TileCollision.NormalTile)
                                    {
                                        // Resolve the collision along the Y axis.
                                        playerPosition = new Vector2(playerPosition.X, playerPosition.Y + depth.Y);
                                        //Console.WriteLine("Hit Ground" + example.Next());

                                        // Perform further collisions with the new bounds.
                                        bounds = PlayerRectangle;
                                    }
                                }
                                else
                                {
                                    if (collision == TileCollision.Impassable || collision == TileCollision.NormalTile) // Ignore platforms.
                                    {
                                        // Resolve the collision along the X axis.
                                        playerPosition = new Vector2(playerPosition.X + depth.X, playerPosition.Y);

                                        // Perform further collisions with the new bounds.
                                        bounds = PlayerRectangle;
                                    }
                                }
                            }
                    }
             
                }
            }


            if (collision != TileCollision.Passable)
                previousCollision = collision;

            // Save the new bounds bottom and top.
            previousTop = bounds.Top;
            previousBottom = bounds.Bottom;
        }


        private bool KeepOnSlope()
        {
            if (jumping == true) {
                return false;
            }

               Point tileDetection = TileInformation.GetTile(playerPosition.X, playerPosition.Y);
               TileCollision currentTile = CollisionRetrieve.GetCollision(tileDetection.X, tileDetection.Y);

               for (int i = 0; i < 3; i++)
               {
                   tileDetection = TileInformation.GetTile(playerPosition.X, playerPosition.Y + (i * 32));
                   currentTile = CollisionRetrieve.GetCollision(tileDetection.X, tileDetection.Y);

                   if (currentTile == TileCollision.RightSlope ||
                       currentTile == TileCollision.LeftSlope)
                       break;
               }

               if (currentTile == TileCollision.Passable && isOnSlope == true)
                   currentTile = previousTileSlope;

            if (playerVelocity.X < -1.0f && (currentTile == TileCollision.RightSlope) && playerVelocity.Y >= 0)
            {
                playerVelocity.Y = Math.Abs(playerVelocity.X);
                playerPosition.Y += 2;
                previousTileSlope = currentTile;
                return true;
            }
            else if (playerVelocity.X > 1.0f && (currentTile == TileCollision.LeftSlope) && playerVelocity.Y >= 0)
            {
                playerVelocity.Y = Math.Abs(playerVelocity.X);
                playerPosition.Y += 2;
                previousTileSlope = currentTile;
                return true;
            }

            return false;
        }

        private void PlayerRotationOnSlope(GameTime gameTime)
        {
            float elapsed = gameTime.ElapsedGameTime.Milliseconds;

            Point currentTilePoint;
            TileCollision currentTile;

            if (isOnSlope) {
                slopeGraceTimer = slopeGraceTime;
            }


            if (slopeGraceTimer > 0)
                slopeGraceTimer -= elapsed;

            if (slopeGraceTimer < 0)
                slopeGraceTimer = 0;

            currentTilePoint = TileInformation.GetTile(playerPosition.X, playerPosition.Y + 10);
            currentTile = CollisionRetrieve.GetCollision(currentTilePoint.X, currentTilePoint.Y);

            if (aniState == AnimationState.SlowRun && slopeGraceTimer > 0 ||
                aniState == AnimationState.FastRun && slopeGraceTimer > 0 ||
                aniState == AnimationState.SlopeFastRun && slopeGraceTimer > 0 ||
                aniState == AnimationState.Slide && slopeGraceTimer > 0)
            {
                if (currentTile == TileCollision.LeftSlope)
                {
                    playerRotation = MathHelper.PiOver4;
                }
                else if (currentTile == TileCollision.RightSlope)
                {
                    playerRotation = -MathHelper.PiOver4;
                }
            }
            else
            {
                playerRotation = 0;
            }
        }



        private void DebugDisplay(SpriteBatch spriteBatch)
        {
            string wallState = " ";

            if (onLeftWall == true || normalLeftWall == true)
            {
                wallState = "Left";
            }
            else if (onRightWall == true || normalRightWall == true)
            {
                wallState = "Right";
            }
            else
            {
                wallState = "None";
            }

            Vector2 currentVel = new Vector2((int)playerVelocity.X, (int)playerVelocity.Y);

            string playerPs = "player Pos" + playerPosition.ToString();
            string cameraPositon = "cam Pos" + camera.Position.ToString();
            string playerVel = "Velocity" + currentVel.ToString();
            string animation = "Animation: " + animationPlaying;


            spriteBatch.DrawString(debugFont, cameraPositon, new Vector2(camera.Position.X + 1000,
                         camera.Position.Y + 10), Color.Black);

            spriteBatch.DrawString(debugFont, playerPs, new Vector2(camera.Position.X + 1000,
                         camera.Position.Y + 30), Color.Black);

            spriteBatch.DrawString(debugFont, playerVel, new Vector2(camera.Position.X + 1000,
                        camera.Position.Y + 50), Color.Black);

            spriteBatch.DrawString(debugFont, "Side On: " + wallState, new Vector2(camera.Position.X + 1000,
                        camera.Position.Y + 70), Color.Black);

            spriteBatch.DrawString(debugFont, animation, new Vector2(camera.Position.X + 1000,
                        camera.Position.Y + 90), Color.Black);

            spriteBatch.DrawString(debugFont,"level Name: " + levelName, new Vector2(camera.Position.X + 1000,
                       camera.Position.Y + 110), Color.Black);

            PolyDebug.DrawLineSegment(spriteBatch, checkCharRoof1.StartPosition, checkCharRoof1.EndPosition, Color.Red, 2);
            PolyDebug.DrawLineSegment(spriteBatch, checkCharRoof2.StartPosition, checkCharRoof2.EndPosition, Color.Red, 2);
            PolyDebug.DrawLineSegment(spriteBatch, checkCharRoof3.StartPosition, checkCharRoof3.EndPosition, Color.Red, 2);

            PolyDebug.DrawLineSegment(spriteBatch, checkCharRight1.StartPosition, checkCharRight1.EndPosition, Color.Red, 2);
            PolyDebug.DrawLineSegment(spriteBatch, checkCharRight2.StartPosition, checkCharRight2.EndPosition, Color.Red, 2);
            PolyDebug.DrawLineSegment(spriteBatch, checkCharRight3.StartPosition, checkCharRight3.EndPosition, Color.Red, 2);

            PolyDebug.DrawLineSegment(spriteBatch, checkCharLeft1.StartPosition, checkCharLeft1.EndPosition, Color.Red, 2);
            PolyDebug.DrawLineSegment(spriteBatch, checkCharLeft2.StartPosition, checkCharLeft2.EndPosition, Color.Red, 2);
            PolyDebug.DrawLineSegment(spriteBatch, checkCharLeft3.StartPosition, checkCharLeft3.EndPosition, Color.Red, 2);

            PolyDebug.DrawLineSegment(spriteBatch, checkAgainstTileLineRight.StartPosition, checkAgainstTileLineRight.EndPosition, Color.Red, 2);
            PolyDebug.DrawLineSegment(spriteBatch, checkAgainstTileLineLeft.StartPosition, checkAgainstTileLineLeft.EndPosition, Color.Red, 2);

            spriteBatch.Draw(block, PlayerRectangle, Color.White);

            //spriteBatch.Draw(block, camera.CameraTrap, Color.White);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Format and display the TimeSpan value. 
            string elapsedTime = String.Format("{0:00}:{1:00}.{2:000}",
             minutes, seconds, milliseconds);

            if (playerVelocity.X < -0.2 && onLeftWall == false && onRightWall == false)
            {
                flip = SpriteEffects.FlipHorizontally;
            }
            else if (playerVelocity.X > 0.2 && onLeftWall == false && onRightWall == false)
            {
                flip = SpriteEffects.None;
            }

            spriteBatch.Begin(SpriteSortMode.Immediate,
                             BlendState.AlphaBlend,
                              SamplerState.PointWrap, null, null, null,
                             camera.TransformMatrix);

            if (debugInformation == true){
                DebugDisplay(spriteBatch);
            }

            if (isOnSlope == true) {
                sprite.Draw(gameTime, spriteBatch, new Vector2(playerPosition.X, playerPosition.Y), playerRotation, flip, Color.White);
            }
            else{
                sprite.Draw(gameTime, spriteBatch, playerPosition, playerRotation, flip, Color.White);
            }

           

            if (HighTime.TimeList.Count <= 0)
            {
                spriteBatch.DrawString(font, elapsedTime, new Vector2(camera.Position.X + 150,
                          camera.Position.Y + 50), Color.White);
            }
            else
            {
                float currentTime = float.Parse(Regex.Replace(elapsedTime, "[ :.-]+", ""));
                float topTime = float.Parse(Regex.Replace(HighTime.TimeList[0].PlayerTime, "[ :.-]+", ""));

                if (currentTime < topTime + 2000)
                {
                    spriteBatch.DrawString(font, elapsedTime, new Vector2(camera.Position.X + 150,
                        camera.Position.Y + 50), Color.Green);
                }
                else if (currentTime < topTime + 5000)
                {
                    spriteBatch.DrawString(font, elapsedTime, new Vector2(camera.Position.X + 150,
                        camera.Position.Y + 50), Color.Yellow);
                }
                else
                {
                    spriteBatch.DrawString(font, elapsedTime, new Vector2(camera.Position.X + 150,
                       camera.Position.Y + 50), Color.Red);
                }
            }
          
            if(startCountDown == true && count != null) {
                spriteBatch.DrawString(font, count, new Vector2(camera.Position.X + 150,
                            camera.Position.Y + 150), Color.White);
            }

            if (particleEngines.Count > 0)
            {
                for (int i = 0; i < particleEngines.Count; i++)
                {
                    particleEngines[i].Draw(spriteBatch, gameTime);

                    if (particleEngines[i].TTL <= 0)
                    {
                        particleEngines.RemoveAt(i);
                    }
                }
            }
         
            spriteBatch.End(); 
        }
    }
}