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
        private Status currentStatus;
        private int currentMaxFrames;
        public readonly int idleMaxFrames;
        public readonly int moveMaxFrames;
        public readonly int attackMaxFrames;
        public readonly Bitmap idleSheet;
        public readonly Bitmap moveSheet;
        public readonly Bitmap attackSheet;
        public Size idleSize { get; private set; }
        public Size moveSize { get; private set; }
        public Size attackSize { get; private set; }
        public int currentFrame { get; private set; }

        public Sprite(Bitmap idleSheet, Size idleSize,
            Bitmap moveSheet = null, Size moveSize = new Size(),
            Bitmap attackSheet = null, Size attackSize = new Size(),
            int oneFramePause = 1)
        {
            this.idleSheet = idleSheet;
            this.moveSheet = moveSheet;
            this.attackSheet = attackSheet;
            this.idleSize = idleSize;
            this.moveSize = moveSize;
            this.attackSize = attackSize;
            idleMaxFrames = this.idleSheet.Width / idleSize.Width;
            currentFrameTime = 0;
            framePause = oneFramePause;
            if (this.moveSheet != null)
                moveMaxFrames = this.moveSheet.Width / moveSize.Width;
            if (this.attackSheet != null)
                attackMaxFrames = this.attackSheet.Width / attackSize.Width;
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
            if (currentFrame >= currentMaxFrames)
                currentFrame = 0;
        }

        public void SetIdle()
        {
            currentStatus = Status.Idle;
            currentMaxFrames = idleMaxFrames;
        }

        public void SetMoving()
        {
            currentStatus = Status.Move;
            currentMaxFrames = moveMaxFrames;
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
                    return moveSheet;
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
                    return moveSize;
                default: return new Size();
            }
        }
    }
}
