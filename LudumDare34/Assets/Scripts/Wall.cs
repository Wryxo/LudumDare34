using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	public float speed;
    public bool twoWay;

    public Sprite one;
    public Sprite zero;

	// Use this for initialization
	void Start () {
		
	}
	
    public void Good(int index)
    {
        var tmp = transform.Find("Center/" + index);
        tmp.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
    }

    public void Good2Way(bool side, int index)
    {
        if (side) { 
            var tmp = transform.Find("Left/" + index);
            tmp.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
        } else
        {
            var tmp = transform.Find("Right/" + index);
            tmp.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
        }
    }

    public void ResetColor()
    {
        Transform tmp;
        tmp = transform.Find("Center/0");
        tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        tmp = transform.Find("Center/1");
        tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        tmp = transform.Find("Center/2");
        tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        tmp = transform.Find("Center/3");
        tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        tmp = transform.Find("Center/4");
        tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        tmp = transform.Find("Center/5");
        tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
    }

    public void ResetColor(bool side)
    {
        Transform tmp;
        if (side) { 
            tmp = transform.Find("Left/0");
            tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            tmp = transform.Find("Left/1");
            tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            tmp = transform.Find("Left/2");
            tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            tmp = transform.Find("Left/3");
            tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            tmp = transform.Find("Left/4");
            tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            tmp = transform.Find("Left/5");
            tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        } else { 
            tmp = transform.Find("Right/0");
            tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            tmp = transform.Find("Right/1");
            tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            tmp = transform.Find("Right/2");
            tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            tmp = transform.Find("Right/3");
            tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            tmp = transform.Find("Right/4");
            tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            tmp = transform.Find("Right/5");
            tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        }
    }

    public void SetNumber(bool[] num)
    {
        Debug.Log(num[0] + " " + num[1] + " " + num[2] + " " + num[3] + " " + num[4] + " " + num[5] + " ");
        var tmp = transform.Find("Center/0");
        tmp.GetComponent<SpriteRenderer>().sprite = num[0] ? one : zero;
        tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        tmp = transform.Find("Center/1");
        tmp.GetComponent<SpriteRenderer>().sprite = num[1] ? one : zero;
        tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        tmp = transform.Find("Center/2");
        tmp.GetComponent<SpriteRenderer>().sprite = num[2] ? one : zero;
        tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        tmp = transform.Find("Center/3");
        tmp.GetComponent<SpriteRenderer>().sprite = num[3] ? one : zero;
        tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        tmp = transform.Find("Center/4");
        tmp.GetComponent<SpriteRenderer>().sprite = num[4] ? one : zero;
        tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        tmp = transform.Find("Center/5");
        tmp.GetComponent<SpriteRenderer>().sprite = num[5] ? one : zero;
        tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
    }

    public void SetNumber2(bool[] left, bool[] right)
    {
        var tmp = transform.Find("Left/0");
        tmp.GetComponent<SpriteRenderer>().sprite = left[0] ? one : zero;
        tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        tmp = transform.Find("Left/1");
        tmp.GetComponent<SpriteRenderer>().sprite = left[1] ? one : zero;
        tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        tmp = transform.Find("Left/2");
        tmp.GetComponent<SpriteRenderer>().sprite = left[2] ? one : zero;
        tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        tmp = transform.Find("Left/3");
        tmp.GetComponent<SpriteRenderer>().sprite = left[3] ? one : zero;
        tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        tmp = transform.Find("Left/4");
        tmp.GetComponent<SpriteRenderer>().sprite = left[4] ? one : zero;
        tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        tmp = transform.Find("Left/5");
        tmp.GetComponent<SpriteRenderer>().sprite = left[5] ? one : zero;
        tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);

        tmp = transform.Find("Right/0");
        tmp.GetComponent<SpriteRenderer>().sprite = right[0] ? one : zero;
        tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        tmp = transform.Find("Right/1");
        tmp.GetComponent<SpriteRenderer>().sprite = right[1] ? one : zero;
        tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        tmp = transform.Find("Right/2");
        tmp.GetComponent<SpriteRenderer>().sprite = right[2] ? one : zero;
        tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        tmp = transform.Find("Right/3");
        tmp.GetComponent<SpriteRenderer>().sprite = right[3] ? one : zero;
        tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        tmp = transform.Find("Right/4");
        tmp.GetComponent<SpriteRenderer>().sprite = right[4] ? one : zero;
        tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        tmp = transform.Find("Right/5");
        tmp.GetComponent<SpriteRenderer>().sprite = right[5] ? one : zero;
        tmp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
    }

    // Update is called once per frame
    void FixedUpdate () {
		transform.position = transform.position + new Vector3(0, -speed, 0);
	}
}
