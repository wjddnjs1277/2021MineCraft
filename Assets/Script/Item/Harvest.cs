using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvest : MonoBehaviour
{
    [System.Flags]
    public enum HarvestType
    {
        Grass = 1 << 0,
        Rock = 1 << 1,
        Wood = 1 << 2,
        Leaf = 1 << 3,
        Snow = 1 << 4,
        Soil = 1 << 5,
    }

    [SerializeField] HarvestType type;
    [SerializeField] protected Item item;
    [SerializeField] protected float farmingTime;

    public float FarmingTime => farmingTime;

    public void OnHarvesting()
    {
        ItemObject itemObj = item.GetDropObject();
        itemObj.transform.position = transform.position;
        itemObj.GetComponent<Rigidbody>().AddForce(Vector3.up * 1.5f, ForceMode.Impulse);

        if( type == HarvestType.Wood || type == HarvestType.Leaf)
        {

        }
        else
            GroundGenerator.Instance.VisualBlock(transform.position);

        Destroy(gameObject);
    }
    public bool IsHarvesting(HarvestType type)
    {
        Debug.Log("is harvesting : " + type.ToString());

        if ((type & this.type) == 0)
        {
            Debug.Log("FALSE");
            return false;
        }

        Debug.Log("TRUE");
        return true;
    }
}
