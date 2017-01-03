using UnityEngine;

using System;
using System.Collections.Generic;

namespace game
{
    public class Grid : MonoBehaviour
    {
        private GameControl game;

        public Gem[,] grid;
        public const int ROWS = 8;
        public const int COLS = 8;
        public const int WIDTH = 30;
        public const int HEIGHT = -30;

        public const float SECS_PER_ROW = 0.1f;

        // User interaction control
        private Gem gemSelected, gemSwap;
        public bool canSelect;

        // Logic
        public List<Gem> checkGems;
        private uint gemsDropping;

        public void Init(GameControl game_)
        {
            game = game_;

            // Create 2d array grid
            grid = new Gem[Grid.COLS, Grid.ROWS];
        }

        public void Reset()
        {
            gemSelected = null;
            gemSwap = null;

            checkGems = new List<Gem>();
            gemsDropping = 0;

            canSelect = false;
        }

        /**
		 * Populate the empty grid spaces
		 * Also handles falling and re-population
		 */
        public void populate(bool preventMatches = false, Gem[] newGems = null)
        {
            for (int c = 0; c < Grid.COLS; c++)
            {
                populateCol(c, preventMatches, c * 0.1f);
            }
        }
        


        /**
		 * Populate column
		 */
        private void populateCol(int c, bool preventMatches, float delayOffset = 0)
        {
            List<Gem> newGems = new List<Gem>(); //TODO: is there a better way to just ignore/swallow 'newGems' parameter?
            populateCol(c, preventMatches, newGems, delayOffset);
        }

        private void populateCol(int c, bool preventMatches, List<Gem> newGems, float delayOffset = 0)
        {
            int dropped = 0;
            //bool lastGem;

            for (int r = Grid.ROWS - 1; r >= 0; r--)
            {
                // Add/drop new gem
                if (grid[c, r] == null)
                {
                    int type = UnityEngine.Random.Range(1, Gem.TYPES);

                    // Add Gem
                    Gem gem = Instantiate(Resources.Load("Gem", typeof(Gem))) as Gem;
                    gem.transform.SetParent(game.gameCanvas);
                    gem.Init(c, r);
                    gem.type = type;
                    gem.GemSelected += _onGemSelected;
                    gem.GemLanded += _onGemLanded;

                    // Check to ensure new gem wont directly match grid
                    if (preventMatches)
                    {
                        List<Gem> matchesHorz = new List<Gem>();
                        List<Gem> matchesVert = new List<Gem>();
                        checkMatch(gem, matchesHorz, matchesVert);
                        // Note: As long as there are 2 or more gem types, 
                        // 		 then an infinite loop should not occur
                        while (matchesHorz.Count > 1 || matchesVert.Count > 1)
                        {
                            matchesHorz.Clear();
                            matchesVert.Clear();
                            gem.type++;
                            if (gem.type > Gem.TYPES)
                                gem.type = 1;

                            checkMatch(gem, matchesHorz, matchesVert);
                        }
                    }
                    gem.ColourType = gem.type;
                    dropped++;

                    grid[c, r] = gem;

                    //lastGem = (c == COLS-1 && r == 0);  // For WIP optimisation
                    gem.DoDrop(delayOffset + (dropped * SECS_PER_ROW) * 1.4f);


                    // Track new gems
                    if (newGems != null)
                        newGems.Add(gem);

                    gemsDropping++;
                }
            }
        }

        /**
		 * Checks if gem matches current surrounding gems
		 * Returns: number of matches found during this check
		 */
        private void checkMatch(Gem gem, List<Gem> matchesHorz, List<Gem> matchesVert)
        {
            checkDirection(matchesHorz, gem, -1, 0);
            checkDirection(matchesHorz, gem, 1, 0);
            checkDirection(matchesVert, gem, 0, -1);
            checkDirection(matchesVert, gem, 0, 1);
        }

