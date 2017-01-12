using UnityEngine;

using System;

namespace game
{
    /// <summary>
    /// Main game logic control flow
    /// </summary>
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

        /// <summary>
        /// Reset all objects ready for a new game
        /// </summary>
        internal void Reset()
        {
            grid.Reset();

            //score = 0;
			//score_multiplier = 100;
			
			gameOverFlag = false;
			
			//countdown.Reset();
			
			//visuals.Reset();
			
			grid.Populate(true);
		}


        /// <summary>
        /// Controller: Update score model and view 
        /// </summary>
        /// <param name="newpoints"></param>
        /// <returns>New score total</returns>
        internal int AddScore(int newpoints)
		{
            throw new NotImplementedException();
            //score += newpoints;
            //TODO: visuals.txtScore.text = score.ToString();
            return newpoints;
		}

        /// <summary>
        /// Called to trigger gameover state
        /// </summary>
        /// <param name="fromCountdown">Gameover from countdown timeout condition</param>
        internal void GameOver(bool fromCountdown = false)
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