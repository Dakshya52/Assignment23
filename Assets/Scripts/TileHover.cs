using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileHover : MonoBehaviour
{
    public Text tileInfoText;
    
    void Update()
    {
        DetectTile();
    }

    void DetectTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            TileInfo tileInfo = hit.collider.GetComponent<TileInfo>();
            if (tileInfo != null)
            {
                tileInfoText.text = $"Tile Position: ({tileInfo.x}, {tileInfo.y})";
            }
        }
        else
        {
            tileInfoText.text = "";
        }
    }
}
