using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
public class Noisetexturecreater : MonoBehaviour {
    public Material noisemat;
    public int HeightWidth;
    private RenderTexture RT;
    private Texture2D texture;
    public RawImage image;
	// Use this for initialization
	void Start () {
        RT = RenderTexture.GetTemporary(HeightWidth,HeightWidth,0);
        Graphics.Blit(texture, RT, noisemat);
        texture = new Texture2D(HeightWidth, HeightWidth, TextureFormat.ARGB32, false);
        RenderTexture.active = RT;
        texture.ReadPixels(new Rect(0, 0, HeightWidth, HeightWidth), 0, 0);
        texture.Apply();
        RenderTexture.ReleaseTemporary(RT);
        image.texture = texture;
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes("D:\\noise.PNG", bytes);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
