# Boulder Dash WPF game created in C#.

# MainWindow class:
- MainWindow(): Constructor method for the MainWindow class. Initializes the main window of the game, creates the game grid, and loads the level.
- Window_KeyDown(object sender, KeyEventArgs e): Event handler for key presses.
- StartGame(): Asynchronous method to start the game. Initializes the game state, hides overlay, and starts game loop.
- CreateGameGrid(): Sets up the game grid by adding rows and columns to the grid.
- LoadLevel(): Loads the game map from a txt file and initializes the game map.
- MapCharacterToCellType(char cellType): Using switch case convert characters to type and return it to LoadLevel method.
- LoadMapElement(CellType cellType, int row, int col): Loads map elements into the game grid based on cell type and position.
- UpdateMap(): Updates the game grid based on the current state of the game map. Get cells types, call CreateImageElement, which return image and pastes it into the appropriate cell.
- ReplaceSpawnWithPlayer(): Replaces spawn cell with player cell on the map.
- CreateImageElement(string imageName, int row, int col): Creates an image element for the game grid.
- UpdateText(string text): Updates the overlay text displayed on the game window.
- CheckWinCondition():  Checks if all gems have been collected and if the player is on the exit cell.

# Player class:
- Player(int startX, int startY): Constructor method for the Player class. Initializes the player's position.
- Move(Game.MovementDirection direction): Moves the player based on the specified direction.
- IsValidMove(int x, int y): Checks if a move to a specific position is valid.
- CanMoveTo(int x, int y): Checks if the player can move to a specific position.

# Boulders class:
- Boulders(MainWindow mainWindow): Constructor method for the Boulders class. Initializes the boulder objects.
- FallCheck(int x, int y): Checks if a boulder can fall and handles its movement.
- SideFallCheck(int x, int y): Checks if a boulder can fall sideways and handles its movement.
- DeathLogic(int x, int y): Handles the game logic when a player is crushed by a boulder.

# Gems class:
- Gems(MainWindow mainWindow): Constructor method for the Gems class. Initializes the gem objects.
- GemFallCheck(int x, int y): Checks if a gem can fall and handles its movement.
- SideFallCheck(int x, int y): Checks if a gem can fall sideways and handles its movement.
- PlayerCollision(int x, int y): Handles the game logic when a player collects a gem.

# Game class:
Game(MainWindow mainWindow): Constructor method for the Game class. Initializes the game objects and lists.
InitializeBouldersAndGems(): Initializes lists to track boulder and gem positions.
Run(System.Windows.Input.Key key): Runs the game loop based on user input.
StartContinuousFallChecks(): Starts continuous fall checks for boulders and gems.
FallCheckForAllBoulders(): Checks for falling boulders and updates their positions.
FallCheckForAllGems(): Checks for falling gems and updates their positions.
