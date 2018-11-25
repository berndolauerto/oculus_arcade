using System;
using System.Runtime.InteropServices;
using UnityEngine;

// based on: https://github.com/lurrrrr/RetroNUI/tree/master/Wrapper
namespace LibRetroWrapper
{
    public class RetroWrapper
    {
        // ====================================================================================================
        // the active ingredients
        private RetroStructs.RetroGameInfo gameInfo = new RetroStructs.RetroGameInfo();
        private string rom_path;
        private RetroEnums.RetroPixelFormat pixelFormat;
        //private RetroStructs.RetroVfsInterface vfsInterface = new RetroStructs.RetroVfsInterface();

        public bool requiresFullPath;
        public Texture2D tex = null;
        public AudioSource _audio = null;
        public string systemDirectory;

        private System.Collections.Generic.Dictionary<string, string> environment_settings = new System.Collections.Generic.Dictionary<string, string>();

        //Prevent GC on delegates as long as the wrapper is running
        private DelegateDefinition.RetroEnvironmentDelegate _environment;
        private DelegateDefinition.RetroVideoRefreshDelegate _videoRefresh;
        private DelegateDefinition.RetroAudioSampleDelegate _audioSample;
        private DelegateDefinition.RetroAudioSampleBatchDelegate _audioSampleBatch;
        private DelegateDefinition.RetroInputPollDelegate _inputPoll;
        private DelegateDefinition.RetroInputStateDelegate _inputState;
        private DelegateDefinition.RetroLogDelegate _logDelegate;

        public static byte[] source;
        public static byte[] dest;

        private uint current_width = 0;
        private uint current_height = 0;
        private uint current_pitch = 0;

        public bool trigger = false;
        public bool a_button = false;
        public bool b_button = false;
        public Int16 gun_x = 0;
        public Int16 gun_y = 0;

