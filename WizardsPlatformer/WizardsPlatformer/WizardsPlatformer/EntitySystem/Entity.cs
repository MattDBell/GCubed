using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

partial class EntityManager{
//abstract class for now in case we decide to add variables.  Can always make it an interface later.  Jeez.
    abstract public class Entity
    {

        protected Transform transform;
        //Called once on creation.  We'll probably create a wrapper around content manager in order to give
        //Handles to resources as opposed to duplicating resources
        public virtual void Init(ContentManager content) { transform = EntityManager.get().GetTransform() ; } 

        //Called once on spawning, separates static resource initialization and per entity instance initialization,
        //For reusing entities.
        public virtual void Spawn(){ transform.ReActivate();}

        //Called once a frame.
        public abstract bool Update(GameTime gameTime);
    
        //This is going to change.  Collision handling should be move into a physics component.
        public abstract void HandleCollisions(List<zCollisionPrimitive> against);

        //This is called when an entity creates an entity and needs a reference to it.
        public abstract void GiveCreatedRef(Entity newlyCreated, EntityManager.ENT_TYPE created);

        //For now
        public abstract void Draw(SpriteBatch sb);
    }
}
