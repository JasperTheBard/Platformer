using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;
using MonoGame.Extended.ViewportAdapters;
using System.Collections;

namespace Game1
{
    
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        Player player = new Player(); //Creates an instance of our player in this class

        Camera2D camera = null; //Creates an instance of our players
        TiledMap map = null; //Creates an instance of a Tilted map
        TiledMapRenderer mapRendered = null; // Creates an Instance of what makes a tilted map 

        TiledMapTileLayer collisionLayer;
        public ArrayList allCollisionTiles = new ArrayList();
        public sprite[,] levelGrid;

        public int tileHeight = 0;
        public int levelTileWidth = 0;
        public int levelTileHeight = 0;

        public Rectangle myMap;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //graphics.PreferredBackBufferHeight = 900;
            //graphics.PreferredBackBufferWidth = 1600;
        }

        protected override void Initialize()
        {
            myMap.X = 0;
            myMap.Y = 0;
            myMap.Width = 5120;
            myMap.Height = 5120;

            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player.Load(Content, this); //call the 'Load' function in the player class
            BoxingViewportAdapter viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, graphics.GraphicsDevice.Viewport.Height, graphics.GraphicsDevice.Viewport.Width);

            camera = new Camera2D(viewportAdapter);
            camera.Position = new Vector2(0, graphics.GraphicsDevice.Viewport.Height);

            map = Content.Load<TiledMap>("Level1");
            mapRendered = new TiledMapRenderer(GraphicsDevice);

            SetUpTiles();
        }

        protected override void UnloadContent()
        {

        }

        public void SetUpTiles()
        {
            tileHeight = map.TileHeight;
            levelTileHeight = map.Height;
            levelTileWidth = map.Width;
            levelGrid = new sprite[levelTileWidth, levelTileHeight];
            foreach(TiledMapTileLayer layer in map.TileLayers)
            {
                if(layer.Name == "Collisions")
                {
                    collisionLayer = layer;
                }
            }
            int columns = 0;
            int rows = 0;
            int loopCount = 0;
            while (loopCount < collisionLayer.Tiles.Count)
            {
                if (collisionLayer.Tiles[loopCount].GlobalIdentifier !=0)
                {
                    sprite tileSprite = new sprite();
                    tileSprite.position.X = columns * tileHeight;
                    tileSprite.position.Y = rows * tileHeight;
                    tileSprite.width = tileHeight;
                    tileSprite.height = tileHeight;
                    tileSprite.UpdateHitBox();
                    allCollisionTiles.Add(tileSprite);
                    levelGrid[columns, rows] = tileSprite;
                }
                columns++;

                if (columns == levelTileWidth)
                {
                    columns = 0;
                    rows++;
                }

                loopCount++;
            }

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            player.Update(deltaTime); //call the 'Update' function from the player class

            camera.Position = player.playerSprite.position - new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            var viewMatrix = camera.GetViewMatrix();
            var projectionMatrix = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0.0f, -1.0f);

            spriteBatch.Begin(transformMatrix: viewMatrix);

            mapRendered.Draw(map, ref viewMatrix, ref projectionMatrix);
            player.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
