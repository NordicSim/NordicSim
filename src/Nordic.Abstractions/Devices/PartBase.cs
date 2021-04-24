using Nordic.Abstractions.Enumerations;

namespace Nordic.Abstractions.Devices
{
    public class PartBase
    {
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the enum PartTypes for the instance 
        /// </summary>
        public PartTypes Type { get; set; }
    }
}