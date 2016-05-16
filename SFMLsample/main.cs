using System;
using System.IO;
using System.Text;
using SFML.Audio;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using MotionNET;

namespace SFMLTest
{
    class Program
    {
        static void Main(string[] args)
        {
            MySFMLProgram app = new MySFMLProgram();
            app.StartSFMLProgram();
        }
    }
    class MySFMLProgram
    {
        RenderWindow _window;
        DataSource _video = new DataSource(); // create the data source from which playback will happen

        Font _font = new Font("./Graphics/GenEiGothicP-Light.otf");

        Music _bgm = new Music("./dtx/bgm.ogg");

        Texture _songJacketTexture = new Texture("./dtx/pre.png");
        Texture _drumLanesTexture = new Texture("./graphics/paret.png");

        public void StartSFMLProgram()
        {
            Text _songTitle = new Text(new string(new char[] { '\u4EBA', '\u4F11' }), _font);
            Text _songArtist = new Text(new string(new char[] { '\u4EBA', '\u4F11' }), _font);

            StreamReader sr = new StreamReader("./dtx/mstr.dtx", Encoding.GetEncoding(932));
            string data = sr.ReadLine();
            while (data != null) {
                if (data.StartsWith("#TITLE: ")) {
                    _songTitle.DisplayedString = data.Substring(8);
                }

               if (data.StartsWith("#ARTIST: ")) {
                    _songArtist.DisplayedString = data.Substring(9);
                    break;
                }
                data = sr.ReadLine();
            }

            _songTitle.CharacterSize = 24;
            _songTitle.Color = new Color(0, 255, 0);
            _songTitle.Font = _font;
            _songTitle.Position = new Vector2f(925.0f, 600.0f);

            _songArtist.CharacterSize = 24;
            _songArtist.Color = new Color(0, 255, 0);
            _songArtist.Font = _font;
            _songArtist.Position = new Vector2f(925.0f, 625.0f);

            Console.Write(_songTitle.DisplayedString + '\n');
            Console.Write(_songArtist.DisplayedString + '\n');

            Sprite _songJacketSprite = new Sprite(_songJacketTexture);
            _songJacketSprite.Position = new Vector2f(915.0f, 400.0f);
            _songJacketSprite.Rotation = -30.0f;
            _songJacketSprite.Scale = new Vector2f(245.0f / _songJacketTexture.Size.X, 245.0f / _songJacketTexture.Size.Y);

            Sprite _drumLanesSprite = new Sprite(_drumLanesTexture);
            _drumLanesSprite.Position = new Vector2f(350.0f, 0);

            if (!_video.LoadFromFile("./dtx/bg.avi")) // load a file into the data source
                return;

            //AudioPlayback audioplayback = new AudioPlayback(_video); // create an audio playback from our data source
            VideoPlayback videoplayback = new VideoPlayback(_video); // create a video playback from our data source

            // scale video to fit the window
            videoplayback.Scale = new Vector2f(1280f / (float)_video.VideoSize.X, 720f / (float)_video.VideoSize.Y);

            _window = new RenderWindow(new VideoMode(1280, 720), "SFML window");
            _window.SetVisible(true);
            _window.Closed += new EventHandler(OnClosed);

            _bgm.Play();

            while (_window.IsOpen)
            {
                _window.DispatchEvents();
                _video.Update(); // update the data source - this is required for any playbacks
                _window.Draw(videoplayback); // draw the video playback

                _window.Draw(_songJacketSprite);
                _window.Draw(_songTitle);
                _window.Draw(_songArtist);
                _window.Draw(_drumLanesSprite);
                
                _window.Display();
                _video.Play();

            }
        }

        void OnClosed(object sender, EventArgs e)
        {
            _bgm.Stop();
            _video.Stop();
            _window.Close();
        }
    }
}