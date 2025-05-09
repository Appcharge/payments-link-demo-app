using UnityEngine;
using System.Collections;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private Transform _from;
    [SerializeField] private Transform _to;
    [SerializeField] private Transform _coinContainer;

    public void CreateCoin(float duration) {
        CoinView coin = Instantiate(_coinPrefab, _from.position, Quaternion.identity, _coinContainer).GetComponent<CoinView>();
        coin.StartMove(_from.position, _to.position, duration);
    }

    public void CreateCoins(int count, float duration) {
        StartCoroutine(CreateCoinsCoroutine(count, duration));
    }

    private IEnumerator CreateCoinsCoroutine(int count, float duration) {
        for (int i = 0; i < count; i++) {
            CreateCoin(duration);
            yield return new WaitForSeconds(0.008f);
        }
    }
}
