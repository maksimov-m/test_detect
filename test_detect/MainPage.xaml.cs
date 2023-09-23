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

    public class ResponseData
    {
        public string Image { get; set; }
        public int[] RectangleCoords { get; set; }
        public string[] Description{ get; set; }
    }

    public async Task SendImage(string url, MultipartFormDataContent form, HttpClient client)
    {
        var response = await client.PostAsync(url, form);


        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<ResponseData>(json);

            // Преобразуем Base64-строку обратно в байты
            var processedImageBytes = Convert.FromBase64String(responseData.Image);

            
            // Создаем изображение из байтов
            ImageSource processedImage = ImageSource.FromStream(() => new MemoryStream(processedImageBytes));

            
            //cameraBox.IsVisible = false;
            //photoBox.IsVisible = true;
            //photo.Source = processedImage;
            //Dictionary<string, ImageSource> keyValuePairs = new Dictionary<string, ImageSource> { { "Content", processedImage } };
            
            if (json.Length > 0 && responseData.Description.Length > 0)
            {
                //await Shell.Current.GoToAsync($"{nameof(ShowInfoPage)}", (IDictionary<string, object>)keyValuePairs);
                string desc = "";
                for (int i = 0; i < responseData.Description.Length; i++)
                {
                    desc += responseData.Description[i] + "\n";
                }
                await Navigation.PushAsync(new ShowInfoPage(processedImage, desc), animated:true);
            }
            else
                await Shell.Current.DisplayAlert("Супер", $"Ничего не найдено", "Ok");
            //textView.Text = "Ничего не найдено";
        }
        else
        {
            await Shell.Current.DisplayAlert("Супер", $"Ничего не найдено", "Ok");
            //textView.Text = "Ничего не найдено";
        }
        //Answer values = JsonConvert.DeserializeObject<Answer>(json);

        //try
        //{
        //    //await Shell.Current.DisplayAlert("Супер", $"{values.Labels[0]} {values.Score[0]}", "Ok");
        //    string answ = "";
        //    for(int i = 0; i < values.Labels.Length; i++)
        //    {
        //        answ = answ + $"Деталь {i + 1}: {values.Labels[i]}\n";
        //    }

        //    textView.Text = $"{answ}";

        //}
        //catch
        //{
        //    await Shell.Current.DisplayAlert("Супер", $"Ничего не найдено", "Ok");
        //    textView.Text = "Ничего не найдено";
        //}
    }

}
    

