using Appcharge.Networking;
using Appcharge.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Appcharge
{
    public class PurchaseManager : MonoBehaviour
    {
        public static PurchaseManager Instance { get; private set; }
        [SerializeField] private FlowManager _flowManager;
        [SerializeField] private RectTransform _debugText;
        private bool _debugMode = false;
        private string _deeplinkURL = "";

        public string DeeplinkURL {
            get {
                return _deeplinkURL;
            }
        }

        public bool DebugMode {
            get {
                return _debugMode;
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Application.deepLinkActivated += onDeepLinkActivated;                
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void onDeepLinkActivated(string deeplinkURL) {
            _deeplinkURL = deeplinkURL;
            Debug.Log("Deeplink URL: " + deeplinkURL);

            PurchaseStatus status = PurchaseStatusUtils.ParseStatusFromDeepLink(deeplinkURL);
            
            switch (status)
            {
                case PurchaseStatus.Success:
                    Debug.Log("Purchase successful");
                    _flowManager.OnSuccess();
                    break;
                case PurchaseStatus.Fail:
                    Debug.Log("Purchase failed");
                    _flowManager.OnFail();
                    break;
                case PurchaseStatus.Cancel:
                    Debug.Log("Purchase canceled");
                    _flowManager.OnCancel();
                    break;
                case PurchaseStatus.Close:
                    Debug.Log("Purchase closed");
                    break;
                default:
                    Debug.Log("Unknown purchase status");
                    break;
            }
        }

        public void OpenCheckout() {
            _flowManager.ShowLoader(true);

            StartCoroutine(
                HttpRequest.CreateSession(_debugMode,
                    url => {
                        Application.OpenURL(url);
                    }, 
                    error => {
                        Debug.Log("Failed to create session: " + error);
                    }
                )
            );
        }

        public void EnableDebugMode() {
            _debugMode = !_debugMode;
            _debugText.gameObject.SetActive(_debugMode);
        }
    }
}