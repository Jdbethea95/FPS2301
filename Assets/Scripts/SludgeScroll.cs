using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SludgeScroll : MonoBehaviour
{

    [SerializeField] float scrollX;
    [SerializeField] float scrollY;
    [SerializeField] Renderer rend;

    float offSetX;
    float offSetY;

    private void Update()
    {
        offSetX = Time.time * scrollX;
        offSetY = Time.time * scrollY;

        rend.material.mainTextureOffset = new Vector2(offSetX, offSetY);
    }
}
