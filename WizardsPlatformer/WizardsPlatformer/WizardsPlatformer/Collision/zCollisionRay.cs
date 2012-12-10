using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

class zCollisionRay : zCollisionPrimitive
{
    Vector2 startPoint;
    Vector2 direction; //must be normalized
    public void SetDirection(Vector2 dir) { dir.Normalize(); direction = dir; }
    public override bool CheckPoint(Microsoft.Xna.Framework.Vector2 testAgainst, out Microsoft.Xna.Framework.Vector2 nearestPoint)
    {
        Vector2 toPoint = testAgainst - startPoint;
        float dot = Vector2.Dot(direction, toPoint);
        nearestPoint = startPoint + direction * dot;
        return false;
    }

    public override CollResult CheckAgainst(zCollisionPrimitive other, EntityManager.Transform myRoot, EntityManager.Transform hisRoot)
    {
        CollResult ret = new CollResult();
        ret.collided = false;
        ret.normal = Vector2.Zero;
        return ret;
    }

    public override CollResult RedirectedCheckAgainst(zCollisionPrimitive other, EntityManager.Transform myRoot, EntityManager.Transform hisRoot)
    {
        CollResult ret = new CollResult();
        ret.collided = false;
        ret.normal = Vector2.Zero;
        return ret;
    }

    public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Microsoft.Xna.Framework.Vector2 offset)
    {
        return;
    }
}

