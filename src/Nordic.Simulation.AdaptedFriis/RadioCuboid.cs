using Microsoft.Extensions.Logging;
using Nordic.Abstractions.Data;
using System;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;

namespace Nordic.Simulation.AdaptedFriis
{
    /// <summary>
    /// Stores the data of the cuboid that is used as input for the wireless radio channel simulation
    /// </summary>
    public class RadioCuboid
    {
        // -- fields

        private readonly ILogger<RadioCuboid> _log;
 
        // --- properties

        public Vertex MinCorner { get; set; }
        public Vertex MaxCorner { get; set; }
        public float Resolution { get; set; }
        public int[] DataDimension { get; set; }
        public int TotalData { get; set; }

        // -- constructors

        public RadioCuboid()
        {
            _log = LoggingProvider.CreateLogger<RadioCuboid>();
            MinCorner = Nordic.Abstractions.Constants.Const.Channel.Radio.Space.MinCorner;
            MaxCorner = Nordic.Abstractions.Constants.Const.Channel.Radio.Space.MaxCorner;
            Resolution = Nordic.Abstractions.Constants.Const.Channel.Radio.Space.Resolution;
        }

        // --- methods

        public Vertex[] CreateRxPositions()
        {
            DataDimension = new int[3] {
                 (int)Math.Ceiling((MaxCorner.X - MinCorner.X) / (Resolution)),
                 (int)Math.Ceiling((MaxCorner.Y - MinCorner.Y) / (Resolution)),
                 (int)Math.Ceiling((MaxCorner.Z - MinCorner.Z) / (Resolution))
             };
            TotalData = DataDimension[0] * DataDimension[1] * DataDimension[2];

            Vertex[] positions = new Vertex[TotalData];
            float mid = Resolution / 2;
            /*
             * 'mid' pushes the data-point into the middle of an imaginary cube with half of the length of resolution 
             * to prevent the point to reach the edges of the box
             */
            int i = 0;
            for (float x = MinCorner.X + mid; x < MaxCorner.X; x = (float)Math.Round(x + Resolution, 3))
            {
                for (float y = MinCorner.Y + mid; y < MaxCorner.Y; y = (float)Math.Round(y + Resolution, 3))
                {
                    for (float z = MinCorner.Z + mid; z < MaxCorner.Z; z = (float)Math.Round(z + Resolution, 3))
                    {
                        positions[i] = new Vertex(x, y, z);
                        i++;
                    }
                }
            }

            _log.Trace($"{positions.Length} rx positions created");
            return positions;
        }

        public override string ToString() => $"Min: {MinCorner} Max: {MaxCorner}";       
    }
}
