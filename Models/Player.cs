using System;
using System.Threading.Tasks;
using Avalonia.Platform;
using NAudio.Wave;
using ReactiveUI;

namespace MusicPlayer.Models  {
    class Player : ReactiveObject { 
        public Song? Song { get; set; }
        private WaveOutEvent? WaveOut { get; set; } //half of the MediaPlayer object from pseudocode
        private StreamMediaFoundationReader? Reader { get; set; } //half of the MediaPlayer object from pseudocode
        bool paused = false;
        private int time;
        public int SetTime { set { if (Reader != null) {Reader.Position = value*Reader.WaveFormat.AverageBytesPerSecond;}}} //sets the player time without triggering update
        public int Time { get => time; set => this.RaiseAndSetIfChanged(ref time, value); } //exposes time as property

        public event EventHandler? PlayerFinished;
        protected virtual void OnPlayerFinished(EventArgs? e) //Event Handler function
        {
            PlayerFinished?.Invoke(this, e ?? new EventArgs());
        }

        public Player(int t, Song? s){
            time = t;
            Song = s;
            Task.Run(TimeWorker);
        }
        public void LoadNew(bool p){ //loads the song in this.Song
            if (Song != null) { //must have loaded song, (condition here to suppress CS8602)
                if (WaveOut != null){ //exit gracefully first
                    WaveOut.PlaybackStopped -= Stop;
                    WaveOut.Stop();
                    WaveOut.Dispose();
                    Reader?.Dispose();
                }
                //create new media out objects
                WaveOut = new WaveOutEvent();
                Reader = new StreamMediaFoundationReader(AssetLoader.Open(new Uri("avares://MusicPlayer/Assets/music/"+Song.Filename)));
                WaveOut.PlaybackStopped += Stop; //bind event of new player
                //reset media out
                WaveOut.Init(Reader);
                Reader.CurrentTime = new TimeSpan(0, 0, 0);
                if (p){ //carry on playing if currently playing
                    Play(); 
                }
            }
        }

        private async Task TimeWorker(){ //async function to update the Time variable and trigger UI updates, as reader.CurrentTime.TotalSeconds is treated as a POCO and fires no updates
            while (true) {
                await Task.Delay(50); //wait for a short amount of time, else this thread will take up CPU time
                if (Reader != null){ 
                    if (Time != (int)Reader.CurrentTime.TotalSeconds){ //if the time in seconds has changed
                        Time = (int)Reader.CurrentTime.TotalSeconds;  //push an update
                    }
                }
                else if (Time != 0) { //if there is no current song
                    Time = 0; //set time to 0
                }
            }
        }
        public void Play(){ //wraps WaveOut as this should not be exposed
            WaveOut?.Play();
        }
        public void Pause(){ //wraps WaveOut, as above
            paused = true;
            WaveOut?.Stop();
        }
        private void Stop(object? e, EventArgs? a){ //callback for PlayerFinished
            if (!paused && Reader != null){ //if finished song/stop song
                OnPlayerFinished(null); //raise own event for VM to catch
                Reader.Position = 0; //it is good practice to keep Reader inside class scope, hence it is called here and not in the VM
            }
            else {
                paused = false; //if only paused, carry on, reset variable
            }
        } 
    }
}