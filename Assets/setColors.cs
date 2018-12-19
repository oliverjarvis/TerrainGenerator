using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class setColors : MonoBehaviour {

	// Use this for initialization
	Texture2D gradient;
	Color[] color;
	public Gradient grad;
	GradientColorKey[] colorKey;
	GradientAlphaKey[] alphaKey;

	void Start () {
		gradient = new Texture2D (320, 28);

		GameObject cs1 = GameObject.Find("colorTab1");
		GameObject cs2 = GameObject.Find("colorTab2");
		GameObject cs3 = GameObject.Find("colorTab3");
		GameObject[] sliders = new GameObject[3]{cs1, cs2, cs3};

		iterateSliders (ref sliders);
		setTexture ();
	}
	void setTexture(){
		GameObject img = GameObject.Find("gradient");
		Sprite spr = Sprite.Create(gradient, new Rect(0,0,315, 28), new Vector2(0.5f, 0.5f));
		img.GetComponent<Image> ().sprite = spr;

		int y = 0;
		while (y<gradient.height) {
			int x = 0;
			while( x < gradient.width){
				gradient.SetPixel(x, y, grad.Evaluate(x/320.0f));
				++x;
			}
			++y;
		}
		
		gradient.Apply();

	}

	public void setGradient(float val){
		gradient = new Texture2D (320, 28);
		
		GameObject cs1 = GameObject.Find("colorTab1");
		GameObject cs2 = GameObject.Find("colorTab2");
		GameObject cs3 = GameObject.Find("colorTab3");
		GameObject[] sliders = new GameObject[3]{cs1, cs2, cs3};
		
		iterateSliders (ref sliders);
		setTexture ();
	}

	void iterateSliders (ref GameObject[] sliders){

		/*Color a1 = new Color(0.45882f, 0.68235f, 0.13725f);
		Color a2 = new Color(0.78039f, 0.49019f, 0.31372f);
		Color a3 = new Color(0.74117f, 0.76471f, 0.78039f);*/

		Color a1 = new Color(0.90196f, 0.65098f, 0.26666f);
		Color a2 = new Color(0.88235f, 0.51372f, 0.22353f);
		Color a3 = new Color(0.71372f, 0.34117f, 0.11372f);

		grad = new Gradient();

		Color[] col = new Color[3]{a1, a2, a3};
		colorKey = new GradientColorKey[col.Length];
		alphaKey = new GradientAlphaKey[2];
		for (int i = 0; i<col.Length; i++) {
			colorKey[i].color = col[i];
			colorKey[i].time = sliders[i].GetComponent<Slider>().value;
		}
		alphaKey [0].alpha = 1.0f;
		alphaKey [0].time = 0.0f;
		alphaKey [1].alpha = 1.0f;
		alphaKey [1].time = 1.0f;

		grad.SetKeys (colorKey, alphaKey);
		Debug.Log (grad.Evaluate (0.25f));
	}

	// Update is called once per frame
	void Update () {
	
	}
}
