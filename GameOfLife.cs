//////////////////////////////////////////////////////////////////////////////////////////////
// Change History:
// Date            Developer          Description
// 01/20/2026      James Daniels      Creation of file to represent class GameOfLife
// 01/22/2026      James Daniels      Added methods to implement Game of Life rules,
//                                    neighbor counting, board swapping, and updated
//                                    board display to include left and right borders
// 01/22/2026      James Daniels      Clarified start-state documentation to match
//                                    instructor-defined pattern and indexing
// 01/24/2026      James Daniels      Adjusted generation output spacing for readability in transcript
// References:
// Conway’s Game of Life (B3/S23), rules summarized from Wikipedia, accessed Jan 2026
/////////////////////////////////////////////////////////////////////////////////////////////

using System;

namespace GLife
{
    internal class GameOfLife
    {
        // permanent (maybe constant)
        private const char DEAD = ' ';  // White_space for DEAD cell
        private const char LIVE = '@';  // marker for LIVE cell

        // readonly vars in C# cannot be unassigned until the end of the ctor
        // once the ctor ends, they can't be reassigned 
        private readonly int numGenerations;
        private readonly int Rowsize;
        private readonly int Colsize;


        // class scope instance variables for representing the game boards
        private char[,] currBoard;
        private char[,] buffBoard;

        // ctor to make the game object
        // it needs to know the number of generations to emulate
        // we also pass in the dimensions of the game board 
        public GameOfLife(int generations, int rows, int cols)
        {
            numGenerations = generations;
            Rowsize = rows;
            Colsize = cols;

            // make the 2 game boards storage for all the cells
            currBoard = new char[Rowsize, Colsize];
            buffBoard = new char[Rowsize, Colsize];

            // prepare the boards - zero out all the cells to initialize to DEAD
            PrepareGameBoard();

            // right now we only have 1 start state, we will overwrite the
            // designated cell locations with LIVE cells
            // origin of the [r,c] loc is roughly halfway down the rows
            // and 1/4th across the columns
            OverlayStartStates(Rowsize / 2, Colsize / 4);

        }

        // Runs the Game of Life simulation for the user-selected number of generations.
        // Each generation prints the current board, computes the next board using B3/S23 rules,
        // then swaps buffers to continue.
        internal void RunSimulation()
        {
            for (int i = 0; i < numGenerations; i++)
            {
                // display the current game board
                Console.WriteLine($"GENERATION: {i + 1}");
                DisplayGameBoard();
                Console.WriteLine();

                // apply the rules of the GOL - putting each result in the buffer board
                ApplyLifeRules();

                // swap the two boards to get ready to repeat
                SwapGameBoards();

            }
        }

        private void ApplyLifeRules()
        {
            // go through the entire gameboard cell by cell
            for (int r = 0; r < Rowsize; r++)
            {
                for (int c = 0; c < Colsize; c++)
                {
                    // 1 - count the number of LIVE neighbors of this cell [r,c]
                    int neighborCount = CountLiveNeighborCells(r, c);

                    // 2 - determine LIVE or DEAD
                    buffBoard[r, c] = DetermineLiveOrDead(neighborCount, r, c);
                }
            }
        }

        // actually apply the rules of the GOL b3/S23 game to the neighbor count for this
        // cell in the location [r,c]
        // return- result should be DEAD or LIVE
        private char DetermineLiveOrDead(int neighborCount, int r, int c)
        {
            if (neighborCount == 2)
            {
                // DEAD ==> DEAD, LIVE ==> LIVE
                return currBoard[r, c];
            }
            else if (neighborCount == 3)
            {
                // Any live cell with two or three live neighbours lives on to the next generation.
                // Any dead cell with exactly 3 neighbours becomes a live cell, as if by reproduction.
                // this covers current LIVE or DEAD cells 
                // one is propogation the other is spontaneous birth 
                // Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
                // DEAD ==>LIVE, LIVE ==> LIVE
                return LIVE;
            }
            else
            {
                // Any live cell with fewer than two live neighbours dies, as if by underpopulation.
                // Any cell with more than three live neighbours dies, as if by overpopulation.
                // so in summary -  with total neighbor is 0|1|4|5|6|7|8
                return DEAD;
            }
        }

