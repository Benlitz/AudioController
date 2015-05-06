using WpfEx.ViewModels;

namespace AudioController
{
    public class DisplayViewModel : ViewModel
    {
        public DisplayViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }
    }
}