        /**
		 * Checks if gem matches current surrounding gems
		 * Returns: number of matches found during this check
		 */
        private int checkDirection(List<Gem> matches, Gem gem, int c, int r)
        {
            // Check out of bounds
            if (gem.c + c < 0 || gem.c + c >= Grid.COLS || gem.r + r < 0 || gem.r + r >= Grid.ROWS)
                return 0;

            // Check for null grid index 
            if (grid[gem.c + c, gem.r + r] == null)
                return 0;

            // Now check for match
            Gem checkGem = grid[gem.c + c, gem.r + r];
            if (checkGem.type == gem.type)
            {
                matches.Add(checkGem);
                return checkDirection(matches, checkGem, c, r) + 1;
            }

            return 0;
        }

        /**
		 * Drop exiting gems into empty grid spaces
         * TODO: MVC VIEW + Model
		 */
        public void dropGems(List<Gem> affectedGems_)
        {
            Gem gem;

            for (int c = 0; c < Grid.COLS; c++)
            {
                int lR = Grid.ROWS - 1;     // Last seen row with gem
                int rowDrop = 0;        // Number of Gems droped in row
                for (int r = Grid.ROWS - 1; r >= 0; r--) //TODO: ROWS - 2 (don't need to check bottom row)
                {
                    // Row has gem?
                    gem = grid[c, r];
                    if (gem != null)
                    {
                        // Drop this gem? Only if no gem below ** & is not on bottom row ** -- change r loop
                        if (r != Grid.ROWS - 1 && grid[c, r + 1] == null)
                        {
                            gem.DoDropDown(rowDrop * SECS_PER_ROW * 0.6f, lR);

                            // Logical move
                            grid[c, lR] = grid[c, r];
                            grid[c, r] = null;
                            gem.r = lR;

                            // Save moved gems for checkMatch()
                            if (affectedGems_ != null)
                                affectedGems_.Add(gem);

                            rowDrop++;
                        }
                        lR--;
                    }
                }

                // Repopulate column
                populateCol(c, false, affectedGems_, rowDrop * SECS_PER_ROW * 0.8f);
            }
        }


        private void _onGemLanded(object sender, EventArgs e)
        {
            playCheck();
        }

        /**
		 * Handle gem swapping using two clicks/touches
		 */
        private void _onGemSelected(object sender, GemSelectedEventArgs e)
        {
            //print("_onGemSelected");
            if (!canSelect)
                return;

            if (gemSelected == null)                // Select
            {
                gemSelected = e.gem;
                gemSelected.transform.localScale = Vector3.one * 1.2f;
                // Set selection to top
                this.transform.SetAsLastSibling();
            }
            else if (gemSelected == e.gem)         // Deselect
            {
                gemSelected.transform.localScale = Vector3.one;
                gemSelected = null;
            }
            else                                    // Swap
            {
                if (PatternMatching.checkSwap(gemSelected, e.gem))
                {
                    gemSwap = e.gem;
                    _swapGems(gemSelected, gemSwap, _completeSwapAni);

                    // Disable selection
                    canSelect = false;
                }
                else
                {
                    // Select this gem instead
                    gemSelected.transform.localScale = Vector3.one * 1f;
                    gemSelected = e.gem;
                    gemSelected.transform.localScale = Vector3.one * 1.2f;
                }
            }
        }


        /**
		 * Handle gem swapping using swipe/drag
         */
        private void _onGemDrag(object sender, GemSelectedEventArgs e)
        {
            if (!canSelect || gemSelected == null)
                return;

            // Within bounds?
            if (e.gem.c >= 0 && e.gem.c < Grid.COLS && e.gem.r >= 0 && e.gem.r < Grid.ROWS)
            {
                gemSwap = e.gem;

                // Gem that is being dragged over is by the side of selected gem?
                if (PatternMatching.checkSwap(gemSelected, gemSwap))
                {
                    _swapGems(gemSelected, gemSwap, _completeSwapAni);

                    // Disable selection
                    canSelect = false;
                }
            }
        }

        private void playMove()
        {
            checkGems.Clear();

            // Drop new gems (and re-populate grid)
            dropGems(checkGems);
        }

        private void playCheck()
        {
            gemsDropping--;

            if (gemsDropping > 0)
                return;

            //gemsDropping = 0;

            // Check and remove any chain matches
            //  Pause slightly first, to indicate to player
            Invoke("checkChaining", 0.2f);
        }


