using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Great_Hero
{
    class Player
    {
        public readonly Bitmap idleSheet;
        public readonly Bitmap moveSheet;
        public int X { get; private set; }
        public int Y { get; private set; }
        public int currentFrame { get; private set; }
        private int maxFrames;
        public Direction currentDirection { get; private set; }
        private int speed;
        private int ySpeedup;
        private AnimationType currentAnimation;

        public enum Direction
        {
            Right = 0,
            Left = 1
        }

        public enum AnimationType
        {
            Idle = 0,
            Move = 1
        }

        public void StepFrame()
        {
            currentFrame++;
            if (currentFrame >= maxFrames)
                currentFrame = 0;
        }

        public Player()
        {
            X = 150;
            Y = 150;
            currentDirection = Direction.Right;
            currentAnimation = AnimationType.Idle;
            speed = 5;
            idleSheet = new Bitmap(@"..\..\..\..\Sprites\PlayerIdle.png");
            moveSheet = new Bitmap(@"..\..\..\..\Sprites\PlayerMove.png");
            ySpeedup = 10;
        }

        public void MovePlayer(Direction direction)
        {
            currentAnimation = AnimationType.Move;
            currentDirection = direction;
            switch (direction)
            {
                case Direction.Left:
                    X -= speed;
                    break;
                case Direction.Right:
                    X += speed;
                    break;
            }
        }

        public void SetIdle()
        {
            currentAnimation = AnimationType.Idle;
            currentFrame = 0;
        }

        public Bitmap GetSheet()
        {
            switch (currentAnimation)
            {
                case AnimationType.Idle:
                    maxFrames = 1;
                    return idleSheet;
                case AnimationType.Move:
                    maxFrames = 2;
                    return moveSheet;
            }
            return idleSheet;
        }
    }
}
