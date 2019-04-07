using System;
using System.Collections.Generic;
using CastleGrimtol.Project.Interfaces;

namespace CastleGrimtol.Project.Models
{
  public class Room : IRoom
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Item> Items { get; set; }
    public Dictionary<string, IRoom> Exits { get; set; }

    public void AddItem(Item item)
    {
      Items.Add(item);
    }

    public void PrintItems()
    {
      Console.WriteLine($"Items available in {this.Name} are: ");
      Items.ForEach(item =>
      {
        Console.WriteLine($"Item: {item.Name} - {item.Description}");
      });
    }

    public void AddExit(string direction, IRoom room)
    {
      Exits.Add(direction, room);
    }

    public virtual IRoom EnterRoom(string dir)
    {
      if (Exits.ContainsKey(dir))
      {
        IRoom roomToEnter = Exits[dir];
        if (roomToEnter is LockedRoom)
        {
          LockedRoom room = (LockedRoom)roomToEnter;
          if (!room.Locked)
          {
            return (IRoom)room;
          }
          Console.Clear();
          System.Console.WriteLine("LOCKED!!!");
          return (IRoom)this;
        }
        return Exits[dir];
      }
      Console.WriteLine("That direction is not an option");
      return (IRoom)this;
    }
    public Room(string name, string desc)
    {
      Name = name;
      Description = desc;
      Items = new List<Item>();
      Exits = new Dictionary<string, IRoom>();
    }
  }



}