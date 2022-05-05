using System;

namespace PSHeavyMetal.Common.Models
{
    public abstract class DataObject
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
    }
}