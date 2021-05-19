using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    public class Sprite
    {
        private int currentFrameTime;
        private readonly int framePause;
        private int currentMaxFrames;
        public readonly int idleMaxFrames;
        public readonly int moveMaxFrames;
        public readonly int attackMaxFrames;
        public readonly int attackMoveMaxFrames;
        public readonly Bitmap idleSheet;
        public readonly Bitmap moveSheet;
        public readonly Bitmap attackSheet;
        public readonly Bitmap attackMoveSheet;
        public Size idleSize { get; private set; }
        public Size moveSize { get; private set; }
        public Size attackSize { get; private set; }
        public Size attackMoveSize { get; private set; }
        public int currentFrame { get; private set; }
        public Status currentStatus { get; private set; }

        public Sprite(Bitmap idleSheet, Size idleSize,
            Bitmap moveSheet = null, Size moveSize = new Size(),
            Bitmap attackSheet = null, Size attackSize = new Size(),
            Bitmap attackMoveSheet = null, Size attackMoveSize = new Size(),
            int oneFramePause = 1)
        {
            this.idleSheet = idleSheet;
            this.moveSheet = moveSheet;
            this.attackSheet = attackSheet;
            this.attackMoveSheet = attackSheet;
            this.idleSize = idleSize;
            this.moveSize = moveSize;
            this.attackSize = attackSize;
            this.attackMoveSize = attackSize;
            idleMaxFrames = this.idleSheet.Width / this.idleSize.Width;
            currentFrameTime = 0;
            framePause = oneFramePause;
            if (this.moveSheet != null)
                moveMaxFrames = this.moveSheet.Width / this.moveSize.Width;
            if (this.attackSheet != null)
                attackMaxFrames = this.attackSheet.Width / this.attackSize.Width;
            if (this.attackMoveSheet == null && this.attackSheet != null)
                this.attackMoveSheet = this.attackSheet;
            if (this.attackMoveSheet != null)
                attackMoveMaxFrames = this.attackMoveSheet.Width / this.attackMoveSize.Width;
            SetIdle();
        }

        public void StepFrame()
        {
            currentFrameTime++;
            if (currentFrameTime >= framePause)
            {
                currentFrame++;
                currentFrameTime = 0;
            }
            if (currentFrame >= currentMaxFrames && (currentStatus != Status.Attack && currentStatus != Status.AttackMove))
                currentFrame = 0;
            else if (currentFrame >= currentMaxFrames && (currentStatus == Status.Attack || currentStatus == Status.AttackMove))
                switch (currentStatus)
                {
                    case Status.Attack:
                        SetIdle();
                        currentFrame = 0;
                        break;
                    case Status.AttackMove:
                        SetMoving();
                        currentFrame = 0;
                        break;
                    default:
                        SetIdle();
                        currentFrame = 0;
                        break;
                }
        }

        public void SetIdle()
        {
            currentStatus = Status.Idle;
            currentMaxFrames = idleMaxFrames;
        }

        public void SetMoving()
        {
            if (moveSheet != null)
            {
                currentStatus = Status.Move;
                currentMaxFrames = moveMaxFrames;
            }
        }

        public void SetAttacking()
        {
            if (attackSheet != null)
            {
                if (currentStatus != Status.Attack && currentStatus != Status.AttackMove)
                    currentFrame = 0;
                currentStatus = Status.Attack;
                currentMaxFrames = attackMaxFrames;
            }
        }

        public void SetAttackingMove()
        {
            if (attackMoveSheet != null)
            {
                if (currentStatus != Status.Attack && currentStatus != Status.AttackMove)
                    currentFrame = 0;
                currentStatus = Status.AttackMove;
                currentMaxFrames = attackMoveMaxFrames;
            }
        }

        public Bitmap GetSheet()
        {
            switch (currentStatus)
            {
                case Status.Idle:
                    return idleSheet;
                case Status.Move:
                    return moveSheet;
                case Status.Attack:
                    return attackSheet;
                case Status.AttackMove:
                    return attackSheet;
                default: return null;
            }
        }

        public Size GetSize()
        {
            switch (currentStatus)
            {
                case Status.Idle:
                    return idleSize;
                case Status.Move:
                    return moveSize;
                case Status.Attack:
                    return attackSize;
                case Status.AttackMove:
                    return attackSize;
                default: return new Size();
            }
        }
    }
}
