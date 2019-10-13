﻿using System;
using System.Collections.Generic;
using System.IO;
using Aseprite;
using Lens.entity;
using Lens.util;
using Lens.util.camera;
using Lens.util.file;
using Lens.util.tween;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Lens.assets {
	public class Audio {
		private const float CrossFadeTime = 0.5f;
		
		private static bool repeat;
		
		public static bool Repeat {
			get => repeat;
			set {
				if (currentPlaying != null) {
					currentPlaying.Repeat = value;
				}

				repeat = value;
			}
		}

		private static Music currentPlaying;
		private static string currentPlayingMusic;
		private static Dictionary<string, SoundEffectInstance> soundInstances = new Dictionary<string, SoundEffectInstance>();
		private static Dictionary<string, Music> musicInstances = new Dictionary<string, Music>();

		private static Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

		private static void LoadSfx(FileHandle file) {
			if (file.Exists()) {
				foreach (var sfx in file.ListFileHandles()) {
					if (sfx.Extension == ".xnb") {
						LoadSfx(sfx.NameWithoutExtension);
					}
				}

				foreach (var dir in file.ListDirectoryHandles()) {
					LoadSfx(dir);
				}
			}
		}
		
		internal static void Load() {
			LoadSfx(FileHandle.FromNearRoot("bin/Sfx/"));
		}

		private static void LoadSfx(string sfx) {
			sfx = Path.GetFileNameWithoutExtension(sfx);
			sounds[sfx] = Assets.Content.Load<SoundEffect>($"bin/Sfx/{sfx}");				
		}
		
		internal static void Destroy() {
			foreach (var sound in sounds.Values) {
				sound.Dispose();
			}
		}

		public static void PlaySfx(string id, float volume = 1, float pitch = 0, float pan = 0) {
			PlaySfx(GetSfx(id), volume, pitch, pan);
		}

		public static SoundEffect GetSfx(string id) {
			SoundEffect effect;

			if (sounds.TryGetValue(id, out effect)) {
				return effect;
			}

			Log.Error($"Sound effect {id} was not found!");
			return null;
		}

		public static void PlaySfx(SoundEffect sfx, float volume = 1, float pitch = 0, float pan = 0) {
			if (!Assets.LoadAudio) {
				return;
			}
			
			sfx.Play(volume, pitch, pan);
		}
		
		public static void PlayMusic(string music, AudioListener listener = null, AudioEmitter emitter = null) {
			if (!Assets.LoadAudio) {
				return;
			}
			
			if (currentPlayingMusic == music) {
				return;
			}

			FadeOut();

			Repeat = true;
			
			if (!musicInstances.TryGetValue(music, out currentPlaying)) {
				currentPlaying = new Music(Assets.Content.Load<AudioFile>($"bin/Music/{music}"));
				musicInstances[music] = currentPlaying;
				currentPlaying.PlayFromStart();
			} else {
				currentPlaying.Paused = false;
			}

			currentPlayingMusic = music;
			currentPlaying.Volume = 0;
			currentPlaying.Repeat = repeat;

			var m = currentPlaying;
			Tween.To(musicVolume, m.Volume, x => m.Volume = x, CrossFadeTime);
		}

		public static void FadeOut() {
			if (currentPlaying != null) {
				var m = currentPlaying;
				
				Tween.To(0, m.Volume, x => m.Volume = x, CrossFadeTime).OnEnd = () => {
					m.Paused = true;
				};
				
				currentPlaying = null;
				currentPlayingMusic = null;
			}
		}
		
		public static void Stop() {
			if (currentPlaying != null) {
				currentPlaying.Stop();
				currentPlaying = null;
				currentPlayingMusic = null;
			}
		}

		private static float musicVolume = 1;

		public static void UpdateMusicVolume(float value) {
			if (currentPlaying != null) {
				currentPlaying.Volume = value;
			}

			musicVolume = value;
		}
		
		public static void Update() {
			currentPlaying?.Update();
		}
	}
}