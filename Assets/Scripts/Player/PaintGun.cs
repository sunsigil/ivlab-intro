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
    [SerializeField]
    LineRenderer line;
    [SerializeField]
    Transform mark;

    Painting target;
    Vector3 target_point;

    public void Fire()
    {
        if (target == null)
        { return; }

        target.Paint(target_point, radius, colour);

        line.gameObject.SetActive(true);
        line.SetPosition(0, anchor.position);
        line.SetPosition(1, target_point);
        line.startWidth = radius;
        line.endWidth = radius * 2;
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

        if (target == null)
        {
            line.gameObject.SetActive(false);
            mark.gameObject.SetActive(false);
            return;
        }

        mark.gameObject.SetActive(true);
        mark.transform.position = Vector3.Lerp(anchor.position, target_point, 0.95f);
        mark.transform.LookAt(Camera.main.transform.position, Vector3.up);
        mark.transform.localScale = new Vector3(radius * 2.5f, radius * 2.5f, 1);
    }
}
