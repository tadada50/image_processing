using System;
using System.Collections.Generic;
using System.Text;

namespace ImageProcessing
{
    public class XYCoordinates
    {
        private static int instanceCounter;
        private readonly int instanceId;

        public XYCoordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.instanceId = ++instanceCounter;
        }

        public int x { get; set; }
        public int y { get; set; }
        public int id { get; set; }
        public int UniqueId
        {
            get { return this.instanceId; }
        }

    }
}
