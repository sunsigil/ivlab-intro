using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintGunMenu : MonoBehaviour
{
    [SerializeField]
    float[] radii;

    Controller controller;
    RadialLayout layout;
    PaintGun gun;

    SizeDot last_selected;

    private void Awake()
    {
        controller = GetComponent<Controller>();
        layout = GetComponent<RadialLayout>();
        gun = FindObjectOfType<PaintGun>();
    }

    private void Start()
    {
        for (int i = 0; i < layout.items.Length; i++)
        {
            RectTransform item = layout.items[i];
            SizeDot dot = item.GetComponent<SizeDot>();
            dot.radius = radii[i % radii.Length];
        }
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        Vector2 m_pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 s_cen = new Vector2(Screen.width, Screen.height) * 0.5f;
        Vector2 m_diff = m_pos - s_cen;

        if (last_selected != null)
        { last_selected.ToggleSelected(false); }
        SizeDot dot = layout.Select(m_diff).GetComponent<SizeDot>();
        dot.ToggleSelected(true);
        gun.SetRadius(dot.radius);
        last_selected = dot;

        if(controller.Released(InputCode.POWER))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            gameObject.SetActive(false);
        }
    }
}
