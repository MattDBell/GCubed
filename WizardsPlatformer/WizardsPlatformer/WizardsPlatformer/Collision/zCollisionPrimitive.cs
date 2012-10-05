using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

class zCollisionPrimitive
{
    protected zCollisionPrimitive() { }

    public virtual bool CheckPoint(Vector2 testPoint, out Vector2 closestPoint)
    {
        closestPoint = testPoint;
        return false;
    }
    public virtual void Draw(SpriteBatch spritebatch)
    {
    }
}
