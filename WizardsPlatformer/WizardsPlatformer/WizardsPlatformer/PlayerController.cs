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
using System.Diagnostics;

partial class EntityManager
{
    public class Player : Entity
    {
        static ENT_TYPE type = ENT_TYPE.PLAYER;
        static bool booted = false;
        static public void BOOT()
        {
            if (booted) return;
            EntityManager.Creators[(int)type] = PlayerIChooseYou;
            booted = true;
        }
        static Entity PlayerIChooseYou()
        {
            return new Player();
        }
        Texture2D m_tex;
        //To be changed at a later date . . .
        bool onGround;
        const float airTouchModifier = 0.25f;
        
        protected Player()
        {
        }

        public override void Init(ContentManager content)
        {
            base.Init(content);
            //Better would be to give it a handle to the texture, but this works for now
            m_tex = content.Load<Texture2D>("dude");
            transform.SetMaxSpeed(100.0f);
            
        }

        public override bool Update(GameTime gametime)
        {
            GamePadState padState = GamePad.GetState(PlayerIndex.One);
            KeyboardState kState = Keyboard.GetState();
            float deltaTime = (float)gametime.ElapsedGameTime.TotalMilliseconds;

            float leftStickX = padState.ThumbSticks.Left.X;
            leftStickX = kState.IsKeyDown(Keys.A) ? -1 : kState.IsKeyDown(Keys.D) ? 1 : 0;
            if(leftStickX != 0)
                transform.SetAcceleration(new Vector2(leftStickX * 100, 0));
            if (onGround)
            {
                //Get normal of ground
                //RemoveComponent(normal);
            }
            else
            {

            }
            return false;


        }
        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(m_tex, new Vector2((int)Math.Round(transform.GetPos().X), (int)Math.Round(transform.GetPos().Y)), Color.White);
        }

        public override void HandleCollisions(List<zCollisionPrimitive> listPrims)
        {
            //Check against all prims, for now if colliding then move me to last pos (towards last pos) until no longer collide

        }

        public override void Spawn()
        {
            transform.RePosition(new Vector2(100, 300));
            base.Spawn();
        }

        public override void GiveCreatedRef(Entity newlyCreated, EntityManager.ENT_TYPE created)
        {
            return;
        }
    }
}