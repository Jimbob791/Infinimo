using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class StatManager : MonoBehaviour
{
    public static StatManager instance;

    [Space]

    public DominoManager dominoManager;
    public DominoScore dominoScore;
    public UpgradeManager upgradeManager;
    public ChipManager chipManager;
    public DeckShopGenerator deckShopManager;
    public DeckMenuController deckMenuController;
    public List<PrestigeUpgrade> upgrades = new List<PrestigeUpgrade>();

    [Space]

    [SerializeField] PrestigeUpgrade superMulti;
    [SerializeField] PrestigeUpgrade superBonus;
    [SerializeField] PrestigeUpgrade autoplayUpgrade;
    [SerializeField] PrestigeUpgrade chipMulti;
    [SerializeField] PrestigeUpgrade offlineUpgrade;

    [Space]

    [SerializeField] TextMeshProUGUI offlineText;
    [SerializeField] GameObject offlineEarningsParent;
    [SerializeField] GameObject clickPrefab;

    [HideInInspector] public bool firstPlay;

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


        firstPlay = !File.Exists(Application.persistentDataPath + "/saveData.json");

        if (!firstPlay)
            LoadData();
    }

    void Start()
    {
        if (firstPlay)
        {
            offlineEarningsParent.SetActive(false);
        }
        else
        {
            SaveData();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            WipeData();
    }

    void WipeData()
    {
        File.Delete(Application.persistentDataPath + "/saveData.json");

        Debug.Log("Data Successfully Wiped");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void LoadData()
    {
        SaveData loadData = JsonUtility.FromJson<SaveData>(File.ReadAllText(Application.persistentDataPath + "/saveData.json"));

        dominoScore.score = Math.Ceiling(loadData.pipsBase * System.Math.Pow(10, loadData.pipsPower));
        chipManager.chips = Math.Ceiling(loadData.chipsBase * System.Math.Pow(10, loadData.chipsPower));
        chipManager.totalChips = Math.Ceiling(loadData.totalChipsBase * System.Math.Pow(10, loadData.totalChipsPower));
        chipManager.currentPips = Math.Ceiling(loadData.progressBase * System.Math.Pow(10, loadData.progressPower));

        deckShopManager.lifetimePurchases = loadData.numBoughtDominoes;
        deckShopManager.lifetimeReloads = loadData.numReloads;
        deckMenuController.lifetimeDestroys = loadData.numDestroys;

        for (int i = 0; i < loadData.upgradeData.Count; i++)
        {
            upgrades[i].level = loadData.upgradeData[i].level;
            upgrades[i].type = GetUpgradeType(loadData.upgradeData[i].upgradeType);
        }

        foreach (LineData data in loadData.lineData)
        {
            dominoManager.CreateLine(data.index, data.multiLevel, data.bonusLevel, data.prestige);
        }

        foreach (Line line in dominoManager.lines)
        {
            for (int i = 0; i < line.prestige; i++)
            {
                upgradeManager.AddPrestigeIcon(line, i + 1);
            }
        }

        dominoManager.deck = new List<Domino>();
        foreach (DominoData data in loadData.dominoData)
        {
            dominoManager.AddDomino(data.leftNum, data.rightNum, data.material);
        }
        
        DateTime lastTime = new DateTime(loadData.timeData.year, loadData.timeData.month, loadData.timeData.day, loadData.timeData.hour, loadData.timeData.minute, loadData.timeData.second);
        CalculateOfflineEarnings(lastTime);

        Debug.Log("Data Successfully Loaded");
    }

    public void SaveData()
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
        saveData.numDestroys = deckMenuController.lifetimeDestroys;

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
            newData.material = dominoManager.deck[i].material;

            saveData.dominoData.Add(newData);
        }

        DateTime currentTime = DateTime.Now;
        saveData.timeData = new TimeData();
        saveData.timeData.year = currentTime.Year;
        saveData.timeData.month = currentTime.Month;
        saveData.timeData.day = currentTime.Day;
        saveData.timeData.hour = currentTime.Hour;
        saveData.timeData.minute = currentTime.Minute;
        saveData.timeData.second = currentTime.Second;

        string dataString = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + "/saveData.json", dataString);

        Debug.Log("Data Successfully Saved");
    }

    private float GetPower(double num)
    {
        float power = num == 0 ? 0 : (int)System.Math.Floor((System.Math.Log10(System.Math.Abs(num))));

        return power;
    }

    public string GetUpgradeString(UpgradeType type)
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

    public UpgradeType GetUpgradeType(string type)
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

    private void CalculateOfflineEarnings(DateTime lastTime)
    {
        TimeSpan difference = DateTime.Now - lastTime;
        double totalSeconds = difference.TotalSeconds;

        float deckTotal = 0;
        foreach (Domino domino in dominoManager.deck)
        {
            deckTotal += domino.leftNum + domino.rightNum;
        }
        double deckAverage = deckTotal / dominoManager.deck.Count;

        double multiTotal = 1;
        foreach (Line line in dominoManager.lines)
        {
            multiTotal += line.multiplier * Mathf.Pow(4, line.index) * Mathf.Pow(2, line.prestige);
        }
        double multiAverage = multiTotal / dominoManager.lines.Count;

        double bonusTotal = 0;
        foreach (Line line in dominoManager.lines)
        {
            bonusTotal += line.additive + line.prestige;
        }
        double bonusAverage = bonusTotal / dominoManager.lines.Count;

        double averageBaseScore = (deckAverage + bonusAverage) * multiAverage;
        double superBonusValue = Math.Pow(10, superBonus.level);
        double superMultiValue = Math.Pow(2, superMulti.level);
        double averageTotalScore = (averageBaseScore + superBonusValue) * superMultiValue;

        double scorePerSecond = (averageTotalScore * dominoManager.lines.Count) / (dominoManager.autoplayTime * (10f / (autoplayUpgrade.level + 10f)));
        double totalOfflineEarnings = scorePerSecond * totalSeconds * 0.2f * (1 + (offlineUpgrade.level * 0.5f));
        dominoScore.score += Math.Ceiling(totalOfflineEarnings);
        chipManager.AddProgress(Math.Ceiling(totalOfflineEarnings * Mathf.Pow(2, chipMulti.level)));

        offlineText.text = dominoScore.FormatLargeNumber(Math.Ceiling(totalOfflineEarnings));
    }

    public void Click()
    {
        Instantiate(clickPrefab);
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
    public int numDestroys;

    public List<UpgradeData> upgradeData = new List<UpgradeData>();
    public List<LineData> lineData = new List<LineData>();
    public List<DominoData> dominoData = new List<DominoData>();

    public TimeData timeData;
}

[System.Serializable]
public class TimeData
{
    public int year;
    public int month;
    public int day;
    public int hour;
    public int minute;
    public int second;
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
    public string material;
}