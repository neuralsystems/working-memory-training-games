﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Sinus : MonoBehaviour {

	// un-optimized version
	public float frequency = 440;
	public double gain = 0.5;

	private double increment;
	private double phase;
	//private double sampling_frequency = 48000;
	private double sampling_frequency ;

	// Use this for initialization
	void Start () {
		sampling_frequency = AudioSettings.outputSampleRate;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

// 	see full documentation of this function at the unity documentations
//	https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnAudioFilterRead.html
	void OnAudioFilterRead(float[] data, int channels)
	{
		// update increment in case frequency has changed
		increment = frequency * 2 * Math.PI / sampling_frequency;
		for (var i = 0; i < data.Length; i = i + channels)
		{
			phase = phase + increment;
			// this is where we copy audio data to make them “available” to Unity
			data[i] = (float)(gain*Math.Sin(phase));
			// if we have stereo, we copy the mono data to each channel
			if (channels == 2) data[i + 1] = data[i];
			if (phase > 2 * Math.PI) phase = 0;
		}
	}
}
