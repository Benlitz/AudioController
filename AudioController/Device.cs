namespace AudioController
{
    public class Device
    {
        internal Device(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public string Name { get; private set; }
     
        internal string Id { get; private set; }
    }
}