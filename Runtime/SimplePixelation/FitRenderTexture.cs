using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class FitRenderTexture : MonoBehaviour
{
    RenderTexture _renderTexture;
    RenderTexture renderTexture{get
    {
        if (!_renderTexture) _renderTexture = GetRenderTex();
        return _renderTexture;
    }
        set { _renderTexture = value; }
    }
    public Camera fetchCam;
    public RawImage renderImage;

    public float targetHeight = 256;
    
    // Update is called once per frame
    void Update()
    {
        if (!fetchCam || !renderImage) return;
        if (fetchCam.targetTexture != _renderTexture) fetchCam.targetTexture = renderTexture;
        if (renderImage.texture != _renderTexture) renderImage.texture = _renderTexture;
        int targetWidth = Mathf.RoundToInt(fetchCam.aspect * targetHeight); //width / height
        if (renderTexture.width != targetWidth || renderTexture.height != Mathf.RoundToInt(targetHeight))
            renderTexture = GetRenderTex();
    }

    RenderTexture GetRenderTex()
    {
        int targetWidth = Mathf.RoundToInt(fetchCam.aspect * targetHeight);
        var tex = new RenderTexture(targetWidth, Mathf.RoundToInt(targetHeight), 24);
        //tex.antiAliasing = 0;
        tex.filterMode = FilterMode.Point;
        return tex;
    }
}
