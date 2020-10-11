using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DropSystem : MonoBehaviour
{
    public int maxDropItem = 1;
    int dropItemNumber;

    public float dropRadius;

    [Range(0, 1)] public float dropRateG;
    [Range(0, 1)] public float dropMultiplier;
    public DropItem[] dropItems;

    public void SortItemPos(Transform t,Vector2 origin, float radius, LayerMask obstructMask)
    {
        // define random items
        GameObject itemToSpawn = null;
        while(dropItemNumber < maxDropItem)
        {
            float dropRate = dropRateG *Mathf.Pow(dropMultiplier, dropItemNumber);
            itemToSpawn = SortItem(dropRate);

            if (itemToSpawn != null)
            {
                // define a position to spawn
                Transform tSpawn = t;
                tSpawn.position = SortPos(origin, radius, obstructMask);
                Instantiate(itemToSpawn, tSpawn);

                dropItemNumber++;
            }
            else
                dropItemNumber = maxDropItem;
        }
    }
    public GameObject SortItem( float dropRate)
    {
        GameObject itemToReturn = null;
        if( Random.value <= dropRate && dropItems.Length > 0)
        {
            float rValue = Random.value;
            float rLimit = 0;

            UpdateDropRateItem(dropItems);

            for (int i = 0; i < dropItems.Length; i++)
            {
               if(rValue >= rLimit && rValue < dropItems[i].dropRate)
                    itemToReturn = dropItems[i].itemToDrop;

                rLimit += dropItems[i].dropRate;
            }
        }
        return itemToReturn;
    }
    public Vector2 SortPos(Vector2 origin, float maxRadius, LayerMask obstructMask)
    {
        Vector2 randomDir = new Vector2(Random.value-0.5f, Random.value-0.5f).normalized;
        float currentRadius = maxRadius * Random.value;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, randomDir, currentRadius, obstructMask);

        if (hit)
            currentRadius = hit.distance * 0.9f;

        return origin + randomDir* currentRadius;
    }

    public void UpdateDropRateItem(DropItem[] dropItems)
    {
        if (dropItems.Length > 0)
        {
            float dropRatio = 0;
            foreach (DropItem dI in dropItems)
                dropRatio += dI.dropRate;// update dropRatio

            for (int i = 0; i < dropItems.Length; i++)
            {
                if (dropRatio != 0)
                    dropItems[i].dropRate /= dropRatio;// rebase dropRate between 0 & 1
            }
        }
    }
    [System.Serializable]
    public struct DropItem
    {
        public GameObject itemToDrop;
        [Range(0,1)]
        public float dropRate;
    }
}

