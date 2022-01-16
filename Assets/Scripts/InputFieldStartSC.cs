using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace KILLER
{
    public class InputFieldStartSC : MonoBehaviour
    {
        public TMPro.TMP_Text txt,txt2;
        public bool selected;
        public KeyCode input;
        public void Start()
        {
            txt = GetComponentInChildren<TMPro.TMP_Text>();
            txt.text = PlayerPrefs.GetString(txt2.text);
            input = (KeyCode)System.Enum.Parse(typeof(KeyCode), txt.text);
        }
        public void Select()
        {
            if (GameObject.Find("SettingsCanvas").GetComponent<Setting>().selected == false)
            {
                GetComponent<Image>().color = new Color32(255, 104, 0, 255);
                selected = true;
                GameObject.Find("SettingsCanvas").GetComponent<Setting>().selected = true;
            }
        }
        void OnGUI()
        {
            Event e = Event.current;
            if (e.isKey && selected)
            {
                GetComponent<Image>().color = new Color32(204, 133, 42,255);
                Debug.Log("Detected key code: " + e.keyCode);
                txt.text = e.keyCode.ToString();
                selected = false;
                GameObject.Find("SettingsCanvas").GetComponent<Setting>().selected = false;
                PlayerPrefs.SetString(txt2.text, e.keyCode.ToString());

                input = (KeyCode)System.Enum.Parse(typeof(KeyCode), e.keyCode.ToString());
                GetComponentInParent<Setting>().SetInputs();
            }
        }
    }
}
