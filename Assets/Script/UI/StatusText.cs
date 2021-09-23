using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusText : MonoBehaviour
{
    [SerializeField] new Text name;
    [SerializeField] Text point;

    public void Show(string name, string point,int add = 0)
    {
        this.name.text = name;
        this.point.text = add == 0 ? point : string.Format("{0}  ( +{1} )", point, add);
    }
}
