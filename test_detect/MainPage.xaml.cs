using Camera.MAUI;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace test_detect;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
        
        btn.Clicked += async (o, e) => await ImageSourceToByteArrayAsync();
        
    }

    public class Answer
    {
        public string[] Labels { get; set; } = null!;


        public string[] Score { get; set; } = null!;

    }



    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        cameraView.Camera = cameraView.Cameras.First();

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await cameraView.StopCameraAsync();
            await cameraView.StartCameraAsync();
        });
    }

    //public static byte[] imageData;
    public async Task ImageSourceToByteArrayAsync()
    {

        
        byte[] imageData;
        using (Stream stream = await ((IStreamImageSource)cameraView.GetSnapShot(Camera.MAUI.ImageFormat.PNG)).GetStreamAsync())
        using (MemoryStream memoryStream = new MemoryStream())
        {
            await stream.CopyToAsync(memoryStream);
            imageData = memoryStream.ToArray();

        }
        string url = "http://192.168.161.49:80/api/ml";

        var client = new HttpClient();


        MultipartFormDataContent form = new MultipartFormDataContent
                {
                    { new ByteArrayContent(imageData, 0, imageData.Length), "file", "pic.jpeg" }
                };

        await SendImage(url, form, client);

    }

    public async Task SendImage(string url, MultipartFormDataContent form, HttpClient client)
    {
        var response = await client.PostAsync(url, form);

        string json = await response.Content.ReadAsStringAsync();

        Answer values = JsonConvert.DeserializeObject<Answer>(json);

        try
        {
            //await Shell.Current.DisplayAlert("Супер", $"{values.Labels[0]} {values.Score[0]}", "Ok");
            textView.Text = $"{values.Labels.Length}  Labels = {values.Labels[0]}\nScore = {values.Score[0]}";

        }
        catch
        {
            await Shell.Current.DisplayAlert("Супер", $"Ничего не найдено", "Ok");
            textView.Text = "Ничего не найдено";
        }
    }

}
    

