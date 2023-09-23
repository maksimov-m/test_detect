namespace test_detect;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();


        Routing.RegisterRoute(nameof(ShowInfoPage), typeof(ShowInfoPage));
    }
}
