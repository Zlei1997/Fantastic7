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
    class PlayControls : GameControls
    {

        //Keys[] playControlList = { Keys.Escape, Keys.W, Keys.A, Keys.S, Keys.D, Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.Space };

        public PlayControls(Keys[] mainMenuControls) : base()
        {
            timerSet[Keys.Up].goalTime = 0;
            timerSet[Keys.Down].goalTime = 0;
            timerSet[Keys.Left].goalTime = 0;
            timerSet[Keys.Right].goalTime = 0;
            timerSet[Keys.S].goalTime = 0;
            timerSet[Keys.D].goalTime = 0;
            timerSet[Keys.A].goalTime = 0;
            timerSet[Keys.W].goalTime = 0;
            //setTimer[Keys.Down].goalTime = (value) ;
        }


        public Boolean getPause()
        {
            return getOneTime(Keys.Escape);

        }



        public Boolean getMoveLeft()
        {
            return getGenericKey(Keys.A) || getGenericKey(Keys.Left);
        }


        public Boolean getMoveRight()
        {
            return getGenericKey(Keys.D) || getGenericKey(Keys.Right);
        }

        public Boolean getMoveUp()
        {
            return getGenericKey(Keys.W) || getGenericKey(Keys.Up);

        }

        public Boolean getMoveDown()
        {
            return getGenericKey(Keys.S) || getGenericKey(Keys.Down);

        }

       

        public Boolean getShootKey()
        {
            return getGenericKey(Keys.Escape);


        }

   




    }

}


