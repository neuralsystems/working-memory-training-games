using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager: MonoBehaviour
{
	public List<AudioClip> HAPPY_SOUNDS_VOICES;
	public List<AudioClip> SAD_SOUND_VOICES;
	public List<AudioClip> NEUTRAL_SOUND_VOICES;
	public List<AudioClip> Instruction_SOUND_VOICES;

	public float PlayHappySound(){
		return RandomSound (HAPPY_SOUNDS_VOICES);
	}

	public float PlaySadSound(){
		return RandomSound (SAD_SOUND_VOICES);
	}

	public float PlayNeutralSound(){
		return RandomSound (NEUTRAL_SOUND_VOICES);
	}
	public float RandomSound(List<AudioClip>  audiolist){
		var x = audiolist[Random.Range(0, audiolist.Count)];
		GetComponent<AudioSource> ().PlayOneShot (x);
		return x.length;
	}

	public float PlaySound(AudioClip clipToPlay){
		GetComponent<AudioSource> ().PlayOneShot (clipToPlay);
		return clipToPlay.length;
	}

	public float PlayInstructionSound(int instruction_code){
		if (instruction_code < Instruction_SOUND_VOICES.Count) {
			return PlaySound (Instruction_SOUND_VOICES [instruction_code]);
		}
		return 0f;
	}

}
