using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
static class XNAisShit
{
    public static float UsableGameTime(this GameTime gt)
    {
        return (float)gt.ElapsedGameTime.TotalSeconds;
    }
}