        private int CountLiveNeighborCells(int r, int c)
        {
            int neighbors = 0;

            if (r == 0 && c == 0)
            {
                // TOP LEFT CORNER
                //if (currBoard[r - 1, c - 1] == LIVE) neighbors++;
                //if (currBoard[r - 1, c] == LIVE) neighbors++;
                //if (currBoard[r - 1, c + 1] == LIVE) neighbors++;
                //if (currBoard[r, c - 1] == LIVE) neighbors++;
                if (currBoard[r, c + 1] == LIVE) neighbors++;
                //if (currBoard[r + 1, c - 1] == LIVE) neighbors++;
                if (currBoard[r + 1, c] == LIVE) neighbors++;
                if (currBoard[r + 1, c + 1] == LIVE) neighbors++;
            }
            else if (r == 0 && c == Colsize - 1)
            {
                // TOP RIGHT CORNER
                //if (currBoard[r - 1, c - 1] == LIVE) neighbors++;
                //if (currBoard[r - 1, c] == LIVE) neighbors++;
                //if (currBoard[r - 1, c + 1] == LIVE) neighbors++;
                if (currBoard[r, c - 1] == LIVE) neighbors++;
                //if (currBoard[r, c + 1] == LIVE) neighbors++;
                if (currBoard[r + 1, c - 1] == LIVE) neighbors++;
                if (currBoard[r + 1, c] == LIVE) neighbors++;
                //if (currBoard[r + 1, c + 1] == LIVE) neighbors++;
            }
            else if (r == Rowsize - 1 && c == Colsize - 1)
            {
                // BOTTOM RIGHT CORNER
                if (currBoard[r - 1, c - 1] == LIVE) neighbors++;
                if (currBoard[r - 1, c] == LIVE) neighbors++;
                //if (currBoard[r-1, c+1] == LIVE) neighbors++;
                if (currBoard[r, c - 1] == LIVE) neighbors++;
                //if (currBoard[r, c+1] == LIVE) neighbors++;
                //if (currBoard[r+1, c-1] == LIVE) neighbors++;
                //if (currBoard[r+1, c] == LIVE) neighbors++;
                //if (currBoard[r+1, c+1] == LIVE) neighbors++;
            }
            else if (r == Rowsize - 1 && c == 0)
            {
                // BOTTOM LEFT CORNER
                //if (currBoard[r - 1, c - 1] == LIVE) neighbors++;
                if (currBoard[r - 1, c] == LIVE) neighbors++;
                if (currBoard[r - 1, c + 1] == LIVE) neighbors++;
                //if (currBoard[r, c - 1] == LIVE) neighbors++;
                if (currBoard[r, c + 1] == LIVE) neighbors++;
                //if (currBoard[r + 1, c - 1] == LIVE) neighbors++;
                //if (currBoard[r + 1, c] == LIVE) neighbors++;
                //if (currBoard[r + 1, c + 1] == LIVE) neighbors++;
            }
            else if (r == 0)
            {
                // TOP EDGE
                //if (currBoard[r - 1, c - 1] == LIVE) neighbors++;
                //if (currBoard[r - 1, c] == LIVE) neighbors++;
                //if (currBoard[r - 1, c + 1] == LIVE) neighbors++;
                if (currBoard[r, c - 1] == LIVE) neighbors++;
                if (currBoard[r, c + 1] == LIVE) neighbors++;
                if (currBoard[r + 1, c - 1] == LIVE) neighbors++;
                if (currBoard[r + 1, c] == LIVE) neighbors++;
                if (currBoard[r + 1, c + 1] == LIVE) neighbors++;
            }
            else if (c == Colsize - 1)
            {
                // RIGHT EDGE
                if (currBoard[r - 1, c - 1] == LIVE) neighbors++;
                if (currBoard[r - 1, c] == LIVE) neighbors++;
                //if (currBoard[r - 1, c + 1] == LIVE) neighbors++;
                if (currBoard[r, c - 1] == LIVE) neighbors++;
                //if (currBoard[r, c + 1] == LIVE) neighbors++;
                if (currBoard[r + 1, c - 1] == LIVE) neighbors++;
                if (currBoard[r + 1, c] == LIVE) neighbors++;
                //if (currBoard[r + 1, c + 1] == LIVE) neighbors++;
            }
            else if (r == Rowsize - 1)
            {
                // BOTTOM EDGE NOT A CORNER
                if (currBoard[r - 1, c - 1] == LIVE) neighbors++;
                if (currBoard[r - 1, c] == LIVE) neighbors++;
                if (currBoard[r - 1, c + 1] == LIVE) neighbors++;
                if (currBoard[r, c - 1] == LIVE) neighbors++;
                if (currBoard[r, c + 1] == LIVE) neighbors++;
                //if (currBoard[r + 1, c - 1] == LIVE) neighbors++;
                //if (currBoard[r + 1, c] == LIVE) neighbors++;
                //if (currBoard[r + 1, c + 1] == LIVE) neighbors++;
            }
            else if (c == 0)
            {
                // LEFT EDGE NOT A CORNER
                //if (currBoard[r - 1, c - 1] == LIVE) neighbors++;
                if (currBoard[r - 1, c] == LIVE) neighbors++;
                if (currBoard[r - 1, c + 1] == LIVE) neighbors++;
                //if (currBoard[r, c - 1] == LIVE) neighbors++;
                if (currBoard[r, c + 1] == LIVE) neighbors++;
                //if (currBoard[r + 1, c - 1] == LIVE) neighbors++;
                if (currBoard[r + 1, c] == LIVE) neighbors++;
                if (currBoard[r + 1, c + 1] == LIVE) neighbors++;
            }
            else
            {
                // Nominal cell somewhere in the middle of the population field
                // NOT a cornor, and NOT an edge
                if (currBoard[r - 1, c - 1] == LIVE) neighbors++;
                if (currBoard[r - 1, c] == LIVE) neighbors++;
                if (currBoard[r - 1, c + 1] == LIVE) neighbors++;
                if (currBoard[r, c - 1] == LIVE) neighbors++;
                if (currBoard[r, c + 1] == LIVE) neighbors++;
                if (currBoard[r + 1, c - 1] == LIVE) neighbors++;
                if (currBoard[r + 1, c] == LIVE) neighbors++;
                if (currBoard[r + 1, c + 1] == LIVE) neighbors++;
            }

            return neighbors;
        }

