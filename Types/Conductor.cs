using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCIIMusicVisualiser8
{
    public class Conductor
    {
        // Handles seconds to beat conversion from a hypothetical audio

        public Conductor(double _BPM, double _beatsPerBar = -1, double _startOffset = 0)
        {
            BPM = _BPM;
            beatsPerBar = _beatsPerBar;
            startOffset = _startOffset;

            millisecondsPerBeat = 60 / BPM * 1000; // seconds per beat * 1000
        }

        public double
            BPM,            // BPM to represent
            startOffset,    // How far in seconds beat 0 is from the start of the audio
            beatsPerBar;    // How many beats per bar in the audio

        public long currentTime;    // The current time of the audio in milliseconds

        public double currentBar // The current bar of the audio
        {
            get { throw new NotImplementedException(); }
        }
        public double currentBeat    // The current beat in the bar of the audio
        {
            get { throw new NotImplementedException(); }
        }

        public double beatsPrecise // Current beat in decimal form
        {
            get { return currentTime / millisecondsPerBeat; }
        }
        public double beatsInteger // Current beat in integer form
        {
            get { throw new NotImplementedException(); }
        }

        private double millisecondsPerBeat;


        public void SetCurrentTime(long _currentTime) // Milliseconds
        {
            currentTime = _currentTime;
        }

        // Sets the BPM, and also the secondsPerbeat
        public void SetBPM(double _BPM)
        {
            BPM = _BPM;
            millisecondsPerBeat = 60 / BPM * 1000;
        }

    }
}
