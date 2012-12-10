using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


partial class CollisionManager
{
    public delegate void CollCallBack ( CollisionComponent mine, CollisionComponent other, CollResult coll, int collNumber, FLAGS others );
    delegate CollisionComponent ComponentCreator(EntityManager.Transform t, CollCallBack callBack);
    static ComponentCreator compCreator;
    static public void BOOT()                                          
    {                                                                  
        CollisionComponent.BOOT();                                     
    }                                                                  
    public enum FLAGS                                             
    {                                                                  
        GROUND          = 1 << 0,
        WALLJUMPABLE    = 1 << 1,
        ENEMY           = 1 << 2,
        PLAYER          = 1 << 3,
        PLAYERWEAPON    = 1 << 4
    }                                                                  
    public class CollisionComponent                                    
    {                                                                  
        //This should keep an AABB, by the way.
        EntityManager.Transform tran;
        FLAGS flags = (FLAGS)0;                          
        CollCallBack callback;                                         
        List<zCollisionPrimitive> prims = new List<zCollisionPrimitive>(); //Multiple prims per coll comp
        CollisionComponent(EntityManager.Transform t, CollCallBack coll) 
        { 
            tran = t; 
            callback = coll;
        }
        static CollisionComponent creator(EntityManager.Transform t, CollCallBack coll)
        {
            return new CollisionComponent(t, coll);
        }
        static public void BOOT() { compCreator = creator;  }
        public void SetTransform(EntityManager.Transform to) { tran = to; }
        public void SetCallBack(CollCallBack to) { callback = to; }
        public void SetFlag(FLAGS flag) { flags |= flag; }
        public void UnSetFlag(FLAGS flag) { flags &= ~flag;  }
        public void AddPrimitive(zCollisionPrimitive prim) { prims.Add(prim); }
        public void CheckCollisionsAgainst(CollisionComponent other)
        {
            int collision = 0;
            for(int i = 0; i < prims.Count; ++i)
            {
                for(int y = 0; y < prims.Count; ++y)
                {
                    CollResult c = prims[i].CheckAgainst(other.prims[y], tran, other.tran);
                    if(c.collided)
                    {

                        if (callback != null) callback(this, other, c, collision, flags);
                        CollResult otherC = c;
                        otherC.normal *= -1;
                        if(other.callback != null) other.callback(other, this, otherC, collision++, other.flags);
                    } //Note, currently multiple callbacks can be made, each with their own index
                }
            }
        
        }
        public void DrawCollisionPrimatives(SpriteBatch spritebatch)
        {
            foreach (zCollisionPrimitive primative in prims)
            {
                primative.Draw(spritebatch, tran.GetPos());
            }
        }
    }
}