        // STANDARD WAY TO SWAP 2 OBJECT A AND B
        private void SwapGameBoards()
        {
            // typical swap we need a 3rd temp object as a placeholder
            char[,] tmp = currBoard; // make a tmp object of the same type that A and B are
                                     // make tmp point to where orignial obj A is 
            currBoard = buffBoard;   // change ref_A to point to orignial obj B
            buffBoard = tmp;         // chang e ref_B to point to original obj A using tmp
                                     // which is now the only way to access orignial obj A
        }

        // standard way to print out the 2 dimensional game board object 
        private void DisplayGameBoard()
        {
            for (int r = 0; r < Rowsize; r++)
            {
                Console.Write("|"); // left border

                for (int c = 0; c < Colsize; c++)
                {
                    Console.Write(currBoard[r, c]);
                }

                Console.WriteLine("|"); // right border + NEWLINE
            }
        }
        // zero out all the cells to start with all DEAD
        private void PrepareGameBoard()
        {
            // std algo to process a 2 dim array
            for (int r = 0; r < Rowsize; r++)
            {
                for (int c = 0; c < Colsize; c++)
                {
                    // fill every cell with DEAD
                    currBoard[r, c] = DEAD;
                    buffBoard[r, c] = DEAD;
                    // usually also reset buffer
                }
            }
        }
        // the passed in original [r,c] locatioon positions the start 
        // pattern in chosen location on the board
        public void OverlayStartStates(int r, int c)
        {
            // set cells on currBoard using r and c
            // The common start state we develop together in class:
            // ------------------------------------------------------------------------------
            // xxxxxxxx xxxxx   xxx      xxxxxxx xxxxx
            // ------------------------------------------------------------------------------
            // 8 LIVE - 1 DEAD - 5 LIVE - 3 DEAD - 3 LIVE - 6 DEAD - 7 LIVE - 1 DEAD - 5 LIVE         

            // 8 LIVE  
            currBoard[r, c + 0] = LIVE;
            currBoard[r, c + 1] = LIVE;
            currBoard[r, c + 2] = LIVE;
            currBoard[r, c + 3] = LIVE;
            currBoard[r, c + 4] = LIVE;
            currBoard[r, c + 5] = LIVE;
            currBoard[r, c + 6] = LIVE;
            currBoard[r, c + 7] = LIVE;

            // 1 DEAD     

            // 5 LIVE  
            currBoard[r, c + 9] = LIVE;
            currBoard[r, c + 10] = LIVE;
            currBoard[r, c + 11] = LIVE;
            currBoard[r, c + 12] = LIVE;
            currBoard[r, c + 13] = LIVE;

            // 3 DEAD    

            // 3 LIVE  
            currBoard[r, c + 17] = LIVE;
            currBoard[r, c + 18] = LIVE;
            currBoard[r, c + 19] = LIVE;

            // 6 DEAD    

            // 7 LIVE  
            currBoard[r, c + 26] = LIVE;
            currBoard[r, c + 27] = LIVE;
            currBoard[r, c + 28] = LIVE;
            currBoard[r, c + 29] = LIVE;
            currBoard[r, c + 30] = LIVE;
            currBoard[r, c + 31] = LIVE;
            currBoard[r, c + 32] = LIVE;

            // 1 DEAD   

            // 5 LIVE  
            currBoard[r, c + 34] = LIVE;
            currBoard[r, c + 35] = LIVE;
            currBoard[r, c + 36] = LIVE;
            currBoard[r, c + 37] = LIVE;
            currBoard[r, c + 38] = LIVE;
        }
    }
}
