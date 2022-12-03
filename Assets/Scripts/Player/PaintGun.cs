using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintGun : MonoBehaviour
{
    [SerializeField]
    Transform anchor;

    [SerializeField]
    GameObject paint_ball;

    Timeline fire_timeline;

    private void Awake()
    {
        fire_timeline = new Timeline(0.1f);
    }

    public void Fire()
    {
        if(fire_timeline.finished)
        {
            GameObject instance = Instantiate(paint_ball);
            instance.transform.position = anchor.position;
            instance.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * 10;
            fire_timeline.Reset();
        }
    }

    private void FixedUpdate()
    {
        fire_timeline.Tick(Time.fixedDeltaTime);
    }
}
