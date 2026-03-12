# GLife — Conway’s Game of Life (C#)

A C# console implementation of Conway’s Game of Life using the classic B3/S23 rules.
Created as a school project to practice grid-based simulation and clean program design.

## Features
- 2D grid implemented with `char[,]`
- LIVE (`@`) and DEAD (` `) cell representation
- Double-buffered boards for generation updates
- Explicit edge and corner handling
- Console output with left/right borders

## How it works
Each generation:
1. Displays the current board
2. Counts live neighbors
3. Applies B3/S23 rules
4. Swaps buffers and repeats

## How to run
Open the solution in Visual Studio and press **F5**.

## References
- Conway’s Game of Life (B3/S23), summarized from Wikipedia (accessed Jan 2026)
