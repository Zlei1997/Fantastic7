using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantastic7
{
    public class Weapon
    {
        public float _Damage;
        public Boolean IsUsing = false;
        public Weapon(float Damage=20)
        {
            _Damage = Damage;
        }
    }
    public class Gun : Weapon
    {
        public int _FireRate;
        public float _BulletSpeed;
        public int _BulletSize;       
        public Boolean _Penetrate;
        public int _ReloadTimer=0;
        public Gun(float Damage=20,int FireRate=10,float BulletSpeed=1500,int BulletSize=8,Boolean Penetrate = false):base(Damage)
        {
            // FireRate: The larger the slower, represent the interval of shooting by ticks.
            _Damage = Damage;
            _FireRate = FireRate;
            _BulletSpeed = BulletSpeed;
            _BulletSize = BulletSize;
            _Penetrate = Penetrate;
        }
    }
    class Bullet : GObject
    {
        public float _BulletSpeed;
        public Bullet(GSprite sprite, CollisionNature collisionNature = CollisionNature.Free,float BulletSpeed=1500) : base(sprite,CollisionNature.Free)
        {
            _BulletSpeed = BulletSpeed;
        }
    }
}
