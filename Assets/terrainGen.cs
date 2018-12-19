using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class terrainGen : MonoBehaviour {

	//TODO 5, 6, 7, 8, 9, 10, 11, 12, 14, 15, 18, 21, 25, 31, 42
	//TODO fixing perlinToMesh() to no longer zoom. Fix by rounding all numbers
	public Texture2D texture;

	WWW www;

	private Mesh mesh;
	private Vector3[] vertices;
	private int vertexCount = 0;
	private Vector3[] normales;
	private Vector2[] uvs;
	private int[] triangles;
	private Color[] colors;
	public float[] y_value;

	public int resolution_x = 2;
	public int resolution_z = 2;

	public float width = 5.0f;
	public float height = 5.0f;

	public float irrVal = 0.0f;

	Gradient gradient;

	public Dictionary<Vector3, float> irrVertices;


	public void setDetail(float value){

		resolution_x = resolution_z = (int)value;
		perlinToMesh ();
		updateMesh ();
	}

	public void setIrregularity(float value){

		irrVal = value;
		//perlinToMesh ();
		updateMesh ();
	}

	public void updateColors(float value){
		colors = new Color[vertices.Length];
		for (int i = 0; i<colors.Length; i+=3) {
			
			Color col = colorize(vertices[i], vertices[i+1], vertices[i+2]);
			
			colors[i]=col;
			colors[i+1]=col;
			colors[i+2]=col;
		}
		mesh.colors = colors;
		mesh.Optimize();
	}


	private float calculateIrregularity(float value, Vector3 vertex){

		float val = width / (float)resolution_x;
		float randomVal = 0.0f;
		if (irrVertices.TryGetValue (vertex, out randomVal)) {
			irrVertices.TryGetValue (vertex, out randomVal);
		} else {
			randomVal = Random.Range(-val, val);
			irrVertices.Add (vertex, randomVal);
		}

		return randomVal*(value*vertex.y);
	}


	Color colorize(Vector3 y1, Vector3 y2, Vector3 y3){
		float average = (y1.y + y2.y + y3.y) / 3;

		GameObject grad = GameObject.Find ("gradient");
		setColors colorSet = grad.GetComponent<setColors> ();
		Gradient gradient = colorSet.grad;

		return gradient.Evaluate(average);
	}

	void Start () {
		/*string url = "http://i.imgur.com/aTEEjsM.png";
		texture = new Texture2D (128, 128, TextureFormat.DXT1, false);
		StartCoroutine(fetch (url));*/
		
	
		

		///

		MeshFilter filter = gameObject.AddComponent< MeshFilter >();
		mesh = filter.mesh;
		mesh.Clear();

		vertices = new Vector3[(resolution_x) * (resolution_z) * 6];

		for (int z = 0; z<(resolution_z); z++) {
			for (int x = 0; x<(resolution_x); x++) {
			
				vertices[vertexCount] = new Vector3(		(width/(resolution_x))*x+(width/resolution_x),		0.0f,		height/(resolution_z)*z+(width/resolution_z));
				vertices[vertexCount+1] = new Vector3(			(width/(resolution_x))*x,							0.0f,		height/(resolution_z)*z);
				vertices[vertexCount+2] = new Vector3(		(width/(resolution_x))*x,							0.0f,		height/(resolution_z)*z+(width/resolution_z));
				vertices[vertexCount+3] = new Vector3(		(width/(resolution_x))*x+(width/resolution_x),		0.0f,		height/(resolution_z)*z+(width/resolution_z));
				vertices[vertexCount+4] = new Vector3(		(width/(resolution_x))*x+(width/resolution_x),		0.0f,		height/(resolution_z)*z);
				vertices[vertexCount+5] = new Vector3(			(width/(resolution_x))*x,							0.0f,		height/(resolution_z)*z);

				vertexCount+=6;
			}
		}

		//Triangles
		triangles = new int[vertexCount];

		for(int i = 0; i < triangles.Length;i++){
			triangles[i]=i;
		}
		normales = new Vector3[vertices.Length];

		for (int n = 0; n<normales.Length; n++)
			normales [n] = Vector3.up;

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.normals = normales;
		mesh.RecalculateBounds();
		mesh.Optimize();
		Debug.Log (mesh);

	} 
	private int lerpAndRound(int val){
		return  Mathf.RoundToInt(Mathf.Lerp (0.0f, 127.0f, (val/(float)resolution_x)));
	}
	IEnumerator fetch(string url){
		WWW www = new WWW (url);
		yield return www;
		www.LoadImageIntoTexture (texture);
	}
	public void perlinToMesh(){
		Debug.Log (mesh);
		GameObject perlin;
		generatePerlin perlinGen;
			
		perlin = GameObject.Find ("perlinGenerator");
		perlinGen = perlin.GetComponent<generatePerlin> ();



		texture = perlinGen.texture;



		y_value = new float[(resolution_x) * (resolution_z) * 6];

		int ycount = 0;

		int z_value = 0;
		int x_value = 0;

		for (int z = 0; z<(resolution_z); z++) {
			x_value=0;
			z_value = lerpAndRound(z);
			for (int x = 0; x<(resolution_x); x++) {
			
				//fix +=

				x_value = lerpAndRound(x);

				/*y_value[ycount]   = (1.5f/255.0f)*(255.0f*texture.GetPixel(x_value+((127/(resolution_x))*(x+1)), z_value+((127/(resolution_z))*(z+1))).r);
				y_value[ycount+1] = (1.5f/255.0f)*(255.0f*texture.GetPixel(x_value, z_value).r);
				y_value[ycount+2] = (1.5f/255.0f)*(255.0f*texture.GetPixel(x_value, z_value+((127/(resolution_z))*(z+1))).r);
				y_value[ycount+3] = (1.5f/255.0f)*(255.0f*texture.GetPixel(x_value+(127/(resolution_x))*(x+1), z_value+((127/(resolution_z))*(z+1))).r);
				y_value[ycount+4] = (1.5f/255.0f)*(255.0f*texture.GetPixel(x_value+(127/(resolution_x))*(x+1), z_value).r);
				y_value[ycount+5] = (1.5f/255.0f)*(255.0f*texture.GetPixel(x_value, z_value).r);*/

                y_value[ycount]   = (1.5f/255.0f)*(255.0f*texture.GetPixel(lerpAndRound(x+1), lerpAndRound(z+1)).grayscale);
				y_value[ycount+1] = (1.5f/255.0f)*(255.0f*texture.GetPixel(x_value, z_value).grayscale);
				y_value[ycount+2] = (1.5f/255.0f)*(255.0f*texture.GetPixel(x_value, lerpAndRound(z+1)).grayscale);
				y_value[ycount+3] = (1.5f/255.0f)*(255.0f*texture.GetPixel(lerpAndRound(x+1), lerpAndRound(z+1)).grayscale);
				y_value[ycount+4] = (1.5f/255.0f)*(255.0f*texture.GetPixel(lerpAndRound(x+1), z_value).grayscale);
				y_value[ycount+5] = (1.5f/255.0f)*(255.0f*texture.GetPixel(x_value, z_value).grayscale);
				//x_value += Mathf.Round(128.0f/(resolution_z));

				ycount += 6; 

			}
		}
		Debug.Log (mesh);
	}

	// Update is called once per frame
	public void updateMesh () {

		MeshFilter filter = gameObject.GetComponent< MeshFilter >();
		mesh = filter.mesh;
		mesh.Clear();

		vertices = new Vector3[(resolution_x) * (resolution_z) * 6];
		irrVertices = new Dictionary<Vector3, float>();
		vertexCount = 0;

	
		for (int z = 0; z<(resolution_z); z++) {
			for (int x = 0; x<(resolution_x); x++) {

				float bottom_left;
				float bottom_right;
				float top_right;
				float top_left;



				vertices[vertexCount] = new Vector3(		(width/(resolution_x))*x+(width/resolution_x),		y_value[vertexCount],		height/(resolution_z)*z+(width/resolution_z));
				//update vertex to irregularit
				if((x+1>0) && (z+1>0) &&(x+1<resolution_x) && (z+1<resolution_z)){
				top_right = calculateIrregularity(irrVal, vertices[vertexCount]);
				vertices[vertexCount] = new Vector3(vertices[vertexCount].x+top_right, vertices[vertexCount].y, vertices[vertexCount].z+top_right);
				//done 
				}
				vertices[vertexCount+1] = new Vector3(			(width/(resolution_x))*x,							y_value[vertexCount+1],		height/(resolution_z)*z);
				//update vertex to irregularit
				if((x>0) && (z>0) && (x<resolution_x) && (z<resolution_z)){
				bottom_left = calculateIrregularity(irrVal, vertices[vertexCount+1]);
				vertices[vertexCount+1] = new Vector3(vertices[vertexCount+1].x+bottom_left, vertices[vertexCount+1].y, vertices[vertexCount+1].z+bottom_left);
				}
				//done 

				vertices[vertexCount+2] = new Vector3(		(width/(resolution_x))*x,							y_value[vertexCount+2],		height/(resolution_z)*z+(width/resolution_z));
				//update vertex to irregularit
				if((x>0) && (z+1>0) &&(x<resolution_x) && z+1<resolution_z){
				top_right = calculateIrregularity(irrVal, vertices[vertexCount+2]);
				vertices[vertexCount+2] = new Vector3(vertices[vertexCount+2].x+top_right, vertices[vertexCount+2].y, vertices[vertexCount+2].z+top_right);
					}
				//done 

				vertices[vertexCount+3] = new Vector3(		(width/(resolution_x))*x+(width/resolution_x),		y_value[vertexCount+3],		height/(resolution_z)*z+(width/resolution_z));
				//update vertex to irregularit
				if((x+1>0) && (z+1>0) && (x+1<resolution_x) && z+1<resolution_z){
				top_right = calculateIrregularity(irrVal, vertices[vertexCount+3]);
				vertices[vertexCount+3] = new Vector3(vertices[vertexCount+3].x+top_right, vertices[vertexCount+3].y, vertices[vertexCount+3].z+top_right);
						}
				//done 

				vertices[vertexCount+4] = new Vector3(		(width/(resolution_x))*x+(width/resolution_x),		y_value[vertexCount+4],		height/(resolution_z)*z);
				//update vertex to irregularit
				if((x+1>0) && (z>0) && (x+1<resolution_x) && z<resolution_z){
				top_right = calculateIrregularity(irrVal, vertices[vertexCount+4]);
				vertices[vertexCount+4] = new Vector3(vertices[vertexCount+4].x+top_right, vertices[vertexCount+4].y, vertices[vertexCount+4].z+top_right);
							}
				//done 

				vertices[vertexCount+5] = new Vector3(			(width/(resolution_x))*x,							y_value[vertexCount+5],		height/(resolution_z)*z);
				//update vertex to irregularit
				if((x>0) && (z>0) && (x<resolution_x) && z<resolution_z){
				bottom_left = calculateIrregularity(irrVal, vertices[vertexCount+5]);
				vertices[vertexCount+5] = new Vector3(vertices[vertexCount+5].x+bottom_left, vertices[vertexCount+5].y, vertices[vertexCount+5].z+bottom_left);
								}
				//done 
				
				vertexCount+=6;
			}
		}
		
		//Triangles
		triangles = new int[vertices.Length];
		for(int i = 0; i < triangles.Length;i++){
			triangles[i]=i;
		}

		normales = new Vector3[vertices.Length];
		for (int i = 0; i<triangles.Length; i+=3) {

			Vector3 a = vertices[i+1]-vertices[i];
			Vector3 b = vertices[i+2]-vertices[i];

			Vector3 cross = Vector3.Cross(a,b);

			normales[i]=cross;
			normales[i+1]=cross;
			normales[i+2]=cross;

		}

		//mesh color
		colors = new Color[vertices.Length];
		for (int i = 0; i<colors.Length; i+=3) {

			Color col = colorize(vertices[i], vertices[i+1], vertices[i+2]);
			  
			colors[i]=col;
			colors[i+1]=col;
			colors[i+2]=col;
		}
		
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.colors = colors;
		mesh.normals = normales;
		mesh.RecalculateBounds();
		mesh.Optimize();

		Debug.Log (mesh);
	}
}

















