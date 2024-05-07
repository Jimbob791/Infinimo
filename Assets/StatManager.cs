using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class StatManager : MonoBehaviour
{
    public static StatManager instance;

    [Space]

    public DominoManager dominoManager;
    public DominoScore dominoScore;
    public UpgradeManager upgradeManager;
    public ChipManager chipManager;
    public DeckShopGenerator deckShopManager;
    public List<PrestigeUpgrade> upgrades = new List<PrestigeUpgrade>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        LoadData();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            SaveData();
        if (Input.GetKeyDown(KeyCode.R))
            WipeData();
    }

    void WipeData()
    {
        SaveData saveData = new SaveData();

        saveData.pipsPower = 0;
        saveData.pipsBase = 0;

        saveData.chipsPower = 0;
        saveData.chipsBase = 0;
        saveData.totalChipsPower = 0;
        saveData.totalChipsBase = 0;

        saveData.progressPower = 0;
        saveData.progressBase = 0;

        saveData.numBoughtDominoes = 0;
        saveData.numReloads = 0;

        for (int i = 0; i < upgrades.Count; i++)
        {
            UpgradeData newData = new UpgradeData();

            newData.level = 0;
            newData.upgradeType = GetUpgradeString(upgrades[i].type);

            saveData.upgradeData.Add(newData);
        }


        saveData.lineData = new List<LineData>();

        dominoManager.deck = new List<Domino>();
        saveData.dominoData = new List<DominoData>();

        string dataString = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + "/saveData.json", dataString);

        Debug.Log("Data Successfully Wiped");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void LoadData()
    {
        SaveData loadData = JsonUtility.FromJson<SaveData>(File.ReadAllText(Application.persistentDataPath + "/saveData.json"));

        dominoScore.score = loadData.pipsBase * System.Math.Pow(10, loadData.pipsPower);
        chipManager.chips = loadData.chipsBase * System.Math.Pow(10, loadData.chipsPower);
        chipManager.totalChips = loadData.totalChipsBase * System.Math.Pow(10, loadData.totalChipsPower);
        chipManager.currentPips = loadData.progressBase * System.Math.Pow(10, loadData.progressPower);

        deckShopManager.lifetimePurchases = loadData.numBoughtDominoes;
        deckShopManager.lifetimeReloads = loadData.numReloads;

        for (int i = 0; i < loadData.upgradeData.Count; i++)
        {
            upgrades[i].level = loadData.upgradeData[i].level;
            upgrades[i].type = GetUpgradeType(loadData.upgradeData[i].upgradeType);
        }

        foreach (LineData data in loadData.lineData)
        {
            dominoManager.CreateLine(data.index, data.multiLevel, data.bonusLevel, data.prestige);
        }

        dominoManager.deck = new List<Domino>();
        foreach (DominoData data in loadData.dominoData)
        {
            dominoManager.AddDomino(data.leftNum, data.rightNum);
        }

        Debug.Log("Data Successfully Loaded");
    }

    void SaveData()
    {
        SaveData saveData = new SaveData();

        saveData.pipsPower = GetPower(dominoScore.score);
        saveData.pipsBase = (float)(dominoScore.score / System.Math.Pow(10, saveData.pipsPower));

        saveData.chipsPower = GetPower(chipManager.chips);
        saveData.chipsBase = (float)(chipManager.chips / System.Math.Pow(10, saveData.chipsPower));
        saveData.totalChipsPower = GetPower(chipManager.totalChips);
        saveData.totalChipsBase = (float)(chipManager.totalChips / System.Math.Pow(10, saveData.totalChipsPower));

        saveData.progressPower = GetPower(chipManager.currentPips);
        saveData.progressBase = (float)(chipManager.currentPips / System.Math.Pow(10, saveData.progressPower));

        saveData.numBoughtDominoes = deckShopManager.lifetimePurchases;
        saveData.numReloads = deckShopManager.lifetimeReloads;

        for (int i = 0; i < upgrades.Count; i++)
        {
            UpgradeData newData = new UpgradeData();

            newData.level = upgrades[i].level;
            newData.upgradeType = GetUpgradeString(upgrades[i].type);

            saveData.upgradeData.Add(newData);
        }

        for (int i = 0; i < dominoManager.lines.Count; i++)
        {
            LineData newData = new LineData();

            newData.index = dominoManager.lines[i].index;
            newData.multiLevel = dominoManager.lines[i].multiplier;
            newData.bonusLevel = dominoManager.lines[i].additive;
            newData.prestige = dominoManager.lines[i].prestige;

            saveData.lineData.Add(newData);
        }

        for (int i = 0; i < dominoManager.deck.Count; i++)
        {
            DominoData newData = new DominoData();

            newData.leftNum = dominoManager.deck[i].leftNum;
            newData.rightNum = dominoManager.deck[i].rightNum;

            saveData.dominoData.Add(newData);
        }

        string dataString = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + "/saveData.json", dataString);

        Debug.Log("Data Successfully Saved");
    }

    private float GetPower(double num)
    {
        float power = num == 0 ? 0 : (int)System.Math.Floor((System.Math.Log10(System.Math.Abs(num))));

        return power;
    }

    private string GetUpgradeString(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.SuperMulti:
                return "superMulti";
            case UpgradeType.SuperBonus:
                return "superBonus";
            case UpgradeType.ScoreSpeed:
                return "scoreSpeed";
            case UpgradeType.AutoplaySpeed:
                return "autoplaySpeed";
            case UpgradeType.BoneyardDiscount:
                return "boneyardDiscount";
            case UpgradeType.UpgradeDiscount:
                return "upgradeDiscount";
            case UpgradeType.BonusDiscount:
                return "bonusDiscount";
            case UpgradeType.ChipBonus:
                return "chipBonus";
            case UpgradeType.PrestigeDiscount:
                return "prestigeDiscount";
            default:
                return "superMulti";
        }
    }

    private UpgradeType GetUpgradeType(string type)
    {
        switch (type)
        {
            case "superMulti":
                return UpgradeType.SuperMulti;
            case "superBonus":
                return UpgradeType.SuperBonus;
            case "scoreSpeed":
                return UpgradeType.ScoreSpeed;
            case "autoplaySpeed":
                return UpgradeType.AutoplaySpeed;
            case "boneyardDiscount":
                return UpgradeType.BoneyardDiscount;
            case "upgradeDiscount":
                return UpgradeType.UpgradeDiscount;
            case "bonusDiscount":
                return UpgradeType.BonusDiscount;
            case "chipBonus":
                return UpgradeType.ChipBonus;
            case "prestigeDiscount":
                return UpgradeType.PrestigeDiscount;
            default:
                return UpgradeType.SuperMulti;
        }
    }
}

[System.Serializable]
public class SaveData
{
    public float pipsBase;
    public float pipsPower;

    public float chipsBase;
    public float chipsPower;
    public float totalChipsBase;
    public float totalChipsPower;
    public float progressBase;
    public float progressPower;

    public int numBoughtDominoes;
    public int numReloads;

    public List<UpgradeData> upgradeData = new List<UpgradeData>();
    public List<LineData> lineData = new List<LineData>();
    public List<DominoData> dominoData = new List<DominoData>();
}

[System.Serializable]
public class UpgradeData
{
    public int level;
    public string upgradeType;
}

[System.Serializable]
public class LineData
{
    public int index;
    public int multiLevel;
    public int bonusLevel;
    public int prestige;
}

[System.Serializable]
public class DominoData
{
    public int leftNum;
    public int rightNum;
}