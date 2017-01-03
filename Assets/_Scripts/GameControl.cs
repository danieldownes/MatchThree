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

    public class GameControl : MonoBehaviour
    {
        public RectTransform gameCanvas;

        // Game Grid
        public Grid grid;
        
       
        //TODO: Score control
        //private int score;
        //private int score_multiplier;
       
        public bool gameOverFlag;
        

        public void Start()
        {
            grid.Init(this);

            Reset();
        }

        public void Reset()
        {
            grid.Reset();

            //score = 0;
			//score_multiplier = 100;
			
			gameOverFlag = false;
			
			//countdown.Reset();
			
			//visuals.Reset();
			
			grid.populate(true);
		}


		
		public int addScore(int newpoints)
		{
			//score += newpoints;
			//TODO: visuals.txtScore.text = score.ToString();
			return newpoints;
		}
		
		public void gameOver(bool fromCountdown = false)
		{
			// If called from FuseCountdown, but is already game over (ie from no more matches), then ignore
			if( fromCountdown && gameOverFlag)
				return;
			
			// If gems are dropping/chaining, then just set game over flag and return
			// Check flag when dropping/chaining has finished
			gameOverFlag = true;
			if( !grid.canSelect)
				return;

            int n = 0;
			for(int r = 0; r < Grid.ROWS; r++)
			{
				for(int c = 0; c < Grid.COLS; c++)
				{
					//Tweener.addCaller(this, { delay:n * 0.01, count:1, onComplete:_removeAndExplodeGem, onCompleteParams:[Gem(grid[c][r])] } );
					n++;
				}
			}
            
            //visuals.setHintGem();

            grid.canSelect = false;
		}
		
		
    }
}