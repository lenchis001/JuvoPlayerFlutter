using Tizen.Flutter.Embedding;

namespace Runner
{
    public class App : Tizen.Applications.CoreUIApplication //FlutterApplication
    {
        protected override void OnCreate()
        {
            base.OnCreate();

            JuvoPlayerWrapper.Run();
            
            //GeneratedPluginRegistrant.RegisterPlugins(this);
        }

        static void Main(string[] args)
        {
            var app = new App();
            app.Run(args);
        }
    }
}
