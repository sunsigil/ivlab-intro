using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintGun : MonoBehaviour
{
    [SerializeField]
    Color colour;
    [SerializeField]
    float radius;

    [SerializeField]
    Transform anchor;

    public void Fire()
    {
        RaycastHit hit;
        Ray ray = new Ray(anchor.position, Camera.main.transform.forward);
        Physics.SphereCast(ray, radius, out hit);

        if(hit.transform == null)
        { return; }

        Painting painting = hit.transform.GetComponent<Painting>();
        if (painting == null)
        { return; }

        painting.Paint(hit.point, radius, colour);
    }
}
