using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TotalHandshakeCount : MonoBehaviour
{
    public int totalFinishedHandshakeCount = 0;
    [SerializeField] private GameObject countText;

    public void UpdateCountOnCanvas()
    {
        totalFinishedHandshakeCount++;
        countText.GetComponent<TextMeshProUGUI>().text = totalFinishedHandshakeCount.ToString();
    }
}
