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
    class GTimer
    {

        public int goalTime;
        int currentTime;
        Boolean goalHit;
        private Boolean active;
        public Boolean Active {

            
            set {

                if (!active)
                {
                   active = true;
                }
                else
                {
                    currentTime = 0;
                    active = false;
                    goalHit = false;
                }


            }
        }


        public GTimer(int goal)
        {
            goalTime = goal;
            currentTime = 0;
            goalHit = false;
        }

        public void update(GameTime gt)
        {
            if (!active || goalTime == -1)
            {
                return;
            }
    
            currentTime += gt.ElapsedGameTime.Milliseconds;

            if(currentTime >= goalTime)
            {
                goalHit = true;
                currentTime -= goalTime;
            }
        }

        public Boolean getGoalHit()
        {

            if (goalHit)
            {
                goalHit = false;
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
