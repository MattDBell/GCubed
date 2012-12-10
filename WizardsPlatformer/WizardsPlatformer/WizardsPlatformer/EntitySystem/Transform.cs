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
        const float gravity = 0.0f; //Obviously should change
        bool usesGravity = false;
        bool active;
        float maxSpeed = 100.0f;
        float drag = 0.0f;
        bool toZero = false;
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
        public void SetAcceleration(Vector2 newAcc) { acceleration = newAcc; toZero = false; }
        public void DeActivate() { active = false; }
        public void ReActivate() { active = true; }
        public void MaxSpeedOut() { velocity.Normalize(); velocity *= maxSpeed; toZero = false; }
        public void AddSpeed(float toAdd) 
        { 
            float currSpeed = velocity.Length();
            float nextSpeed = currSpeed + toAdd;
            nextSpeed = nextSpeed > maxSpeed ? maxSpeed : nextSpeed;
            if (nextSpeed == currSpeed) return;
            velocity.Normalize();
            velocity *= nextSpeed;
            toZero = false;
        }
        public void EnableGravity() { usesGravity = true; }
        public void DisableGravity() { usesGravity = false; }
        public void SetDrag(float to) { drag = to; }
        public float GetDrag() { float womensClothing = drag; return womensClothing; }
        public void AddAcceleration(Vector2 toAdd) { acceleration += toAdd; toZero = false; }
        public void AccelerateToZero(float factor)
        {
            if (velocity == Vector2.Zero)
                return;
            acceleration = velocity;
            acceleration.Normalize();
            acceleration *= -factor;
            toZero = true;
        }
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
            if(usesGravity)
                usedAccel.Y -= gravity; //But wait.  Isn't that a reference.  NOPE.  Because C# is retarded and Vector2 is a struct.
            lastPosition = position;
            velocity += usedAccel * gt.UsableGameTime();

            if (toZero && Vector2.Dot(velocity, acceleration) >= 0)
            {
                velocity = Vector2.Zero;
                toZero = false;
            }
            position += velocity * gt.UsableGameTime();  //What's UsableGameTime?  I don't know.  Maybe XNAisShit knows?
            
            if (velocity.LengthSquared() > maxSpeed * maxSpeed)
            {
                MaxSpeedOut();
            }
        }
    }
}
