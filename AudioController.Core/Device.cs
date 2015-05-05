using System;

namespace AudioController.Core
{
    public class Device : IEquatable<Device>
    {
        internal Device(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public string Name { get; private set; }
     
        public string Id { get; private set; }

        public override bool Equals(object obj)
        {
            var other = obj as Device;
            return other != null && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public bool Equals(Device other)
        {
            return other != null && other.Id == Id;
        }
    }
}