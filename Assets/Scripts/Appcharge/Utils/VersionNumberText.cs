using UnityEngine;
using UnityEngine.UI;

public class VersionNumberText : MonoBehaviour
{
    [SerializeField] private Text _versionNumberText;

    void Start()
    {
        _versionNumberText.text = "v" + "6";
    }
}