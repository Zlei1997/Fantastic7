using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Fantastic7
{
    class LocInfo
    {
        public CollisionHandler.Direction mainDirection { get; set; }
        public CollisionHandler.Direction minorDirection { get; set; }
        public float mainDistance { get; set; }
        public float minorDistance { get; set; }
    }
    class EnemieType:Entity
    {
        public Entity player { get; set; }
        protected LocInfo locInfo=new LocInfo();
        public EnemieType(GSprite sprite, int maxHealth = -1, int interactionDamage = 0, int speed = 400, CollisionNature collisionNature = CollisionNature.KnockBack, Weapon mainweapon = null) : base(sprite, maxHealth, interactionDamage,speed,collisionNature,mainweapon)
        {
            
        }
        protected void getPlayerstatus()
        {
            if (player != null)
            {
                locInfo.mainDirection = CollisionHandler.CheckDirection(this.CollisionRect(), player.CollisionRect());
                locInfo.mainDistance = getDistance(locInfo.mainDirection);
                locInfo.minorDirection = getMinorDirection(locInfo.mainDirection);
                locInfo.minorDistance = getDistance(locInfo.minorDirection);
            }
        }
        private float getDistance(CollisionHandler.Direction d)
        {
            float myX = this.CollisionRect().Value.Center.X;
            float myY = this.CollisionRect().Value.Center.Y;
            float playerX = player.CollisionRect().Value.Center.X;
            float playerY = player.CollisionRect().Value.Center.Y;
            if (d == CollisionHandler.Direction.North || d == CollisionHandler.Direction.South)
            {
                return Math.Abs(myY - playerY);
            }
            else
            {
                return Math.Abs(myX - playerX);
            }
        }
        private CollisionHandler.Direction getMinorDirection(CollisionHandler.Direction d)
        {
            float myX = this.CollisionRect().Value.Center.X;
            float myY = this.CollisionRect().Value.Center.Y;
            float playerX = player.CollisionRect().Value.Center.X;
            float playerY = player.CollisionRect().Value.Center.Y;
            if (d == CollisionHandler.Direction.North || d == CollisionHandler.Direction.South)
            {
                if (playerX - myX > 0)
                {
                    return CollisionHandler.Direction.West;
                }
                else
                {
                    return CollisionHandler.Direction.East;
                }
            }
            else
            {
                if (playerY - myY > 0)
                {
                    return CollisionHandler.Direction.North;
                }
                else
                {
                    return CollisionHandler.Direction.South;
                }
            }
        }
        public static CollisionHandler.Direction oppDirection(CollisionHandler.Direction d)
        {
            switch (d)
            {
                case CollisionHandler.Direction.North:
                    return CollisionHandler.Direction.South;
                case CollisionHandler.Direction.South:
                    return CollisionHandler.Direction.North;
                case CollisionHandler.Direction.West:
                    return CollisionHandler.Direction.East;
                case CollisionHandler.Direction.East:
                    return CollisionHandler.Direction.West;
            }
            return CollisionHandler.Direction.North;
        }

    }
    class Ranger:EnemieType
    {
        public int safeRange {get; set;}
        public Ranger(GSprite sprite, int maxHealth = -1, int interactionDamage = 0, int speed = 400, CollisionNature collisionNature = CollisionNature.KnockBack, Weapon mainweapon = null,int saferange=300) : base(sprite,maxHealth,interactionDamage,speed,collisionNature,mainweapon)
        {
            safeRange = saferange + EventHandler.rand.Next(-100,100);
        }
        public void update(GameTime gt)
        {
            getPlayerstatus();
            if (locInfo.mainDistance < safeRange)
            {
                direction = locInfo.mainDirection;
                moveForward(_movementSpeed * (float)gt.ElapsedGameTime.TotalSeconds);
            }
            else if (locInfo.mainDistance > safeRange + EventHandler.rand.Next(0, 100))
            {
                direction = oppDirection(locInfo.mainDirection);
                moveForward(_movementSpeed * (float)gt.ElapsedGameTime.TotalSeconds);

            }
            if (locInfo.minorDistance > 20)
            {
                direction = oppDirection(locInfo.minorDirection);
                moveForward(_movementSpeed * (float)gt.ElapsedGameTime.TotalSeconds);
            }
            if (locInfo.minorDistance < 50)
            {
                direction = oppDirection(locInfo.mainDirection);
                _mainweapon.IsUsing = true;
            }
            else
            {
               _mainweapon.IsUsing = false;
            }
        }
    }
    class Charger : EnemieType
    {
        public Charger(GSprite sprite, int maxHealth = -1, int interactionDamage = 0, int speed = 400, CollisionNature collisionNature = CollisionNature.KnockBack, Weapon mainweapon = null) : base(sprite,maxHealth,interactionDamage,speed,collisionNature,mainweapon)
        {

        }
        public void update(GameTime gt)
        {
            getPlayerstatus();
            if (locInfo.mainDistance - locInfo.minorDistance > 100 || locInfo.minorDistance < 20)
            {
                direction = oppDirection(locInfo.mainDirection);
                moveForward(movementSpeed * (float)gt.ElapsedGameTime.TotalSeconds);
            }
            else
            {
                direction = oppDirection(locInfo.minorDirection);
                moveForward(movementSpeed * (float)gt.ElapsedGameTime.TotalSeconds);
            }

        }
    }
}
