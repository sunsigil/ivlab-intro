using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintGun : MonoBehaviour
{
    [SerializeField]
    Color[] colours;
    int colour_idx;
    Color colour => colours[colour_idx];
    [SerializeField]
    float radius;

    [SerializeField]
    Transform anchor;
    [SerializeField]
    LineRenderer line;
    [SerializeField]
    Transform mark;

    Painting target;
    Vector3 target_point;
    PaintMode mode;

    public void ShiftColour(int offset)
    {
        if(offset == -1)
        { offset = colours.Length + offset; }
        colour_idx = (colour_idx + offset) % colours.Length;
    }

    public void SetRadius(float radius)
    {
        this.radius = radius;
    }

    public void SetMode(PaintMode mode)
    {
        this.mode = mode;
    }

    private void Awake()
    {
        SetRadius(0);
    }

    private void Update()
    {
        if(target != null)
        {
            mark.gameObject.SetActive(true);
            mark.transform.position = Vector3.Lerp(anchor.position, target_point, 0.95f);
            mark.transform.forward = target.transform.up;
            mark.transform.localScale = new Vector3(radius * 3f, radius * 3f, 1);

            if (mode != PaintMode.NONE)
            {
                if (mode == PaintMode.ADD)
                {
                    target.Paint(target_point, radius, colour);
                }
                else
                {
                    target.Erase(target_point, radius);
                }

                line.gameObject.SetActive(true);
                line.SetPosition(0, anchor.position);
                line.SetPosition(1, target_point);
                line.startWidth = 0;
                line.endWidth = radius * 2;
            }
            else
            {
                line.gameObject.SetActive(false);
            }
        }
        else
        {
            line.gameObject.SetActive(false);
            mark.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = new Ray(anchor.position, Camera.main.transform.forward);
        Physics.SphereCast(ray, radius, out hit);

        if (hit.transform == null)
        {
            target = null;
            return;
        }

        target = hit.transform.GetComponent<Painting>();
        target_point = hit.point;
    }
}
