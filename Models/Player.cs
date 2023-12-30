using System;
using System.Threading.Tasks;
using Avalonia.Platform;
using NAudio.Wave;

namespace MusicPlayer.Models {
    class Player {
        Song? song { get; set; }
        long time { get; set; }
        WaveOutEvent? waveOut { get; set; }
        StreamMediaFoundationReader? reader { get; set; }
        bool paused = false;
        public Player(long Time, Song? s){
            time = Time;
            song = s;
        }

        public void LoadNew(){
            if (song != null){
                waveOut?.Dispose();
                reader?.Dispose();
                waveOut = new WaveOutEvent();
                Console.WriteLine("avares://MusicPlayer/Assets/music/"+song.filename);
                reader = new StreamMediaFoundationReader(AssetLoader.Open(new Uri("avares://MusicPlayer/Assets/music/"+song.filename)));
                waveOut.Init(reader);
                waveOut.PlaybackStopped += Stop;
                reader.Position = time;
            }
        }
        public void Play(){
            waveOut.Play();
        }
        public void Pause(){
            paused = true;
            waveOut.Stop();
            Stop(null, null);
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