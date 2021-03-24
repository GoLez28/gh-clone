using System;
using System.Threading;
using System.Diagnostics;
using System.IO;
using OpenTK.Audio.OpenAL;
using System.Collections.Generic;
using OpenTK.Audio;
using Un4seen.Bass;
using Un4seen.Bass.Misc;

namespace Upbeat {
    class Sound {
        public static List<int> maniaSounds = new List<int>();
        public static List<string> maniaSoundsDir = new List<string>();
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
            if (Config.al) {
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
                for (int i = 0; i < maniaSounds.Count; i++) {
                    AL.Source(maniaSounds[i], ALSourcef.Gain, Audio.masterVolume * maniaVolume);
                }
            } else {
                for (int i = 0; i < 5; i++)
                    Bass.BASS_ChannelSetAttribute(badnote[i], BASSAttribute.BASS_ATTRIB_VOL, Audio.masterVolume * fxVolume);
                Bass.BASS_ChannelSetAttribute(fail, BASSAttribute.BASS_ATTRIB_VOL, Audio.masterVolume * fxVolume);
                Bass.BASS_ChannelSetAttribute(rewind, BASSAttribute.BASS_ATTRIB_VOL, Audio.masterVolume * fxVolume);
                Bass.BASS_ChannelSetAttribute(ripple, BASSAttribute.BASS_ATTRIB_VOL, Audio.masterVolume * fxVolume);
                Bass.BASS_ChannelSetAttribute(spActivate, BASSAttribute.BASS_ATTRIB_VOL, Audio.masterVolume * fxVolume);
                Bass.BASS_ChannelSetAttribute(spAvailable, BASSAttribute.BASS_ATTRIB_VOL, Audio.masterVolume * fxVolume);
                Bass.BASS_ChannelSetAttribute(spRelease, BASSAttribute.BASS_ATTRIB_VOL, Audio.masterVolume * fxVolume);
                Bass.BASS_ChannelSetAttribute(spAward, BASSAttribute.BASS_ATTRIB_VOL, Audio.masterVolume * fxVolume);
                Bass.BASS_ChannelSetAttribute(loseMult, BASSAttribute.BASS_ATTRIB_VOL, Audio.masterVolume * fxVolume);
                Bass.BASS_ChannelSetAttribute(clickMenu, BASSAttribute.BASS_ATTRIB_VOL, Audio.masterVolume * fxVolume);
                Bass.BASS_ChannelSetAttribute(applause, BASSAttribute.BASS_ATTRIB_VOL, Audio.masterVolume * fxVolume);
                Bass.BASS_ChannelSetAttribute(hitNormal, BASSAttribute.BASS_ATTRIB_VOL, Audio.masterVolume * maniaVolume);
                Bass.BASS_ChannelSetAttribute(hitFinal, BASSAttribute.BASS_ATTRIB_VOL, Audio.masterVolume * maniaVolume);
                for (int i = 0; i < maniaSounds.Count; i++) {
                    Bass.BASS_ChannelSetAttribute(maniaSounds[i], BASSAttribute.BASS_ATTRIB_VOL, Audio.masterVolume * maniaVolume);
                }
            }
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
        public static void FreeManiaSounds () {
            if (!Config.al) {
                Bass.BASS_StreamFree(badnote[0]);
                for (int i = 0; i < maniaSounds.Count; i++) {
                    Bass.BASS_StreamFree(maniaSounds[i]);
                }
            } else {
                //How do i remove?
            }
            maniaSounds.Clear();
            maniaSoundsDir.Clear();
        }
        public static void ChangeEngine() {
            Config.al = !Config.al;
            if (!Config.al) {
                Bass.BASS_StreamFree(badnote[0]);
                Bass.BASS_StreamFree(badnote[1]);
                Bass.BASS_StreamFree(badnote[2]);
                Bass.BASS_StreamFree(badnote[3]);
                Bass.BASS_StreamFree(badnote[4]);
                Bass.BASS_StreamFree(fail);
                Bass.BASS_StreamFree(rewind);
                Bass.BASS_StreamFree(ripple);
                Bass.BASS_StreamFree(spActivate);
                Bass.BASS_StreamFree(spAvailable);
                Bass.BASS_StreamFree(spRelease);
                Bass.BASS_StreamFree(spAward);
                Bass.BASS_StreamFree(loseMult);
                Bass.BASS_StreamFree(hitNormal);
                Bass.BASS_StreamFree(hitFinal);
                Bass.BASS_StreamFree(clickMenu);
                Bass.BASS_StreamFree(applause);
            } else {
                //How do i remove?
            }
            Load();
        }
        public static void playSound(int ID) {
            if (Config.al) {
                AL.SourceStop(ID);
                //AL.Source(fail, ALSourcef.SecOffset, 0);
                AL.SourcePlay(ID);
            } else {
                Bass.BASS_ChannelSetPosition(ID, 0,
                                             BASSMode.BASS_POS_BYTE);
                Bass.BASS_ChannelPlay(ID, false);
            }
        }
        public static int loadSound(string file, int id, bool rawDir = false) {
            string path = "Content/Skins/" + Textures.skin + "/Sounds/" + file + ".wav";
            if (rawDir) {
                path = file;
            } else {
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
            }
            if (Config.al) {
                int channels = 2, bits_per_sample = 16, sample_rate = 44100;
                byte[] sound_data = new byte[0];

                try {
                    sound_data = LoadMp3(path, out channels, out bits_per_sample, out sample_rate);
                } catch {
                    Console.WriteLine("Something bad happened reading " + file);
                }
                int buffer = AL.GenBuffer();
                int source = AL.GenSource();
                AL.BufferData(buffer, GetSoundFormat(channels, bits_per_sample), sound_data, sound_data.Length, sample_rate);
                AL.Source(source, ALSourcei.Buffer, buffer);
                return source;
            } else {
                return Bass.BASS_StreamCreateFile(path, 0, 0, BASSFlag.BASS_DEFAULT);
            }
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
