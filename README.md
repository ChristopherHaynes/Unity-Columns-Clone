# Unity-Columns-Clone
A clone of an old Atari "match 3" game for Android. Created from scratch as best as possible from memory, features high scores, save games, animations and sounds. Made using the Unity game engine with all scripts written in C#. All assests are either open source or self created.

## Gameplay
The goal of the game is to drop sets of 3 blocks into the grid in order to make identical colour matches of 3, 4 or 5 blocks in either vertical, horizontal or diagonal directions. If a match is made then the blocks are destroyed and all other blocks will drop to fill any empty spaces below them. Before being dropped, a set of 3 blocks can be rotated in 90 degree increments, but the order of the blocks can not be changed. The set can also be moved left and right to any viable position above the grid. On the right of the screen the next set of 3 blocks is shown in a preview window. Initially there are 6 different coloured blocks which may appear in any given set. As the game progress, up to 3 more colours can be added once the player passes scores of 10,000, 30,0000 and 50,000. The game ends when any block passes the red line at the top of the grid after being dropped. Once a game ends the highscore is recorded, as well as the date and the number of 3, 4, 5, horizontal, vertical and diagonal matches that occured (these values can be seen on the right of the screen in-game). The game can be saved at any time after a move has been made by pressing the "Save & Quit" button in the top left of the screen.

![In-game Screenshot](https://github.com/ChristopherHaynes/Unity-Columns-Clone/blob/master/res/columns-screenshot.png?raw=true)

## Controls
__Blue Button Arrows__ - Rotate the set of blocks 90 degrees clockwise, or anti-clockwise.

__Pink Button Arrows__ - Move the set of blocks one space left, or right.

__Green Button Arrow__ - Drop the set of blocks in the currently selected position and rotation.

__Save & Quit Button__ - Save the current state of the game and return to the main menu.

## License
Copyright (c) 2015-2021 Chris Haynes and others

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
