using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle15.Models
{
    public partial class Game
    {
        private const int EmptySlot = -1;

        private const int DefaultLength = 4;

        private readonly Random _r;

        private int[] _state;

        public Game()
        {
        }

        public Game(int sideLength = DefaultLength)
        {
            _r = new Random();
            SideLength = sideLength;
            _state = Enumerable.Range(1, SideLength * SideLength).ToArray();
            _state[Size - 1] = EmptySlot;
            ShuffleState();
            
            while(!IsSolvable())
                ShuffleState();
        }

        private enum Move
        {
            Up,
            Down,
            Right,
            Left,
            Nowhere
        }

        private int Size => _state.Length;

        public void MoveTile(int tileNum)
        {
            var tilePos = GetTilePosition(tileNum);
            var m = GetAvaliableMoveForPosition(tilePos);

            if (m != Move.Nowhere)
            {
                MoveTile(tilePos, m);
            }
        }

        private void ShuffleState()
        {
            _state = _state.OrderBy(x => _r.Next()).ToArray();
        }

        private bool IsSolvable()
        {
            var cnt = 0;
            var empty = 0;
            for (var i = 0; i < Size; i++)
            {
                if (_state[i] == EmptySlot)
                    empty = i + 1;

                for (var j = i + 1; j < Size; j++)
                {
                    if (_state[i] > _state[j])
                        cnt++;
                }
            }
            return (cnt + empty) % 2 == 0;
        }

        private int GetTilePosition(int tileNum)
        {
            if (tileNum < 1 || tileNum >= Size)
                throw new ArgumentException($"The tile must be in range (1, {Size - 1})");

            for (var i = 0; i < Size; i++)
            {
                if (_state[i] == tileNum)
                    return i;
            }

            return -1;
        }

        private Move GetAvaliableMoveForPosition(int position)
        {
            // not near the left bound and there is an empty slot on the left => go left
            if ((position + 1) % SideLength != 1 && _state[position - 1] == EmptySlot)
                return Move.Left;

            // not near the right bound and there is an empty slot on the right => go right
            if ((position + 1) % SideLength != 0 && _state[position + 1] == EmptySlot)
                return Move.Right;

            // not near the upper bound and there is an empty slot above => go up
            if (position >= SideLength && _state[position - SideLength] == EmptySlot)
                return Move.Up;

            // not near the bottom bound and there is an empty slot below => go down
            if (position < Size - SideLength && _state[position + SideLength] == EmptySlot)
                return Move.Down;

            // otherwise
            return Move.Nowhere;
        }

        private void Swap(int pos1, int pos2)
        {
            var buf = _state[pos1];
            _state[pos1] = _state[pos2];
            _state[pos2] = buf;
        }

        private void MoveTile(int position, Move m)
        {
            Steps++;
            switch (m)
            {
                case Move.Up:
                    Swap(position, position - SideLength);
                    break;
                case Move.Down:
                    Swap(position, position + SideLength);
                    break;
                case Move.Right:
                    Swap(position, position + 1);
                    break;
                case Move.Left:
                    Swap(position, position - 1);
                    break;
            }
            CheckState();
        }

        private void CheckState()
        {
            for (var i = 0; i < Size - 1; i++)
            {
                if (_state[i] != i + 1)
                {
                    IsSolved = false;
                    return;
                }
            }

            IsSolved = true;
        }
    }
}
