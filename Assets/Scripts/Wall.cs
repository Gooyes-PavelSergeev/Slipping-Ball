using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void Update()
    {
        var normalizedPos = Camera.main.WorldToViewportPoint(this.transform.position);
        if (normalizedPos.x < -1f)
        {
            if (this.gameObject != null) Destroy(this.gameObject);
        }
    }
}
