using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace KILLER
{
    public class InputFieldStartSC : MonoBehaviour
    {
        public TMPro.TMP_Text txt,txt2;
        public bool selected;
        public void Start()
        {
            txt = GetComponentInChildren<TMPro.TMP_Text>();
            txt.text = PlayerPrefs.GetString(txt2.text);
        }
        public void Select()
        {
            if (GameObject.Find("SettingsCanvas").GetComponent<Setting>().selected == false)
            {
                GetComponent<Image>().color = new Color32(186, 45, 11, 255);
                selected = true;
                GameObject.Find("SettingsCanvas").GetComponent<Setting>().selected = true;
            }
        }
        void OnGUI()
        {
            Event e = Event.current;
            if (e.isKey && selected)
            {
                GetComponent<Image>().color = new Color32(242, 100, 25,255);
                Debug.Log("Detected key code: " + e.keyCode);
                txt.text = e.keyCode.ToString();
                selected = false;
                GameObject.Find("SettingsCanvas").GetComponent<Setting>().selected = false;
                PlayerPrefs.SetString(txt2.text, e.keyCode.ToString());
            }
        }
    }
}
