using HeritageV04.Services.Abstractions;
using Android.Widget;
using HeritageV04.Droid;
using Android.App;

[assembly: Xamarin.Forms.Dependency(typeof(MessageAndroid))]
namespace HeritageV04.Droid
{
    public class MessageAndroid : IMessage
    {

        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }

    }
}