        private unsafe void RetroVideoRefresh(void* data, uint width, uint height, uint pitch)
        {
            IntPtr pixels = (IntPtr)data;
            
            switch (pixelFormat)
            {
                case RetroEnums.RetroPixelFormat.RETRO_PIXEL_FORMAT_XRGB8888:
                    {
                        int sourceSize = (int)(pitch * height);
                        int destSize = (int)(width * height * sizeof(uint));

                        if (tex == null || dest == null || width != current_width || height != current_height)
                        {
                            tex = new Texture2D((int)width, (int)height, TextureFormat.BGRA32, false);

                            source = new byte[sourceSize];
                            dest = new byte[destSize];
                        }

                        current_width = width;
                        current_height = height;
                        current_pitch = pitch;

                        Marshal.Copy(pixels, source, 0, sourceSize);
                        
                        int idx = 0;
                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width * sizeof(uint); x++)
                            {
                                dest[idx] = source[pitch*y + x];
                                idx++;
                            }
                        }
                        
                        tex.LoadRawTextureData(dest);
                        tex.filterMode = FilterMode.Point;

                        tex.Apply();
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private unsafe void RetroAudioSample(short left, short right)
        {
            throw new NotImplementedException();
        }
        
        public unsafe void RetroAudioSampleBatch(short* data, uint frames)
        {
            if (_audio == null)
            {
                return;
            }

            short* ptr = data;
            float[] samples = new float[frames];

            for (int i = 0; i < frames; i++)
            {
                float value = Mathf.Clamp(*ptr / 32768.0f,-1.0f,1.0f);
                samples[i] = value;
                ptr += sizeof(short);
            }

            AudioClip clip = AudioClip.Create("sample", (int)frames, 1, 44100, false);
            clip.SetData(samples, 0);
            _audio.PlayOneShot(clip);
        }

        public unsafe void RetroInputPoll()
        {
            
        }

        public unsafe Int16 RetroInputState(uint port, uint device, uint index, uint id)
        {
            if (port == 0 && device == (uint)RetroEnums.RetroDevices.RETRO_DEVICE_LIGHTGUN)
            {
                if (id == (uint)RetroEnums.RetroLightgun.RETRO_DEVICE_ID_LIGHTGUN_SCREEN_X)
                {
                    return gun_x;
                }
                if (id == (uint)RetroEnums.RetroLightgun.RETRO_DEVICE_ID_LIGHTGUN_SCREEN_Y)
                {
                    return gun_y;
                }
                if (id == (uint)RetroEnums.RetroLightgun.RETRO_DEVICE_ID_LIGHTGUN_TRIGGER)
                {
                    return (short)(trigger ? 1 : 0);
                }
                if (id == (uint)RetroEnums.RetroLightgun.RETRO_DEVICE_ID_LIGHTGUN_AUX_A)
                {
                    return (short)(a_button ? 1 : 0);
                }
                if (id == (uint)RetroEnums.RetroLightgun.RETRO_DEVICE_ID_LIGHTGUN_AUX_B)
                {
                    return (short)(b_button ? 1 : 0);
                }
            }
            return 0;
        }

        public unsafe void RetroLogCallback(int log_level, string format, IntPtr args)
        {
            Debug.Log(format);
        }

        private bool _ejected = false;
        public unsafe bool RetroSetEjectState(bool ejected)
        {
            Debug.LogFormat("Set Ejected State: {0}", ejected);
            _ejected = ejected;
            return true;
        }

        public unsafe bool RetroGetEjectState()
        {
            Debug.LogFormat("Get Ejected State: {0}", _ejected);
            return _ejected;
        }

        uint _image_index = 0;
        public unsafe uint RetroGetImageIndex()
        {
            Debug.LogFormat("Get Image Index: {0}", _image_index);
            return _image_index;
        }

        public unsafe bool RetroSetImageIndex(uint index)
        {
            Debug.LogFormat("Set Image Index: {0}", index);
            _image_index = index;
            return true;
        }

        uint _num_images = 1;
        public unsafe uint RetroGetNumImages()
        {
            Debug.LogFormat("Get Num Images: {0}", _num_images);
            return _num_images;
        }

        public unsafe bool RetroReplaceImageIndex(uint index, ref RetroStructs.RetroGameInfo info)
        {
            throw new NotImplementedException();
        }

        public unsafe bool RetroAddImageIndex()
        {
            throw new NotImplementedException();
        }
        
        public unsafe bool RetroEnvironment(uint cmd, void * data)
        {
            IntPtr ptr = new IntPtr(data);

            RetroEnums.ConfigurationConstants constant = (RetroEnums.ConfigurationConstants)cmd;
            
            switch (constant)
            {
                case RetroEnums.ConfigurationConstants.RETRO_ENVIRONMENT_GET_CAN_DUPE:
                    bool* canDupe = (bool*)data;
                    *canDupe = false;
                    break;

                case RetroEnums.ConfigurationConstants.RETRO_ENVIRONMENT_SET_PERFORMANCE_LEVEL:
                    int* performanceLevel = (int*)data;
                    Debug.LogFormat("Core Performance level {0}", *performanceLevel);
                    break;

                case RetroEnums.ConfigurationConstants.RETRO_ENVIRONMENT_GET_SYSTEM_DIRECTORY:
                    Debug.LogFormat("System directory: {0}", systemDirectory);

                    char** sysDirPtr = (char**)data;
                    (*sysDirPtr) = StringToChar(systemDirectory);

                    break;

                case RetroEnums.ConfigurationConstants.RETRO_ENVIRONMENT_GET_LOG_INTERFACE:
                    RetroStructs.RetroLog * retroLog = (RetroStructs.RetroLog*)data;
                    retroLog->log = Marshal.GetFunctionPointerForDelegate(_logDelegate);
                    break;
                    
                case RetroEnums.ConfigurationConstants.RETRO_ENVIRONMENT_SET_MESSAGE:
                    RetroStructs.RetroMessage* msg = (RetroStructs.RetroMessage*)data;
                    string msgStr = Marshal.PtrToStringAnsi((IntPtr)msg->msg);
                    Debug.LogFormat("Message: {0}", msgStr);
                    break;

                case RetroEnums.ConfigurationConstants.RETRO_ENVIRONMENT_SET_PIXEL_FORMAT:
                    pixelFormat = *(RetroEnums.RetroPixelFormat*)data;
                    Debug.LogFormat("Pixel format - {0}", pixelFormat.ToString());
                    break;

                case RetroEnums.ConfigurationConstants.RETRO_ENVIRONMENT_GET_SAVE_DIRECTORY:
                    char** saveDir = (char**)data;
                    (*saveDir) = StringToChar(systemDirectory);
                    break;

                case RetroEnums.ConfigurationConstants.RETRO_ENVIRONMENT_GET_VARIABLE_UPDATE:
                    {
                        bool* hasUpdated = (bool*)data;
                        *hasUpdated = false;
                    }
                    break;

                case RetroEnums.ConfigurationConstants.RETRO_ENVIRONMENT_SET_VARIABLES:
                    if (data != null)
                    {
                        {
                            RetroStructs.RetroVariable* variable = (RetroStructs.RetroVariable*)data;
                            while (variable->key != null)
                            {
                                string key = Marshal.PtrToStringAnsi((IntPtr)variable->key);
                                string value = Marshal.PtrToStringAnsi((IntPtr)variable->value);

                                string options = value.Split(';')[1];
                                string default_option = options.Split('|')[0].Trim();

                                environment_settings.Add(key, default_option);

                                variable++;
                            }
                        }
                    }

                    return true;

                case RetroEnums.ConfigurationConstants.RETRO_ENVIRONMENT_GET_VARIABLE:
                    {
                        RetroStructs.RetroVariable* variable = (RetroStructs.RetroVariable*)data;
                        string key = Marshal.PtrToStringAnsi((IntPtr)variable->key);
                        try
                        {
                            string value = environment_settings[key];
                            variable->value = StringToChar(value);
                            return true;
                        }
                        catch (System.Collections.Generic.KeyNotFoundException e)
                        {
                            return false;
                        }
                    }

                case RetroEnums.ConfigurationConstants.RETRO_ENVIRONMENT_GET_VFS_INTERFACE:
                    return false;

                case RetroEnums.ConfigurationConstants.RETRO_ENVIRONMENT_SET_DISK_CONTROL_INTERFACE:
                    return false;

                case RetroEnums.ConfigurationConstants.RETRO_ENVIRONMENT_GET_RUMBLE_INTERFACE:
                    return false;

                case RetroEnums.ConfigurationConstants.RETRO_ENVIRONMENT_SET_CONTROLLER_INFO:
                    RetroStructs.RetroControllerInfo* controllerInfo = (RetroStructs.RetroControllerInfo*)data;
                    while(controllerInfo->types != null)
                    {
                        RetroStructs.RetroControllerDescription* description = (RetroStructs.RetroControllerDescription*)controllerInfo->types;

                        string descText = Marshal.PtrToStringAnsi((IntPtr)description->desc);
                        uint id = description->id;
                        Debug.LogFormat("#### Controller {0} description: {1}", id, descText);

                        controllerInfo++;
                    }
                    return true;

                case RetroEnums.ConfigurationConstants.RETRO_ENVIRONMENT_SET_INPUT_DESCRIPTORS:
                    RetroStructs.RetroInputDescriptor* inputs = (RetroStructs.RetroInputDescriptor*)data;
                    while(inputs->description != null)
                    {
                        uint port = inputs->port;
                        uint device = inputs->device;
                        uint index = inputs->index;
                        uint id = inputs->id;

                        string descText = Marshal.PtrToStringAnsi((IntPtr)inputs->description);

                        Debug.LogFormat("### Port: {0} Device: {1} Index: {2} Id: {3} Desc:{4}",
                            port, device, index, id, descText);

                        inputs++;
                    }
                    break;

                default:
                    Debug.LogFormat("Unimplemented environment constant: {0}", constant.ToString());
                    return false;
            }

            return true;
        }

        private unsafe void Init()
        {
            int _apiVersion = RetroImports.RetroApiVersion();
            RetroStructs.RetroSystemInfo info = new RetroStructs.RetroSystemInfo();
            RetroImports.RetroGetSystemInfo(ref info);

            string _coreName = Marshal.PtrToStringAnsi((IntPtr)info.library_name);
            string _coreVersion = Marshal.PtrToStringAnsi((IntPtr)info.library_version);
            string _validExtensions = Marshal.PtrToStringAnsi((IntPtr)info.valid_extensions);
            requiresFullPath = info.need_fullpath;
            bool _blockExtract = info.block_extract;

            Debug.LogFormat("CoreName: {0}", _coreName);
            Debug.LogFormat("CoreVersion: {0}", _coreVersion);
            Debug.LogFormat("Valid Extensions: {0}", _validExtensions);
            Debug.LogFormat("Requires Full Path: {0}", requiresFullPath);

            _environment = new DelegateDefinition.RetroEnvironmentDelegate(RetroEnvironment);
            _videoRefresh = new DelegateDefinition.RetroVideoRefreshDelegate(RetroVideoRefresh);
            _audioSample = new DelegateDefinition.RetroAudioSampleDelegate(RetroAudioSample);
            _audioSampleBatch = new DelegateDefinition.RetroAudioSampleBatchDelegate(RetroAudioSampleBatch);
            _inputPoll = new DelegateDefinition.RetroInputPollDelegate(RetroInputPoll);
            _inputState = new DelegateDefinition.RetroInputStateDelegate(RetroInputState);
            _logDelegate = new DelegateDefinition.RetroLogDelegate(RetroLogCallback);

            RetroImports.RetroSetEnvironment(_environment);
            RetroImports.RetroSetVideoRefresh(_videoRefresh);
            RetroImports.RetroSetAudioSample(_audioSample);
            RetroImports.RetroSetAudioSampleBatch(_audioSampleBatch);
            RetroImports.RetroSetInputPoll(_inputPoll);
            RetroImports.RetroSetInputState(_inputState);
            
            RetroImports.RetroInit();
        }

        public static unsafe char * StringToChar(string s)
        {
            IntPtr p = Marshal.StringToHGlobalAnsi(s);
            return (char*)(p.ToPointer());
        }

        public void Initialise()
        {
            Init();
        }

        public void Update()
        {
            RetroImports.RetroRun();
        }

        public void Shutdown()
        {
            RetroImports.RetroDeInit();
        }

        public unsafe bool LoadRom(string pathToRom)
        {
            rom_path = pathToRom;
            gameInfo.path = StringToChar(rom_path);
            
            bool ret = RetroImports.RetroLoadGame(ref gameInfo);

            if (ret)
            {
                RetroStructs.RetroSystemAVInfo av = new RetroStructs.RetroSystemAVInfo();
                RetroImports.RetroGetSystemAVInfo(ref av);

                // annoyingly mednafen maps guncon to 260 NOT 4 which is the lightgun value
                RetroImports.RetroSetControllerPortDevice(0, 260);
            }

            return ret;
        }
    }
}
