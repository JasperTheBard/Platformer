using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;
using MonoGame.Extended.ViewportAdapters;
using System.Collections;
using System.Collections.Generic;

namespace Game1
{
    
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        Player player = new Player(); //Creates an instance of our player in this class

        public List<Enemie> enemies = new List<Enemie>();
        public Chest goal = null;

        Camera2D camera = null; //Creates an instance of our players
        TiledMap map = null; //Creates an instance of a Tilted map
        TiledMapRenderer mapRendered = null; // Creates an Instance of what makes a tilted map 

        TiledMapTileLayer collisionLayer;
        public ArrayList allCollisionTiles = new ArrayList();
        public sprite[,] levelGrid;

        public int tileHeight = 0;
        public int levelTileWidth = 0;
        public int levelTileHeight = 0;

        public Vector2 gravity = new Vector2(0, 1500);

        Song gameMusic;

        SpriteFont arialFont;
        int score = 0;
        int lives = 3;
        Texture2D heart = null;


        public Rectangle myMap;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 1600;
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

            arialFont = Content.Load<SpriteFont>("Font");

            heart = Content.Load<Texture2D>("heart");

            BoxingViewportAdapter viewportAdapter = new BoxingViewportAdapter(Window, 
                                                                              GraphicsDevice, 
                                                                              graphics.GraphicsDevice.Viewport.Width * 2, 
                                                                              graphics.GraphicsDevice.Viewport.Height * 2);
            //BoxingViewportAdapter viewportAdapter = new BoxingViewportAdapter(Window, 
            //                                                                  GraphicsDevice, 
            //                                                                  graphics.GraphicsDevice.Viewport.Height, 
            //                                                                  graphics.GraphicsDevice.Viewport.Width);

            camera = new Camera2D(viewportAdapter);
            camera.Position = new Vector2(0, graphics.GraphicsDevice.Viewport.Height);

            map = Content.Load<TiledMap>("Level1");
            mapRendered = new TiledMapRenderer(GraphicsDevice);

            gameMusic = Content.Load<Song>("GameTheme");
            MediaPlayer.Play(gameMusic);

            SetUpTiles();
            LoadObject();
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

            foreach (Enemie enemie in enemies)
            {
                enemie.Update(deltaTime);
            }

            camera.Position = player.playerSprite.position - new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, 
                                                                         graphics.GraphicsDevice.Viewport.Height / 2);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            var viewMatrix = camera.GetViewMatrix();
            var projectionMatrix = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, 
                                                                      GraphicsDevice.Viewport.Height, 0,
                                                                      0.0f, -1.0f);
            //Begin Drawing----------------------------------------------------------------------------------------------------------\\
            spriteBatch.Begin(transformMatrix: viewMatrix);
            mapRendered.Draw(map, ref viewMatrix, ref projectionMatrix);
            player.Draw(spriteBatch);
            goal.Draw(spriteBatch);

            foreach (Enemie enemie in enemies)
            {
                enemie.Draw(spriteBatch);
            }

            spriteBatch.End();

            //UI---------------------------------------------------------------------------------------------------------------------\\
            spriteBatch.Begin();

            spriteBatch.DrawString(arialFont, "Score :" + score.ToString(), new Vector2(20, 20), Color.Orange);

            int loopCount = 0;
            while (loopCount < lives)
            {
                spriteBatch.Draw(heart, new Vector2(GraphicsDevice.Viewport.Width - 80 - loopCount * 20, 20), Color.White);
                loopCount++;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
        void LoadObject()
        {
            foreach (TiledMapObjectLayer layer in map.ObjectLayers)
            {
                if (layer.Name == "Enemies")
                {
                    foreach (TiledMapObject thing in layer.Objects)
                    {
                        Enemie enemie = new Enemie();
                        Vector2 tiles = new Vector2((int)(thing.Position.X / tileHeight), (int)(thing.Position.Y / tileHeight));
                        enemie.enemieSprite.position = tiles * tileHeight;
                        enemie.Load(Content, this);
                        enemies.Add(enemie);
                    }
                }
                if (layer.Name == "Goal")
                {
                    TiledMapObject thing = layer.Objects[0];
                    if (thing != null)
                    {
                        Chest chest = new Chest();
                        chest.chestSprite.position = new Vector2(thing.Position.X, thing.Position.Y);
                        chest.Load(Content, this);
                        goal = chest;
                    }
                }
            }
        }
    }
}
