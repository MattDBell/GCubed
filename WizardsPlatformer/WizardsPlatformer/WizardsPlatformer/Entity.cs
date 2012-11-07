using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

//abstract class for now in case we decide to add variables.  Can always make it an interface later.  Jeez.
abstract class Entity
{
    //Called once on creation.  We'll probably create a wrapper around content manager in order to give
    //Handles to resources as opposed to duplicating resources
    public abstract void Init(ContentManager content);

    //Called once on spawning, separates static resource initialization and per entity instance initialization,
    //For reusing entities.
    public abstract void Spawn();

    //Called once a frame.
    public abstract void Update(GameTime gameTime);
    
    //Called once a draw call.
    public abstract void Draw();
    
    //This is going to change.  Collision handling should be move into a physics component.
    public abstract void HandleCollisions(List<zCollisionPrimitive> against);
}

