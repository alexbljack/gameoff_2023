using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AccountData 
{
    public string path;
    public string KingdomName;
    public int GameResult;
    public int YearsOnThrone;
}

public class Account : MonoBehaviour
{
    public string KingdomName;
    public int GameResult;
    public int YearsOnThrone;

    public void ClearData() 
    {
        KingdomName = "";
        GameResult = 0;
        YearsOnThrone = 0;
    }

    public void SaveData() 
    {
        var accPath = $"{Application.persistentDataPath}/Account/";
        if (!Directory.Exists(accPath))
            Directory.CreateDirectory(accPath);
        var accountFilePath = $"{accPath}Account_{DateTime.Now:dd_MM_yyyy_hh_mm_ss}.dat";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(accountFilePath);

        var accountData = new AccountData();
        accountData.KingdomName = KingdomName;
        accountData.GameResult = GameResult;
        accountData.YearsOnThrone = YearsOnThrone;

        bf.Serialize(file, accountData);
        file.Close();
        Debug.Log($"Account data saved! Path: {accountFilePath}");
    }

    public List<AccountData> LoadData()
    {
        DirectoryInfo fileInfo = new DirectoryInfo($"{Application.persistentDataPath}/Account/");
        BinaryFormatter bf = new BinaryFormatter();
        var accountsData = new List<AccountData>();
        if (fileInfo != null)
        {
            foreach (var item in fileInfo.GetFiles()) 
            {
                FileStream file = File.Open(item.FullName, FileMode.Open);
                AccountData data = (AccountData)bf.Deserialize(file);
                data.path = item.FullName;
                accountsData.Add(data);
                file.Close();
                Debug.Log($"Account data loaded! Path: {item.FullName}");
            }
        }

        return accountsData;
    }
}
