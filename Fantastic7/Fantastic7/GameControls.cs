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
    class GameControls
    {

        /* Keyboard "Key"
         * 
         * 0 = escape
         * 1 = enter
         * 
         * 2 = w
         * 3 = a
         * 4 = s
         * 5 = d
         * 6 = left arrow
         * 7 = right arrow
         * 8 = up arrow
         * 9 = down arrow
         * 
         * 10 = spacebar (shoot)
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         */
        protected Keys[] keyList; //set of controls
        protected enum keyState {noPress, press, release, hold};
        protected Dictionary<Keys,keyState> keyBuffer; //Buffer of Previous states
        private Keys[] defaultControls = { Keys.Escape, Keys.Enter, Keys.W, Keys.A, Keys.S, Keys.D, Keys.Left, Keys.Right, Keys.Up, Keys.Down};

        protected Dictionary<Keys, GTimer> timerSet;
       


        //TODO :||: Add an alternate constructor that accepts List<Keys>//
        public GameControls(Keys[] controlList)
        {
            keyBuffer = new Dictionary<Keys, keyState>();
            keyList = controlList;
            timerSet = new Dictionary<Keys, GTimer>();


            foreach (Keys k in keyList)
            {

                keyBuffer.Add(k, keyState.noPress);
                timerSet.Add(k, new GTimer(50));
            }
        }




        public GameControls()
        {
            keyBuffer = new Dictionary<Keys, keyState>();
            keyList = defaultControls;
            timerSet = new Dictionary<Keys, GTimer>();
            
            
            foreach(Keys k in keyList)
            {
                
                keyBuffer.Add(k, keyState.noPress);
                timerSet.Add(k, new GTimer(50));
            }

            
        }


        public void update(GameTime gt)
        {

            foreach (Keys k in keyList)
            {


                timerSet[k].update(gt);



                switch (keyBuffer[k])
                {
                    case keyState.noPress:

                        if (Keyboard.GetState().IsKeyDown(k))
                        {
                            keyBuffer[k] = keyState.press;
                            timerSet[k].Active = true;
                        }
                        else
                        {

                        }

                        break;

                    case keyState.press:

                        if (Keyboard.GetState().IsKeyDown(k))
                        {
                            keyBuffer[k] = keyState.hold;
                        }
                        else
                        {
                            keyBuffer[k] = keyState.release;
                            timerSet[k].Active = false;
                        }

                        break;

                    case keyState.release:

                        if (Keyboard.GetState().IsKeyDown(k))
                        {
                            keyBuffer[k] = keyState.press;
                            timerSet[k].Active = true;
                        }
                        else
                        {
                            keyBuffer[k] = keyState.noPress;
                        }

                        break;

                    case keyState.hold:

                        if (Keyboard.GetState().IsKeyDown(k))
                        {

                        }
                        else
                        {
                            keyBuffer[k] = keyState.release;
                            timerSet[k].Active = false;
                        }

                        break;



                }

            }


        }

        public Boolean getGenericKey(Keys k)
        {
            //if(state == onPress){return true;}
            //if(state == hold){if(timer.ready){timer.reset; return true;}
            //else{return false;}

            if (keyBuffer[k] == keyState.press )
            {

                return true;

            }
            else if (keyBuffer[k] == keyState.hold )
            {

                if (timerSet[k].getGoalHit() )
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

        }

        public Boolean getOneTime(Keys k)
        {
            //if(state == onPress){return true;}
            //if(state == hold){if(timer.ready){timer.reset; return true;}
            //else{return false;}

            if (keyBuffer[k] == keyState.press)
            {

                return true;

            }
            else { 
                return false;
            }

        }


    }
}
