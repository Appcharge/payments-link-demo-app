using UnityEngine;
using UnityEngine.UI;

public class CoinShadow : MonoBehaviour
{
    [SerializeField] private Image _image;
    void Start()
    {
        _image.CrossFadeAlpha(0, 0.1f, false);
        Invoke("DestroySelf", 0.1f);
    }

    private void DestroySelf() {
        Destroy(gameObject);
    }
}
