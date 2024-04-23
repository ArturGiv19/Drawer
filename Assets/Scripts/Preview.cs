using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preview : MonoBehaviour
{
    public Texture curTexture;
    public string fileName;

    [SerializeField] private Renderer renderer;
    [SerializeField] private GameObject outline;
    public void SetTexture(Texture _texture)
    {
        renderer.material.mainTexture = _texture;
        curTexture = _texture;
    }
    public void Select()
    {
        outline.SetActive(true);
    }
    public void Deselect()
    {
        if (outline != null)
            outline.SetActive(false);
    }
}
