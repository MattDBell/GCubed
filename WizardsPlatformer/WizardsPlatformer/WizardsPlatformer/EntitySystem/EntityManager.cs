using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

delegate void FunctionName(int stupid);

class EntityManager
{
    public enum ENT_TYPE {
        PLAYER,
        TOTAL
    };
    static EntityManager instance;
    static public EntityManager get() { return instance; } // I AM A SHITTY SINGLETON IMPLEMENTATION
    static List<Entity> allTehEntities;  // I AM HAS THE ENTITIES!
    static public Entity Create(ENT_TYPE type); // I AM THE FACTORY!
    static public bool Update(GameTime gT);
}
