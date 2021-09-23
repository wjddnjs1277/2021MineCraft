using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeResult : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Image isCraftItem;

    public event System.Action<RecipeResult> OnShowRecipeInfo;
    public event System.Action OnCloseShowRecipeInfo;

    Recipe recipe;

    bool isCraft;

    public Recipe ResultRecipe => recipe;

    public void Setup(Recipe recipe)
    {
        this.recipe = recipe;

        itemImage.sprite = recipe.Result.ItemSprite;
        Clear();
    }

    public void Clear()
    {
        for(int i = 0; i< recipe.MaterialArray.Length; i++)
        {
            Recipe.Material material = recipe.MaterialArray[i];

            int itemCount = PlayerState.Instance.Inven.GetItemCount(material.item);
            isCraft = itemCount >= material.count;

            if (isCraft == false)
                break;
        }
        isCraftItem.enabled = isCraft ? false : true;
    }

    public void OnCursorEnter()
    {
        OnShowRecipeInfo?.Invoke(this);
    }
    public void OnCursorExit()
    {
        OnCloseShowRecipeInfo?.Invoke();
    }
    public void OnCursorClick()
    {
        bool isSuccess = PlayerState.Instance.Inven.UseMaterials(ResultRecipe);
        if (isSuccess)
        {
            PlayerState.Instance.Inven.Push(recipe.Result.GetCopy());
        }
        Clear();
    }
}
