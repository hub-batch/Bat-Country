using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointAndClick
{
    public class Item
    {
        public Texture2D ItemTex { get; set; }
        public Rectangle ItemSize { get; set; }
        public string ItemName { get; set; }
        public bool HasCollected { get; set; }
    }
}
