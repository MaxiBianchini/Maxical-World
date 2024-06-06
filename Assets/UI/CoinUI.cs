using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CoinUI : MonoBehaviour
    {
        [SerializeField] private Text text;

        private float _coins;
        private void Update()
        {
            _coins = CoinManager.Instance.GetTotalCoins();
            text.text = $"Coins: {_coins}";
        }
    }
}
