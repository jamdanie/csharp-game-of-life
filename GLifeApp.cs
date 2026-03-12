///////////////////////////////////////////////////////////////////////////////////////////////
// Change History:
// Date            Developer          Description
// 01/20/2026      James Daniels      Creation of file to represent application entry point
// 01/20/2026      James Daniels      Added user explanation of the Game of Life
// 01/22/2026      James Daniels      Added console resizing to prevent output wrapping
//                                    and improve Game of Life display in PowerShell
// 01/22/2026      James Daniels      Added input validation to restrict generation count
//                                    to numeric values between 3 and 5 only as the rubric requires
// References:
// Conway’s Game of Life (B3/S23), rules summarized from Wikipedia, accessed Jan 2026
///////////////////////////////////////////////////////////////////////////////////////////////

using System;

namespace GLife
{

//The Game of Life is a computational phenomenon invented by British mathematician
//John Conway circa 1970 to show possibilities of population simulations in a mathematically
//controlled environment.The original computations were not executed on machines, they were performed
//using marbles on a game board similar to Go.The results of processing were determined using pencil 
//and paper from one generation to the next.
    internal class GLifeApp
    {

        // Main entry point for the program
        // execution starts here
        static void Main(string[] args)
        {
            // Attempt to resize the console window and buffer so the Game of Life board
            // fits on the screen without wrapping lines or shifting borders.
            // terminals may block resizing, so this is wrapped in a try/catch
            // to prevent the program from crashing if resizing is not allowed.
            try
            {
                Console.SetBufferSize(130, 2000);
                Console.SetWindowSize(130, 50);
            }
            catch
            {
            // If the console cannot be resized, safely ignore and continue running as normal.
            }


            // present the user interface to the user 
            // explain the Game of Life program
            UserExplanation();

            // Validate user input to allow only numbers between 3 and 5.
            // Letters or invalid numbers will show an error and re-prompt the user.
            int numGenerations;

            while (true)
            {
                Console.WriteLine("How many generations do you want to simulate (3–5)?");

                if (int.TryParse(Console.ReadLine(), out numGenerations))
                {
                    if (numGenerations >= 3 && numGenerations <= 5)
                        break;
                }

                Console.WriteLine("Invalid input. Please enter a number between 3 and 5.");
            }

            GameOfLife game = new GameOfLife(numGenerations, 25, 110);
            game.RunSimulation();
        }

        static public void UserExplanation()
        {

            // use the complete assignment description as an explanation of the GOL
            Console.WriteLine(@"
******************************************************************************************************************
Game of Life
The Game of Life program assignment follows the material from Chapters 7 and 8 in your book. 
There are 2 major focus points for this lab exercise:

2 dimensional arrays - This is our first data structure that moves beyond a simple linear 
list of values.We will expand this and also look at some of the built in collections in the C# FCL.

--nested for loops(2 loops, an inner loop and an outer loop) - A natural fit for processing data 
from a 2 dimensional array, since the data structure is logically similar to a spreadsheet or 
matrix consisting of rows and columns of storage cells.

The Game of Life is a computational phenomenon invented by British mathematician 
John Conway circa 1970 to show possibilities of population simulations in a mathematically 
controlled environment.The original computations were not executed on machines, they were performed 
using marbles on a game board similar to Go. The results of processing were determined using pencil 
                and paper from one generation to the next.

Recently, according to the Wikipedia page on Game of Life, a researcher has carried out a 
GOL simulation to the 6,366,548,773,467,669,985,195,496,000th(6 octillionth) generation of a 
Turing machine, computed in less than 30 seconds on an Intel Core Duo 2 GHz CPU.

The standard Game of Life is symbolized as B3 / S23.A cell is Born if it has exactly three neighbors, 
Survives if it has two or three living neighbors, and dies otherwise.

The first number, or list of numbers, is what is required for a dead cell to be born.
The second set is the requirement for a live cell to survive to the next generation.

The deliverables for this assignment: the Life.zip - the complete zip file of your development tree.
Your zip file should include a plain text file showing your testing of the running program for 3 to 5 
generations of output using the standard input file start state that we developed together in class (shown below). 
The Powershell transcript feature is strongly recommended for doing the testing - or copy and paste to notepad 
if your powershell setup doesn't allow use of the transcript.

Make sure to read the rubric covering the requirements for full credit on the assignment.

The common start state we develop together in class:
----------------------------------------------------
xxxxxxxx xxxxx   xxx      xxxxxxx xxxxx
----------------------------------------------------
This is a single row comprised of the following sequence of cells:
8 LIVE - 1 DEAD - 5 LIVE - 3 DEAD - 3 LIVE - 6 DEAD - 7 LIVE - 1 DEAD - 5 LIVE");
        }

    }
}
