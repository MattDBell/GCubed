using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

partial class EntityManager
{
    delegate Entity Creator();
    static Creator[] Creators;
    public static void BOOT()
    {
        Creators = new Creator[(int)ENT_TYPE.TOTAL];
        Player.BOOT();
    }
    public enum ENT_TYPE
    {
        PLAYER,
        TOTAL
    };
    int Ent_Count;
    EntityManager()
    {
        Ent_Count = (int)ENT_TYPE.TOTAL; //Yes casting to int was getting this annoying.
        allTehEntities = new List<Entity>[Ent_Count];
        activeMark = new int[Ent_Count];
        for(int i = 0; i < Ent_Count; ++i)
        {
            activeMark[i] = 0;
        }
    }
    bool inUpdate = false;
    bool quit = false; 
    //==============SingleTon stuff========================
    static EntityManager instance;
    static public EntityManager get() { if (instance == null) instance = new EntityManager(); return instance; } // I AM A SHITTY SINGLETON IMPLEMENTATION
    
    //==============Factory stuff==========================
    ContentManager cm;
    
    // I AM THE FACTORY!  Hate non-abstract factories.  *whine whine whine*.  This will actually just buffer requests and then create
    public Entity Create(ENT_TYPE type)
    {
        Entity created = null;
        if (allTehEntities[(int)type] == null)
            allTehEntities[(int)type] = new List<Entity>();
        if(activeMark[(int) type] < allTehEntities[(int) type].Count) //There is an inactive... YAY!
        {
            created = allTehEntities[(int)type][activeMark[(int)type]];
            activeMark[(int)type]++;
            return created;
        }
        created = Creators[(int)type]();

        if (created == null)
            return null;
        if(cm != null)  
           created.Init(cm);
        allTehEntities[(int)type].Add(created);
        activeMark[(int)type]++;
        return created;

    }
    //=============Management stuff=======================
    List<Entity>[] allTehEntities;  // I AM HAS THE ENTITIES!  Stored by ENT_TYPE
    int[] activeMark; //entities below activeMark are active.  Above are inactive.  Because deleting is wrong!
    public void Init(ContentManager cm)
    {
        this.cm = cm;
        foreach( List<Entity> eList in allTehEntities)
        {
            if (eList == null) continue;
            foreach (Entity e in eList)
                e.Init(cm);
        }
    }
    List<int> noLongerActive = new List<int>(); //would be awesome if this was static function variable but noooo.  So now it's outside of the function.  In the cold.
    //This is what happens, C#, when you remove useful functionality.  Poor variables end up paying for it by freezing to death!
    public bool Update(GameTime gT)
    {
        inUpdate = true;
        Integrate(gT);
        for(int listIndex = 0; listIndex < Ent_Count; ++listIndex)
        {
            List<Entity> eList = allTehEntities[listIndex];
            if (eList == null) continue;
            noLongerActive.Clear();
            for (int i = 0; i < activeMark[listIndex]; ++i)
            {
                if (eList[i].Update(gT)) noLongerActive.Add(i);
            }
            for (int i = 0; i < noLongerActive.Count; ++i)
            {
                Entity removed = eList[noLongerActive[i]];
                eList[noLongerActive[i]] = eList[activeMark[listIndex] - 1];
                eList[activeMark[listIndex] - 1] = removed;
                activeMark[listIndex]--;
            }
        }
        inUpdate = false; //Creation can create more!  FOR EBAR!
        return quit; //True means... something <.<  I promise... Eventually
    }
    public void Draw(SpriteBatch sb) //for now
    {
        for (int listIndex = 0; listIndex < Ent_Count; ++listIndex)
        {
            List<Entity> eList = allTehEntities[listIndex];
            if (eList == null) continue;
            for (int i = 0; i < activeMark[listIndex]; ++i)
            {
                eList[i].Draw(sb);
            }
        }
    }
    public void SetQuit() { quit = true; }
}
