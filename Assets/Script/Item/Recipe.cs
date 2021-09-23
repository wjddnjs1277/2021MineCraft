using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "NewItem/Recipe")]
public class Recipe : ScriptableObject
{
    [System.Serializable]
    public class Material
    {
        public Item item;
        public int count;
    }

    [SerializeField] Material[] materialArray;
    [SerializeField] Item result;

    public Material[] MaterialArray => materialArray;
    public Item Result => result;

}
