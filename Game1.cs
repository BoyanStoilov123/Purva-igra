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
        private Texture2D _platformTexture;

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
            _platforms[0] = new Rectangle(220, 590, 180, 90);
            _platforms[1] = new Rectangle(490, 490, 180, 90);
            _platforms[2] = new Rectangle(710, 400, 180, 90);
        }

        protected override void Initialize()
        {
            _ground = 690;

            _player = new Player(
                new Vector2(50, 335),
                new Vector2(90, 90)
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
            _platformTexture = Content.Load<Texture2D>("images/Platform 1");

            Texture2D playerTexture = Content.Load<Texture2D>("images/main-character-sqr");
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
                _spriteBatch.Draw(_platformTexture, _platforms[i], Color.RosyBrown);
            }

            _player.Draw(_spriteBatch);

            _enemy.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ResolveCollisions()
        {
            for (int i = 0; i < _platforms.Length; i++)
            {
                Vector2 collisionData = GetCollisionData(_player.Collider, _platforms[i]);
                if (collisionData == Vector2.Zero)
                    continue;

                _player.Position += collisionData;
                if (collisionData.X != 0)
                {
                    _player.Velocity.X = 0;
                }
                else
                {
                    if (collisionData.Y < 0)
                    {
                        _player.Velocity.Y = 0;
                    }
                    else
                    {
                        _player.Velocity.Y = 0.1f;
                    }
                }
            }
        }

        private Vector2 GetCollisionData(Rectangle a, Rectangle b)
        {
            Vector2 result = Vector2.Zero;
            if (a.Intersects(b))
            {
                Rectangle overlap = Rectangle.Intersect(a, b);
                if (overlap.Width < overlap.Height)
                {
                    int direction = a.Center.X < b.Center.X ? -overlap.Width : overlap.Height;
                    result.X = direction;
                }
                else
                {   
                    int direction = a.Center.Y < b.Center.Y ? -overlap.Height : overlap.Width;
                    result.Y = direction;
                }
            }
            return result;
        }
    }
}
