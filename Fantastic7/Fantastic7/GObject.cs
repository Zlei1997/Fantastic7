using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fantastic7
{
    class GObject
    {
        public Vector2 _pos;
        protected GSprite _sprite;
        public CollisionNature _collisionNature;
        public CollisionHandler.Direction direction = CollisionHandler.Direction.North;
        public enum CollisionNature {
            Stable,
            Free,
            KnockBack
        };

        public GObject(GSprite sprite,CollisionNature collisionNature=CollisionNature.Stable)
        {
            _pos = sprite.getPosition();
            _sprite = sprite;
            _collisionNature = collisionNature;
        }

        //Changes object position to new point.
        public void jumpTo(Vector2 position)
        {
            _pos.X = position.X;
            _pos.Y = position.Y;
            _sprite.jumpTo(position);
        }

        //Changes object position by a vector.
        public void move(Vector2 vector)
        {
            _pos += vector;
            _sprite.jumpTo(_pos);
            //Console.Out.WriteLine("Obj: " + _pos.Y + "\tSpr: " + _sprite.getPosition().Y);
        }

        public void draw(SpriteBatchPlus sb, float scale)
        {
            _sprite.draw(sb, scale);
        }
        public GSprite getSprite()
        {
            return _sprite;
        }
        public Vector2 getPosition()
        {
            return _pos;
        }
        public void moveForward(float distance)
        {
            switch (direction)
            {
                case CollisionHandler.Direction.North:
                    move(new Vector2(0, -distance));
                    break;
                case CollisionHandler.Direction.South:
                    move(new Vector2(0, distance));
                    break;
                case CollisionHandler.Direction.West:
                    move(new Vector2(-distance, 0));
                    break;
                case CollisionHandler.Direction.East:
                    move(new Vector2(distance, 0));
                    break;
            }
        }
        public Rectangle? CollisionRect()
        {
            if (_sprite is NSprite)
            {
                NSprite n = (NSprite)_sprite;
                int width = n.getRect().Width;
                int height = n.getRect().Height;
                return new Rectangle((int)_pos.X, (int)_pos.Y, width, height);
            }
            return null;
        }
    }

    class Entity : GObject
    {
        public int _maxHealth { get; private set; }
        public int _curHealth { get; private set; }
        protected int _intDamage;
        protected bool damage;
        protected int _movementSpeed;
        protected bool dead = false; //Used to mark it as ready for garbage collection
        
        public Weapon _mainweapon;

        public int movementSpeed { get { return _movementSpeed; } }

        //By default, the entities are set as invulnerable if the max health is not changed
        //Interaction damage is used when entities interact, as long as they both use damage, interaction damage is
        //subtracted from the other entity
        public Entity(GSprite sprite, int maxHealth = -1, int interactionDamage = 0, int speed = 400, CollisionNature collisionNature = CollisionNature.KnockBack,Weapon mainweapon=null
            ) : base(sprite,collisionNature)
        {
            if (maxHealth < 0) damage = false;
            else damage = true;
            _maxHealth = maxHealth;
            _curHealth = maxHealth;
            _intDamage = interactionDamage;
            _movementSpeed = speed;
            direction = (CollisionHandler.Direction)EventHandler.rand.Next(0, 4);
            _mainweapon = mainweapon;
        }

        //Used to deal damage and gain back health. Unbounded so negative numbers can be passed it
        //Use negative numbers to deal damage, and positive to gain health
        //Damage change will only occure if entity is flagged with using health, rather than being invulnerable
        public void modifyHealth(int delta)
        {
            if (damage)
            {
                _curHealth += delta;
                if (_curHealth < 0)
                {
                    _curHealth = 0;
                    dead = true;
                }
                else if (_curHealth > _maxHealth) _curHealth = _maxHealth;
            }
        }

        //Bounded health change, only positive numbers will used. Will heal entity by current amount
        public void getHealth(int regain)
        {
            if (regain > 0) modifyHealth(regain);
        }
    }
}
