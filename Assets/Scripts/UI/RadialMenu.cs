using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    [SerializeField]
    int width;
    [SerializeField]
    int n;

    [SerializeField]
    RectTransform item_prefab;

    float R; // Total radius of menu
    float r; // Item radius

    float s; // Side length of regular polygon connecting all items
    float a; // Radius of said regular polygon

    void Awake()
    {
        // Basic "draw a picture" understanding:
        // R = width / 2
        // r = R - a
        // s = 2r
        // 
        // Fun fact:
        // a = s / 2sin(pi/n)
        //
        // Therefore:
        // a = s / 2sin(pi/n) = r / sin(pi/n)
        // r = R - a = R - r / sin(pi/n)
        // R = r + r / sin(pi/n) = r(1 + 1 / sin(pi/n))
        // r = R / (1 + 1 / sin(pi/n))

        R = width * 0.5f;
        r = R / (1 + 1 / Mathf.Sin(Mathf.PI / n));

        s = 2 * r;
        a = R - r;
    }

    private void Start()
    {
        float arc = 2 * Mathf.PI / n;
        float phase = Mathf.PI / 2 - arc;
        
        for(float t = phase; t < 2 * Mathf.PI + phase; t += arc)
        {
            RectTransform item = Instantiate(item_prefab, transform);
            float span = 2 * Mathf.Sqrt(r * r * 0.5f);
            item.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, span);
            item.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, span);
            item.localPosition = a * new Vector3(Mathf.Cos(t), Mathf.Sin(t), 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
