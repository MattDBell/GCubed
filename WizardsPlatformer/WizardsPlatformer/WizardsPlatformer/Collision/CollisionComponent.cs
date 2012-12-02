using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
delegate void CollCallBack ( CollisionComponent mine, CollisionComponent other, CollResult coll, int collNumber );

class CollisionComponent
{
    //This should keep an AABB, by the way.
    EntityManager.Transform myTrans;
    CollCallBack myCallBack;
    List<zCollisionPrimitive> prims; //Multiple prims per coll comp
    void CheckCollisionsAgainst(CollisionComponent other)
    {
        int collision = 0;
        for(int i = 0; i < prims.Count; ++i)
        {
            for(int y = 0; y < prims.Count; ++y)
            {
                CollResult c = prims[i].CheckAgainst(other.prims[y], myTrans, other.myTrans);
                if(c.collided)
                {
                    myCallBack(this, other, c, collision);
                    CollResult otherC = c;
                    otherC.normal *= -1;
                    other.myCallBack(other, this, otherC, collision++);
                } //Note, currently multiple callbacks can be made, each with their own index
            }
        }
        
    }
}
