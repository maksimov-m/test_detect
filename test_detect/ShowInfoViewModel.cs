using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_detect;

[QueryProperty("Content", "Content")]
public partial class ShowInfoViewModel : ObservableObject
{
    [ObservableProperty]
    object content;

    [ObservableProperty]
    ImageSource imageSource;


    //public class ResponseData
    //{
    //    public string Image { get; set; }
    //    public int[] RectangleCoords { get; set; }
    //}

    //public ShowInfoViewModel()
    //{
    //    Content content = new ImageSource();
    //    var responseData = JsonConvert.DeserializeObject<ResponseData>(content);

    //    // Преобразуем Base64-строку обратно в байты
    //    var processedImageBytes = Convert.FromBase64String(responseData.Image);

    //    // Создаем изображение из байтов
    //    imageSource = ImageSource.FromStream(() => new MemoryStream(processedImageBytes));
    //}

}
