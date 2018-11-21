﻿using System;
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
        public const int AudioBatchSize = 4096;
        public static float[] AudioBatch = new float[AudioBatchSize];
        public static int BatchPosition;
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

        private DelegateDefinition.RetroSetEjectState _setEjectState;
        private DelegateDefinition.RetroGetEjectState _getEjectState;
        private DelegateDefinition.RetroGetImageIndex _getImageIndex;
        private DelegateDefinition.RetroSetImageIndex _setImageIndex;
        private DelegateDefinition.RetroGetNumImages _getNumImages;
        private DelegateDefinition.RetroReplaceImageIndex _replaceImageIndex;
        private DelegateDefinition.RetroAddImageIndex _retroAddImageIndex;

        public static byte[] source;
        public static byte[] dest;

        private uint current_width = 0;
        private uint current_height = 0;
        private uint current_pitch = 0;

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
                            Debug.LogFormat("Width: {0} Height: {1} Pitch: {2}", width, height, pitch);
                            tex = new Texture2D((int)width, (int)height, TextureFormat.BGRA32, false);

                            source = new byte[sourceSize];
                            dest = new byte[destSize];
                        }

                        current_width = width;
                        current_height = height;
                        current_pitch = pitch;

                        Marshal.Copy(pixels, source, 0, destSize);
                        
                        int idx = 0;
                        int offset = 0;
                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width * sizeof(uint); x++)
                            {
                                dest[idx] = source[offset + x];
                                idx++;
                            }
                            offset += (int)pitch;
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

        private float[] samples = null;
        public unsafe void RetroAudioSampleBatch(short* data, uint frames)
        {
            if (_audio == null)
            {
                return;
            }

            short* ptr = data;

            if (samples == null || samples.Length < frames)
            {
                samples = new float[frames];
            }

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
            // todo
        }

        public unsafe ushort RetroInputState(uint port, uint device, uint index, uint id)
        {
            // todo
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

            _setEjectState = new DelegateDefinition.RetroSetEjectState(RetroSetEjectState);
            _getEjectState = new DelegateDefinition.RetroGetEjectState(RetroGetEjectState);
            _getImageIndex = new DelegateDefinition.RetroGetImageIndex(RetroGetImageIndex);
            _setImageIndex = new DelegateDefinition.RetroSetImageIndex(RetroSetImageIndex);
            _getNumImages = new DelegateDefinition.RetroGetNumImages(RetroGetNumImages);
            _replaceImageIndex = new DelegateDefinition.RetroReplaceImageIndex(RetroReplaceImageIndex);
            _retroAddImageIndex = new DelegateDefinition.RetroAddImageIndex(RetroAddImageIndex);

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
            }

            return ret;
        }
    }
}

