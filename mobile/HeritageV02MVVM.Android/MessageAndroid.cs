using HeritageV02MVVM.Services.Abstraction;
using Android.Widget;
using HeritageV02MVVM.Droid;
using Android.App;

[assembly: Xamarin.Forms.Dependency(typeof(MessageAndroid))]
namespace HeritageV02MVVM.Droid
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