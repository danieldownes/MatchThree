using System.Collections.Generic;

namespace game
{

    public class PatternMatching
    {
        public static bool checkSwap(Gem g1, Gem g2)
        {
            return ((g1.c + 1 == g2.c && g1.r == g2.r) ||
                    (g1.c - 1 == g2.c && g1.r == g2.r) ||
                    (g1.r + 1 == g2.r && g1.c == g2.c) ||
                    (g1.r - 1 == g2.r && g1.c == g2.c));
        }


        /** 
		*  Check patterns:
		*	   a     b     c		Key:
		*	  O--   -O-   O.oo		 O = Gem to check
		*	  .oo   o.o   ----    	 . = Gem will move to
		*	                     	 o = Matching gems
		*	                     	 - = Irrelevant gem
		*/
        public static int findPossibleMoves(Gem[,] grid, List<Gem> movableGems)
        {
            Gem g;
            int m = 0;  // Moves counter

            for (int c = 0; c < GameControl.COLS; c++)
            {
                for (int r = 0; r < GameControl.ROWS; r++)
                {
                    // Gem to check
                    g = grid[c, r];

                    if (PatternMatching.checkPattern(grid, g, 1, 1, 2, 1, true, true)         /* Pattern: a(x,y) */
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

        /** 
	    *  Check two gems that are offset from the provided gem are the same type
	    *   g: the gem to check
	    *   x1, y1, x2, y2: the offset position of a gem to check, relative to g
	    *   flipX: will also check the vertical reflection for the same pattern
	    *   flipY: will also check the horizontal reflection for the same pattern
	    *   flipX & flipY: will also check the vertical & horizontal reflection for the same pattern
	    */
        private static bool checkPattern(Gem[,] grid, Gem g, int x1, int y1, int x2, int y2, bool flipX, bool flipY)
        {
            bool canMove = PatternMatching.hasSameGem(grid, g.type, g.c + x1, g.r + y1) && PatternMatching.hasSameGem(grid, g.type, g.c + x2, g.r + y2);    // Base pattern
            if (flipX)
                canMove = canMove || PatternMatching.hasSameGem(grid, g.type, g.c - x1, g.r + y1) && PatternMatching.hasSameGem(grid, g.type, g.c - x2, g.r + y2);  // Flip X
            if (flipY)
                canMove = canMove || PatternMatching.hasSameGem(grid, g.type, g.c + x1, g.r - y1) && PatternMatching.hasSameGem(grid, g.type, g.c + x2, g.r - y2);  // Flip Y
            if (flipX && flipY)
                canMove = canMove || PatternMatching.hasSameGem(grid, g.type, g.c - x1, g.r - y1) && PatternMatching.hasSameGem(grid, g.type, g.c - x2, g.r - y2);  // Flip X & Y

            return canMove;
        }

        private static bool hasSameGem(Gem[,] grid, int type, int x_, int y_)
        {
            return (x_ >= 0 && x_ < GameControl.COLS && y_ >= 0 && y_ < GameControl.ROWS)           /* bounds check */
                     && (grid != null && grid[x_, y_].type == type);        /* gem type check */
        }
    }
}