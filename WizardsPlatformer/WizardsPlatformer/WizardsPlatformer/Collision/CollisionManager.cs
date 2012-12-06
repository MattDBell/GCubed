using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

partial class CollisionManager
{
    List<CollisionComponent> allComponents = new List<CollisionComponent>();
    
    static CollisionManager instance;
    static CollisionManager() { instance = new CollisionManager();  }
    public static CollisionManager Get() { return instance; }


    public void CheckCollisions() { }
    public CollisionComponent GetCollComponent() 
    { 
        CollisionComponent ret = compCreator(null, null);
        allComponents.Add(ret);
        return ret;
    }
    public void DrawCollisionPrimatives(SpriteBatch spritebatch)
    {
        foreach (CollisionComponent thing in allComponents)
        {
            thing.DrawCollisionPrimatives(spritebatch);
        }
    }
}
