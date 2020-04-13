# Server
This is the server of my game.

## How to run it
Due to lack of built-in priority queue in c#,
the project uses a priority queue from https://github.com/BlueRaja/High-Speed-Priority-Queue-for-C-Sharp,
so you need to download it to run the program.

## Expected changes
The armor class and the effect class are not in use yet.
Some weapons will have an effect that they will cause to their victims.
Some armors will have an effect that they will cause to their owner.
For instance, some effect might be burning, freezing, bleeding, regeneration, and so on.
Effect might be limited by time or not.
In addition, there should be more game modes, such as teams, flags, zombie apocalypse and more.

## Project structure
The project is divided by namespaces.

Contact - contains everything that is relevant to the contact with the clients.
Universe - contains what is important for the running of the game.
Entity - contains what is important for the agents in the game.
Arsenal - contains weapons, bullets, armor, and bag.
Tangible - contains shape, model, text, vector, and so on.
Data - contains what is relevant for reading the data from files.

There is also a constants file that contains the constants for the project.

### Contact
Contains everything that is relevant to the contact with the clients.

Bytes - each instance of bytes is a list of bytes.
This class is responsible for the encoding of variables, such as int, string, double and more.

ClientData - contains data about a connected client.

WSListener - because browsers not allow simple connection in tcp,
we need to communicate with with the client through this class,
that implement Web Socket protocol.

Server - responsible for running the game and handling the clients.

### Universe
Contains what is important for the running of the game.

Block - the map is made of it.

Map - made of blocks. Can check for collisions with the blocks, and print the map on a screen instance.

Screen - an instance of screen is a frame to send to the client.

Teams - contains the teams for the game, and calls live for agents, and interact between enemies.

World - abstract class for specific game mode.

Modes - contains game modes (a mode might be, for example, flags).

### Entity
Contains what is important for the agents in the game.

Agent - abstract class. Player and Zombie are types of agent.

AgentAction - contains which keys the agent pressed on, where is he look to, and so on.
Note that not only players have it, when a zombie wants to move he should do it through this class.

Navigator - helps a zombie find a way to his target, or escape from it.

Player, Zombie - just what they are.

### Arsenal
Contains weapons, bullets, armor, and bag.

Bag contains some weapons, and a launcher. While a weapon always shoot the same bullets, and has delay,
the launcher can shoot everything and every time. The agent doesn't command when the launcher shoot, just the weapons;
the world (i.e. the game system) controls when the launcher shoots and what.

Weapon contains bullet data and a delay. It always shoots the same bullet.
Weapon inheritance from launcher.

Launcher has a list of bullets that are being shot. 

Armor and effect are not in use yet.

IBullet is the interface for every type of bullet.

IBulletData is the interface for every type of bullet data.

Bullets namespace contains types of bullets.

### Tangible
Contains shape, model, text, vector, and so on.

Collision check - check for collision between two rectangles, by their points, and a vector movement for the first rectangle.
IMPORTANT: Collision returns a double, not a bool. That is because it actually returns when there will be a collision between them. For example, if the return value is 0.5, its means that after movement of 0.5 * movement there will be a collision. If the return value is 1 or greater, then there will not be a collision. If the return value is 0, the rectangles are tight; if it's less than 0, there is already a collision.
There can be an error in range if -+Constants.Epsilone2.

Rect - a rect. Might be as part of a model or on his own.

IClashAble - an interface for everything that is clash-able.

IShape - interface for any shape.

Model - made of shapes. In the beginning of any connection with a client the server sends to the client all the models. It allows the server next to say to the client things such as “put model … at location …” and use less data transfering.

Thing - actually like an instance of a model. It has a modelID, position, and angle.

Vector - just a vector in the 2d space.
