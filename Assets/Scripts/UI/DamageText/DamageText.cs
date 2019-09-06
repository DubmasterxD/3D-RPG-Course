using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        private Text damageText  = null;

        private void Awake()
        {
            damageText = GetComponentInChildren<Text>();
        }

        public void SetText(string text)
        {
            damageText.text = text;
        }

        public void DestroyText()
        {
            Destroy(gameObject);
        }
    }
}
