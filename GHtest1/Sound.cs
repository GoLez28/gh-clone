using System;
using System.Threading;
using System.Diagnostics;
using System.IO;
using OpenTK.Audio.OpenAL;
using OpenTK.Audio;

namespace GHtest1 {
    class Sound {
        public static int[] badnote = new int[5] { 0, 0, 0, 0, 0 };
        public static int fail;
        public static int rewind;
        public static int ripple;
        public static int spActivate;
        public static int spAvailable;
        public static int spRelease;
        public static int spAward;
        public static int loseMult;
        public static void Load() {
            badnote[0] = loadSound("bad_note1.wav", badnote[0]);
            badnote[1] = loadSound("bad_note2.wav", badnote[1]);
            badnote[2] = loadSound("bad_note3.wav", badnote[2]);
            badnote[3] = loadSound("bad_note4.wav", badnote[3]);
            badnote[4] = loadSound("bad_note5.wav", badnote[4]);
            fail = loadSound("song_fail.wav", fail);
            rewind = loadSound("rewind_highway.wav", rewind);
            ripple = loadSound("notes_ripple_up.wav", ripple);
            spActivate = loadSound("star_deployed.wav", spActivate);
            spAvailable = loadSound("star_available.wav", spAvailable);
            spRelease = loadSound("star_release.wav", spRelease);
            spAward = loadSound("tar_awarded.wav", spAward);
            loseMult = loadSound("lose_multiplier.wav", loseMult);
        }
        public static void playSound(int ID) {
            AL.SourcePlay(ID);
            Console.WriteLine(ID);
        }
        public static int loadSound(string file, int id) {
            int channels, bits_per_sample, sample_rate;
            byte[] sound_data;
            if (File.Exists("Content/Skins/" + Textures.skin + "/Sounds/" + file)) {
                try {
                    sound_data = LoadWave(File.Open("Content/Skins/" + Textures.skin + "/Sounds/" + file, FileMode.Open), out channels, out bits_per_sample, out sample_rate);
                } catch (Exception e) { Console.WriteLine(e + ", File: " + file); return id; }
            } else if (File.Exists("Content/Skins/Default/Sounds/" + file)) {
                try {
                    sound_data = LoadWave(File.Open("Content/Skins/Default/Sounds/" + file, FileMode.Open), out channels, out bits_per_sample, out sample_rate);
                } catch (Exception e) { Console.WriteLine(e + ", File: " + file); return id; }
            } else { Console.WriteLine("sound file doesn't exist: " + file);  return id; }
            if (id != 0) {
                AL.DeleteSource(id);
            }
            int buffer = AL.GenBuffer();
            int source = AL.GenSource();
            AL.BufferData(buffer, GetSoundFormat(channels, bits_per_sample), sound_data, sound_data.Length, sample_rate);

            AL.Source(source, ALSourcei.Buffer, buffer);
            //AL.SourcePlay(source);
            return source;
        }

        // Loads a wave/riff audio file.
        public static byte[] LoadWave(Stream stream, out int channels, out int bits, out int rate) {
            if (stream == null)
                throw new ArgumentNullException("stream");

            using (BinaryReader reader = new BinaryReader(stream)) {
                // RIFF header
                string signature = new string(reader.ReadChars(4));
                if (signature != "RIFF")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                int riff_chunck_size = reader.ReadInt32();

                string format = new string(reader.ReadChars(4));
                if (format != "WAVE")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                // WAVE header
                string format_signature = new string(reader.ReadChars(4));
                if (format_signature != "fmt ")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int format_chunk_size = reader.ReadInt32();
                int audio_format = reader.ReadInt16();
                int num_channels = reader.ReadInt16();
                int sample_rate = reader.ReadInt32();
                int byte_rate = reader.ReadInt32();
                int block_align = reader.ReadInt16();
                int bits_per_sample = reader.ReadInt16();
                string dummy = new string(reader.ReadChars(2));
                string data_signature = new string(reader.ReadChars(4));
                if (data_signature != "data") {
                    Console.WriteLine(data_signature);
                    throw new NotSupportedException("Specified wave file is not supported.");
                }

                int data_chunk_size = reader.ReadInt32();

                channels = num_channels;
                bits = bits_per_sample;
                rate = sample_rate;

                return reader.ReadBytes((int)reader.BaseStream.Length);
            }
        }

        public static ALFormat GetSoundFormat(int channels, int bits) {
            switch (channels) {
                case 1: return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }
    }
}
