using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SizeDot : MonoBehaviour
{
    [SerializeField]
    Image back_image;
    [SerializeField]
    Image dot_image;
    RectTransform dot_rect;

    float span;
    float _radius;
    public float radius
    {
        get => _radius;
        set => _radius = value;
    }

    Color back_initial;
    Color dot_initial;

    public void ToggleSelected(bool toggle)
    {
        back_image.color = toggle ? back_initial : back_initial * 0.5f;
        dot_image.color = toggle ? dot_initial : dot_initial * 0.5f;
    }

    private void Awake()
    {
        dot_rect = dot_image.GetComponent<RectTransform>();
        back_initial = back_image.color;
        dot_initial = dot_image.color;
    }

    private void OnEnable()
    {
        span = (int)GetComponent<RectTransform>().sizeDelta.x;
        ToggleSelected(false);
    }

    // Update is called once per frame
    void Update()
    {
        dot_rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _radius * 2 * span);
        dot_rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _radius * 2 * span);
    }
}
