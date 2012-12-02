using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

//May add more stuff later.  Also, should pool.
class CollResult
{
    public bool collided;
    public Vector2 normal;
}

abstract class zCollisionPrimitive
{
    protected Vector2 Offset; //This is just simple for now.
    abstract public bool CheckPoint(Vector2 testAgainst, out Vector2 nearestPoint);
    /// <summary>
    /// Primitive should try to deduce the type of the other primitive.  If it has the logic to handle collision
    /// with the type of the other primitive it will do so and return the result.  Else it will call RedirectedCheck
    /// Against.
    /// </summary>
    /// <param name="other">Prim to Collide Against</param>
    /// <param name="myRoot">Transform passed from CollisionComponent</param>
    /// <param name="hisRoot">Transform passed from other's CollisionComponent</param>
    /// <returns>CollResult instance describing collision</returns>
    abstract public CollResult CheckAgainst(zCollisionPrimitive other, EntityManager.Transform myRoot, EntityManager.Transform hisRoot);
    /// <summary>
    /// Called if other could not handle the collision so passed the buck to this class.  If this does not have
    /// the logic to handle the collision it should assert.  Chances are if this happens it's Glenn's fault.
    /// </summary>
    /// <param name="other">Primitive that has already attempted to resolve collision</param>
    /// <param name="myRoot">Transform passed from CollisionComponent</param>
    /// <param name="hisRoot">Transform passed from other's CollisionComponent</param>
    /// <returns>CollResult instance describing collision</returns>
    abstract public CollResult RedirectedCheckAgainst(zCollisionPrimitive other, EntityManager.Transform myRoot, EntityManager.Transform hisRoot);
    abstract public void Draw(SpriteBatch sb);
}