        public void checkChaining()
        {
            // Check affected gems on grid for new matches
            int matchesN = 0;

            foreach (Gem gem in checkGems)
            {
                int n = checkAndRemove(gem);
                matchesN += n;

                // Display notification for each gem that a match was found
                if (n > 0)
                {
                    //TOOD: ScoreNotification note = new ScoreNotification(this, gem.x, gem.y, matchesN * score_multiplier);
                }
            }

            if (matchesN > 0 && checkGems != null && checkGems.Count > 0)
            {
                // Increase score 
                //int inc = addScore(matchesN * score_multiplier);

                // Increase chain multiplier
                //score_multiplier += 100;

                // Chain matches
                playMove();
            }
            else
            {
                // No (more) chain matches possible

                canSelect = true;

                // Check game over
                if( game.gameOverFlag)
                {
                    game.gameOver();
                    return;
                }

                // Find possible moves
                List<Gem> possibleMoves = new List<Gem>();
                PatternMatching.findPossibleMoves(grid, possibleMoves);

                if (possibleMoves.Count > 0)
                {
                    //TODO: visuals.setHintGem(hints[Math.floor(Math.random() * hints.length)]);
                }
                else
                {
                    // No moves remaining, game over
                    game.gameOver();

                    // (Another game-design option would be to shuffle the gems)
                }

            }
        }


        private void deselectGem()
        {
            gemSelected.transform.localScale = Vector3.one;
            gemSelected = null;
            gemSwap = null;
        }

        // Control
        private delegate void OnCompleteSwap();
        private void _swapGems(Gem g1, Gem g2, Action onCompleteSwap)
        {
            //TODO: Visual swap
            LeanTween.moveLocal(g1.gameObject, g2.transform.localPosition, Gem.SWAP_ANI_TIME).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.moveLocal(g2.gameObject, g1.transform.localPosition, Gem.SWAP_ANI_TIME).setEase(LeanTweenType.easeInOutQuad)
                                .setOnComplete(onCompleteSwap);

            // Model swap
            int swap_c = g2.c;
            int swap_r = g2.r;
            g2.c = g1.c;
            g2.r = g1.r;
            grid[g1.c, g1.r] = g2;
            grid[swap_c, swap_r] = g1;
            g1.c = swap_c;
            g1.r = swap_r;
        }

        private void _completeSwapAni()
        {
            // Check matches
            int matchesN = 0;
            matchesN += checkAndRemove(gemSelected);
            matchesN += checkAndRemove(gemSwap);
            
            // Successful swap?
            if (matchesN > 0)
            {
                //addScore(matchesN * score_multiplier);
                //TODO: ScoreNotification note = new ScoreNotification(this, _gemSelected.x, _gemSelected.y, matchesN * score_multiplier);

                // Reset multiplier
                //score_multiplier = 100;


                deselectGem();


                playMove();
            }
            else
            {
                // Not good, swap back!
                _swapGems(gemSwap, gemSelected, _completeSwapBackAni);
            }
        }

        private void _completeSwapBackAni()
        {
            canSelect = true;
            deselectGem();
        }



        private int checkAndRemove(Gem gemCheck)
        {
            List<Gem> matchesH = new List<Gem>();
            List<Gem> matchesV = new List<Gem>();
            List<Gem> matches = new List<Gem>();

            // Check for matches in both vertical and horizontal directions
            checkMatch(gemCheck, matchesH, matchesV);

            // Horizontal matches?
            if (matchesH.Count > 1)
                matches.AddRange(matchesH);

            // Vertical matches?
            if (matchesV.Count > 1)
                matches.AddRange(matchesV);

            // Add gemCheck if there are matches
            if (matches.Count > 1)
                matches.Add(gemCheck);

            // Remove matches
            foreach (Gem gem in matches)
            {
                //TODO: visuals.explodeGem(gem);
                //TODO: Object pooling
                _removeAndExplodeGem(gem);
            }

            return matches.Count;
        }

        private void _removeAndExplodeGem(Gem gem)
        {
            //visuals.explodeGem(gem);
            grid[gem.c, gem.r] = null;
            Destroy(gem.gameObject);
        }
    }
}
