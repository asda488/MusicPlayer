using System;
using Avalonia.Platform;
using NAudio.Wave;

namespace MusicPlayer.Models {
    class Player {
        public Song? Song { get; set; }
        public long Time { get; set; }
        WaveOutEvent? WaveOut { get; set; }
        public StreamMediaFoundationReader? Reader { get; set; }
        bool paused = false;
        public Player(long Time, Song? s){
            this.Time = Time;
            Song = s;
        }

        public void LoadNew(){
            WaveOut?.Dispose();
            Reader?.Dispose();
            WaveOut = new WaveOutEvent();
            Reader = new StreamMediaFoundationReader(AssetLoader.Open(new Uri("avares://MusicPlayer/Assets/music/"+Song.Filename)));
            WaveOut.Init(Reader);
            WaveOut.PlaybackStopped += Stop;
            Reader.CurrentTime = new TimeSpan(0, 0, (int)Time);
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
                
            }
            else {
                paused = false;
            }
        } 
    }
}