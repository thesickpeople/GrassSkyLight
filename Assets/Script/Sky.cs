using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Sky : MonoBehaviour
{
    public Material skymat;
    //public Texture2D _NoiseTex;
    //public Vector4 Windway;
    private Camera thiscamera;
    private Transform thiscameratrans;
    
    private void OnEnable()
    {
        thiscamera = this.GetComponent<Camera>();
        thiscameratrans = this.transform;
        thiscamera.depthTextureMode |= DepthTextureMode.Depth;
    }
    [ImageEffectOpaque]
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        SetRay();
        //skymat.SetTexture("_NoiseTex", _NoiseTex);
        //skymat.SetVector("_Windway", Windway);
        Graphics.Blit(source, destination,skymat);
    }
    private void SetRay()
    {
        Matrix4x4 frustumCorners = Matrix4x4.identity;//返回单位矩阵

        float fov = thiscamera.fieldOfView;
        float near = thiscamera.nearClipPlane;
        float aspect = thiscamera.aspect;

        float halfHeight = near * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
        Vector3 toRight =thiscameratrans.right * halfHeight * aspect;
        Vector3 toTop = thiscameratrans.up * halfHeight;

        Vector3 topLeft = thiscameratrans.forward * near + toTop - toRight;
        float scale = topLeft.magnitude / near;

        topLeft.Normalize();
        topLeft *= scale;

        Vector3 topRight = thiscameratrans.forward * near + toRight + toTop;
        topRight.Normalize();
        topRight *= scale;

        Vector3 bottomLeft = thiscameratrans.forward * near - toTop - toRight;
        bottomLeft.Normalize();
        bottomLeft *= scale;

        Vector3 bottomRight = thiscameratrans.forward * near + toRight - toTop;
        bottomRight.Normalize();
        bottomRight *= scale;

        frustumCorners.SetRow(0, bottomLeft);
        frustumCorners.SetRow(1, bottomRight);
        frustumCorners.SetRow(2, topRight);
        frustumCorners.SetRow(3, topLeft);

        skymat.SetMatrix("_FrustumCornersRay", frustumCorners);
        skymat.SetMatrix("_UnityMatVP", frustumCorners);
    }
}