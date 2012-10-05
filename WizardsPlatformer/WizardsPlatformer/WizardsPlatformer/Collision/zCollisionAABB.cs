using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

class zCollisionAABB : zCollisionPrimitive
{
    private Rectangle m_rect;

    public zCollisionAABB(Rectangle rect) { m_rect = rect; }
    private zCollisionAABB() { }

    public void DrawFilled(SpriteBatch spritebatch,Color color)
    {
        Utilities.DrawFilledRect(m_rect, spritebatch, color);
    }
    public void DrawOutline(SpriteBatch spritebatch, Color color)
    {
        Utilities.DrawRect(m_rect, spritebatch, color);
    }
    
    public bool PointCollides(Vector2 point)
    {
        return m_rect.Contains((int)point.X,(int)point.Y);
    }
    public override void Draw(SpriteBatch spritebatch)
    {
        DrawFilled(spritebatch,new Color(1,0,1,0.5f));
    }
    public override bool CheckPoint(Vector2 testPoint, out Vector2 closestPoint)
    {
        float x1 = m_rect.X;
        float x2 = m_rect.X + m_rect.Width;
        float y1 = m_rect.Y;
        float y2 = m_rect.Y + m_rect.Height;

        if (testPoint.X < x1) {closestPoint.X = x1;}
        else if (testPoint.X > x2){closestPoint.X = x2;}
        else{closestPoint.X = testPoint.X;}

        if (testPoint.Y < y1) { closestPoint.Y = y1; }
        else if (testPoint.Y > y2) { closestPoint.Y = y2; }
        else { closestPoint.Y = testPoint.Y; }

        return closestPoint == testPoint;
    }
}
