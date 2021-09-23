using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "NewItem/Weapon")]
public class Weapon : Item
{
    [SerializeField] Harvest.HarvestType type;
    [SerializeField] int addAttack;
    [SerializeField] float harvestTimeDcrease;
    [SerializeField] GameObject modelObject;

    public int AddAttack => addAttack;
    public float HarvestTimeDcrease => harvestTimeDcrease;
    public Harvest.HarvestType HarvestType => type;
    
    public Weapon(Weapon copy) : base(copy)
    {
        addAttack = copy.addAttack;
        harvestTimeDcrease = copy.harvestTimeDcrease;
        modelObject = copy.modelObject;
    }

    public override Item GetCopy()
    {
        return new Weapon(this);
    }

    protected override bool IsEquals(Item item)
    {
        if(item is Weapon)
        {
            Weapon target = item as Weapon;
            return type == target.type;
        }

        return false;
    }
    public override GameObject GetObject()
    {
        return Instantiate(modelObject);
    }
}
