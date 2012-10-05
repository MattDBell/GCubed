using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


public class Utilities
{
    private static Texture2D tex;
    private static Color defaultColor;
    
     private Utilities() {}
     ~Utilities() { }

    public static void Init(ContentManager Content)
    {
        tex = Content.Load<Texture2D>("Utils\\pen");   //A white 2x2 texture
        defaultColor = Color.Black;
    }

#region Primitive Drawing Methods
    public static void SetDefaultColor(Color color){defaultColor = color;}
    public static void DrawRect(Rectangle rect, SpriteBatch spritebatch)
    {
        DrawRect(rect, spritebatch, defaultColor);
    }
    public static void DrawRect(Rectangle rect, SpriteBatch spritebatch,Color color)
    {
        spritebatch.Draw(tex, new Rectangle(rect.Left, rect.Top, rect.Width, 1),color);//Top
        spritebatch.Draw(tex, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), color);//Bottom

        spritebatch.Draw(tex, new Rectangle(rect.Left, rect.Top, 1, rect.Height), color);//Left
        spritebatch.Draw(tex, new Rectangle(rect.Right, rect.Top, 1, rect.Height), color);//Right
    }

    public static void DrawFilledRect(Rectangle rect, SpriteBatch spritebatch)
    {
        DrawFilledRect(rect, spritebatch, defaultColor);
    }
    public static void DrawFilledRect(Rectangle rect, SpriteBatch spritebatch, Color fillColor)
    {
        spritebatch.Draw(tex, rect, fillColor);
    }
    public static void DrawFilledRect(Rectangle rect, SpriteBatch spritebatch, Color fillColor,float rotation)
    {
        spritebatch.Draw(tex, rect,new Rectangle(0, 0, tex.Width, tex.Height), fillColor, rotation, Vector2.Zero, SpriteEffects.None, 0);
    }

    public static void DrawCircle(Vector2 center, float radius, SpriteBatch spritebatch)
    {
        DrawCircle(center, radius, spritebatch, defaultColor);
    }
    public static void DrawCircle(Vector2 center, float radius, SpriteBatch spritebatch,Color color)
    {
        const int MAXSTEPS = 64;
        const int MINSTEPS = 8;

        int numStepsInCircle = (int)(radius / 2.5f);
        if (numStepsInCircle < MINSTEPS)
            numStepsInCircle = MINSTEPS;
        else if (numStepsInCircle > MAXSTEPS)
            numStepsInCircle = MAXSTEPS;

        Vector2 lastPoint, firstPoint;
        firstPoint = new Vector2(radius,0) + center;
        lastPoint = firstPoint;
        for (float i = (float)(2.0f * Math.PI) / (float)numStepsInCircle; i < (2.0f * Math.PI); i += (float)((2.0f * Math.PI) / (float)numStepsInCircle))
        {
            Vector2 ehSteve = new Vector2((float)(Math.Cos(i) * radius), (float)(Math.Sin(i) * radius)) + center;

            DrawLine(ehSteve, lastPoint, spritebatch,color);
            lastPoint = ehSteve;
        }
            //Draw the last line
            DrawLine(firstPoint, lastPoint, spritebatch,color);
    }

    public static void DrawPoint(Vector2 pos, SpriteBatch spritebatch)
    {
        DrawPoint(pos, spritebatch, defaultColor);
    }
    public static void DrawPoint(Vector2 pos, SpriteBatch spritebatch, Color color)
    {
        spritebatch.Draw(tex, new Rectangle((int)pos.X, (int)pos.Y, 1, 1), color);
    }
    
    public static void DrawLine(Vector2 p1, Vector2 p2, SpriteBatch spritebatch, int lineWidth = 1)
    {
        DrawLine(p1, p2, spritebatch, defaultColor,lineWidth);
    }
    public static void DrawLine(Vector2 p1, Vector2 p2, SpriteBatch spritebatch,Color color, int lineWidth= 1)
    {
        Vector2 vect = p1 - p2;
        float radians = (float)Math.Atan2(vect.Y, vect.X);
        spritebatch.Draw(tex, new Rectangle((int)p1.X,(int)(p1.Y - (0.5f*lineWidth)), (int)vect.Length(), lineWidth),
            new Rectangle(0, 0, 2, 2), color, radians + 3.14f, Vector2.Zero, SpriteEffects.None, 0);
    }
#endregion //Primitive Drawing Methods

    public static void ClosestPointOnLine(Vector2 a, Vector2 b,Vector2 p, out Vector2 closestPoint)
    {
        float u = ((p.X - a.X) * (b.X - a.X) + (p.Y - a.Y) * (b.Y - a.Y)) / (b - a).LengthSquared();
        closestPoint.X = a.X + u * (b.X - a.X);
        closestPoint.Y = a.Y + u * (b.Y - a.Y);
    }

    public static void ClosestPointOnLineSegment(Vector2 a, Vector2 b, Vector2 p, out Vector2 closestPoint)
    {
        float u = ((p.X - a.X) * (b.X - a.X) + (p.Y - a.Y) * (b.Y - a.Y)) / (b - a).LengthSquared();
        u = MathHelper.Clamp(u, 0, 1);
        closestPoint.X = a.X + u * (b.X - a.X);
        closestPoint.Y = a.Y + u * (b.Y - a.Y);
    }
}

