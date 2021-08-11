using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Threading;
using JuvoPlayer;
using JuvoPlayer.Common;
using System.Collections.Generic;
using TVMultimedia = Tizen.TV.Multimedia;
using Tizen.Multimedia;
using ElmSharp;

namespace Runner{
    internal static class JuvoPlayerWrapper {
        private static JuvoLogger.ILogger Logger = JuvoLogger.LoggerManager.GetInstance().GetLogger("JuvoPlayer");

        private static TVMultimedia.Player platformPlayer;
        private static TVMultimedia.DRMManager platformDrmMgr;
        private static PlayerServiceProxy<PlayerServiceImpl> juvoPlayer;

        public static async void Run() {
            Logger.Info("Before run.");

            var window = new ElmSharp.Window("SimplePlayer")
            {
                Geometry = new ElmSharp.Rect(0, 0, 1920, 1080)
            };
            window.Show();
            Logger.Info("Window is shown.");

            //Playback launching functions
            async Task PlayPlatformMediaClean(String videoSourceURL, TVMultimedia.Player player)
            {
                player.SetSource(new MediaUriSource(videoSourceURL));
                await player.PrepareAsync();
                player.Start();
            }

            async Task PlayPlatformMediaDRMed(String videoSourceURL, String licenseServerURL, TVMultimedia.Player player)
            {
                platformDrmMgr = TVMultimedia.DRMManager.CreateDRMManager(TVMultimedia.DRMType.Playready);

                platformDrmMgr.Init($"org.tizen.juvo_player_flutter_example");
                platformDrmMgr.AddProperty("LicenseServer", licenseServerURL);
                platformDrmMgr.Url = videoSourceURL;
                platformDrmMgr.Open();
                player.SetDrm(platformDrmMgr);

                await PlayPlatformMediaClean(videoSourceURL, player);
            }

            void PlayJuvoPlayerClean(String videoSourceURL, IPlayerService player)
            {
                Logger.Info("PlayJuvoPlayerClean started.");
                player.SetSource(new ClipDefinition
                {
                    Title = "Title",
                    Type = "dash",
                    Url = videoSourceURL,
                    Subtitles = new List<SubtitleInfo>(),
                    Poster = "Poster",
                    Description = "Descritption",
                    DRMDatas = new List<DrmDescription>()
                });

                player.StateChanged()
                    .ObserveOn(SynchronizationContext.Current)
                    .Where(state => state == JuvoPlayer.Common.PlayerState.Prepared)
                    .Subscribe(state =>
                    {
                        Logger.Info("player.Start() call.");
                        player.Start();
                    });
                Logger.Info("PlayJuvoPlayerClean finished.");
            }

             void PlayJuvoPlayerDRMed(String videoSourceURL, String licenseServerURL, String drmScheme, IPlayerService player)
            {
                var drmData = new List<DrmDescription>();
                drmData.Add(new DrmDescription
                {
                    Scheme = drmScheme,
                    LicenceUrl = licenseServerURL,
                    KeyRequestProperties = new Dictionary<string, string>() { { "Content-Type", "text/xml; charset=utf-8" } },
                });

                player.SetSource(new ClipDefinition
                {
                    Title = "Title",
                    Type = "dash",
                    Url = videoSourceURL,
                    Subtitles = new List<SubtitleInfo>(),
                    Poster = "Poster",
                    Description = "Descritption",
                    DRMDatas = drmData
                });

                player.StateChanged()
                   .ObserveOn(SynchronizationContext.Current)
                   .Where(state => state == JuvoPlayer.Common.PlayerState.Prepared)
                   .Subscribe(state =>
                   {
                       player.Start();
                   });
            }
 
            async Task Play()
            {     
                /////////////Clean contents////////////////////
                //var url = "http://yt-dash-mse-test.commondatastorage.googleapis.com/media/car-20120827-manifest.mpd";
                //var url = "https://livesim.dashif.org/livesim/testpic_2s/Manifest.mpd";
                var url = "https://bitdash-a.akamaihd.net/content/sintel/sintel.mpd";
                //var url = "http://distribution.bbb3d.renderfarming.net/video/mp4/bbb_sunflower_1080p_30fps_normal.mp4";
                //var url = "http://wowzaec2demo.streamlock.net/live/bigbuckbunny/manifest_mvtime.mpd";

                /////////////Play Ready encrypted content//////
                //var url = "https://media.axprod.net/TestVectors/v7-MultiDRM-SingleKey/Manifest_1080p.mpd";
                //var license = "https://drm-widevine-licensing.axtest.net/AcquireLicense";
                //var url = "http://yt-dash-mse-test.commondatastorage.googleapis.com/media/oops_cenc-20121114-signedlicenseurl-manifest.mpd";
                //var license = ""; //The license url is embeded in the video source .mpd file above

                /////////////Widevine encrypted content////////
                //var url = "https://bitmovin-a.akamaihd.net/content/art-of-motion_drm/mpds/11331.mpd";
                //var license = "https://widevine-proxy.appspot.com/proxy";
                //var url = "https://storage.googleapis.com/wvmedia/cenc/h264/tears/tears_uhd.mpd";
                //var license = "https://proxy.uat.widevine.com/proxy?provider=widevine_test";
                //var url = "https://media.axprod.net/TestVectors/v7-MultiDRM-SingleKey/Manifest_1080p.mpd";
                //var license = "https://drm-widevine-licensing.axtest.net/AcquireLicense";

                //////The TV platform MediaPlayer (URL data source only).
                //platformPlayer = new TVMultimedia.Player { Display = new Display(window) };
                //await PlayPlatformMediaClean(url, platformPlayer);
                //await PlayPlatformMediaDRMed(url, license, platformPlayer);

                //////The JuvoPlayer backend (elementary stream data source).
                
                juvoPlayer = new PlayerServiceProxy<PlayerServiceImpl>();
                juvoPlayer.SetWindow(window);
                PlayJuvoPlayerClean(url, juvoPlayer);
                
                //PlayJuvoPlayerDRMed(url, license, "playready", juvoPlayer);
                //PlayJuvoPlayerDRMed(url, license, "widevine", juvoPlayer);
            }
        
            Logger.Info("Before Play().");
            try{
                await Play();
            }
            catch(System.Exception e) {
                Logger.Error("An exception occured on attept ot play. Message: " + e.Message);
            }
            Logger.Info("After Play().");
        }
    }
}