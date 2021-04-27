using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    class Map
    {
        private List<Room> roomSamples;
        private int seed;
        private Random roomSequenceRandom;
        private List<Room> rooms;
        private int currentRoom;



        public Map(int? seed = null)
        {
            if (seed != null)
                this.seed = (int)seed;
            else
                this.seed = GenerateSeed();
            roomSequenceRandom = new Random(this.seed);

            roomSamples = new List<Room>
            {
                new Room(new List<Platform>
                {
                    new Platform(200, 600, 350)
                }),
                new Room(new List<Platform>
                {
                    new Platform(100, 300, 350),
                    new Platform(500, 700, 350)
                }),
                new Room(new List<Platform>
                {
                    new Platform(100, 300, 400),
                    new Platform(500, 700, 400),
                    new Platform(300, 500, 300)
                }, 
                10)
            };
        }

        private int GenerateSeed()
        {
            var r = new Random();
            return r.Next(10000);
        }

        public void GenerateRooms(int roomsCount = 9)
        {
            currentRoom = 0;
            rooms = new List<Room>();
            rooms.Add(new Room(new List<Platform>()));
            if (roomSamples.Count != 0)
                for (var i = 0; i < roomsCount - 1; i++)
                    rooms.Add(roomSamples[roomSequenceRandom.Next(roomSamples.Count)]);
        }

        public bool IsCurrentRoomLast() => currentRoom >= rooms.Count;

        public bool IsCurrentRoomFirst() => currentRoom <= 0;

        public Room GoToNext()
        {
            currentRoom++;
            return Current();
        }

        public Room GoToPrevious()
        {
            currentRoom--;
            return Current();
        }

        public Room Current() => rooms[currentRoom];
    }
}
