using UnityEngine;
using UnityEngine.UI;
using System;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;
        Text healthText;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            healthText = GetComponent<Text>();
        }

        void Update()
        {
            if (fighter.currentTarget != null)
            {
                healthText.text = String.Format("{0:0}%", fighter.currentTarget.GetPercentage());
            }
            else
            {
                healthText.text = "N/A";
            }
        }
    }
}
