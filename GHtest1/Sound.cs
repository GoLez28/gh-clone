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
        public static int clickMenu;
        public static int applause;
        public static float fxVolume = 1;
        public static float maniaVolume = 1;
        public static void setVolume() {
            for (int i = 0; i < 5; i++)
                AL.Source(badnote[i], ALSourcef.Gain, Audio.masterVolume * fxVolume);
            AL.Source(fail, ALSourcef.Gain, Audio.masterVolume * fxVolume);
            AL.Source(rewind, ALSourcef.Gain, Audio.masterVolume * fxVolume);
            AL.Source(ripple, ALSourcef.Gain, Audio.masterVolume * fxVolume);
            AL.Source(spActivate, ALSourcef.Gain, Audio.masterVolume * fxVolume);
            AL.Source(spAvailable, ALSourcef.Gain, Audio.masterVolume * fxVolume);
            AL.Source(spRelease, ALSourcef.Gain, Audio.masterVolume * fxVolume);
            AL.Source(spAward, ALSourcef.Gain, Audio.masterVolume * fxVolume);
            AL.Source(loseMult, ALSourcef.Gain, Audio.masterVolume * fxVolume);
            AL.Source(clickMenu, ALSourcef.Gain, Audio.masterVolume * fxVolume);
            AL.Source(applause, ALSourcef.Gain, Audio.masterVolume * fxVolume);
            AL.Source(hitNormal, ALSourcef.Gain, Audio.masterVolume * maniaVolume);
            AL.Source(hitFinal, ALSourcef.Gain, Audio.masterVolume * maniaVolume);
        }
        public static void Load() {
            badnote[0] = loadSound("bad_note1", badnote[0]);
            badnote[1] = loadSound("bad_note2", badnote[1]);
            badnote[2] = loadSound("bad_note3", badnote[2]);
            badnote[3] = loadSound("bad_note4", badnote[3]);
            badnote[4] = loadSound("bad_note5", badnote[4]);
            fail = loadSound("song_fail", fail);
            rewind = loadSound("rewind_highway", rewind);
            ripple = loadSound("notes_ripple_up", ripple);
            spActivate = loadSound("star_deployed", spActivate);
            spAvailable = loadSound("star_available", spAvailable);
            spRelease = loadSound("star_release", spRelease);
            spAward = loadSound("star_awarded", spAward);
            loseMult = loadSound("lose_multiplier", loseMult);
            hitNormal = loadSound("hit2", hitNormal);
            hitFinal = loadSound("hit1", hitFinal);
            clickMenu = loadSound("click-short", clickMenu);
            applause = loadSound("applause", applause);
            setVolume();
        }
        public static void playSound(int ID) {
            AL.SourcePlay(ID);
            Console.WriteLine(ID);
        }
        public static int loadSound(string file, int id) {
            int channels = 2, bits_per_sample = 16, sample_rate = 44100;
            byte[] sound_data = new byte[0];
            string path = "Content/Skins/" + Textures.skin + "/Sounds/" + file + ".wav";
            if (!File.Exists(path)) {
                path = "Content/Skins/Default/Sounds/" + file + ".wav";
                if (!File.Exists(path)) {
                    path = "Content/Skins/" + Textures.skin + "/Sounds/" + file + ".ogg";
                    if (!File.Exists(path)) {
                        path = "Content/Skins/Default/Sounds/" + file + ".ogg";
                        if (!File.Exists(path)) {
                            path = "Content/Skins/" + Textures.skin + "/Sounds/" + file + ".mp3";
                            if (!File.Exists(path)) {
                                path = "Content/Skins/Default/Sounds/" + file + ".mp3";
                                if (!File.Exists(path)) {
                                    Console.WriteLine("file does not exist!: " + file);
                                    return id;
                                }
                            }
                        }
                    }
                }
            }
            try {
                sound_data = LoadMp3(path, out channels, out bits_per_sample, out sample_rate);
                Console.WriteLine(file + ", Sound s:{0}, r:{1}, c:{2} d:{3}", bits_per_sample, sample_rate, channels, sound_data.Length);
            } catch {
                Console.WriteLine("Something bad happens reading " + file);
            }
            int buffer = AL.GenBuffer();
            int source = AL.GenSource();
            AL.BufferData(buffer, GetSoundFormat(channels, bits_per_sample), sound_data, sound_data.Length, sample_rate);
            AL.Source(source, ALSourcei.Buffer, buffer);
            return source;
        }
        public static byte[] LoadMp3(string path, out int channels, out int bits, out int rate) {
            int stream = Bass.BASS_StreamCreateFile(path, 0, 0, BASSFlag.BASS_STREAM_DECODE);
            Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, 1f);
            int length = (int)Bass.BASS_ChannelGetLength(stream);
            //Bass.BASS_ChannelUpdate(stream, length);
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
        public static ALFormat GetSoundFormat(int channels, int bits) {
            switch (channels) {
                case 1: return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }
    }
}
