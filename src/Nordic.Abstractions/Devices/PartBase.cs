using System;
using Nordic.Abstractions.Enumerations;

namespace Nordic.Abstractions.Devices
{
    public class PartBase
    {
        /// <summary>
        /// Gets the unique identifier of the part.
        /// </summary>
        public Guid Uid { get; }

        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the enum PartTypes for the instance 
        /// </summary>
        public PartTypes Type { get; set; }

		public PartBase()
		{
            Uid = Guid.NewGuid();
		}
    }
}