using UnityEngine;

using System;

namespace game
{
    /// <summary>
    /// Main game logic control flow
    /// </summary>
    internal class GameControl : MonoBehaviour
    {
        public RectTransform gameCanvas;

        // Game Grid
        internal Grid grid;

        internal Score score;
        
        internal bool gameOverFlag;


        public void Start()
        {
            grid = this.GetComponent<Grid>();
            grid.Init(this);

            score = this.GetComponent<Score>();
            score.Init();

            Reset();
        }

        /// <summary>
        /// Reset all objects ready for a new game
        /// </summary>
        internal void Reset()
        {
            gameOverFlag = false;
            grid.Reset();
            grid.Populate(true);
            score.Init();
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
            
            //TODO: setHintGem();

            grid.canSelect = false;
		}
		
		
    }
}