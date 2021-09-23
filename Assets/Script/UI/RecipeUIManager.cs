using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUIManager : Singleton<RecipeUIManager>
{
    [SerializeField] Transform parent;
    [SerializeField] RecipeResult prefab;
    [SerializeField] Text recipeInfo;
    [SerializeField] Text recipeMaterialInfo;
    [SerializeField] Image recipeInfoBack;

    Recipe[] allRecipe;

    private void Start()
    {
        allRecipe = Resources.LoadAll<Recipe>("Recipe");
        
        for(int i=0; i < allRecipe.Length; i++)
        {
            RecipeResult result = Instantiate(prefab, parent);
            result.OnShowRecipeInfo += OnShowInfo;
            result.OnCloseShowRecipeInfo += OnCloseInfo;
            result.Setup(allRecipe[i]);
        }

        recipeInfoBack.enabled = false;
        recipeInfo.text = string.Empty;
        recipeMaterialInfo.text = string.Empty;
    }
    private void OnEnable()
    {
        Clear();
    }

    private void OnDisable()
    {
        OnCloseInfo();
        Clear();
    }

    public void Clear()
    {
        RecipeResult[] result = parent.GetComponentsInChildren<RecipeResult>(); 
        for(int i=0; i < result.Length; i++)
        {
            result[i].Clear();
        }
    }

    private void OnShowInfo(RecipeResult result)
    {
        recipeInfoBack.enabled = true;
        recipeInfoBack.transform.position = result.transform.position;
        recipeInfo.text = result.ResultRecipe.Result.ToString();
        for(int i=0; i<result.ResultRecipe.MaterialArray.Length; i++)
        {
            Recipe.Material material = result.ResultRecipe.MaterialArray[i];
            string tempText = string.Format(material.item.ItemName + " : " + material.count +"\n");
            recipeMaterialInfo.text += tempText;
        }
    }
    private void OnCloseInfo()
    {
        recipeInfo.text = string.Empty;
        recipeMaterialInfo.text = string.Empty;
        recipeInfoBack.enabled = false;
    }
}
