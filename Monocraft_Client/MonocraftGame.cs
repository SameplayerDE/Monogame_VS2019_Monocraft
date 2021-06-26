using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monocraft_Client_NET.Events;
using Monocraft_Client_NET.Handlers.GameLogic;
using Monocraft_Client_NET.Input;
using Monocraft_Client_NET.Interfaces;
using Monocraft_Client_NET.Rendering;
using Monocraft_Client_NET.Rendering.Renderer;
using Monocraft_Protocol.Data.Enums;
using System;
using System.Collections.Generic;

namespace Monocraft_Client_NET
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MonocraftGame : Game
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

        public Camera Camera;

        public CubeMesh Cube = new CubeMesh();

        public Client Client = new Client();

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

            new Listener(this);

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

            Client.BlockRenderer = BlockRenderer;
            Client.Camera = Camera;
            Client.Connect("localhost", 25566);
            Client.Login("Sameplayer");

            if (Client.ClientConnectionState == ClientConnectionState.Disconnected)
            {
                Exit();
            }

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
            BlockRenderer.Effect = Content.Load<Effect>("Shader/DiffuseShader");
            // TODO: use this.Content to load your game content here
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Client.Draw(spriteBatch, gameTime);
            //BlockRenderer.DrawModel(Cube, Camera.View, Camera.Projection, gameTime);

            base.Draw(gameTime);
        }
    }
}
