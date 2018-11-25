using UnityEngine;

public class GameController : MonoBehaviour {

    public MeshRenderer displaySurface = null;
    public AudioSource audioSource = null;

    private LibRetroWrapper.RetroWrapper wrapper = null;
    private bool romLoaded = false;

    private string cueName = "TimeCrisis.cue";

    // Use this for initialization
    void Start () {
        Application.targetFrameRate = 60;

        unsafe
        {
#if UNITY_EDITOR
            string systemPath = "D:/roms";
            string romPath = systemPath + "/" + cueName;
#else
            string systemPath = Application.persistentDataPath;
            string romPath = systemPath + "/" + cueName";
#endif
            wrapper = new LibRetroWrapper.RetroWrapper();
            wrapper.systemDirectory = systemPath;

            wrapper._audio = audioSource;
            wrapper.Initialise();

            romLoaded = wrapper.LoadRom(romPath);

            Debug.LogFormat("Loading -> {0} = {1}", romPath, romLoaded);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (wrapper != null)
        {
            ControlUpdate();

            if (romLoaded)
            {
                wrapper.Update();

                if (wrapper.tex != null && displaySurface != null)
                {
                    displaySurface.sharedMaterial.mainTexture = wrapper.tex;
                }
            }
        }
	}

    private void OnDestroy()
    {
        if (wrapper != null)
        {
            Debug.Log("Destroying core");
            wrapper.Shutdown();
        }
    }

    private void ControlUpdate()
    {
        short delta = 400;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            wrapper.gun_x-=delta;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            wrapper.gun_x+=delta;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            wrapper.gun_y -= delta;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            wrapper.gun_y += delta;
        }

        wrapper.trigger = Input.GetKey(KeyCode.Space);
        wrapper.a_button = Input.GetKey(KeyCode.A);
        wrapper.b_button = Input.GetKey(KeyCode.D);
    }
}
