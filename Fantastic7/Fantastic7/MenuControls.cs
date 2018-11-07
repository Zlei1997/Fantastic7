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
    class MenuControls : GameControls 
    {

        public MenuControls(Keys[] mainMenuControls) : base() {

            timerSet[Keys.Escape].goalTime = -1;
            //setTimer[Keys.Down].goalTime = (value) ;
        }

        public Boolean getNextKey()
        {
            //if(state == onPress){return true;}
            //if(state == hold){if(timer.ready){timer.reset; return true;}
            //else{return false;}


            return getGenericKey(Keys.Down) || getGenericKey(Keys.S);

            /*
            if (keyBuffer[Keys.Down] == keyState.press || keyBuffer[Keys.S] == keyState.press)
            {
                
                return true;
                
            }
            else if (keyBuffer[Keys.Down] == keyState.hold || keyBuffer[Keys.S] == keyState.hold)
            {

                if (timerSet[Keys.Down].getGoalHit() || timerSet[Keys.S].getGoalHit())
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
            */
        }


        public Boolean getPrevKey()
        {
            return getGenericKey(Keys.Up) || getGenericKey(Keys.W);
        }


        public Boolean getSelect()
        {
            return getOneTime(Keys.Enter) ;
        }

        public Boolean getExit()
        {
            return getOneTime(Keys.Escape);
        }






    }
}
