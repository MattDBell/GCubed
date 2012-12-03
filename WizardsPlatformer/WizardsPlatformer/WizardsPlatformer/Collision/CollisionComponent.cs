using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


partial class CollisionManager
{
    public delegate void CollCallBack ( CollisionComponent mine, CollisionComponent other, CollResult coll, int collNumber );
    delegate zCollisionPrimitive PrimCreator(Vector2 offset);
    delegate CollisionComponent ComponentCreator(EntityManager.Transform t, CollCallBack callBack);
    static ComponentCreator compCreator;
    static PrimCreator[] creators;
    static public void BOOT() 
    { 
        creators = new PrimCreator[(int)Primitives.TOTAL];
        //zCollisionAABB.BOOT();
        //zCollisionPath.BOOT();
        //zCollisionCircle.BOOT();
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
        List<zCollisionPrimitive> prims; //Multiple prims per coll comp
        CollisionComponent(EntityManager.Transform t, CollCallBack coll) { tran = t; callback = coll;}
        static CollisionComponent creator(EntityManager.Transform t, CollCallBack coll)
        {
            return new CollisionComponent(t, coll);
        }
        static public void BOOT() { compCreator = creator;  }
        void SetTransform(EntityManager.Transform to) { tran = to; }
        void SetCallBack(CollCallBack to) { callback = to; }
        void AddPrimitive(Primitives prim, Vector2 offset ) { }
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
                        callback(this, other, c, collision);
                        CollResult otherC = c;
                        otherC.normal *= -1;
                        other.callback(other, this, otherC, collision++);
                    } //Note, currently multiple callbacks can be made, each with their own index
                }
            }
        
        }
    }
}