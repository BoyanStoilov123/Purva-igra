using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Purva_igra
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _squareTexture;
        private float _ground;
        private Vector2 _screenSize;

        private Texture2D _backround;

        private Player _player;

        private Rectangle[] _platforms;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _screenSize = new Vector2(1280, 720);
            _graphics.PreferredBackBufferWidth = (int)_screenSize.X;
            _graphics.PreferredBackBufferHeight = (int)_screenSize.Y;

            _platforms = new Rectangle[3];
            _platforms[0] = new Rectangle(220, 590, 150, 30);
            _platforms[1] = new Rectangle(420, 520, 100, 30);
            _platforms[2] = new Rectangle(620, 470, 70, 30);
        }

        protected override void Initialize()
        {
            _ground = 690;

            _player = new Player(
                new Vector2 (50, 335),
                new Vector2 (40, 65)
            );
          

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _backround = Content.Load<Texture2D>("images/backround");

            _squareTexture = new Texture2D(GraphicsDevice, 1, 1);
            _squareTexture.SetData(new[] { Color.Beige });
        }

        protected override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboard = Keyboard.GetState();

            if (gamePad.Buttons.Back == ButtonState.Pressed
                || keyboard.IsKeyDown(Keys.Escape))
                Exit();

            Vector2 direction = new Vector2();
            if (keyboard.IsKeyDown(Keys.A))
            {
                direction.X = -1;
            }

            if (keyboard.IsKeyDown(Keys.D))
            {
                direction.X = 1;
            }

            if (keyboard.IsKeyDown(Keys.Space) && (_player.Velocity.Y == 0))
            {
                _player.Jump();
            }
            
            _player.Update(deltaTime);
            _player.SetDirection(direction);

            ResolveCollisions();
            
            if ((_player.Position.Y + _player.Size.Y) >= _ground)
            {
                _player.Velocity.Y = 0;
                _player.Position.Y = _ground - _player.Size.Y;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(
               _backround, Vector2.Zero, Color.White);

            for (int i = 0; i < _platforms.Length; ++i)
            {
                _spriteBatch.Draw(_squareTexture, _platforms[i], Color.RosyBrown);
            }
            
            _spriteBatch.Draw(
                _squareTexture,
                new Rectangle(
                    (int)_player.Position.X,
                    (int)_player.Position.Y,
                    (int)_player.Size.X,
                    (int)_player.Size.Y),
                Color.Beige);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ResolveCollisions()
        {
            bool isCollidingLeft = (_player.Position.X + _player.Size.X) > _platforms[0].Left;
            bool isCollidingTop = (_player.Position.Y + _player.Size.Y) > _platforms[0].Top;
            bool isCollidingRight = _player.Position.X < _platforms[0].Right; 
            bool isCollidingBottom = _player.Position.Y < _platforms[0].Bottom;


            if (isCollidingLeft && isCollidingTop)
            {
                _player.Position.X = _platforms[0].Left - _player.Size.X;
                _player.Position.Y = _platforms[0].Top - _player.Size.Y;
                _player.Velocity.Y = 0;
            }


        }

    }
}
