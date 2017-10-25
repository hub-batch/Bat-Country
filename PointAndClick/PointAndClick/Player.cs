using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointAndClick
{
    public class Player
    {
        public List<Item> PlayerInventory = new List<Item>();

        public int PlayerState { get; set; }
    }
}
