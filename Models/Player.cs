using System;
using System.Threading.Tasks;
using Avalonia.Platform;
using NAudio.Wave;
using ReactiveUI;

namespace MusicPlayer.Models  {
    class Player : ReactiveObject {
        public Song? Song { get; set; }
        public WaveOutEvent? WaveOut { get; set; }
        public StreamMediaFoundationReader? Reader { get; set; }
        bool paused = false;
        private int time;
        public int SetTime { set => Reader.Position = value*Reader.WaveFormat.AverageBytesPerSecond; }
        public int Time { get => time; set => this.RaiseAndSetIfChanged(ref time, value); }

        public event EventHandler PlayerFinished;
        protected virtual void OnPlayerFinished(EventArgs? e)
        {
            PlayerFinished?.Invoke(this, e);
        }

        public Player(int t, Song? s){
            time = t;
            Song = s;
            Task.Run(timeWorker);
        }
        public void LoadNew(bool p){
            if (WaveOut != null){
                WaveOut.PlaybackStopped -= Stop;
            }
            WaveOut?.Dispose();
            Reader?.Dispose();
            WaveOut = new WaveOutEvent();
            Reader = new StreamMediaFoundationReader(AssetLoader.Open(new Uri("avares://MusicPlayer/Assets/music/"+Song.Filename)));
            WaveOut.Init(Reader);
            WaveOut.PlaybackStopped += Stop;
            Reader.CurrentTime = new TimeSpan(0, 0, time);
            if (p){
                Play();
            }
        }

        private async Task timeWorker(){
            while (true) {
                await Task.Delay(50); //close enough
                try {
                    if (Time != (int)Reader.CurrentTime.TotalSeconds){
                        Time = (int)Reader.CurrentTime.TotalSeconds;
                    }
                    
                }
                catch (Exception) {
                    if (Time != 0){
                        Time = 0;
                    }

                }
            }
        }

        public void Play(){
            WaveOut.Play();
        }
        public void Pause(){
            paused = true;
            WaveOut.Stop();
        }
        private void Stop(object? e, EventArgs? a){
            if (!paused){
                OnPlayerFinished(null);
                Reader.Position = 0; //this is here as calling it from the VM would be bad practice
            }
            else {
                paused = false;
            }
        } 
    }
}