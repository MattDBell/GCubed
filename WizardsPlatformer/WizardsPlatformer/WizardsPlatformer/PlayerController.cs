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
        CollisionManager.CollisionComponent myComp;
        CollisionManager.CollisionComponent rayCast;
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
            myComp = CollisionManager.Get().GetCollComponent();
            zCollisionCircle prim = new zCollisionCircle(Vector2.Zero, 5.0f);
            myComp.AddPrimitive(prim);
            myComp.SetTransform(transform);
            myComp.SetCallBack(PhysicsCallback);
            rayCast = CollisionManager.Get().GetCollComponent();
            //zCollisionLine line = new zCollisionLine(Vector2.Zero, new Vector2(0, -1));
            //rayCast.AddPrimitive(line);
            rayCast.SetTransform(transform);
            rayCast.SetCallBack(RayCastCallBack);
            
        }
        public void PhysicsCallback(CollisionManager.CollisionComponent mine, CollisionManager.CollisionComponent other, CollResult coll, int collNumber)
        {
            if(other.CheckFlag(CollisionManager.FLAGS.GROUND))
            {
                //Move position to above the path (bottom of collision should be above the ground.  Above being relative to normal
                //set velocity to be speed magnitude perpendicular to normal of the primitive it's colliding against, 
            }
        }
        public void RayCastCallBack(CollisionManager.CollisionComponent mine, CollisionManager.CollisionComponent other, CollResult coll, int collNumber)
        {
            if(other.CheckFlag(CollisionManager.FLAGS.GROUND))
            {

            }
        }
        public override bool Update(GameTime gametime)
        {
            transform.EnableGravity();
            GamePadState padState = GamePad.GetState(PlayerIndex.One);
            KeyboardState kState = Keyboard.GetState();
            float deltaTime = (float)gametime.ElapsedGameTime.TotalMilliseconds;

            float leftStickX = padState.ThumbSticks.Left.X;
            leftStickX = kState.IsKeyDown(Keys.A) ? -1 : kState.IsKeyDown(Keys.D) ? 1 : 0;
            if (leftStickX != 0)
            {
                transform.SetDrag(0);
                transform.SetAcceleration(new Vector2(leftStickX * 100, 0));
            }
            else
            {
                transform.AccelerateToZero(80);
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