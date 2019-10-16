using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;//设置阴影模式所必须
public class Meshterrain : MonoBehaviour
{
    public Transform Chan;
    public Material grassmat;
    public Mesh quad;
    public Texture2D heightMap;
    public float terrainHeight;
    public int terrainSize;
    public Material terrainMat;
    private GameObject[] grass;
    public int grasscount;
    [Range(0.0f,1f)]
    private float grassdensity;
    private List<Matrix4x4[]> matrixlist = new List<Matrix4x4[]>();
    private Vector4[] pos = new Vector4[1023];
    Vector4[] colors = new Vector4[1023];
    void Start()
    {
        CreateTerrain();
        Meshcomputing();
    }
    private void Update()
    {
        Creategrassupdate();
    }
    private void Meshcomputing()
    {
        grassdensity = (float)terrainSize / grasscount;
        Matrix4x4[] matrices = new Matrix4x4[1023];
        Quaternion rotation = Quaternion.Euler(0, 0, 0);
        Vector3 scale = Vector3.one;
        int mm = 0;
        for (int i = 0; i < grasscount; i++)
        {
            for (int j = 0; j < grasscount; j++)
            {
                float ran = Random.Range(-0.19f, 0.2f);
                float ii = i * grassdensity;
                float jj = j * grassdensity;
                Vector3 position = new Vector3(ii+ran , heightMap.GetPixel(Mathf.FloorToInt(ii), Mathf.FloorToInt(jj)).grayscale * terrainHeight - 19.5f, jj+ran );
                matrices[mm]=Matrix4x4.TRS(position, rotation, scale);
                mm++;
                if (mm%1022 == 0 )
                {
                    matrixlist.Add(new Matrix4x4[1023]);
                    matrixlist[matrixlist.Count - 1] = matrices;
                    matrices = new Matrix4x4[1023];
                    mm = 0;
                }
            }
        }
        for (int i = 0; i < 1023; i++)
        {
            float r = Random.Range(0.160f, 0.165f);
            float b = Random.Range(0.290f, 0.360f);
            colors[i] = new Vector4(r, .945f, b,1);
        }
       
        
    }
    private void CreateTerrain()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        for (int i = 0; i < terrainSize; i++)
        {
            for (int j = 0; j < terrainSize; j++)
            {
                vertices.Add(new Vector3(i, heightMap.GetPixel(i, j).grayscale * terrainHeight, j));
                if (i == 0 || j == 0) continue;
                triangles.Add(terrainSize * i + j);
                triangles.Add(terrainSize * i + j - 1);
                triangles.Add(terrainSize * (i - 1) + j - 1);
                triangles.Add(terrainSize * (i - 1) + j - 1);
                triangles.Add(terrainSize * (i - 1) + j);
                triangles.Add(terrainSize * i + j);
            }
        }
        Vector2[] uvs = new Vector2[vertices.Count];
        for (var i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }
        GameObject terrain = this.gameObject;
        terrain.AddComponent<MeshFilter>();
        MeshRenderer renderer = terrain.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = terrainMat;
        renderer.shadowCastingMode = ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        Mesh groundMesh = new Mesh();
        groundMesh.vertices = vertices.ToArray();
        groundMesh.uv = uvs;
        groundMesh.triangles = triangles.ToArray();
        groundMesh.RecalculateNormals();//生成法线
        terrain.GetComponent<MeshFilter>().mesh = groundMesh;
        terrain.AddComponent<MeshCollider>();
        vertices.Clear();
    }
    private void Creategrassupdate()
    {
        MaterialPropertyBlock props = new MaterialPropertyBlock();
       
        Vector4 pos1=new Vector4(Chan.transform.position.x, Chan.transform.position.y, Chan.transform.position.z, 1);
        for (int i = 0; i < 1023; i++)
        {
            pos[i] = pos1;
        }
        props.SetVectorArray("_Color", colors);
        props.SetVectorArray("_Stepon", pos);
        foreach (Matrix4x4[] mat in matrixlist)
        {
            Graphics.DrawMeshInstanced(quad, 0, grassmat, mat, 1023,props,ShadowCastingMode.Off,false);
        }
    }
}
