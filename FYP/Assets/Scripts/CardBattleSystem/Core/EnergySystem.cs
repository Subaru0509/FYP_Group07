using UnityEngine;

namespace CardBattleSystem
{
    public class EnergySystem : MonoBehaviour
    {
        public static EnergySystem Instance { get; private set; }

        [SerializeField] private int maxEnergy = 3;
        [SerializeField] private int currentEnergy;

        public int CurrentEnergy => currentEnergy;
        public int MaxEnergy => maxEnergy;

        public System.Action<int, int> OnEnergyChanged; // currentEnergy, maxEnergy

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            currentEnergy = maxEnergy;
        }

        public void RestoreEnergy()
        {
            currentEnergy = maxEnergy;
            OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
        }

        public void ResetEnergy()
        {
            currentEnergy = maxEnergy;
            OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
        }

        public bool CanSpend(int amount)
        {
            return currentEnergy >= amount;
        }

        public bool SpendEnergy(int amount)
        {
            if (!CanSpend(amount))
                return false;

            currentEnergy -= amount;
            OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
            return true;
        }

        public void AddEnergy(int amount)
        {
            currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
            OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
        }

        public void SetMaxEnergy(int newMax)
        {
            maxEnergy = newMax;
            currentEnergy = Mathf.Min(currentEnergy, maxEnergy);
            OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
        }
    }
}
