using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantastic7
{
    class EventHandler
    {
        public static Random rand = new Random();
        private Map _currmap;
        private CollisionHandler CollisionHandler;
        private EntityBehaviorHandler EntityBehaviorHandler;
        public EventHandler(Map currmap)
        {
            _currmap = currmap;
            CollisionHandler = new CollisionHandler(_currmap);
            EntityBehaviorHandler = new EntityBehaviorHandler(_currmap);

        }
        public void handle(GameTime gt)
        {
            CollisionHandler.handle();
            EntityBehaviorHandler.handle(gt);
        }
    }
    class CollisionHandler
    {
        private Map _currmap;
        private Room _currRoom;
        private Entity _player;
        public enum Direction
        {
            North,
            East,
            South,
            West
        };
        public CollisionHandler(Map currmap)
        {
            _currmap = currmap;
            _player = _currmap.player;
        }
        public void handle()
        {
            _currRoom = _currmap._currRoom;
            DoorCollisionHandle();
            ObjectCollisionHandle();
            FloorCollisionHandle();            
        }
        public void FloorCollisionHandle()
        {
            //Check and deal object collision with the wall
            List<GObject> _go = _currRoom.getGObjects();
            for (int i = 0; i < _go.Count; i++)
            {
                if(_go[i] is Bullet && FloorCollisionDetect(_go[i]))
                {
                    _currRoom.getGObjects().Remove(_go[i]);
                    return;
                }
                int objX = (int)_go[i].CollisionRect().Value.X;
                int objY = (int)_go[i].CollisionRect().Value.Y;
                int objWidth = _go[i].CollisionRect().Value.Width;
                int objHeight = _go[i].CollisionRect().Value.Height;
                if (_go[i].getPosition().X < _currRoom.floor.X)
                {
                    _go[i].jumpTo(new Vector2(_currRoom.floor.X, objY));
                }                    
                if (_go[i].getPosition().X > (_currRoom.floor.X + _currRoom.floor.Width - objWidth))
                {
                    _go[i].jumpTo(new Vector2((_currRoom.floor.X + _currRoom.floor.Width - objWidth), objY));
                }                    
                if (_go[i].getPosition().Y < _currRoom.floor.Y)
                {
                    _go[i].jumpTo(new Vector2(objX, _currRoom.floor.Y));
                }                    
                if (_go[i].getPosition().Y > (_currRoom.floor.Y + _currRoom.floor.Height - objHeight))
                {
                    _go[i].jumpTo(new Vector2(objX, (_currRoom.floor.Y + _currRoom.floor.Height - objHeight)));
                }                    
            }
        }
        public Boolean FloorCollisionDetect(GObject go)
        {
            //check object collision with the wall
            int objWidth = go.CollisionRect().Value.Width;
            int objHeight = go.CollisionRect().Value.Height;
            if (go.getPosition().X < _currRoom.floor.X)
            {
                return true;
            }
            if (go.getPosition().X > (_currRoom.floor.X + _currRoom.floor.Width - objWidth))
            {
                return true;
            }
            if (go.getPosition().Y < _currRoom.floor.Y)
            {
                return true;
            }
            if (go.getPosition().Y > (_currRoom.floor.Y + _currRoom.floor.Height - objHeight))
            {
                return true;
            }
            return false;
        }
        public void DoorCollisionHandle()
        {
            //Check and deal object collision with the door(including changing room)
            Direction direction;
            GObject[] doors = _currRoom.getDoors();

            foreach (GObject door in doors)
            {
                if (_player.CollisionRect().Value.Intersects(door.CollisionRect().Value))
                {
                    int doorY = door.CollisionRect().Value.Y;
                    int doorX = door.CollisionRect().Value.X;
                    Room DirectionRoom = null;

                    // Looking for which side door player is touching
                    if (doorY <= 0 && doorX < 1280 / 2)
                    {
                        DirectionRoom = _currRoom.up;
                        direction = Direction.North;
                    }
                    else if (doorX <= 0 && doorY < 720 / 2)
                    {
                        DirectionRoom = _currRoom.left;
                        direction = Direction.West;
                    }
                    else if (doorX > 1000 && doorY < 720 / 2)
                    {
                        DirectionRoom = _currRoom.right;
                        direction = Direction.East;
                    }
                    else
                    {
                        DirectionRoom = _currRoom.down;
                        direction = Direction.South;
                    }

                    // Changing rooms and also moving player into new room while facing correct position
                    if (DirectionRoom != null)
                    {
                        _currRoom.removeObject(_player);
                        _currmap.changeRoomByInstance(DirectionRoom);
                        _currRoom = _currmap._currRoom;
                        // move player to new room
                        //_player = (Entity)g;
                        _currRoom.addObject(_player);
                        // Based on door entered, reposition player location
                        if (direction.Equals(Direction.North))
                            _player.jumpTo(new Vector2(1280 / 2, 720 - 180));
                        else if (direction.Equals(Direction.East))
                            _player.jumpTo(new Vector2(130, 720 / 2));
                        else if (direction.Equals(Direction.South))
                            _player.jumpTo(new Vector2(1280 / 2, 130));
                        else if (direction.Equals(Direction.West))
                            _player.jumpTo(new Vector2(1280 - 180, 720 / 2));
                    }
                    //Console.Out.WriteLine("Player touched door at X:" + door.CollisionRect().Value.X + " Y: " + door.CollisionRect().Value.Y);
                }
            }
        }
        public void ObjectCollisionHandle()
        {
            //Core collision handler
            List<GObject> _go = _currRoom.getGObjects();
            for (int i = 0; i<_go.Count-1; i++)
            {
                for (int j=i+1;j<_go.Count; j++)
                {
                    if (_go[i].CollisionRect().Value.Intersects(_go[j].CollisionRect().Value))
                    {
                        if (_go[i] is Bullet)
                        {
                            _currRoom.getGObjects().Remove(_go[i]);
                            return;
                        }
                        if (_go[j] is Bullet)
                        {
                            _currRoom.getGObjects().Remove(_go[j]);
                            return;
                        }
                        if (_go[i]._collisionNature == GObject.CollisionNature.Free)
                        {
                            FreeCollisionHandle(_go[i], _go[j]);
                        }
                        else if (_go[j]._collisionNature == GObject.CollisionNature.Free)
                        {
                            FreeCollisionHandle(_go[j], _go[i]);
                        }
                        else if(_go[i]._collisionNature == GObject.CollisionNature.Stable)
                        {
                            StableCollisionHandle(_go[j], _go[i]);
                        }
                        else if (_go[j]._collisionNature == GObject.CollisionNature.Stable)
                        {
                            StableCollisionHandle(_go[i], _go[j]);
                        }
                        else if(_go[i]._collisionNature == GObject.CollisionNature.KnockBack && _go[j]._collisionNature== GObject.CollisionNature.KnockBack)
                        {
                            KnockbackCollisionHandle(_go[i], _go[j]);
                        }
                    }
                }
            }
        }
        private Direction CheckDirection(Rectangle? x,Rectangle? y)
        {
            //Check x's relative direction compare to y
            //ex. return North indicates that x is at the North of y
            float deltaX = y.Value.Center.X - x.Value.Center.X;
            float deltaY = y.Value.Center.Y - x.Value.Center.Y;
            float delta = deltaY / deltaX;
            if (delta>1 || delta<-1)
            {
                if (deltaY>0)
                {
                    return Direction.North;
                }
                else
                {
                    return Direction.South;
                }
            }
            else
            {
                if (deltaX > 0)
                {
                    return Direction.West;
                }
                else
                {
                    return Direction.East;
                }
            }
        }
        private void FreeCollisionHandle(GObject x,GObject y)
        {
            switch (CheckDirection(x.CollisionRect(), y.CollisionRect()))
            {
                case Direction.North:
                    x.jumpTo(new Vector2(x.CollisionRect().Value.X, y.CollisionRect().Value.Top-x.CollisionRect().Value.Height));
                    break;
                case Direction.South:
                    x.jumpTo(new Vector2(x.CollisionRect().Value.X, y.CollisionRect().Value.Bottom));
                    break;
                case Direction.West:
                    x.jumpTo(new Vector2(y.CollisionRect().Value.Left - x.CollisionRect().Value.Width, x.CollisionRect().Value.Y));
                    break;
                case Direction.East:
                    x.jumpTo(new Vector2(y.CollisionRect().Value.Right, x.CollisionRect().Value.Y));
                    break;
            }
        }
        private void StableCollisionHandle(GObject x, GObject y)
        {
            switch (CheckDirection(x.CollisionRect(), y.CollisionRect()))
            {
                case Direction.North:
                    x.jumpTo(new Vector2(x.CollisionRect().Value.X, y.CollisionRect().Value.Top - x.CollisionRect().Value.Height));
                    break;
                case Direction.South:
                    x.jumpTo(new Vector2(x.CollisionRect().Value.X, y.CollisionRect().Value.Bottom));
                    break;
                case Direction.West:
                    x.jumpTo(new Vector2(y.CollisionRect().Value.Left - x.CollisionRect().Value.Width, x.CollisionRect().Value.Y));
                    break;
                case Direction.East:
                    x.jumpTo(new Vector2(y.CollisionRect().Value.Right, x.CollisionRect().Value.Y));
                    break;
            }
        }
        private void KnockbackCollisionHandle(GObject x, GObject y)
        {
            Random r = new Random();
            float KnockBackDistance = 40;
            int Skew = 15 * (r.Next(0,2) * 2 - 1);
            int SkewBias = 25;
            switch (CheckDirection(x.CollisionRect(), y.CollisionRect()))
            {
                case Direction.North:
                    x.move(new Vector2(Skew + r.Next(-SkewBias,SkewBias),-KnockBackDistance));
                    y.move(new Vector2(-Skew + r.Next(-SkewBias, SkewBias), KnockBackDistance));
                    break;
                case Direction.South:
                    x.move(new Vector2(Skew + r.Next(-SkewBias, SkewBias), KnockBackDistance));
                    y.move(new Vector2(-Skew + r.Next(-SkewBias, SkewBias), -KnockBackDistance));
                    break;
                case Direction.West:
                    x.move(new Vector2(-KnockBackDistance, Skew + r.Next(-SkewBias, SkewBias)));
                    y.move(new Vector2(KnockBackDistance, -Skew + r.Next(-SkewBias, SkewBias)));
                    break;
                case Direction.East:
                    x.move(new Vector2(KnockBackDistance, Skew + r.Next(-SkewBias, SkewBias)));
                    y.move(new Vector2(-KnockBackDistance, -Skew + r.Next(-SkewBias, SkewBias)));
                    break;
            }
        }
    }
    class EntityBehaviorHandler
    {
        private Map _currmap;
        private Room _currRoom;
        private Entity _player;
        public EntityBehaviorHandler(Map currmap)
        {
            _currmap = currmap;
            _player = _currmap.player;
        }
        public void handle(GameTime gt)
        {
            _currRoom = _currmap._currRoom;
            EntityMoveHandle(gt);
            WeaponUseHandle(gt);
        }
        public void EntityMoveHandle(GameTime gt)
        {
            //Random movement for Entities
            List<GObject> _go = _currRoom.getGObjects();
            Entity en;
            foreach (GObject go in _go)
            {
                if (go is Entity)
                {
                    en = (Entity)go;
                    if (!en.Equals(_player))
                    {
                        if (EventHandler.rand.Next(0, 1000) < 20)
                        {
                            en.direction = (CollisionHandler.Direction)EventHandler.rand.Next(0, 4);
                        }                     
                        en.moveForward (en.movementSpeed * (float)gt.ElapsedGameTime.TotalSeconds);
                        if (EventHandler.rand.Next(0, 1000) < 10)
                        {
                            en._mainweapon.IsUsing = true;
                        }
                        if (EventHandler.rand.Next(0, 1000) > 980)
                        {
                            en._mainweapon.IsUsing = false;
                        }
                    }                       
                }
            }
        }
        public void WeaponUseHandle(GameTime gt)
        {
            //check usage and status of the weapon
            List<GObject> _go = _currRoom.getGObjects();
            Entity en;
            for (int i=0;i<_go.Count;i++)
            {
                if (_go[i] is Entity)
                {
                    en = (Entity)_go[i];
                    if (en._mainweapon!=null)
                    {
                        if (en._mainweapon is Gun)
                        {
                            GunUseHandle(gt, en);
                        }
                    }
                }
                else if(_go[i] is Bullet)
                {
                    BulletShotHandle(gt, _go[i]);
                }
            }
        }
        public void GunUseHandle(GameTime gt,Entity en)
        {
            //Shot the bullet in the right direction
            Gun gun = (Gun)en._mainweapon;
            if (gun.IsUsing)
            {
                if (gun._ReloadTimer % gun._FireRate != 0)
                {
                    gun._ReloadTimer++;
                    return;
                }
                int LaunchX = 0;
                int LaunchY = 0;
                switch (en.direction)
                {
                    case CollisionHandler.Direction.North:
                        LaunchX = en.CollisionRect().Value.Center.X - gun._BulletSize / 2;
                        LaunchY = en.CollisionRect().Value.Top - gun._BulletSize - 5;
                        break;
                    case CollisionHandler.Direction.South:
                        LaunchX = en.CollisionRect().Value.Center.X - gun._BulletSize / 2;
                        LaunchY = en.CollisionRect().Value.Bottom + 5;
                        break;
                    case CollisionHandler.Direction.West:
                        LaunchX = en.CollisionRect().Value.Left - gun._BulletSize - 5;
                        LaunchY = en.CollisionRect().Value.Center.Y - gun._BulletSize / 2;
                        break;
                    case CollisionHandler.Direction.East:
                        LaunchX = en.CollisionRect().Value.Right + 5;
                        LaunchY = en.CollisionRect().Value.Center.Y - gun._BulletSize / 2;
                        break;

                }
                Bullet b = new Bullet(new NSprite(new Rectangle(LaunchX, LaunchY, gun._BulletSize, gun._BulletSize), Color.Black), GObject.CollisionNature.Free, gun._BulletSpeed);
                b.direction = en.direction;
                _currRoom.addObject(b);
                gun._ReloadTimer++;
            }
            else if (gun._ReloadTimer % gun._FireRate != 0)
            {
                gun._ReloadTimer++;
            }
            
        }
        public void BulletShotHandle(GameTime gt, GObject go)
        {
            //Handle the Bullet track after shot
            Bullet bullet = (Bullet)go;
            bullet.moveForward(bullet._BulletSpeed * (float)gt.ElapsedGameTime.TotalSeconds);
        }
    }
}
