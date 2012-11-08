//----------------------------------------------------------------------------------------
//	Copyright 2009-2011 Matt Whiting, All Rights Reserved.
//  For educational purposes only.
//  Please do not distribute or republish in electronic or print form without permission.
//  Thanks - CrashLotus@gmail.com
//----------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// The Profiler provides an easy-to-use hierarchical timing Profiler.
/// You may create sub-sections within a Profiler for example Update might have a sub-section for Update_Enemies.
/// 
/// You have to Initialize the Profiler system before you can use it.  Call Profiler.Initialize() ONCE in your LoadContent function.
/// This will load in ProfilerContent/ProfileBar.png and ProfilerContent/ProfileFont.spritefont.
/// Be sure to include those in the Content of your project.
/// 
/// Like a stopwatch, each Profiler has a Start(), Stop(), and Reset() function.  The total time for each Profiler is recorded when you
/// call Reset().
/// Stop() the Profiler before you call Reset().
/// You do not need to call Reset() for sub-profilers... only top-level Profilers.
/// </summary>
public class Profiler
{
    Stopwatch m_timer;
    String m_name;
    Profiler m_parent;
    Color m_color;
    LinkedList<Profiler> m_children;

    // The Profiler will keep track of the most recent 60 samples so you can ask for the maximum or the average over the past second
    const int NUM_SAMPLE = 60;
    float[] m_sample;
    int m_curSample;
    
    static LinkedList<Profiler> s_timerStack;
    static Texture2D s_barTex;
    static SpriteFont s_font;

    const float s_barWidth = 100.0f * 60.0f / 1000.0f;
    const float s_barHeight = 20.0f;
    const float s_indent = 20.0f;
 
    /// <summary>
    /// Create a Profiler
    /// </summary>
    /// <param name="name">The name of the profiler to draw with the bars</param>
    /// <param name="color">The color of the bar</param>
    /// <param name="parent">If this is a sub-profiler, give the parent Profiler. For top-level put "null"</param>
    public Profiler(String name, Color color, Profiler parent)
    {
        m_timer = new Stopwatch();
        m_name = name;
        m_color = color;
        m_children = new LinkedList<Profiler>();
        m_sample = new float[NUM_SAMPLE];
        for (int i = 0; i < NUM_SAMPLE; ++i)
            m_sample[i] = 0.0f;
        m_curSample = 0;
        m_parent = parent;
        if (null != m_parent)
            m_parent.AddChild(this);
    }

    /// <summary>
    /// One-time initialization loads the texture for the bars.
    /// Call this in your LoadContent
    /// </summary>
    public static void Initialize(Game game)
    {
        s_timerStack = new LinkedList<Profiler>();
        s_barTex = game.Content.Load<Texture2D>("ProfilerContent/ProfileBar");
        s_font = game.Content.Load<SpriteFont>("ProfilerContent/ProfileFont");
    }

    void AddChild(Profiler child)
    {
        m_children.AddLast(child);
    }

    public void Start()
    {
        m_timer.Start();
        if (s_timerStack.Count > 0)
        {
            s_timerStack.First().m_timer.Stop();
        }
        s_timerStack.AddFirst(this);
    }

    public void Stop()
    {
        if (s_timerStack.Contains(this))
        {
            m_timer.Stop();
            s_timerStack.Remove(this);
            if (s_timerStack.Count > 0)
            {
                s_timerStack.First().m_timer.Start();
            }
        }
    }

    /// <summary>
    /// Resets() the Profiler back to 0 and records the Total time in the array of NUM_SAMPLES samples.
    /// Recursively calls any sub-profilers and Resets them as well.
    /// </summary>
    public void Reset()
    {
        float time = (float)m_timer.Elapsed.TotalMilliseconds;
        m_sample[m_curSample] = time;
        ++m_curSample;
        if (m_curSample >= NUM_SAMPLE)
        {
            m_curSample = 0;
        }
        m_timer.Reset();
        foreach (Profiler child in m_children)
        {
            child.Reset();
        }
    }

    /// <summary>
    /// Returns the total time recorded this frame.
    /// You'll want to stop the Profiler before you call this.
    /// Recursively adds up any sub-profilers as well
    /// </summary>
    public float GetTotal()
    {
        float total = (float)m_timer.Elapsed.TotalMilliseconds;
        foreach (Profiler child in m_children)
        {
            total += child.GetTotal();
        }
        return total;
    }

    /// <summary>
    /// Returns the maximum over the most recent NUM_SAMPLE frames
    /// Recursively adds up any sub-profilers as well
    /// </summary>
    public float GetMax()
    {
        float max = 0.0f;
        for (int i = 0; i < NUM_SAMPLE; ++i)
        {
            if (m_sample[i] > max)
                max = m_sample[i];
        }
        foreach (Profiler child in m_children)
        {
            max += child.GetMax();
        }
        return max;
    }

    /// <summary>
    /// Returns the average over the most recent NUM_SAMPLE frames
    /// Recursively adds up any sub-profilers as well
    /// </summary>
    public float GetAverage()
    {
        float total = 0.0f;
        for (int i = 0; i < NUM_SAMPLE; ++i)
        {
            total += m_sample[i];
        }
        foreach (Profiler child in m_children)
        {
            total += child.GetMax();
        }
        return total / NUM_SAMPLE;
    }

    /// <summary>
    /// Draws the Profile Bars.  This function will recursively call any sub-profilers within the Profiler you give it.
    /// </summary>
    /// <param name="spriteBatch">You need to give it a spriteBatch to use.</param>
    /// <param name="pos">The top-left corner of where you want the Profiler drawn</param>
    /// <param name="indent">Pass 0 here.  This is used internally</param>
    public void DrawBars(SpriteBatch spriteBatch, Vector2 pos, int indent)
    {
        Rectangle rect = new Rectangle((int)pos.X, (int)pos.Y, (int)(s_barWidth * GetMax()), (int)s_barHeight);
        Color color = m_color;
        color.A = 80;
        spriteBatch.Draw(s_barTex, rect, color);
        rect.Width = (int)(s_barWidth * GetTotal());
        spriteBatch.Draw(s_barTex, rect, m_color);
        int tickX = (int)(1000.0f / 60.0f * s_barWidth + pos.X);
        rect.X = tickX;
        rect.Width = 1;
        spriteBatch.Draw(s_barTex, rect, Color.White);

        Vector2 textPos = new Vector2(pos.X + s_indent * indent, pos.Y);
        spriteBatch.DrawString(s_font, m_name, textPos, Color.White);

        foreach (Profiler child in m_children)
        {
            pos.Y += s_barHeight;
            child.DrawBars(spriteBatch, pos, indent + 1);
        }
    }
}