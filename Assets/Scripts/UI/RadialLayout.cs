using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialLayout : MonoBehaviour
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

    RectTransform[] items;

    public RectTransform Select(Vector2 position)
    {
        float min_dist = Mathf.Infinity;
        RectTransform near_item = null;

        foreach(RectTransform item in items)
        {
            Vector2 line = position - item.anchoredPosition;
            float dist = line.magnitude;

            if(dist < min_dist)
            {
                min_dist = dist;
                near_item = item;
            }
        }

        return near_item;
    }

    public int SelectIndex(Vector2 position)
    {
        float min_dist = Mathf.Infinity;
        int near_i = -1;

        for(int i = 0; i < n; i++)
        {
            Vector2 line = position - items[i].anchoredPosition;
            float dist = line.magnitude;

            if (dist < min_dist)
            {
                min_dist = dist;
                near_i = i;
            }
        }

        return near_i;
    }

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

        float arc = 2 * Mathf.PI / n;
        float phase = Mathf.PI / 2 - arc;

        items = new RectTransform[n];
        int count = 0;

        for (float t = phase; count < n; t += arc)
        {
            RectTransform item = Instantiate(item_prefab, transform);
            items[count] = item;
            count++;

            float span = 2 * Mathf.Sqrt(r * r * 0.5f);
            item.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, span);
            item.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, span);
            item.localPosition = a * new Vector3(Mathf.Cos(t), Mathf.Sin(t), 0);
        }
    }
}
