using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Appcharge;

public class FlowManager : MonoBehaviour
{
    public RectTransform[] RectTransforms;
    public Image[] FadeImages;
    public CanvasGroup[] CanvasGroups;
    private int _currentIndex = 0;
    private int _currentCoins = 5000;
    [SerializeField] private Text _currentHeartsText;
    [SerializeField] private Text _currentCoinsText;
    [SerializeField] private Text _deeplinkURLText;
    [SerializeField] private CoinManager _coinManager;
    void Start()
    {             
        foreach (var image in RectTransforms)
        {
            image.gameObject.SetActive(false);
        }

        RectTransforms[_currentIndex].gameObject.SetActive(true);
        RectTransforms[_currentIndex + 1].gameObject.SetActive(true);
        Invoke("NextWithFade", 3.2f);
    }

    public void NextWithFade()
    {
        StartCoroutine(FadeCanvasGroup(CanvasGroups[_currentIndex], 1f, 0f, 0.5f));
        _currentIndex++;
        FadeImages[_currentIndex].gameObject.SetActive(true);
    }

    public void ExitShopScene() {
        StartCoroutine(FadeCanvasGroup(CanvasGroups[1], 1f, 0f, 0.3f));
        StartCoroutine(FadeCanvasGroup(CanvasGroups[2], 1f, 0f, 0.3f));
    }

    public void ExitSuccessScene() {
        StartCoroutine(FadeCanvasGroup(CanvasGroups[3], 1f, 0f, 0.3f));
        
        int oldCoins = _currentCoins;
        int newCoins = _currentCoins + 10000;
        StartCoroutine(InterpolateCoins(oldCoins, newCoins, 1.2f));
        _currentCoins = newCoins;
        _coinManager.CreateCoins(20, 0.4f);
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsed = 0f;
        cg.alpha = start;
        while (elapsed < duration)
        {
            cg.alpha = Mathf.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cg.alpha = end;
        RectTransforms[2].gameObject.SetActive(false);
        RectTransforms[3].gameObject.SetActive(false);
        RectTransforms[4].gameObject.SetActive(false);
    }

    public void OpenLeftShop()
    {
        CanvasGroups[1].alpha = 1;
        FadeImages[1].CrossFadeAlpha(1, 1f, false);
        RectTransforms[2].gameObject.SetActive(true);
    }

    public void OpenRightShop()
    {
        CanvasGroups[2].alpha = 1;
        FadeImages[2].CrossFadeAlpha(1, 1f, false);
        RectTransforms[3].gameObject.SetActive(true);
    }

    private IEnumerator InterpolateCoins(int from, int to, float duration) {
        float elapsed = 0f;
        int lastValue = from;
        while (elapsed < duration) {
            float t = elapsed / duration;
            int value = Mathf.RoundToInt(Mathf.Lerp(from, to, t));
            _currentCoinsText.text = value.ToString();
            if (value > lastValue) {
                lastValue = value;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        _currentCoinsText.text = to.ToString();
    }

    public void OnSuccess() {
        ShowLoader(false);
        foreach (var rectTransform in RectTransforms)
        {
            rectTransform.gameObject.SetActive(false);
        }

        _deeplinkURLText.gameObject.SetActive(PurchaseManager.Instance.DebugMode);

        if (_deeplinkURLText.gameObject.activeSelf) {
            string deeplinkURL = PurchaseManager.Instance.DeeplinkURL;
            _deeplinkURLText.text = deeplinkURL;
        }

        CanvasGroups[3].alpha = 1;
        RectTransforms[1].gameObject.SetActive(true);
        RectTransforms[4].gameObject.SetActive(true);
    }

    public void OnFail() {
        ShowLoader(false);
        foreach (var rectTransform in RectTransforms)
        {
            rectTransform.gameObject.SetActive(false);
        }

        RectTransforms[1].gameObject.SetActive(true);
    }

    public void OnClose() {
        ShowLoader(false);
    }

    public void OnCancel() {
        ShowLoader(false);
    }

    public void ShowLoader(bool show) {
        RectTransforms[2].gameObject.SetActive(false);
        RectTransforms[3].gameObject.SetActive(false);
        RectTransforms[5].gameObject.SetActive(show);
        CanvasGroups[4].alpha = show ? 1 : 0;
    }
}