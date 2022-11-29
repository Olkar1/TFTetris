using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempFog : MonoBehaviour
{
    // Scroll main texture based on time

    float scrollSpeed = 0.05f;
    Renderer rend;

    void Start() {
        rend = GetComponent<Renderer>();
    }

    void Update() {
        float offset = Time.time * scrollSpeed;
        rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
