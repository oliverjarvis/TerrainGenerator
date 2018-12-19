using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class generatePerlin : MonoBehaviour {

	private static int noiseWidth = 128;
	private static int noiseHeight = 128;

	public float[,] noise = new float[noiseWidth,noiseHeight];
	
	public Texture2D texture;

	// Use this for initialization
	void Start () {
	/*	generateNoise ();
		texture = new Texture2D (128, 128);
		GameObject img = GameObject.Find("Image");
		Sprite spr = Sprite.Create(texture, new Rect(0,0,noiseWidth, noiseHeight), new Vector2(0.5f, 0.5f));
		img.GetComponent<Image> ().sprite = spr;
		int y = 0;
		while (y<texture.height) {
			int x = 0;
			while( x < texture.width){
				Color color = new Color(25600.0f * noise[x,y], 25600.0f * noise[x,y], 25600.0f * noise[x,y]);
				texture.SetPixel(x, y, color);
				++x;
			}
			++y;
		}

		texture.Apply();*/

	}

	void generateNoise(){
		for (int x = 0; x<noiseWidth; x++) {
			for(int y = 0; y <noiseHeight; y++){
				float ran = Random.Range(0.0f, 129.0f);
				noise[x,y]= (ran%32768)/32768.0f;
			}
		}
	}

	public Texture2D currentNoise{
		get{ return texture;}
	}

	public void giveMessage(){

		Debug.LogWarning ("generatePerlin");
		generateNoise ();
		texture = new Texture2D (128, 128);
		GameObject img = GameObject.Find("Image");
		Sprite spr = Sprite.Create(texture, new Rect(0,0,noiseWidth, noiseHeight), new Vector2(0.5f, 0.5f));
		img.GetComponent<Image> ().sprite = spr;
		int y = 0;
		while (y<texture.height) {
			int x = 0;
			while( x < texture.width){
				Color color = new Color(turbulence(x,y,16), turbulence(x,y,16), turbulence(x,y,16));
				texture.SetPixel(x, y, color);
				++x;
			}
			++y;
		}
		
		texture.Apply();

	}



	public float smoothNoise(float x, float y)
	{
		float fractX = x - (int)x;
		float fractY = y - (int)y;

		int x1 = ((int)x + noiseWidth) % noiseWidth;
		int y1 = ((int)y + noiseHeight)%noiseHeight;

		int x2 = (x1 + noiseWidth - 1)%noiseWidth;
		int y2 = (y1 + noiseHeight -1)%noiseHeight;

		float value = 0.0f;
		value += fractX * fractY * noise[x1,y1];
		value += fractX * (1-fractY)*noise[x1, y2];
		value += (1-fractX) * fractY * noise[x2,y1];
		value += (1-fractX) * (1-fractY)*noise[x2, y2];
		return value;
	}

	public float turbulence(float x, float y, float size)
	{
		float value = 0.0f;
		float initialSize = size;
		while (size>=1) {
			value += smoothNoise(x/size, y/size)*size;
			size /= 2.0f;
		}
		return(128.0f * value / initialSize);
	}
	// Update is called once per frame
	void Update () {

	}
}
