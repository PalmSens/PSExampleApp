using System;

namespace PSExampleApp.Common.Models
{
    public abstract class DataObject
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
    }
}