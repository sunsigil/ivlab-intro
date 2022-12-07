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

    private void Awake()
    {
        controller = GetComponent<Controller>();
        layout = GetComponent<RadialLayout>();
        gun = FindObjectOfType<PaintGun>();
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        for(int i = 0; i < layout.items.Length; i++)
        {
            RectTransform item = layout.items[i];
            SizeDot dot = item.GetComponent<SizeDot>();
            dot.SetRadius(radii[i]);
        }

        Vector2 m_pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 s_cen = new Vector2(Screen.width, Screen.height) * 0.5f;
        Vector2 m_diff = m_pos - s_cen;

        int rad_index = layout.SelectIndex(m_diff) % radii.Length;
        gun.SetRadius(radii[rad_index]);

        if(Input.GetKeyUp(KeyCode.Mouse2))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            gameObject.SetActive(false);
        }
    }
}
