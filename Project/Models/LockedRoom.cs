using System;
using System.Collections.Generic;
using CastleGrimtol.Project.Interfaces;

namespace CastleGrimtol.Project.Models
{
  class LockedRoom : Room
  {
    public bool Locked { get; private set; }
    public Item UnlockedWith { get; set; }

    public void Unlock(Item item)
    {

      Console.WriteLine($"In the unlock function with Item:  {item.Name}");
      if (item == UnlockedWith)
      {
        Locked = false;
        Console.WriteLine("You called your friend to open their door; they couldn't hear you knocking because they were vacuuming.");
      }
      else if (item != UnlockedWith)
      {
        Console.WriteLine("Cannot use item on door");
      }
    }

    public LockedRoom(string name, string desc, Item unlockedWith) : base(name, desc)
    {
      UnlockedWith = new Item("test", "testing one two");
      UnlockedWith = unlockedWith;
      Locked = false;

    }
  }
}