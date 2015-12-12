using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathGenerator : MonoBehaviour {

	public string Story;
	public int storyIndex;

	private Dictionary<string, Pair> chars;

	// Use this for initialization
	void Awake () {
        chars = new Dictionary<string, Pair>();
		InitChars();
        Story = Story.ToLower();
        storyIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {

	}

    public Pair GenerateNext()
    {
        if (storyIndex >= Story.Length)
        {
            return null;
        }
        string c = " ";
        while (c == " " || !chars.ContainsKey(c)) { 
            c = Story[storyIndex].ToString();
            storyIndex++;
        }
        return chars[c];
    }

	void InitChars() {
        string[] tmp = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", ".", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "#" };
        for (int i = 0; i< 26; i++) {
            chars.Add(tmp[i], new Pair(i, i+37));
		}
        chars.Add(".", new Pair(26, 63));
        for (int i = 27; i < 37; i++)
        {
            chars.Add(tmp[i], new Pair(i, i));
        }
    }
}

public class Pair {
	public int l;
	public int u;

	public Pair(int lower, int upper){
		l = lower;
		u = upper;
	}
}
