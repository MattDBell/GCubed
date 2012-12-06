using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

class zCollisionCircle : zCollisionPrimitive
{
    Vector2 m_pos;
    float m_radius;

    public zCollisionCircle(Vector2 centerPos, float radius)
    {
        m_pos = centerPos;
        m_radius = radius;
    }

    public override bool CheckPoint(Vector2 testPoint, out Vector2 closestPoint)
    {
        closestPoint = testPoint;
        Vector2 vecBetween = testPoint - m_pos;

        float squaredLength = vecBetween.LengthSquared();
        if (squaredLength <= m_radius * m_radius)
            return true;
        float length = (float)Math.Sqrt(squaredLength);
            vecBetween /= length;

            vecBetween *= m_radius;
            closestPoint = m_pos + vecBetween;
            return false;
    }
    public void ClosestPointOnCircle(Vector2 testPoint, out Vector2 closestPoint)
    {
        Vector2 vecBetween = testPoint - m_pos;
        vecBetween.Normalize();
        vecBetween *= m_radius;
        closestPoint = m_pos + vecBetween;
    }

    public override void Draw(SpriteBatch spritebatch, Vector2 offset)
    {
        Utilities.DrawCircle(m_pos, m_radius, spritebatch, Color.Purple);
    }

    override public CollResult CheckAgainst(zCollisionPrimitive other, EntityManager.Transform myRoot, EntityManager.Transform hisRoot)
    {
        throw new Exception("The method or operation is not implemented.");
    }

    override public CollResult RedirectedCheckAgainst(zCollisionPrimitive other, EntityManager.Transform myRoot, EntityManager.Transform hisRoot)
    {
        throw new Exception("The method or operation is not implemented.");
    }
}
