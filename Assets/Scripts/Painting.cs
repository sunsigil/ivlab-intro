using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painting : MonoBehaviour
{
    [SerializeField]
    int width;
    [SerializeField]
    int height;
    float aspect;
    [SerializeField]
    Color background;

    [SerializeField]
    ComputeShader shader;
    ComputeBuffer colour_buffer;
    float[] colour_array;
    RenderTexture texture;
    Material material;

    int kernel_id;
    int texture_id;
    int colour_buffer_id;
    int width_id;
    int height_id;

    void Clear(Color c)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = (y * width + x) * 4;

                colour_array[index + 0] = c.r;
                colour_array[index + 1] = c.g;
                colour_array[index + 2] = c.b;
                colour_array[index + 3] = c.a;
            }
        }
    }

    void Write(int x, int y, Color c)
    {
        if(y < 0 || y >= height || x < 0 || x >= width)
        { return;  }

        int index = (y * width + x) * 4;

        colour_array[index + 0] = c.r;
        colour_array[index + 1] = c.g;
        colour_array[index + 2] = c.b;
        colour_array[index + 3] = c.a;
    }

    void DotWrite(int x, int y, int r, Color c)
    {
        for(int box_y = y-r; box_y < y+r; box_y++)
        {
            if(box_y < 0 || box_y >= height)
            { continue; }

            for(int box_x = x-r; box_x < x+r; box_x++)
            {
                if(box_x < 0 || box_x >= width)
                { continue; }

                Vector2Int box_dist = new Vector2Int(box_x - x, box_y - y);

                if(box_dist.magnitude <= r)
                {
                    Write(box_x, box_y, c);
                }
            }
        }
    }

    public void Paint(Vector3 point, float r, Color c)
    {
        Vector3 origin = GetComponent<Collider>().bounds.min;
        Vector3 extent = GetComponent<Collider>().bounds.max;
        Vector3 span = extent - origin;

        float ppu = width / span.x;
        int pixel_r = Mathf.CeilToInt(r * ppu);
        
        float x_dist = 1 - (point.x - origin.x) / span.x;
        float y_dist = 1 - (point.y - origin.y) / span.y;

        int row = (int)(y_dist * height);
        int col = (int)(x_dist * width);

        DotWrite(col, row, pixel_r, c);
    }

    public void Erase(Vector3 point, float r)
    {
        Paint(point, r, background);
    }

    // Start is called before the first frame update
    void Awake()
    {
        shader = (ComputeShader)Instantiate(Resources.Load("PaintCompute"));

        aspect = width / height;
        transform.localScale = new Vector3(transform.localScale.x * aspect, transform.localScale.y, transform.localScale.z);

        colour_buffer = new ComputeBuffer(width * height, sizeof(float) * 4);
        colour_array = new float[width * height * 4];

        texture = new RenderTexture(width, height, 32);
        texture.enableRandomWrite = true;
        texture.filterMode = FilterMode.Point;
        texture.Create();

        material = GetComponent<MeshRenderer>().material;
        material.mainTexture = texture;

        kernel_id = shader.FindKernel("CSMain");
        texture_id = Shader.PropertyToID("Result");
        colour_buffer_id = Shader.PropertyToID("_ColourBuffer");
        width_id = Shader.PropertyToID("_Width");
        height_id = Shader.PropertyToID("_Height");

        shader.SetTexture(kernel_id, texture_id, texture);
        shader.SetBuffer(kernel_id, colour_buffer_id, colour_buffer);
        shader.SetInt(width_id, width);
        shader.SetInt(height_id, height);

        Clear(background);
    }

    void Update()
    {
        colour_buffer.SetData(colour_array);
        shader.Dispatch(kernel_id, Mathf.CeilToInt(width / 8f), Mathf.CeilToInt(height / 8f), 1);
    }

    private void OnDestroy()
    {
        colour_buffer.Dispose();
    }
}
