using CommunityToolkit.Maui.Views;
using RadioApp.Setting;
using RadioApp.ViewModels;
using System.Collections.ObjectModel;

namespace RadioApp.Pages;

public partial class ChooseTagPage : Popup
{
    public ObservableCollection<MusicTypeTagViewModel> AllTypes { get; set; }
    public ChooseTagPage(List<MusicTypeTag> allTypes)
    {
        AllTypes = new ObservableCollection<MusicTypeTagViewModel>();
        foreach (var types in allTypes)
        {
            var tags = new ObservableCollection<HomeTagViewModel>();
            foreach (var tag in types.Tags)
            {
                tags.Add(new HomeTagViewModel()
                {
                    Id = tag.Id,
                    Name = tag.Name
                });
            }

            AllTypes.Add(new MusicTypeTagViewModel()
            {
                TypeName = types.TypeName,
                Tags = tags
            });
        }

        if (Config.Desktop)
        {
            this.Size = new Size(650, 480);
        }
        else
        {
            var disp = DeviceDisplay.Current.MainDisplayInfo;
            var screenWidth = disp.Width / disp.Density;
            var screenHeight = disp.Height / disp.Density;
            this.Size = new Size(screenWidth, screenHeight);
        }

        InitializeComponent();
    }

    private void TagSelected_Tapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter == null)
        {
            return;
        }
        Close(e.Parameter.ToString());
    }
}