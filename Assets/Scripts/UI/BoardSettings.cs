/// AUTHOR: Matthew Moffitt
/// FILENAME: BoardSettings.cs
/// SPECIFICATION: Changes board settings
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using Catan.Settings;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Catan.UI
{
    /// <summary>
    /// Allows the player to change the size and shape of the board by selecting a preset
    /// </summary>
    public class BoardSettings : MonoBehaviour
    {
        // UI members
        public TextMeshProUGUI presetName;
        public TextMeshProUGUI presetDesc;

        /// <summary>
        /// Changes the preset selected and displayed
        /// </summary>
        /// <param name="delta"></param>
        public void ChangePresetIndex(int delta)
        {
            GameSettings.chosenPreset = (GameSettings.chosenPreset + delta) % GameSettings.presets.Length;
            if (GameSettings.chosenPreset < 0) GameSettings.chosenPreset += GameSettings.presets.Length;
            UpdateUI();
        }

        /// <summary>
        /// Updates the UI
        /// </summary>
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