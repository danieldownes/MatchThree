using System.Collections.Generic;

namespace game
{
    /// <summary>
    /// 
    /// </summary>
    internal class PatternMatching
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="g1"></param>
        /// <param name="g2"></param>
        /// <returns></returns>
        internal static bool checkSwap(Gem g1, Gem g2)
        {
            return ((g1.c + 1 == g2.c && g1.r == g2.r) ||
                    (g1.c - 1 == g2.c && g1.r == g2.r) ||
                    (g1.r + 1 == g2.r && g1.c == g2.c) ||
                    (g1.r - 1 == g2.r && g1.c == g2.c));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="movableGems"></param>
        /// <returns></returns>
        internal static int findPossibleMoves(Gem[,] grid, List<Gem> movableGems)
        {
            /** 
		    *  Check patterns:
		    *	   a     b     c		Key:
		    *	  O--   -O-   O.oo		 O = Gem to check
		    *	  .oo   o.o   ----    	 . = Gem will move to
		    *	                     	 o = Matching gems
		    *	                     	 - = Irrelevant gem
		    */

            Gem g;
            int m = 0;  // Moves counter

            for( int c = 0; c < Grid.COLS; c++)
            {
                for( int r = 0; r < Grid.ROWS; r++)
                {
                    // Gem to check
                    g = grid[c, r];

                    if( PatternMatching.checkPattern(grid, g, 1, 1, 2, 1, true, true)         /* Pattern: a(x,y) */
                     || PatternMatching.checkPattern(grid, g, -1, 1, 1, 1, true, false)       /* Pattern: b(x,y) */
                     || PatternMatching.checkPattern(grid, g, 2, 0, 3, 0, true, false)        /* Pattern: c(x,y) */
                     || PatternMatching.checkPattern(grid, g, 1, 1, 1, 2, true, true)         /* Pattern: a(y,x) */
                     || PatternMatching.checkPattern(grid, g, 1, -1, 1, 1, false, true)       /* Pattern: b(y,x) */
                     || PatternMatching.checkPattern(grid, g, 0, 2, 0, 3, true, false))       /* Pattern: c(y,x) */
                    {
                        movableGems.Add(g);
                        m++;
                    }
                }
            }
            return m;
        }


        /// <summary>
        /// Check two gems that are offset from the provided gem are the same type 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="g">Gem to check aginst the grid offsets</param>
        /// <param name="x1">Offset position of a gem to check, relative to g</param>
        /// <param name="y1">Offset position of a gem to check, relative to g</param>
        /// <param name="x2">Offset position of a gem to check, relative to g</param>
        /// <param name="y2">Offset position of a gem to check, relative to g</param>
        /// <param name="flipX">Also check the vertical reflection for the same pattern</param>
        /// <param name="flipY">Also check the horizontal reflection for the same pattern</param>
        /// <returns></returns>
        private static bool checkPattern(Gem[,] grid, Gem g, int x1, int y1, int x2, int y2, bool flipX, bool flipY)
        {
            bool canMove = PatternMatching.hasSameGem(grid, g.type, g.c + x1, g.r + y1) && PatternMatching.hasSameGem(grid, g.type, g.c + x2, g.r + y2);    // Base pattern
            if( flipX)
                canMove = canMove || PatternMatching.hasSameGem(grid, g.type, g.c - x1, g.r + y1) && PatternMatching.hasSameGem(grid, g.type, g.c - x2, g.r + y2);  // Flip X
            if( flipY)
                canMove = canMove || PatternMatching.hasSameGem(grid, g.type, g.c + x1, g.r - y1) && PatternMatching.hasSameGem(grid, g.type, g.c + x2, g.r - y2);  // Flip Y
            if( flipX && flipY)
                canMove = canMove || PatternMatching.hasSameGem(grid, g.type, g.c - x1, g.r - y1) && PatternMatching.hasSameGem(grid, g.type, g.c - x2, g.r - y2);  // Flip X & Y

            return canMove;
        }

        /// <summary>
        /// Check if gem in grid[x, y] == type, with bounds checking
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="type"></param>
        /// <param name="x_"></param>
        /// <param name="y_"></param>
        /// <returns></returns>
        private static bool hasSameGem(Gem[,] grid, int type, int x_, int y_)
        {
            return (x_ >= 0 && x_ < Grid.COLS && y_ >= 0 && y_ < Grid.ROWS)         /* bounds check   */
                     && (grid != null && grid[x_, y_].type == type);                /* gem type check */
        }
    }
}