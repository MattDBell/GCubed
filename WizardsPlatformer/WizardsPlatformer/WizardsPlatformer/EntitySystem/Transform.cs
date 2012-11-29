using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

/*
 *  Why the hell does this have it's own class?
 *  Because Glenn is sick.  That'll teach him!
 */
partial class EntityManager
{
    List<Transform> transforms = new List<Transform>();
    public Transform GetTransform() { Transform newT = new Transform(); transforms.Add(newT); return newT; }
    void Integrate(GameTime gt) {foreach(Transform t in transforms) t.integrate(gt); }
    public class Transform
    {
        const float gravity = 0.0f;
        bool active;
        float maxSpeed = 100.0f;
        Vector2 position = Vector2.Zero, velocity = Vector2.Zero, acceleration = Vector2.Zero, lastPosition = Vector2.Zero;
        public void Set(Vector2 position, Vector2 velocity, Vector2 acceleration, float maxSpeed)
        {
            this.position = position;
            this.lastPosition = Vector2.Zero;
            this.velocity = velocity;
            this.acceleration = acceleration;
            this.maxSpeed = maxSpeed;
            active = true;
        }
        //Why aren't we using Properties, Matt?  Because they're fucking retarded, Glenn
        public Vector2 GetPos(){ return position; }
        public Vector2 GetLastPos() { return lastPosition;}  
        public void RePosition(Vector2 newPos)      
        { 
            position = newPos; 
            velocity = Vector2.Zero; 
            acceleration = Vector2.Zero; 
        }
        public void SetMaxSpeed(float newMaxSpeed)  { maxSpeed = newMaxSpeed; }
        public void SetAcceleration(Vector2 newAcc) { acceleration = newAcc; }
        public void DeActivate() { active = false; }
        public void ReActivate() { active = true; }
        public void MaxSpeedOut()                   { velocity.Normalize(); velocity *= maxSpeed; }
        public void AddSpeed(float toAdd) 
        { 
            float currSpeed = velocity.Length();
            float nextSpeed = currSpeed + toAdd;
            nextSpeed = nextSpeed > maxSpeed ? maxSpeed : nextSpeed;
            if (nextSpeed == currSpeed) return;
            velocity.Normalize();
            velocity *= nextSpeed;
        }  
        public void AddAcceleration(Vector2 toAdd) { acceleration += toAdd; }
        public void RemoveComponentAlong(Vector2 normal) 
        { 
            float comp = Vector2.Dot(normal, velocity); 
            velocity -= comp * normal; 
            comp = Vector2.Dot(normal, acceleration); 
            acceleration -= normal * comp; 
        } 
        public void integrate(GameTime gt) 
        {
            if (!active)
                return;
            Vector2 usedAccel = acceleration;
            usedAccel.Y -= gravity; //But wait.  Isn't that a reference.  NOPE.  Because C# is retarded and Vector2 is a struct.
            lastPosition = position;
            position += velocity * gt.UsableGameTime();  //What's UsableGameTime?  I don't know.  Maybe XNAisShit knows?
            velocity += usedAccel * gt.UsableGameTime();
            if (velocity.LengthSquared() > maxSpeed * maxSpeed)
            {
                MaxSpeedOut();
            }
        }
    }
}
