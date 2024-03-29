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

using Player = EntityManager.Player;
/// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GLEED2D.Level testLevel;
        Player m_player;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //graphics.PreferredBackBufferWidth = 1280;
            //graphics.PreferredBackBufferHeight = 720;

            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 360;
        }
        public static bool drawNormals;
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            EntityManager.BOOT();
            CollisionManager.BOOT();
            base.Initialize();
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            IniLoader gIni = new IniLoader("Content\\WIZZ.ini");
            string firstLevel;
            
            gIni.GetString("startOnLevel", out firstLevel,"MAIN");
            gIni.GetBool("drawNormals", out drawNormals, false);
            string levelPath;
            gIni.GetString("levelPath", out levelPath, Constants.LEVEL_PATH);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            testLevel = GLEED2D.Level.FromFile(levelPath + firstLevel + ".xml", Content);
            Utilities.Init(Content);

            SuperHackyLevelParsing();

            m_player = (Player)EntityManager.get().Create(EntityManager.ENT_TYPE.PLAYER);
            m_player.Init(Content);
            m_player.Spawn();
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            //This is important and results in Collisions -> Transformations -> Update
            CollisionManager.Get().CheckCollisions();
            EntityManager.get().Update(gameTime);

            base.Update(gameTime);

        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            SamplerState sampleState = new SamplerState();
            sampleState.Filter = TextureFilter.Point;

            //spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, sampleState, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            spriteBatch.Begin();

            EntityManager.get().Draw(spriteBatch);
            CollisionManager.Get().DrawCollisionPrimatives(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        void SuperHackyLevelParsing()
        {
            foreach (GLEED2D.Layer layer in testLevel.Layers)
            {
                if (layer.Name.ToLower() == "collision")
                {
                    //For now only process this layer

                    //All items on this layer should be either a path, circle, or AABB
                    foreach (GLEED2D.Item item in layer.Items)
                    {
                        //I don't like this, it's fine for when they were serialized out but not for loading them.
                        //One size does not fit all
                        if (item.GetType() == typeof(GLEED2D.PathItem))
                        {
                            CollisionManager.CollisionComponent foo = CollisionManager.Get().GetCollComponent();

                            EntityManager.Transform footrans = EntityManager.get().GetTransform();
                            footrans.Set(item.Position,Vector2.Zero,Vector2.Zero,0);
                            item.Position = Vector2.Zero;
                            foo.SetTransform(footrans);
                            foo.AddPrimitive(new zCollisionPath((GLEED2D.PathItem)item));
                        }
                        else if (item.GetType() == typeof(GLEED2D.RectangleItem))
                        {
                            GLEED2D.RectangleItem gleeRect = (GLEED2D.RectangleItem)item;
                            Rectangle zeRect = new Rectangle((int)gleeRect.Position.X,(int)gleeRect.Position.Y,(int)gleeRect.Width,(int)gleeRect.Height);

                            CollisionManager.CollisionComponent foo = CollisionManager.Get().GetCollComponent();
                            EntityManager.Transform footrans = EntityManager.get().GetTransform();
                            footrans.Set(item.Position, Vector2.Zero, Vector2.Zero, 0);
                            item.Position = Vector2.Zero;
                            foo.SetTransform(footrans);
                            foo.AddPrimitive(new zCollisionAABB(zeRect));
                        }
                        else if (item.GetType() == typeof(GLEED2D.CircleItem))
                        {
                            GLEED2D.CircleItem gleeCircle = (GLEED2D.CircleItem)item;
                            CollisionManager.CollisionComponent foo = CollisionManager.Get().GetCollComponent();
                            EntityManager.Transform footrans = EntityManager.get().GetTransform();
                            footrans.Set(item.Position, Vector2.Zero, Vector2.Zero, 0);
                            item.Position = Vector2.Zero;
                            foo.SetTransform(footrans);
                            foo.AddPrimitive(new zCollisionCircle(gleeCircle.Position, gleeCircle.Radius));
                        }
                    }
                }
            }
        }
    }