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
        private Enemy _enemy;

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

            _enemy = new Enemy(
                new Vector2(700, 200),
                new Vector2(40, 65)
            );



            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _backround = Content.Load<Texture2D>("images/backround");

            _squareTexture = new Texture2D(GraphicsDevice, 1, 1);
            _squareTexture.SetData(new[] { Color.Beige });

            Texture2D playerTexture = new Texture2D(GraphicsDevice, 1, 1);
            playerTexture.SetData(new[] { Color.Beige });
            _player.LoadContent(playerTexture);

            Texture2D enemyTexture = new Texture2D(GraphicsDevice, 1, 1);
            enemyTexture.SetData(new[] { Color.Beige });
            _enemy.LoadContent(enemyTexture);
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

            _enemy.Update(deltaTime);

            ResolveCollisions();
            
            if ((_player.Position.Y + _player.Size.Y) >= _ground)
            {
                _player.Velocity.Y = 0;
                _player.Position.Y = _ground - _player.Size.Y;
            }

            if ((_enemy.Position.Y + _enemy.Size.Y) >= _ground)
            {
                _enemy.Velocity.Y = 0;
                _enemy.Position.Y = _ground - _enemy.Size.Y;
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
            for (int i = 0; i < _platforms.Length; ++i)
            {

                bool isCollidingLeft = (_player.Position.X + _player.Size.X) > _platforms[i].Left;
                bool isCollidingTop = (_player.Position.Y + _player.Size.Y) > _platforms[i].Top;
                bool isCollidingRight = _player.Position.X < _platforms[i].Right;
                bool isCollidingBottom = _player.Position.Y < _platforms[i].Bottom;
                bool isColliding = isCollidingLeft && isCollidingTop && isCollidingRight && isCollidingBottom;


                if (isColliding)
                {
                    if ((isCollidingLeft || isCollidingRight) && (!isCollidingTop && !isCollidingBottom))
                    {
                        _player.Velocity.X *= -1;
                    }

                    if (isCollidingBottom)
                    {
                        _player.Velocity.Y *= -1;
                    }

                    if (isCollidingTop)
                    {
                        _player.Velocity.Y = 0;
                        _player.Position.Y = _platforms[i].Top - _player.Size.Y;
                    }
                }


            }
        }
    }
}
