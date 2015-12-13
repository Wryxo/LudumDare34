using UnityEngine;
using System.Collections;

public class DJScript : MonoBehaviour {

    public AudioClip[] MusicClips;

    private AudioSource music;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
        music = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
	    if (!music.isPlaying)
        {
            music.clip = MusicClips[Random.Range(0, MusicClips.Length)];
            music.Play();
        }
	}

    public void ToggleMute()
    {
        music.mute = !music.mute;
    }
}
