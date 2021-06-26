using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monocraft_Client_NET.Events;
using Monocraft_Client_NET.Handlers.GameLogic;
using Monocraft_Client_NET.Input;
using Monocraft_Client_NET.Interfaces;
using Monocraft_Client_NET.Rendering;
using Monocraft_Client_NET.Rendering.Mesh;
using Monocraft_Client_NET.Rendering.Renderer;
using Monocraft_Core_NET.Model;
using Monocraft_Protocol_NET;
using Monocraft_Protocol_NET.Data.Enums;
using Monocraft_Protocol_NET.Packets.Serverbound.Play;
using System;
using System.Collections.Generic;

namespace Monocraft_Client_NET
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MonocraftGame : Game, IMouseMoveListener, IKeyboardKeyListener
    {

        public static MonocraftGame Instance { get; private set; }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private MouseHandler _mouseHandler;
        private KeyboardHandler _keyboardHandler;
        private GameHandler _gameHandler;

        public List<IInputListener> InputListener = new List<IInputListener>();
        public List<IGameListener> GameListener = new List<IGameListener>();

        public BlockRenderer BlockRenderer;
        public EntityRenderer EntityRenderer;
        public BlockyModelRenderer BlockyModelRenderer;
        public BlockClusterRenderer BlockClusterRenderer;

        public Camera Camera;

        public Client Client = new Client();

        public TimeSpan SendRotation = new TimeSpan();
        public TimeSpan Current = new TimeSpan();

        public ResourceModel TNT;
        float timer = 1;

        public static TextureManager TextureManager { get; private set; } = new TextureManager();
        public static ModelManager ModelManager { get; private set; } = new ModelManager();

        public static float FrameRate;

        public MonocraftGame()
        {

            Instance = this;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _mouseHandler = new MouseHandler(this);
            _keyboardHandler = new KeyboardHandler(this);
            _gameHandler = new GameHandler(this);

            Components.Add(_mouseHandler);
            Components.Add(_keyboardHandler);
            Components.Add(_gameHandler);

            Orium.RegisterListener(new Listener(this), this);
            Orium.RegisterListener(this, this);
            

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            var centerX = GraphicsDevice.Viewport.Width / 2;
            var centerY = GraphicsDevice.Viewport.Height / 2;
            Mouse.SetPosition(centerX, centerY);

            //_mouseHandler.Scroll += OnMouseComponentScroll;
            _mouseHandler.OnMove += OnMouseMove;
            //_mouseHandler.ButtonDown += OnMouseComponentButtonDown;
            //_mouseHandler.ButtonUp += OnMouseComponentButtonUp;
            _keyboardHandler.OnKeyDown += OnKeyboardKeyDown;
            _keyboardHandler.OnKeyUp += OnKeyboardKeyUp;
            //GamePadComponent.ButtonDown += OnGamePadButtonDown;
            //GamePadComponent.ButtonUp += OnGamePadButtonUp;

            _gameHandler.OnEntityDamage += OnEntityDamage;

            Camera = new Camera(GraphicsDevice);
            BlockRenderer = new BlockRenderer();
            EntityRenderer = new EntityRenderer();
            BlockyModelRenderer = new BlockyModelRenderer();
            BlockClusterRenderer = new BlockClusterRenderer();

            base.Initialize();
        }

        private void OnEntityDamage(object sender, EntityDamageEventArgs e)
        {
            foreach (IGameListener gameListener in GameListener)
            {
                if (gameListener is IEntityListener)
                {
                    if (gameListener is IEntityDamageListener)
                    {
                        IEntityDamageListener entityDamageListener = gameListener as IEntityDamageListener;
                        entityDamageListener?.OnEntityDamageEvent(e);
                    }
                }
            }
        }

        private void OnKeyboardKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            foreach (IInputListener inputListener in InputListener)
            {
                if (inputListener is IKeyboardListener)
                {
                    if (inputListener is IKeyboardKeyListener)
                    {
                        IKeyboardKeyListener keyboardKeyListener = inputListener as IKeyboardKeyListener;
                        keyboardKeyListener?.OnKeyUp(e);
                    }
                }
            }
        }

        private void OnKeyboardKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            foreach (IInputListener inputListener in InputListener)
            {
                if (inputListener is IKeyboardListener)
                {
                    if (inputListener is IKeyboardKeyListener)
                    {
                        IKeyboardKeyListener keyboardKeyListener = inputListener as IKeyboardKeyListener;
                        keyboardKeyListener?.OnKeyDown(e);
                    }
                }
            }
        }

        private void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            foreach (IInputListener inputListener in InputListener)
            {
                if (inputListener is IMouseListener)
                {
                    if (inputListener is IMouseMoveListener)
                    {
                        IMouseMoveListener mouseMoveListener = inputListener as IMouseMoveListener;
                        mouseMoveListener?.OnMouseMoveEvent(e);
                    }
                }
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            TextureManager.LoadContent(Content);
            ModelManager.LoadContent(Content);

            BlockRenderer.Effect = Content.Load<Effect>("bin/Shader/TextureShader");
            EntityRenderer.Effect = Content.Load<Effect>("bin/Shader/DiffuseColorShader");
            BlockyModelRenderer.Effect = Content.Load<Effect>("bin/Shader/TextureShader");
            BlockClusterRenderer.Effect = Content.Load<Effect>("bin/Shader/TextureShader");
            // TODO: use this.Content to load your game content here
            
            if (TextureManager.Loaded)
            {
                Client.LoadContent();
                Client.BlockRenderer = BlockRenderer;
                Client.EntityRenderer = EntityRenderer;
                Client.BlockClusterRenderer = BlockClusterRenderer;
                Client.BlockyModelRenderer = BlockyModelRenderer;
                Client.Camera = Camera;
                Client.Connect("localhost", 25566);
                Client.Login("Sameplayer");

                if (Client.ClientConnectionState == ClientConnectionState.Disconnected)
                {
                    Exit();
                }
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer -= elapsed;
            if (timer < 0)
            {
                timer = 1;
                Window.Title = $"{(int)FrameRate}";
            }

            Current = gameTime.TotalGameTime;

            Camera.Update(gameTime);
            Client.Update(gameTime);



            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            FrameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var rasterState = new RasterizerState
            {
                CullMode = CullMode.None,
                FillMode = FillMode.WireFrame
            };

            GraphicsDevice.RasterizerState = rasterState;

            Client.Draw(spriteBatch, gameTime);
            //BlockyModelRenderer.DrawModel(new BlockyModelMesh(ModelManager.GetModel("oak_planks")), new Vector3(-114, 3, 191), Camera.View, Camera.Projection, gameTime);
            BlockRenderer.DrawModel(Cube, Camera.View, Camera.Projection, gameTime);
            //BlockClusterRenderer.DrawModel(new BlockClusterModel(Vector3.Zero, new Vector3(5, 1, 1), "oak_planks"), new Vector3(-114, 1, 185), Camera.View, Camera.Projection, gameTime);
            BlockClusterRenderer.DrawModel(new BlockClusterModel(Vector3.Zero, new Vector3(16, 16, 16), "oak_planks"), new Vector3(-128, 16, 176), Camera.View, Camera.Projection, gameTime);

            base.Draw(gameTime);
        }

        public void OnMouseMoveEvent(MouseMoveEventArgs args)
        {

            if (IsActive)
            {

                if (Client.ConnectionState == ConnectionState.Play && Client.ConfirmTeleportation)
                {

                    Camera.RotateX(args.DeltaY);
                    Camera.RotateY(-args.DeltaX);

                    if (Current - SendRotation >= new TimeSpan(0, 0, 0, 0, 50))
                    {
                        PacketStream packetStream = new PacketStream();
                        packetStream.WritePacket(new PlayerRotation(-MathHelper.ToDegrees(Camera.RotationY), MathHelper.ToDegrees(Camera.RotationX), true), 256);
                        SendRotation = Current;
                        Client.SendMessage(packetStream.ToPacket());
                    }
                }

            }
            
        }

        public void OnKeyDown(KeyboardKeyEventArgs args)
        {
            if (IsActive)
            {

                if (Client.ConnectionState == ConnectionState.Play && Client.ConfirmTeleportation)
                {

                    //Camera.RotateX(args.DeltaY);
                    //Camera.RotateY(-args.DeltaX);

                    /*if (Current - SendRotation >= new TimeSpan(0, 0, 0, 0, 50))
                    {
                        PacketStream packetStream = new PacketStream();
                        packetStream.WritePacket(new PlayerRotation(-MathHelper.ToDegrees(Camera.RotationY), MathHelper.ToDegrees(Camera.RotationX), true), 256);
                        SendRotation = Current;
                        Client.SendMessage(packetStream.ToPacket());
                    }*/
                }

            }
        }

        public void OnKeyUp(KeyboardKeyEventArgs args)
        {
            if (IsActive)
            {

                if (Client.ConnectionState == ConnectionState.Play && Client.ConfirmTeleportation)
                {

                    //Camera.RotateX(args.DeltaY);
                    //Camera.RotateY(-args.DeltaX);

                    /*if (Current - SendRotation >= new TimeSpan(0, 0, 0, 0, 50))
                    {
                        PacketStream packetStream = new PacketStream();
                        packetStream.WritePacket(new PlayerRotation(-MathHelper.ToDegrees(Camera.RotationY), MathHelper.ToDegrees(Camera.RotationX), true), 256);
                        SendRotation = Current;
                        Client.SendMessage(packetStream.ToPacket());
                    }*/
                }

            }
        }
    }
}
