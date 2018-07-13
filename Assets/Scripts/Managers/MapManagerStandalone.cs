using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManagerStandalone : MonoBehaviour {

    public ItemSpawnPoint itemSpawnPoint;
    private List<float> RandomList = new List<float>();
    private int ItemListIndex = 0;
    public int Seed = 1;
    private float[] ProbabilityValue = null;
    private int[] ItemsID = null;
    private float totalProbabilityValue = 0;

    /// <summary>
    /// 地面物件字典,GroundItemID唯一标识
    /// </summary>
    public Dictionary<int, GroundItem> Items = new Dictionary<int, GroundItem>();

    // Use this for initialization
    void Start ()
    {
        ResourcesManager.Instance.LoadItems_sync();
        ResourcesManager.Instance.LoadUITextures();
        ItemInfoManager.Instance.LoadInfo();


        ProbabilityValue = ItemInfoManager.Instance.GetTotalOccurrenceProbability();
        ItemsID = ItemInfoManager.Instance.GetAllItemsID();
        GenerateStandalone(Seed);
    }
	
	// Update is called once per frame
	void Update () {
       
    }

    public void GenerateStandalone(int seed)
    {

        RandomList = InitTotalProbabilityValue(ProbabilityValue);
        GenerateItem();
    }

    private List<float> InitTotalProbabilityValue(float[] probabilityValue)
    {
        if (totalProbabilityValue == 0)
        {
            for (int i = 0; i < probabilityValue.Length; i++)
            {
                totalProbabilityValue += probabilityValue[i];
            }
        }
        List<float> temp_RandomList = new List<float>();
        Random.InitState(Seed);
        for (int i = 0; i < itemSpawnPoint.ItemSpawnPoints.Length; i++)
        {
            temp_RandomList.Add(Random.Range(0, totalProbabilityValue));
        }
        return temp_RandomList;
    }

    private int CalcIndex(int RandomListIndex, float[] probabilityValue)
    {

        for (int i = 0; i < probabilityValue.Length; i++)
        {
            if (RandomList[RandomListIndex] < probabilityValue[i])
            {
                return i;
            }
            else
            {
                RandomList[RandomListIndex] -= probabilityValue[i];
            }
        }
        return probabilityValue.Length - 1;

    }

    void GenerateItem()
    {
        //地上物品生成：
        GameObject tmp = null;
        int tmp_ID = 0;
        GroundItem tmp_groundItem;

        for (int j = 0; j < itemSpawnPoint.ItemSpawnPoints.Length; j++)
        {
            tmp_ID = ItemsID[CalcIndex(j, ProbabilityValue)];
            //Debug.LogError("Index: "+j +" ID: "+tmp_ID+" ResName: "+"ResName: "+ItemInfoManager.Instance.GetResname(tmp_ID) +" ItemName: "+ ItemInfoManager.Instance.GetItemName(tmp_ID));
            tmp = Instantiate(ResourcesManager.Instance.GetItem(ItemInfoManager.Instance.GetResname(tmp_ID)),
                new Vector3(itemSpawnPoint.ItemSpawnPoints[j].position.x, 0, itemSpawnPoint.ItemSpawnPoints[j].position.z), Quaternion.Euler(new Vector3(-90, 0, 0)));
            tmp_groundItem = tmp.AddComponent<GroundItem>();
            tmp_groundItem.ItemID = tmp_ID;
            tmp_groundItem.GroundItemID = j;
            //tmp.transform.SetParent(itemSpawnPoint.ItemSpawnPoints[j]);
            Items.Add(j, tmp_groundItem);
        }
    }
}
