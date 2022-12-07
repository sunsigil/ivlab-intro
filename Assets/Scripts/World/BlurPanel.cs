using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurPanel : MonoBehaviour
{
    [SerializeField]
    Material material;
    [SerializeField]
    RenderTexture in_tex;

    [SerializeField]
    ComputeShader shader;
    RenderTexture out_tex;

    int kernel_id;
    int in_tex_id;
    int width_id;
    int height_id;
    int out_tex_id;

    // Start is called before the first frame update
    void Awake()
    {
        in_tex.width = Screen.width;
        in_tex.height = Screen.height;
        in_tex.enableRandomWrite = true;
        shader = (ComputeShader)Instantiate(Resources.Load("GaussCompute")); 

        out_tex = new RenderTexture(in_tex);
        out_tex.enableRandomWrite = true;
        out_tex.Create();

        material.mainTexture = out_tex;

        kernel_id = shader.FindKernel("CSMain");
        in_tex_id = Shader.PropertyToID("_InTex");
        width_id = Shader.PropertyToID("_Width");
        height_id = Shader.PropertyToID("_Height");
        out_tex_id = Shader.PropertyToID("_OutTex");

        shader.SetTexture(kernel_id, in_tex_id, in_tex);
        shader.SetInt(width_id, in_tex.width);
        shader.SetInt(height_id, in_tex.height);
        shader.SetTexture(kernel_id, out_tex_id, out_tex);
    }

    void Update()
    {
        shader.Dispatch(kernel_id, Mathf.CeilToInt(in_tex.width / 8f), Mathf.CeilToInt(in_tex.height / 8f), 1);
    }
}
