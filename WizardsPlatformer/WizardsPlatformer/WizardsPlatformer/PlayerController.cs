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
            EntityManager.Creators[(int)type] = PlayerIChooseYou;
            booted = true;
        }
        static Entity PlayerIChooseYou()
        {
            return new Player();
        }
        Texture2D m_tex;
        Vector2 m_pos;

        Vector2 m_lastPos;  //Superhack


        //To be changed at a later date . . .
        bool onGround;

        float m_moveSpeed = 30.0f / 1000.0f;   //Pixels per second
        const float gravity = 9.8f;   //Pixels per second
        const float airTouchModifier = 0.25f;

        Vector2 m_speeds;

        protected Player()
        {
        }

        public override void Init(ContentManager content)
        {
            //Better would be to give it a handle to the texture, but this works for now
            m_tex = content.Load<Texture2D>("dude");
        }

        public override bool Update(GameTime gametime)
        {
            GamePadState padState = GamePad.GetState(PlayerIndex.One);
            float deltaTime = (float)gametime.ElapsedGameTime.TotalMilliseconds;

            float leftStickX = padState.ThumbSticks.Left.X;

            if (onGround)
            {
                //Handle Jump
                if (padState.IsButtonDown(Buttons.A))
                {
                    //onGround = false;
                    //m_speeds.Y += -10.0f/1000.0f;
                }

                m_speeds.X = m_moveSpeed * leftStickX;
                //TODO apply in direction of forward instead
                m_pos += m_speeds * deltaTime;

            }
            else
            {

            }
            return false;


        }
        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(m_tex, new Vector2((int)Math.Round(m_pos.X), (int)Math.Round(m_pos.Y)), Color.White);
        }

        public override void HandleCollisions(List<zCollisionPrimitive> listPrims)
        {
            //Check against all prims, for now if colliding then move me to last pos (towards last pos) until no longer collide

        }

        public override void Spawn()
        {
            m_pos = new Vector2(100, 300);
            m_lastPos = m_pos;
            onGround = true;
            m_speeds = new Vector2(0);
        }

        public override void GiveCreatedRef(Entity newlyCreated, EntityManager.ENT_TYPE created)
        {
            return;
        }
    }
}