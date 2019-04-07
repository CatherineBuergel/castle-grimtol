using System.Collections.Generic;
using CastleGrimtol.Project.Models;

namespace CastleGrimtol.Project.Interfaces
{
  public interface IGameService
  {
    Room CurrentRoom { get; set; }
    Player CurrentPlayer { get; set; }

    //Initializes the game, creates rooms, their exits, and add items to rooms
    void Setup();

    //Restarts the game 
    void Reset();

    //Setup and Starts the Game loop
    void StartGame();

    //Gets the user input and calls the appropriate command
    void GetUserInput();

    // #region Console Commands

    //Stops the application
    void Quit();

    //Should display a list of commands to the console
    void Help();

    //Validate CurrentRoom.Exits contains the desired direction
    //if it does change the CurrentRoom
    void Go(string direction);

    //When taking an item be sure the item is in the current room 
    //before adding it to the player inventory, Also don't forget to 
    //remove the item from the room it was picked up in
    void TakeItem(string itemName);

    //No need to Pass a room since Items can only be used in the CurrentRoom
    //Make sure you validate the item is in the room or player inventory before
    //being able to use the item
    void UseItem(string itemName);

    //Print the list of items in the players inventory to the console
    void Inventory();

    //Display the CurrentRoom Description, Exits, and Items
    void Look();

    // #endregion
  }
}
