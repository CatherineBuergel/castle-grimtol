using System;
using System.Collections.Generic;
using System.Threading;
using CastleGrimtol.Project.Interfaces;
using CastleGrimtol.Project.Models;

namespace CastleGrimtol.Project
{
  public class GameService : IGameService
  {
    public Room CurrentRoom { get; set; }
    public Player CurrentPlayer { get; set; }
    public Item Phone { get; set; }
    LockedRoom FriendsHouse { get; set; }
    public bool Playing { get; set; }
    public List<string> Commands { get; set; }
    public List<Item> FatalFoods { get; set; }

    public void Play()
    {
      Console.Clear();
      Setup();
      InvitePlayer();
      Console.WriteLine("You can enter (help) at any time to view a list of commands");
      Console.WriteLine($"Welcome {CurrentPlayer.PlayerName}");
      Console.WriteLine($"You are in the {CurrentRoom.Name}. {CurrentRoom.Description}");
      while (Playing)
      {
        Console.Write("What would you like to do?  ");
        GetUserInput();
        // Console.Clear();
      }
    }

    #region
    public void CreatePlayer()
    {
      Console.Write("Enter Player Name: ");
      string name = Console.ReadLine();
      CurrentPlayer = new Player(name);
      Console.Clear();

    }
    public void Setup()
    {
      //Initialize command list
      //Go, Inventory, Look, Quit, Use, Take, Help

      Commands.Add("Go");
      Commands.Add("Help");
      Commands.Add("Take");
      Commands.Add("Use");
      Commands.Add("Inventory");
      Commands.Add("Look");
      Commands.Add("Quit");
      Commands.Add("Reset");

      Item EpiPen = new Item("EpiPen", "Kicks like a mule, saves like a lifeguard");
      Item Phone = new Item("Phone", "Great for calling friends or 911");
      Item Snack = new Item("Snack", "A filling, allergen free snack");
      Item Coffee = new Item("Coffee", "As long as it's not Hazelnut, you're probably safe");
      Item Muffin = new Item("Muffin", "This muffin is iterally surrounded by other pastries that contain nuts.");
      Item GranolaBar = new Item("GranolaBar", "You should know better than to ever eat a granola bar.");
      Item KaleChips = new Item("KaleChips", "No ingredients listed.");
      Item Sandwich = new Item("Sandwich on Whole Grain Bread", "Who doesn't love a good turkey sandwich?");
      Item Taco = new Item("Taco", "Not a lot of nuts in Mexican Cuisine");

      //initialize all rooms here
      //Items: game - if you use game then place fills up with people; coffee; 
      Room Main = new Room("Living room", @"You are at home in your living room. On your coffee table is an EpiPen,
       your phone, and a snack. To the north is your front door which leads to a Hiking Trail.");
      LockedRoom FriendsHouse = new LockedRoom("Friends house", "Your friend's door is locked.", Phone);
      Console.WriteLine(FriendsHouse.UnlockedWith.Name);
      Room Hiking = new Room("Hiking Trail", @"
      You are on a dirt hiking trail, surrounded by trees and sunlight. You are hungry and pass a stranger,
      they offer you a granola bar. You may take it or you may continue north to your Friends house.");
      Room VeganCafe = new Room("you're at a vegan cafe meeting a friend", @"
      Do you just drink coffee or risk it for some baked goods that may contain nuts");
      Room Work = new Room("you're at a work event", "do you order a sandwich or a taco");
      Room Home = new Room("Home", "you made it home to your own kitchen, you win the game!");

      Main.AddExit("north", Hiking);
      Hiking.AddExit("south", Main);
      Hiking.AddExit("north", FriendsHouse);
      FriendsHouse.AddExit("south", Hiking);
      FriendsHouse.AddExit("north", VeganCafe);
      VeganCafe.AddExit("south", FriendsHouse);
      VeganCafe.AddExit("east", Work);
      Work.AddExit("west", VeganCafe);
      Work.AddExit("south", Home);

      Main.AddItem(EpiPen);
      Main.AddItem(Snack);
      Main.AddItem(Phone);
      Hiking.AddItem(GranolaBar);
      FriendsHouse.AddItem(KaleChips);
      VeganCafe.AddItem(Coffee);
      VeganCafe.AddItem(Muffin);
      Work.AddItem(Sandwich);
      Work.AddItem(Taco);

      FatalFoods.Add(GranolaBar);
      FatalFoods.Add(KaleChips);
      FatalFoods.Add(Sandwich);
      FatalFoods.Add(Muffin);

      CurrentRoom = Main;
      Playing = true;


    }
    public void InvitePlayer()
    {
      Console.Write($"Welcome to the Dangerous Game of Having a Nut Allergy. In this game you will navigate through life with a nut allergy. If you can make it through your day without dying and make it home to your kitchen, then you win. Would you like to play? y/n:  ");
      string Input = Console.ReadLine().ToLower();
      if (Input == "y")
      {
        CreatePlayer();

      }
      else if (Input == "n")
      {
        Console.WriteLine("Maybe later then!");
        InvitePlayer();
      }
      else
      {
        Console.WriteLine("That is not a valid entry");
        Console.Clear();
        InvitePlayer();
      }


    }

    public void GetUserInput()
    {
      //   Console.Write("What would you like to do?   ");
      string[] Input = Console.ReadLine().ToLower().Split(" ");
      string Command = Input[0];
      string Option = "";
      if (Input.Length > 1)
      {
        Option = Input[1];
      }
      switch (Command)
      {
        case "go":
          Go(Option);
          break;
        case "help":
          Console.Clear();
          Help();
          break;
        case "inventory":
          Console.Clear();
          Inventory();
          break;
        case "look":
          Console.Clear();
          Look();
          break;
        case "reset":
          Console.Clear();
          Reset();
          break;
        case "take":
          Console.Clear();
          TakeItem(Option);
          break;
        case "use":
          if (Option == "phone")
          {
            Item Phone = validatePlayerInventory(Option);
            Console.WriteLine($"{Phone.Name} is getting passed along");
            FriendsHouse.Unlock(Phone);
            break;
          }
          UseItem(Option);
          break;
        case "quit":
          Quit();
          break;
        default:
          Console.Clear();
          Console.WriteLine($"{Option} is not a valid command");
          GetUserInput();
          break;
      }
      //   Console.Clear();

    }

    public Item validatePlayerInventory(string itemName)
    {
      Item validated = CurrentPlayer.Inventory.Find(i => i.Name.ToLower() == itemName);
      if (validated != null)
      {
        Console.WriteLine($"{validated.Name} is getting validated");
        return validated;
      }
      Console.WriteLine("But it's gone too far");
      validated = new Item("Not a valid item", "Not a valid item");
      Console.WriteLine("it's not getting validated");
      return validated;
    }


    public void Go(string direction)
    {
      CurrentRoom = (Room)CurrentRoom.EnterRoom(direction);
      Console.WriteLine($"You are standing in the {CurrentRoom.Name}. {CurrentRoom.Description}");
      GetUserInput();
    }

    public void Help()
    {
      Console.WriteLine("Command List:");
      Commands.ForEach(c =>
      {
        Console.WriteLine(c);
      });
      GetUserInput();
    }

    public void Inventory()
    {
      Console.WriteLine("Your inventory contains: ");
      CurrentPlayer.Inventory.ForEach(item =>
      {
        Console.WriteLine(item.Name);
      });
      GetUserInput();

    }

    public void Look()
    {
      Console.WriteLine(CurrentRoom.Description);
      GetUserInput();
    }

    public void Quit()
    {
      Console.Write("Are you sure you want to quit y/n?");
      string input = Console.ReadLine().ToLower();
      if (input == "y")
      {
        Environment.Exit(0);
      }
      else if (input == "n")
      {
        Console.Clear();
        GetUserInput();
      }
      else
      {
        Console.WriteLine($"{input} is not a valid option. Please choose y or n");
        Quit();
      }
    }

    public void Reset()
    {
      Play();
    }
    public void StartGame()
    {
      throw new System.NotImplementedException();
    }

    public bool CheckIngredients(string itemName)
    {
      Item found = FatalFoods.Find(i => i.Name.ToLower() == itemName);
      Item Epi = CurrentPlayer.Inventory.Find(i => i.Name.ToLower() == "epipen");
      if (found != null && Epi != null)
      {
        Console.WriteLine($@"You are allergic to {itemName} however you have used your Epi Pen!
        Unfortuntely, you are now saddled with $10,000 in medical bills, so you still lose.");
        Thread.Sleep(5000);
        Play();
      }
      else if (found != null)
      {
        Console.WriteLine($"Your allergy strikes again! {found.Description}");
        Thread.Sleep(5000);
        Play();
      }
      else
      {
        return false;
      }
      return true;
    }

    public void TakeItem(string itemName)
    {
      bool died = CheckIngredients(itemName);
      if (!died)
      {
        Item found = CurrentRoom.Items.Find(i => i.Name.ToLower() == itemName);
        if (found != null)
        {
          CurrentPlayer.Inventory.Add(found);
          CurrentRoom.Items.Remove(found);
          Console.Clear();
          Console.WriteLine($@"You have added {found.Name} to your inventory.
    {found.Description}");

        }
        if (found == null)
        {
          Console.WriteLine("Not a valid item");
        }
      }

    }

    public void UseItem(string itemName)
    {
      switch (itemName)
      {
        case "snack":
          Console.WriteLine("You ate a cheese stick! It was delicious and now you're less willing to eat suspicious food.");
          break;
        case "epipen":
          Console.WriteLine("You used your EpiPen for no reason. Now your heart is racing and you're out $70.");
          break;
        default:
          Console.WriteLine($"You cannot use {itemName}");
          break;
      }
    }
    #endregion

    public GameService()
    {
      Commands = new List<string>();
      FatalFoods = new List<Item>();

      FriendsHouse = new LockedRoom("locked", "room", Phone);
      Phone = new Item("phone", "for calls");
    }
  }
}