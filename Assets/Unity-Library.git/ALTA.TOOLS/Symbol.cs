using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Alta.Plugin;
namespace Alta.Tools{
    [RequireComponent(typeof(Text)), ExecuteInEditMode]
	public class Symbol : MonoBehaviour {
	    private Text uiText;
	    public string symbol;
		public static FontIcons Fontdata;

        public static void LoadData()
        {
            Fontdata = CssHelper.ReadFile(System.IO.Path.Combine(Application.streamingAssetsPath, "font-awesome.min.css"));
        }

        public static void LoadTextAsset(TextAsset txt)
        {
            Fontdata = txt.csstoFontIcons();
        }
	
	    void Awake()
	    {
	        uiText = GetComponent<Text>();
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(this.symbol) && Application.isPlaying)
                return;
            LoadData();
#endif
	
	    }
	    // Use this for initialization
	    void Start () 
	    {        
	        if (uiText != null && !string.IsNullOrEmpty(this.symbol) && Fontdata!=null)
	        {
	            string tmp = Fontdata[this.symbol.Trim()].Code;
	            if (!string.IsNullOrEmpty(tmp))
	            {
	                uiText.text = tmp;             
	            }
	        }
		}
		
		// Update is called once per frame
		void Update () 
	    {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(this.symbol) && Application.isPlaying)
                return;
	        if (uiText != null && Fontdata!= null)
	        {
	            string tmp = Fontdata[uiText.text.Trim()].Code;
	            if (!string.IsNullOrEmpty(tmp))
	            {
	                uiText.text = tmp;
	            }
	        }
	#endif
		}
	}
}
