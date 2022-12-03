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
    float[] colours;
    ComputeBuffer colour_buffer;
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

                colours[index + 0] = c.r;
                colours[index + 1] = c.g;
                colours[index + 2] = c.b;
                colours[index + 3] = c.a;
            }
        }

        colour_buffer.SetData(colours);
    }

    void Write(int row, int col, Color c)
    {
        if(row < 0 || row >= height || col < 0 || col >= width)
        { return;  }

        int index = (row * width + col) * 4;

        colours[index + 0] = c.r;
        colours[index + 1] = c.g;
        colours[index + 2] = c.b;
        colours[index + 3] = c.a;
    }

    void Paint(int row, int col, int r, Color c)
    {
        for(int grid_y = row-r; grid_y < row+r; grid_y++)
        {
            if(grid_y < 0 || grid_y >= height)
            { continue; }

            for(int grid_x = col-r; grid_x < col+r; grid_x++)
            {
                if(grid_x < 0 || grid_x >= width)
                { continue; }

                Vector2Int grid_dist = new Vector2Int(grid_x - col, grid_y - row);

                if(grid_dist.magnitude <= r)
                {
                    Write(grid_y, grid_x, c);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        aspect = width / height;
        transform.localScale = new Vector3(transform.localScale.x * aspect, transform.localScale.y, transform.localScale.z);

        colours = new float[width * height * 4];
        colour_buffer = new ComputeBuffer(width * height, sizeof(float) * 4);

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
    }

    private void Start()
    {
        Clear(background);
    }

    void Update()
    {
        colour_buffer.SetData(colours);

        shader.SetTexture(kernel_id, texture_id, texture);
        shader.SetBuffer(kernel_id, colour_buffer_id, colour_buffer);
        shader.SetInt(width_id, width);
        shader.SetInt(height_id, height);
        
        shader.Dispatch(kernel_id, Mathf.CeilToInt(width / 8f), Mathf.CeilToInt(height / 8f), 1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 centroid = Vector3.zero;
        for(int i = 0; i < collision.contactCount; i++)
        {
            centroid += collision.GetContact(i).point;
        }
        centroid /= collision.contactCount;

        Vector3 origin = GetComponent<Collider>().bounds.min;
        Vector3 extent = GetComponent<Collider>().bounds.max;
        Vector3 point = GetComponent<Collider>().ClosestPointOnBounds(centroid);
        float x_dist = 1 - (point.x - origin.x) / (extent.x - origin.x);
        float y_dist = 1 - (point.y - origin.y) / (extent.y - origin.y);
        int row = (int)(y_dist * height);
        int col = (int)(x_dist * width);

        print($"{x_dist}, {y_dist}");
        print($"{col}, {row}");
        Paint(row, col, 6, Color.red);

        Destroy(collision.collider.gameObject);
    }

    private void OnDestroy()
    {
        colour_buffer.Release();
        colour_buffer = null;
    }
}
