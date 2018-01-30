using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using CSharpSynth.Effects;
using CSharpSynth.Sequencer;
using CSharpSynth.Synthesis;
using CSharpSynth.Midi;

[RequireComponent(typeof(AudioSource))]
public class MIDIPlayer : MonoBehaviour
{
    //Public
    //Check the Midi's file folder for different songs
    private string midiFilePath;
    public bool ShouldPlayFile = true;

    //Try also: "FM Bank/fm" or "Analog Bank/analog" for some different sounds
    private string bankFilePath = "GM Bank/gm";
    private int bufferSize = 1024;
    //public int midiNote = 60;
    //public int midiNoteVolume = 100;
    //[Range(0, 127)] //From Piano to Gunshot
    //public int midiInstrument = 40;
    //Private 
    private float[] sampleBuffer;
    private float gain = 1f;
    private MidiSequencer midiSequencer;
    private StreamSynthesizer midiStreamSynthesizer;


    //private float sliderValue = 1.0f;
    //private float maxSliderValue = 127.0f;
    private bool isPaused = false;
    public Text txtPath;
    public TimeSpan stopTime;

    // Awake is called when the script instance
    // is being loaded.
    void Awake()
    {
        midiFilePath = Application.persistentDataPath + "/Midis/" + ButtonPrefab.nameOfSong + ".txt";
        txtPath.text = midiFilePath;
        midiStreamSynthesizer = new StreamSynthesizer(44100, 1, bufferSize, 40);

        sampleBuffer = new float[midiStreamSynthesizer.BufferSize];

        midiStreamSynthesizer.LoadBank(bankFilePath);

        midiSequencer = new MidiSequencer(midiStreamSynthesizer);
        //These will be fired by the midiSequencer when a song plays. Check the console for messages if you uncomment these
        //midiSequencer.NoteOnEvent += new MidiSequencer.NoteOnEventHandler(MidiNoteOnHandler);
        //midiSequencer.NoteOffEvent += new MidiSequencer.NoteOffEventHandler (MidiNoteOffHandler);			
    }

    void LoadSong(string midiPath)
    {
        midiSequencer.LoadMidi(midiPath, false);
        midiSequencer.Play();
    }

    // Start is called just before any of the
    // Update methods is called the first time.
    void Start()
    {
    }

    // Update is called every frame, if the
    // MonoBehaviour is enabled.
    void Update()
    {

        //if (!midiSequencer.isPlaying)
        //{
        //    //if (!GetComponent<AudioSource>().isPlaying)
        //    if (ShouldPlayFile)
        //    {
        //        LoadSong(midiFilePath);
        //    }
        //}
        //else if (!ShouldPlayFile)
        //{
        //    midiSequencer.Stop(true);
        //}


    }

    // See http://unity3d.com/support/documentation/ScriptReference/MonoBehaviour.OnAudioFilterRead.html for reference code
    //	If OnAudioFilterRead is implemented, Unity will insert a custom filter into the audio DSP chain.
    //
    //	The filter is inserted in the same order as the MonoBehaviour script is shown in the inspector. 	
    //	OnAudioFilterRead is called everytime a chunk of audio is routed thru the filter (this happens frequently, every ~20ms depending on the samplerate and platform). 
    //	The audio data is an array of floats ranging from [-1.0f;1.0f] and contains audio from the previous filter in the chain or the AudioClip on the AudioSource. 
    //	If this is the first filter in the chain and a clip isn't attached to the audio source this filter will be 'played'. 
    //	That way you can use the filter as the audio clip, procedurally generating audio.
    //
    //	If OnAudioFilterRead is implemented a VU meter will show up in the inspector showing the outgoing samples level. 
    //	The process time of the filter is also measured and the spent milliseconds will show up next to the VU Meter 
    //	(it turns red if the filter is taking up too much time, so the mixer will starv audio data). 
    //	Also note, that OnAudioFilterRead is called on a different thread from the main thread (namely the audio thread) 
    //	so calling into many Unity functions from this function is not allowed ( a warning will show up ). 	
    private void OnAudioFilterRead(float[] data, int channels)
    {
        //This uses the Unity specific float method we added to get the buffer
        midiStreamSynthesizer.GetNext(sampleBuffer);
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = sampleBuffer[i] * gain;
        }
    }

    public void MidiNoteOnHandler(int channel, int note, int velocity)
    {
        Debug.Log("NoteOn: " + note.ToString() + " Velocity: " + velocity.ToString());

    }

    public void MidiNoteOffHandler(int channel, int note)
    {
        Debug.Log("NoteOff: " + note.ToString());
    }

    public void PlaySong()
    {
        if (!isPaused)
        {
            LoadSong(midiFilePath);
            Debug.Log("Load Song");
        }
        else
        {
            midiSequencer.Time = stopTime;
            midiSequencer.Play();
            Debug.Log("Play Song");
            isPaused = false;
        }
        
    }

    public void PauseSong()
    {
        isPaused = true;
        stopTime = new TimeSpan(0, midiSequencer.Time.Minutes, midiSequencer.Time.Seconds);
        Debug.Log("Min: " + stopTime.Minutes + " Sec: " + stopTime.Seconds);
        midiSequencer.Stop(false);
    }
}
