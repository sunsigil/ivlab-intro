using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeDot : MonoBehaviour
{
    [SerializeField]
    RectTransform dot;

    float span;
    float radius;
    
    public void SetRadius(float radius)
    {
        this.radius = span * radius;
    }

    private void OnEnable()
    {
        span = (int)GetComponent<RectTransform>().sizeDelta.x;
        radius = 0;
    }

    // Update is called once per frame
    void Update()
    {
        dot.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, radius * 2);
        dot.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, radius * 2);
    }
}
