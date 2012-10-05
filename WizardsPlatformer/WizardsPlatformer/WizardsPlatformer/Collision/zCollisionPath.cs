using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

/// <summary>
/// A series of line segments
/// </summary>
class zCollisionPath:zCollisionPrimitive
{
    //Vector2[] m_localPoints;
    Vector2[] m_normals;
    Vector2[] m_midpoints;
    Vector2[] m_worldPoints;
    public int m_lineWidth;
    bool m_isPolygon;
    public Color m_lineColor;

    private Rectangle m_boundingRect;

    public zCollisionPath(GLEED2D.PathItem path)
    {
      //  m_localPoints = path.LocalPoints;
        m_worldPoints = path.WorldPoints;
        m_lineWidth = path.LineWidth;
        m_lineColor = path.LineColor;
        m_isPolygon = path.IsPolygon;   //TODO handle this thing!

        m_normals = new Vector2[m_worldPoints.Length - 1];
        m_midpoints = new Vector2[m_worldPoints.Length - 1];

        float leftMoast;
        float rightMost;
        float topMost;
        float bottomMost;

        leftMoast = rightMost = m_worldPoints[0].X;
        topMost = bottomMost = m_worldPoints[0].Y;

        for (int i = 0; i < m_worldPoints.Length - 1; ++i)
        {
            Vector2 line = m_worldPoints[i + 1] - m_worldPoints[i];

            m_midpoints[i] = (line / 2.0f) + m_worldPoints[i];
            m_normals[i].X = line.Y;
            m_normals[i].Y = -line.X;
            m_normals[i].Normalize();

            //Check against bounding coords
            
            //Since we initialized to location 0 we use location i+1 to ensure we also check the last points in the line
            if (m_worldPoints[i + 1].X < leftMoast)
                leftMoast = m_worldPoints[i + 1].X;
            else if (m_worldPoints[i + 1].X > rightMost)
                rightMost = m_worldPoints[i + 1].X;

            if (m_worldPoints[i + 1].Y < topMost)
                topMost = m_worldPoints[i + 1].Y;
            else if (m_worldPoints[i + 1].Y > bottomMost)
                bottomMost = m_worldPoints[i + 1].Y;
        }

        //Floor the top left coord and ciel the width and height, better to have a slightly bigger box then a too small one
        m_boundingRect = new Rectangle((int)leftMoast, (int)topMost, (int)Math.Ceiling(rightMost - leftMoast),(int) Math.Ceiling(bottomMost - topMost));
    }
    private zCollisionPath() { }

    public override bool CheckPoint(Vector2 testPoint, out Vector2 closestPoint)
    {
        closestPoint = testPoint;

        //First check against bounding rect. . .
        if (!m_boundingRect.Contains((int)testPoint.X,(int)testPoint.Y))
        {
            return false;
        }

        //Step 1, find index of closest point
        int closestIndex = 0;
        float closestLengthSqr = (testPoint - m_worldPoints[0]).Length();
        for (int i = 1; i < m_worldPoints.Length; ++i)
        {
            float lengthSqr = (testPoint - m_worldPoints[i]).Length();
            if (lengthSqr < closestLengthSqr)
            {
                closestLengthSqr = lengthSqr;
                closestIndex = i;
            }
        }
        //Now that we have the closest point and the closest index get the two closest lines
        float lengthA = float.MaxValue;
        float lengthB = float.MaxValue;

        Vector2 closeA = testPoint;
        Vector2 closeB = testPoint;
        
        if (closestIndex != 0)
        {
            Utilities.ClosestPointOnLineSegment(m_worldPoints[closestIndex-1], m_worldPoints[closestIndex], testPoint, out closeB);
            lengthB = (closeB - testPoint).LengthSquared();
        }
        if (closestIndex != m_worldPoints.Length - 1)
        {
            Utilities.ClosestPointOnLineSegment(m_worldPoints[closestIndex], m_worldPoints[closestIndex+1], testPoint, out closeA);
            lengthA = (closeA - testPoint).LengthSquared();
        }

        if (lengthA < lengthB)
            closestPoint = closeA;
        else
            closestPoint = closeB;


        return true;
    }

    public void Draw(SpriteBatch spritebatch, bool drawNormals)
    {
        for (int i = 0; i < m_worldPoints.Length - 1; ++i)
        {
            Utilities.DrawLine(m_worldPoints[i], m_worldPoints[i + 1], spritebatch);
        }

        if (drawNormals)
        {
            for (int i = 0; i < m_worldPoints.Length - 1; ++i)
            {
                Utilities.DrawLine(m_midpoints[i], m_midpoints[i] + (m_normals[i] * 10.0f), spritebatch, Color.White);
            }
        }

        Utilities.DrawRect(m_boundingRect, spritebatch, Color.Green);
    }
    public override void Draw(SpriteBatch spritebatch)
    {
        Draw(spritebatch, Game1.drawNormals);
    }
}
