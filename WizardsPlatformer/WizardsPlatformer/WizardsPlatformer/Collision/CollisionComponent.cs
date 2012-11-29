using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

delegate void CollCallBack ( CollisionComponent mine, CollisionComponent other );

class CollisionComponent
{
    
    List<zCollisionPrimitive> prims; //Multiple prims per coll comp

}
