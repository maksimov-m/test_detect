namespace test_detect;

public partial class ShowInfoPage : ContentPage
{


	public ShowInfoPage(ImageSource img, string desc)
	{
		InitializeComponent();
        photo.Source = img;
        textView.Text = desc;
        //if (Image != null)
        //{
        //    photo.Source = Image;
        //}
        }

    
}