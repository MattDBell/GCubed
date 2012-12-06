using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


partial class CollisionManager
{
    public delegate void CollCallBack ( CollisionComponent mine, CollisionComponent other, CollResult coll, int collNumber );
    delegate CollisionComponent ComponentCreator(EntityManager.Transform t, CollCallBack callBack);
    static ComponentCreator compCreator;
    static public void BOOT()                                          
    {                                                                  
        CollisionComponent.BOOT();                                     
    }                                                                  
    public enum Primitives                                             
    {                                                                  
        AABB,                                                          
        CIRCLE,                                                        
        PATH,                                                          
        TOTAL                                                          
    }                                                                  
    public class CollisionComponent                                    
    {                                                                  
        //This should keep an AABB, by the way.
        EntityManager.Transform tran;                                  
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

        public void AddPrimitive(zCollisionPrimitive prim) { prims.Add(prim); }
        void CheckCollisionsAgainst(CollisionComponent other)
        {
            int collision = 0;
            for(int i = 0; i < prims.Count; ++i)
            {
                for(int y = 0; y < prims.Count; ++y)
                {
                    CollResult c = prims[i].CheckAgainst(other.prims[y], tran, other.tran);
                    if(c.collided)
                    {

                        if (callback != null) callback(this, other, c, collision);
                        CollResult otherC = c;
                        otherC.normal *= -1;
                        if(other.callback != null) other.callback(other, this, otherC, collision++);
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