using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Canvas))]
public class CanvasOriented : MonoBehaviour
{
    private Canvas Canvas;

    void Start()
    {
        Canvas = GetComponent<Canvas>();
        Assert.IsNotNull(Canvas, "Canvas not present");
        Canvas.renderMode = RenderMode.WorldSpace;
        Canvas.worldCamera = Camera.main;
    }

    void Update()
    {
        Canvas.transform.LookAt(transform.position + Canvas.worldCamera.transform.rotation * Vector3.forward, Canvas.worldCamera.transform.rotation * Vector3.up);
    }
}
