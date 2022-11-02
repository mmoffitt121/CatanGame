using Catan.Settings;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Catan.UI
{

    public class BoardSettings : MonoBehaviour
    {
        public TextMeshProUGUI presetName;
        public TextMeshProUGUI presetDesc;
        public void ChangePresetIndex(int delta)
        {
            GameSettings.chosenPreset = (GameSettings.chosenPreset + delta) % GameSettings.presets.Length;
            if (GameSettings.chosenPreset < 0) GameSettings.chosenPreset += GameSettings.presets.Length;
            UpdateUI();
        }

        public void UpdateUI()
        {
            presetName.text = GameSettings.presets[GameSettings.chosenPreset].name;
            presetDesc.text = GameSettings.presets[GameSettings.chosenPreset].desc;
        }

        public void Start()
        {
            UpdateUI();
        }
    }
}