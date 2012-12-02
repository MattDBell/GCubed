using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
delegate void CollCallBack ( CollisionComponent mine, CollisionComponent other );

class CollisionComponent
{
    EntityManager.Transform myTrans;
    List<zCollisionPrimitive> prims; //Multiple prims per coll comp
    void CheckCollisionsAgainst(CollisionComponent other)
    {
        //First prim is AABB
        
    }
}
