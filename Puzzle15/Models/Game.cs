using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle15.Models
{
    public partial class Game
    {
        public int Id { get; set; }

        public string State
        {
            get => string.Join(",", _state);
            set => _state = value.Split(",").Select(n => Convert.ToInt32(n)).ToArray();
        }

        public int SideLength { get; set; }

        public bool IsSolved { get; set; }

        public int Steps { get; set; }
    }
}
