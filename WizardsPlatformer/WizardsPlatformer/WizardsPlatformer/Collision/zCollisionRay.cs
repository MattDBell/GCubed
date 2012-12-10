using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

class zCollisionRay : zCollisionPrimitive
{
    Vector2 startPoint;
    Vector2 direction; //must be normalized
    float distance;
    public void SetDirection(Vector2 dir) { dir.Normalize(); direction = dir; }
    public override bool CheckPoint(Microsoft.Xna.Framework.Vector2 testAgainst, out Microsoft.Xna.Framework.Vector2 nearestPoint)
    {
        Vector2 toPoint = testAgainst - startPoint;
        float dot = Vector2.Dot(direction, toPoint);
        dot = dot > distance ? distance : dot;
        nearestPoint = startPoint + direction * dot;
        return nearestPoint == testAgainst;
    }

    public override CollResult CheckAgainst(zCollisionPrimitive other, EntityManager.Transform myRoot, EntityManager.Transform hisRoot)
    {
        CollResult ret = new CollResult();
        ret.collided = false;
        ret.normal = Vector2.Zero;
        if(other is zCollisionCircle)
        {
            zCollisionCircle otherCircle = other as zCollisionCircle;
            Vector2 nearest;
            Vector2 throwAway;
            //          HIS LOCAL SPACE         WORLD SPACE        MY LOCAL SPACE
            CheckPoint(otherCircle.GetCenter() + hisRoot.GetPos() - myRoot.GetPos(), out nearest);
            //                                MY LOCAL  WORLD               HIS LOCAL
            bool hit = otherCircle.CheckPoint(nearest + myRoot.GetPos() - hisRoot.GetPos(), out throwAway);
            ret.collided = hit;
            ret.normal = throwAway - otherCircle.GetCenter();
            return ret;
        }
        //if(other is zCollisionPath)
        //{
        //    zCollisionPath otherPath = other as zCollisionPath;
        //
        //}
        return other.RedirectedCheckAgainst(this, hisRoot, myRoot);
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

