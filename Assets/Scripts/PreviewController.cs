using Es.InkPainter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewController : MonoBehaviour
{
    public static PreviewController instance;
    public PreviewController() { instance = this; }

    public InkCanvas drawedObject;

    [SerializeField] private Preview preveiwPrefab;
    [SerializeField] private List<Preview> previews;
    [SerializeField] private Transform parentPreviews;
    private float minX = -0.03f;
    private float maxX = 0.03f;
    private float rememberX;
    private bool move = false;
    private Vector2 startPos;
    private Vector2 delta;
    private Preview curPreview;

    public void LoadPreviews(Texture[] loadedTextures, string[] fileNames)
    {
        for (int i = 0; i < loadedTextures.Length; i++)
        {
            AddPreview(loadedTextures[i], fileNames[i]);
        }
    }
    public void AddPreview(Texture _texture, string fileName)
    {
        Preview _curPreview = Instantiate(preveiwPrefab, parentPreviews);
        _curPreview.gameObject.SetActive(true);
        _curPreview.SetTexture(_texture);
        _curPreview.transform.localPosition = new Vector3(0.03f * previews.Count, 0, 0);
        _curPreview.fileName = fileName;
        previews.Add(_curPreview);
        maxX += 0.03f;
    }
    private void SelectPreview(Preview _selected)
    {
        curPreview?.Deselect();
        if (curPreview == _selected)
        {
            curPreview = null;
            SaveController.instance.ClearImage();
            return;
        }
        curPreview = _selected;
        drawedObject.GetComponent<Renderer>().material.SetTexture("_MainTex", _selected.curTexture);
        drawedObject.enabled = true;
        drawedObject.Init();
        curPreview.Select();
    }
    public void DeselectCur()
    {
        curPreview?.Deselect();
    }
    public void DeleteSelected()
    {
        if (curPreview == null)
            return;

        maxX -= 0.03f;
        previews.Remove(curPreview);
        Destroy(curPreview.gameObject);
        SaveController.instance.ClearImage();
        SaveController.instance.Delete(curPreview);
        UpdatePreviews();
    }
    private void MovePreviews(Vector2 _delta)
    {
        parentPreviews.transform.localPosition = new Vector3(Mathf.Clamp(rememberX - _delta.x * 0.0001f, -maxX * 0.8f, -minX * 0.8f), parentPreviews.transform.localPosition.y, parentPreviews.transform.localPosition.z);
    }
    public void UpdatePreviews()
    {
        for(int i = 0; i < previews.Count; i++)
        {
            previews[i].transform.localPosition = new Vector3(0.03f * i, 0, 0);
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Preview selectedPreview = hit.collider.GetComponent<Preview>();
                if (selectedPreview != null)
                {
                    SelectPreview(selectedPreview);
                }
            }
            if (Input.mousePosition.y < Screen.height * 0.3f)
            {
                startPos = Input.mousePosition;
                move = true;
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (!move)
                return;
            delta = startPos - (Vector2)Input.mousePosition;
            MovePreviews(delta);
        }
        if (Input.GetMouseButtonUp(0))
        {
            move = false;
            rememberX = parentPreviews.transform.localPosition.x;
        }
    }
}

