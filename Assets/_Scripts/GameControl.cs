using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using game;

namespace game
{
    //Ideas: 
    // Vector2Int
    // Grid MVC
    // IMoveable, GemMoveable, BombMoveable - MVC
    // Object Factory, with Object Pool support
    // Unit Tests

    internal class GameControl : MonoBehaviour
    {
        internal RectTransform gameCanvas;

        // Game Grid
        internal Grid grid;


        //TODO: Score control
        //private int score;
        //private int score_multiplier;

        internal bool gameOverFlag;


        internal void Start()
        {
            grid.Init(this);

            Reset();
        }

        internal void Reset()
        {
            grid.Reset();

            //score = 0;
			//score_multiplier = 100;
			
			gameOverFlag = false;
			
			//countdown.Reset();
			
			//visuals.Reset();
			
			grid.populate(true);
		}



        internal int addScore(int newpoints)
		{
			//score += newpoints;
			//TODO: visuals.txtScore.text = score.ToString();
			return newpoints;
		}

        internal void gameOver(bool fromCountdown = false)
		{
			// If called from FuseCountdown, but is already game over (ie from no more matches), then ignore
			if( fromCountdown && gameOverFlag)
				return;
			
			// If gems are dropping/chaining, then just set game over flag and return
			// Check flag when dropping/chaining has finished
			gameOverFlag = true;
			if( !grid.canSelect)
				return;

            grid.DropAllGems();
            
            //visuals.setHintGem();

            grid.canSelect = false;
		}
		
		
    }
}