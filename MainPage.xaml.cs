using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace Zettelkasten
{
    public class WordCard
    {
        public string Memo { get; set; } = "";
        public ObservableCollection<string> Tags { get; set; } = new();
    }

    public class MainViewModel : BindableObject
    {
        public ObservableCollection<WordCard> WordCards { get; set; } = new();
        public ObservableCollection<string> AvailableTags { get; set; } = new();

        private string _newTag = "";
        public string NewTag
        {
            get => _newTag;
            set
            {
                _newTag = value;
                OnPropertyChanged();
            }
        }

        private string _memo = "";
        public string Memo
        {
            get => _memo;
            set
            {
                _memo = value;
                OnPropertyChanged();
            }
        }

        public Command AddTagCommand => new(() =>
        {
            if (!string.IsNullOrWhiteSpace(NewTag) && !AvailableTags.Contains(NewTag))
            {
                AvailableTags.Add(NewTag);
                NewTag = "";
            }
        });

        public Command SaveCardCommand => new(() =>
        {
            if (!string.IsNullOrWhiteSpace(Memo))
            {
                WordCards.Add(new WordCard { Memo = Memo, Tags = new ObservableCollection<string>(AvailableTags) });
                Memo = "";
                AvailableTags.Clear();
            }
        });
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private WordCard? _draggedCard;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }

        private void OnDragStarting(object sender, DragStartingEventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is WordCard card)
            {
                _draggedCard = card;
                e.Data.Properties.Add("DraggedCard", card);
            }
        }

        private void OnDrop(object sender, DropEventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is WordCard targetCard && _draggedCard != null)
            {
                var viewModel = BindingContext as MainViewModel;
                if (viewModel != null)
                {
                    int oldIndex = viewModel.WordCards.IndexOf(_draggedCard);
                    int newIndex = viewModel.WordCards.IndexOf(targetCard);
                    if (oldIndex != -1 && newIndex != -1 && oldIndex != newIndex)
                    {
                        viewModel.WordCards.Move(oldIndex, newIndex);
                    }
                }
            }
        }
    }
}