/*
 * 
 * case RetroEnums.ConfigurationConstants.RETRO_ENVIRONMENT_SET_INPUT_DESCRIPTORS:
                    RetroStructs.RetroInputDescriptor* inputs = (RetroStructs.RetroInputDescriptor*)data;
                    // to do
                    break;
 * 
 * case RetroEnums.ConfigurationConstants.RETRO_ENVIRONMENT_SET_VARIABLES:
                    //if (data != null)
                    //{
                    //    {
                    //        RetroVariable* variable = (RetroVariable*)data;
                    //        while (variable->key != null)
                    //        {
                    //            string key = Marshal.PtrToStringAnsi((IntPtr)variable->key);
                    //            string value = Marshal.PtrToStringAnsi((IntPtr)variable->value);

                    //            string options = value.Split(';')[1];
                    //            string default_option = options.Split('|')[0].Trim();

                    //            environment_settings.Add(key, default_option);

                    //            Debug.LogFormat("Environment variable: {0} => {1}", key, default_option);
                    //            Debug.LogFormat("Options: {0}", value);

                    //            variable++;
                    //        }
                    //    }
                    //}

                    return false;

                    //break;

                case RetroEnums.ConfigurationConstants.RETRO_ENVIRONMENT_GET_VARIABLE:
                    {
                        //try
                        //{
                        //    RetroVariable* variable = (RetroVariable*)data;
                        //    string key = Marshal.PtrToStringAnsi((IntPtr)variable->key);
                        //    Debug.LogFormat("Key: {0}", key);
                        //    string value = environment_settings[key];
                        //    variable->value = StringToChar(value);

                        //    Debug.LogFormat("Environment variable: {0} => {1}", key, value);
                        //}
                        //catch (System.Collections.Generic.KeyNotFoundException e)
                        //{
                        //    Debug.LogFormat("############## Couldn't find environment option: {0}", constant.ToString());
                        //    return false;
                        //}

                        return false;
                    }
                    //break;

                    case RetroEnums.ConfigurationConstants.RETRO_ENVIRONMENT_GET_RUMBLE_INTERFACE:
                    RetroStructs.RetroRumbleInterface* rumbleInterface = (RetroStructs.RetroRumbleInterface*)data;
                    rumbleInterface->callback = Marshal.GetFunctionPointerForDelegate(_rumbleDelegate);
                    break;

    case RetroEnums.ConfigurationConstants.RETRO_ENVIRONMENT_GET_VFS_INTERFACE:
                    return false;
                    RetroStructs.RetroVfsInterfaceInfo* vfsInfo = (RetroStructs.RetroVfsInterfaceInfo*)data;
                    Debug.LogFormat("Required VFS interface: {0}", vfsInfo->required_interface_version);

                    vfsInterface.get_path = Marshal.GetFunctionPointerForDelegate(_vfsGetPath);
                    vfsInterface.open = Marshal.GetFunctionPointerForDelegate(_vfsOpen);
                    vfsInterface.close = Marshal.GetFunctionPointerForDelegate(_vfsClose);
                    vfsInterface.size = Marshal.GetFunctionPointerForDelegate(_vfsSize);
                    vfsInterface.tell = Marshal.GetFunctionPointerForDelegate(_vfsTell);
                    vfsInterface.seek = Marshal.GetFunctionPointerForDelegate(_vfsSeek);
                    vfsInterface.write = Marshal.GetFunctionPointerForDelegate(_vfsWrite);
                    vfsInterface.read = Marshal.GetFunctionPointerForDelegate(_vfsRead);
                    vfsInterface.flush = Marshal.GetFunctionPointerForDelegate(_vfsFlush);
                    vfsInterface.remove = Marshal.GetFunctionPointerForDelegate(_vfsRemove);
                    vfsInterface.rename = Marshal.GetFunctionPointerForDelegate(_vfsRename);
                    vfsInterface.truncate = Marshal.GetFunctionPointerForDelegate(_vfsTruncate);

                    IntPtr structPtr = Marshal.AllocHGlobal(Marshal.SizeOf(vfsInterface));
                    Marshal.StructureToPtr(vfsInterface, structPtr, false);

                    vfsInfo->iface = structPtr;
                    
                    break;

    

            _vfsGetPath = new DelegateDefinition.RetroVfsGetPath(RetroVfsGetPath);
            _vfsOpen = new DelegateDefinition.RetroVfsOpen(RetroVfsOpen);
            _vfsClose = new DelegateDefinition.RetroVfsClose(RetroVfsClose);
            _vfsSize = new DelegateDefinition.RetroVfsSize(RetroVfsSize);
            _vfsTell = new DelegateDefinition.RetroVfsTell(RetroVfsTell);
            _vfsSeek = new DelegateDefinition.RetroVfsSeek(RetroVfsSeek);
            _vfsWrite = new DelegateDefinition.RetroVfsWrite(RetroVfsWrite);
            _vfsRead = new DelegateDefinition.RetroVfsRead(RetroVfsRead);
            _vfsFlush = new DelegateDefinition.RetroVfsFlush(RetroVfsFlush);
            _vfsRemove = new DelegateDefinition.RetroVfsRemove(RetroVfsRemove);
            _vfsRename = new DelegateDefinition.RetroVfsRename(RetroVfsRename);
            _vfsTruncate = new DelegateDefinition.RetroVfsTruncate(RetroVfsTruncate);

    public unsafe char* RetroVfsGetPath(IntPtr stream)
        {
            Debug.Log("VFS GetPath");
            return null;
        }

        public unsafe IntPtr RetroVfsOpen(string path, uint mode, uint hints)
        {
            Debug.LogFormat("VFS Open: {0} Mode: {1} Hints: {2}", path, mode, hints);
            return IntPtr.Zero;
        }

        public unsafe int RetroVfsClose(IntPtr stream)
        {
            Debug.Log("VFS close");
            return 0;
        }

        public unsafe ulong RetroVfsSize(IntPtr stream)
        {
            Debug.Log("VFS Size");
            return 0;
        }

        public unsafe ulong RetroVfsTruncate(IntPtr stream, ulong length)
        {
            Debug.Log("VFS Truncate");
            return 0;
        }

        public unsafe ulong RetroVfsTell(IntPtr stream)
        {
            Debug.Log("VFS Tell");
            return 0;
        }

        public unsafe ulong RetroVfsSeek(IntPtr stream, ulong offset, int seek_position)
        {
            Debug.Log("VFS Seek");
            return 0;
        }

        public unsafe ulong RetroVfsRead(IntPtr stream, IntPtr s, ulong len)
        {
            Debug.Log("VFS Read");
            return 0;
        }

        public unsafe ulong RetroVfsWrite(IntPtr stream, IntPtr s, ulong len)
        {
            Debug.Log("VFS Write");
            return 0;
        }

        public unsafe int RetroVfsFlush(IntPtr stream)
        {
            Debug.Log("VFS Flush");
            return 0;
        }

        public unsafe int RetroVfsRemove(string path)
        {
            Debug.Log("VFS Remove");
            return 0;
        }

        public unsafe int RetroVfsRename(string old_path, string new_path)
        {
            Debug.Log("VFS Rename");
            return 0;
        }

    
        private DelegateDefinition.RetroVfsGetPath _vfsGetPath;
        private DelegateDefinition.RetroVfsOpen _vfsOpen;
        private DelegateDefinition.RetroVfsClose _vfsClose;
        private DelegateDefinition.RetroVfsSize _vfsSize;
        private DelegateDefinition.RetroVfsTruncate _vfsTruncate;
        private DelegateDefinition.RetroVfsTell _vfsTell;
        private DelegateDefinition.RetroVfsSeek _vfsSeek;
        private DelegateDefinition.RetroVfsRead _vfsRead;
        private DelegateDefinition.RetroVfsWrite _vfsWrite;
        private DelegateDefinition.RetroVfsFlush _vfsFlush;
        private DelegateDefinition.RetroVfsRemove _vfsRemove;
        private DelegateDefinition.RetroVfsRename _vfsRename;

    
                case RetroEnums.RetroPixelFormat.RETRO_PIXEL_FORMAT_RGB565:
                    {
                        if (tex == null)
                        {
                            tex = new Texture2D((int)width, (int)height, TextureFormat.RGB565, false);
                        }

                        var imagedata = new IntPtr(data);

                        int srcsize = (int)(2 * (pitch * height));
                        int dstsize = (int)(2 * (width * height));

                        if (source == null || source.Length != srcsize)
                        {
                            source = new byte[srcsize];
                        }

                        if (dest == null || dest.Length != dstsize)
                        {
                            dest = new byte[dstsize];
                        }
                        Marshal.Copy(imagedata, source, 0, srcsize);

                        int m = 0;
                        for (int y = 0; y < height; y++)
                        {
                            for (int k = (int)(y * pitch); k < width * 2 + y * pitch; k++)
                            {
                                dest[m] = source[k];
                                m++;
                            }
                        }

                        tex.LoadRawTextureData(dest);
                        tex.filterMode = FilterMode.Point;
                        tex.Apply();
                    }
                    break;

    case RetroEnums.ConfigurationConstants.RETRO_ENVIRONMENT_SET_DISK_CONTROL_INTERFACE:
                    RetroStructs.RetroDiskControlCallback* diskControl = (RetroStructs.RetroDiskControlCallback*)data;
                    
                    diskControl->set_eject_state = Marshal.GetFunctionPointerForDelegate(_setEjectState);
                    diskControl->get_eject_State = Marshal.GetFunctionPointerForDelegate(_getEjectState);
                    diskControl->get_image_index = Marshal.GetFunctionPointerForDelegate(_getImageIndex);
                    diskControl->set_image_index = Marshal.GetFunctionPointerForDelegate(_setImageIndex);
                    diskControl->get_num_images = Marshal.GetFunctionPointerForDelegate(_getNumImages);
                    diskControl->replace_image_index = Marshal.GetFunctionPointerForDelegate(_replaceImageIndex);
                    diskControl->add_image_index = Marshal.GetFunctionPointerForDelegate(_retroAddImageIndex);
                    break;

 * 
 */