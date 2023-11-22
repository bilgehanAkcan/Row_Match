# Row_Match

Row_Match is a level-based mobile game implemented in Unity and C#. The goal of the game is to create the maximum number of rows that have the same type of game items with limited moves.

When the game launches, a levels button welcomes the user. After tapping the button, a levels popup which lists all the available levels appears. Each level offers different number of moves and grid positioning.
Levels having a green play button can be played whereas levels having gray button, indicating that the level is locked, can't be played until the previous levels are achieved. Each game level is comprised of a 
rectangular grid, where each cell can have one item. The grid width and height vary from level to level. There are four types of items in the game and any adjacent item in the grid can be swapped by swiping.
After each swipe operation, the move count is decreased by one. The user should make all the cells in a row have the same type of game items in order to earn a score for each completed item. The highest score for 
each level is stored and displayed on the levels and game screen. If the user finishes the level with a new highest score, the user is celebrated with animations and the highest score for the corresponding level is 
updated.
