using System;

namespace PSExampleApp.Common.Models
{
    public class ApplicationSettings : DataObject
    {
        public byte[] BackgroundImage { get; set; }
        public string Title { get; set; }
        public Guid? ActiveUserId { get; set; }
    }
}