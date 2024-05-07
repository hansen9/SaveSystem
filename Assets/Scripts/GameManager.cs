using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour, IDataPersistence
{
    
    public int level = 0;
    public TextMeshProUGUI levelText;

    
    public void Increment()
    {
        if (levelText != null)
        {
            ++level;
            levelText.text = level.ToString();
        }
    }
 
    public void Decrement()
    {
        if (levelText != null)
        {
            --level;
            levelText.text = level.ToString();
        }
    }

    public void LoadData(GameData data)
    {
        this.level = data.level;
        levelText.text = data.level.ToString();
    }

    public void SaveData(GameData data)
    {
        data.level = this.level;
    }
}
