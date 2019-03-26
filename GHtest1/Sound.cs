using System;
using System.Threading;
using System.Diagnostics;
using System.IO;
using OpenTK.Audio.OpenAL;
using System.Collections.Generic;
using OpenTK.Audio;
using Un4seen.Bass;
using Un4seen.Bass.Misc;

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
        public static int hitNormal;
        public static int hitFinal;
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
            spAward = loadSound("star_awarded.wav", spAward);
            loseMult = loadSound("lose_multiplier.wav", loseMult);
            hitNormal = loadSound("hit2.mp3", hitNormal);
            hitFinal = loadSound("hit1.mp3", hitFinal);
        }
        public static void playSound(int ID) {
            AL.SourcePlay(ID);
            Console.WriteLine(ID);
        }
        public static int loadSound(string file, int id) {
            int channels, bits_per_sample = 16, sample_rate;
            byte[] sound_data;
            if (file.Contains(".mp3") || file.Contains(".wav")) {
                if (File.Exists("Content/Skins/" + Textures.skin + "/Sounds/" + file)) {
                    sound_data = LoadMp3("Content/Skins/" + Textures.skin + "/Sounds/" + file, out channels, out bits_per_sample, out sample_rate);
                } else if (File.Exists("Content/Skins/Default/Sounds/" + file)) {
                    sound_data = LoadMp3("Content/Skins/Default/Sounds/" + file, out channels, out bits_per_sample, out sample_rate);
                } else {
                    Console.WriteLine("mp3 file doesn't exist: " + file);
                    return id;
                }
                /*
                sample_rate = 44100;
                channels = 1;
                */
                //bits_per_sample = 16;
                Console.WriteLine(file + ", MP3 s:{0}, r:{1}, c:{2} d:{3}", bits_per_sample, sample_rate, channels, sound_data.Length);
            } else if (file.Contains(".wav")) {
                /*if (File.Exists("Content/Skins/" + Textures.skin + "/Sounds/" + file)) {
                    try {
                        sound_data = LoadWave(File.Open("Content/Skins/" + Textures.skin + "/Sounds/" + file, FileMode.Open), out channels, out bits_per_sample, out sample_rate);
                    } catch (Exception e) { Console.WriteLine(e + ", File: " + file); return id; }
                } else if (File.Exists("Content/Skins/Default/Sounds/" + file)) {
                    try {
                        sound_data = LoadWave(File.Open("Content/Skins/Default/Sounds/" + file, FileMode.Open), out channels, out bits_per_sample, out sample_rate);
                    } catch (Exception e) { Console.WriteLine(e + ", File: " + file); return id; }
                } else { Console.WriteLine("sound file doesn't exist: " + file); return id; }
                if (id != 0) {
                    AL.DeleteSource(id);
                }
                
            Console.WriteLine("WAV s:{0}, r:{1}, c:{2}, d:{3}", bits_per_sample, sample_rate, channels, sound_data.Length);*/
                return id;
            } else {
                return id;
            }
            /*for (int i = 2000; i < 2020; i++) {
                Console.WriteLine(sound_data[i]);
            }*/
            int buffer = AL.GenBuffer();
            int source = AL.GenSource();
            //bits_per_sample = 16;
            AL.BufferData(buffer, GetSoundFormat(channels, bits_per_sample), sound_data, sound_data.Length, sample_rate);

            AL.Source(source, ALSourcei.Buffer, buffer);
            //AL.SourcePlay(source);
            return source;
        }
        public static byte[] LoadMp3(string path, out int channels, out int bits, out int rate) {
            int stream = Bass.BASS_StreamCreateFile(path, 0, 0, BASSFlag.BASS_DEFAULT);
            Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, 1f);
            //while (Bass.BASS_ChannelIsActive(stream) == BASSActive.BASS_ACTIVE_PLAYING) ; 
            int length = (int)Bass.BASS_ChannelGetLength(stream);
            Bass.BASS_ChannelUpdate(stream, length);
            byte[] buffer = new byte[length];
            List<byte[]> chunks = new List<byte[]>();
            int pos = 0;
            while (pos < length) {
                Bass.BASS_ChannelSetPosition(stream, pos, BASSMode.BASS_POS_BYTE);
                Bass.BASS_ChannelUpdate(stream, length);
                int size = Bass.BASS_ChannelGetData(stream, buffer, length);
                byte[] chunk = new byte[size];
                for (int i = 0; i < chunk.Length; i++) {
                    if (i >= buffer.Length)
                        break;
                    chunk[i] = buffer[i];
                }
                chunks.Add(chunk);
                pos += size;
            }
            int bufferindex = 0;
            buffer = new byte[length];
            foreach (byte[] chunk in chunks) {
                for (int i = 0; i < chunk.Length; i++) {
                    if (bufferindex >= length)
                        break;
                    buffer[bufferindex] = chunk[i];
                    bufferindex++;
                }
            }
            BASS_CHANNELINFO info = new BASS_CHANNELINFO();
            Bass.BASS_ChannelGetInfo(stream, info);
            channels = info.chans;
            bits = info.sample;
            bits = info.origres;
            rate = info.freq;
            return buffer;
        }
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
