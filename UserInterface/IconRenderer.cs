using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconRenderer : MonoBehaviour
{
    [SerializeField] private Camera icon_camera;
    [SerializeField] private Transform target_transform;

    [HideInInspector] private GameObject part_icon;

    public Texture2D render_icon(Part part, int size) {
        part_icon = Instantiate(part.prefab);
        part_icon.transform.rotation = target_transform.rotation;
        part_icon.transform.position = target_transform.position;
        part_icon.layer = LayerMask.NameToLayer("UIIconRendering");
        
        // initialize
        Texture2D icon = new Texture2D(size, size, TextureFormat.RGB24, false);
        RenderTexture rt = new RenderTexture(size, size, 24);

        icon_camera.targetTexture = rt;
        RenderTexture.active = rt;

        // grab and apply the texture
        icon_camera.Render();
        icon.ReadPixels(new Rect(0, 0, size, size), 0, 0);
        icon.Apply();

        // cleanup
        icon_camera.targetTexture = null;
        RenderTexture.active = null;

        Destroy(rt);
        if (part_icon) Destroy(part_icon);
        
        return icon;
    }

    public void clean() {
        Destroy(icon_camera);
    }
}
