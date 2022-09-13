# SeriousGames.MultiplayerSample

This repository corresponds to the Serious Games praktikum project regarding the multiplayer feature to be added on Geo & Games main project. 

Currently the project contains the functionality of multiple players in different devices to connect each other and interact in a playground, where physics are simulated and replicated in real time. The players can enter different sessions using a Passcode provided by hosts that create those same sessions as well as leaving at anytime, pressing the `escape` key and indicating to leave. Once connected to the session they can move around in the playground and pick up objects painted red by pressing the `e`. They can also reproduce particle effects visible by all players in the session by pressing the `spacebar`.

## Set Up & Play

1. Install Github, crucial packages will not be downloaded properly if Github is not installed on the computer.
2. Download the repository and open it on Unity, all the required packages will be imported automatically.
3. Set current scene to 'InitBootstrap'
4. Press Play


## Testing with multiple players

In order to test the performance of the multiplayer capabilities there are two main options available:

1. Creating a project build `File > BuildSettings` and creating a project and opening multiple game windows.
2. Using ParrelSync, which creates a clone of the project were it can subsequently be opened.