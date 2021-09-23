using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugesBar : MonoBehaviour
{
    [SerializeField] Image progressBar;

    public void UpdateBar(float current, float max)
    {
        progressBar.fillAmount = current / max;
    }
}
