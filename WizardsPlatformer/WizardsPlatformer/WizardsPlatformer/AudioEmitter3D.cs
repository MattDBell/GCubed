//----------------------------------------------------------------------------------------
//	Copyright 2009-2011 Matt Whiting, All Rights Reserved.
//  For educational purposes only.
//  Please do not distribute or republish in electronic or print form without permission.
//  Thanks - CrashLotus@gmail.com
//----------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using AudioHandle = StaticPool<AudioCue>.Handle;



/// <summary>
/// This is a helper class for automatically connecting a 3D sound to a GameObj3D.
/// You must call Update() once per frame from within the Update() function of your GameObj3D.
/// Be sure to finish updating your GameObj3D BEFORE you call this function.
/// This class assumes that the GameObj3D you give it is a root object - that means the object is not a child of
/// another object in a hierarchy.
/// If your GameObj3D is NOT a root object, you will need to pass in "null" as the "parentObj" in the constructor.
/// At that point, you must compute the Object-To-World Matrix yourself and pass that into UpdateMatrix() once per
/// frame instead of calling Update().
/// 
/// WARNING: If you use this to play a looping sound, you must remember to call StopSound() to stop the sound.
/// This class will NOT automatically destroy the sound if your object is deleted.
/// </summary>
public class AudioEmitter3D
{
    GameObj3D m_parentObj;
    Vector3 m_offset;
    Vector3 m_oldPos;
    AudioEmitter m_emitter;
    AudioHandle m_sound;

    /// <summary>
    /// Construct a new AudioEmitter3D.  This will automatically start the specified sound playing.
    /// </summary>
    /// <param name="parentObj">The GameObj3D this sound is attached to.  You may specify "null".</param>
    /// <param name="sound">The sound to play.  You may specify "null".</param>
    public AudioEmitter3D(GameObj3D parentObj, string sound, Vector3? offset)
    {
        m_parentObj = parentObj;
        m_offset = Vector3.Zero;
        if (null != offset)
        {
            m_offset = (Vector3)offset;
        }

        //set up the emitter
        m_emitter = new AudioEmitter();
        Matrix objMat = Matrix.Identity;
        if (null != parentObj)
        {
            objMat = m_parentObj.GetMatrix();
        }
        m_emitter.Forward = objMat.Forward;
        m_emitter.Up = objMat.Up;
        m_emitter.Position = Vector3.Transform(m_offset, objMat);
        m_emitter.Velocity = Vector3.Zero;

        m_oldPos = m_emitter.Position;

        m_sound.Invalidate();

        if (null != sound)
            PlaySound(sound);
    }

    /// <summary>
    /// Stop the sound
    /// </summary>
    public void StopSound()
    {
        AudioComponent.Get().StopSound(m_sound);
        m_sound.Invalidate();
    }

    /// <summary>
    /// Start a new sound.
    /// </summary>
    /// <param name="sound">The name of the sound to play</param>
    public void PlaySound(string sound)
    {
        StopSound();    //This class can only track one sound at a time so kill any previous sounds.
        m_sound = AudioComponent.Get().PlaySound3D(sound, m_emitter);
    }

    /// <summary>
    /// Update the sound to track the parent GameObj3D automatically.
    /// </summary>
    /// <param name="gameTime">GameTime</param>
    public void Update(GameTime gameTime)
    {
        Matrix objMat = m_parentObj.GetMatrix();
        UpdateMatrix(objMat, gameTime);
    }

    /// <summary>
    /// Update the sound with a new matrix manually.
    /// </summary>
    /// <param name="matrix">Object-To-World Matrix for the 3D sound</param>
    /// <param name="gameTime"></param>
    public void UpdateMatrix(Matrix matrix, GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        m_emitter.Forward = matrix.Forward;
        m_emitter.Up = matrix.Up;
        m_emitter.Position = Vector3.Transform(m_offset, matrix);
        m_emitter.Velocity = (m_emitter.Position - m_oldPos) / dt;
        m_oldPos = m_emitter.Position;

        AudioComponent.Get().UpdateSound3D(m_sound, m_emitter);
    }

    /// <summary>
    /// Sets a user-defined variable for the sound
    /// </summary>
    /// <param name="varName">name of the variable to set</param>
    /// <param name="value">value of the variable to set</param>
    public void SetVariable(string varName, float value)
    {
        AudioComponent.Get().SetVariable(m_sound, varName, value);
    }
}
