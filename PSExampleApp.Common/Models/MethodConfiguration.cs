namespace PSExampleApp.Common.Models
{
    public class MethodConfiguration : DataObject
    {
        /// <summary>
        /// The method that is serialized for saving purposes
        /// </summary>
        public byte[] SerializedMethod { get; set; }
    }
}