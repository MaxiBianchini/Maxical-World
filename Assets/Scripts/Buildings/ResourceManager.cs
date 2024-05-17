using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; } //this Singleton can be only modify by this class (private set)

    public event EventHandler OnResourceAmountChanged; //Event that will be executed when a resource amount has changed.

    [SerializeField] private List<ResourceAmount> startingResourceAmountList; // Just gold for now
    [SerializeField] private int pasiveGoldAmount;
    
    private Dictionary<ResourceTypeSO, int> resourceAmountDictionary; //A dictionary for all resources and amounts haderer

    private void Awake()
    {
        Instance = this;
        resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();

        //Load resourceTypeList into an object without creating an empty object. Unity looks for it in Resources folder
        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name); 

        //Initialise all resources in 0 at the beginning
        foreach(ResourceTypeSO resourceType in resourceTypeList.list)
        {
            resourceAmountDictionary[resourceType] = 0;
        }

        foreach(ResourceAmount resourceAmount in startingResourceAmountList)
        {
            AddResource(resourceAmount.resourceType, resourceAmount.amount);
        }

        StartCoroutine(PasiveAddResourceRoutine(startingResourceAmountList[0].resourceType, pasiveGoldAmount));
        

    }

    private IEnumerator PasiveAddResourceRoutine(ResourceTypeSO goldResource, int goldAmount)
    {
        AddResource(goldResource, goldAmount);
        yield return new WaitForSeconds(10f);

        StartCoroutine(PasiveAddResourceRoutine(goldResource, goldAmount));
    }

   

    private void TestLogResourceAmountDictionary()
    {
        foreach (ResourceTypeSO resourceType in resourceAmountDictionary.Keys)
        {
            Debug.Log(resourceType.nameString + ": " + resourceAmountDictionary[resourceType]); 
        }
    }

    //Method to add resources to the player
    public void AddResource(ResourceTypeSO resourceType, int amount)
    {
        resourceAmountDictionary[resourceType] += amount;
        //Debug.Log(resourceAmountDictionary[resourceType]);
        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty); //Send the event with this changes and 2nd parameter empty as default
        
    }


    //Return the amount of resources
    public int GetResourceAmount(ResourceTypeSO resourceType)
    {
        return resourceAmountDictionary[resourceType];
    }

    public bool CanAfford(ResourceAmount[] resourceAmountArray)
    {
        foreach(ResourceAmount resourceAmount in resourceAmountArray)
        {
            if(GetResourceAmount(resourceAmount.resourceType) >= resourceAmount.amount)
            {
                //Can afford
                return true;
            }
            else
            {
                //Cannot afford this
                return false;
            }
        }

        return true;
    }


    public void SpendResources(ResourceAmount[] resourceAmountArray)
    {
        foreach (ResourceAmount resourceAmount in resourceAmountArray)
        {
            resourceAmountDictionary[resourceAmount.resourceType] -= resourceAmount.amount;
            OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
        }
    }


}
