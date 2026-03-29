using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameProgressionSaveManager
{
    public static PlayerManager instance;
    public Player player;

    public int currency;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F1))
        //{
        //    Cheat_Get500Currency();
        //}

        if (currency >= 999999)
        {
            currency = 999999;
        }

    }

    private void Cheat_Get500Currency()
    {
        currency += 500;
    }

    public bool BuyIfAvailable(int _price)
    {
        if (currency < _price)
        {
            Debug.Log("Not enough money!");
            return false;
        }

        currency -= _price;
        return true;
    }

    public int GetCurrentCurrency()
    {
        return currency;
    }

    public void LoadData(GameData _data)
    {
        this.currency = _data.currecny;
    }

    public void SaveData(ref GameData _data)
    {
        _data.currecny = this.currency;
    }
}
