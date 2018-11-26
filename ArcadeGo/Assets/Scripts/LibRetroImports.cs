using System;
using System.Runtime.InteropServices;

namespace LibRetroWrapper
{
    public class RetroImports
    {
        // core name
#if !UNITY_EDITOR
        public const string corefile = "retropsxhw";//"retropsx";
#else
        public const string corefile = "mednafen_psx_hw_libretro";// "pcsx_rearmed_libretro";
#endif

        [DllImport(corefile, EntryPoint = "retro_api_version")]
        public static extern int RetroApiVersion();

        [DllImport(corefile, EntryPoint = "retro_init")]
        public static extern void RetroInit();

        [DllImport(corefile, EntryPoint = "retro_get_system_info")]
        public static extern void RetroGetSystemInfo(ref RetroStructs.RetroSystemInfo info);

        [DllImport(corefile, EntryPoint = "retro_get_system_av_info")]
        public static extern void RetroGetSystemAVInfo(ref RetroStructs.RetroSystemAVInfo info);

        [DllImport(corefile, EntryPoint = "retro_load_game")]
        public static extern bool RetroLoadGame(ref RetroStructs.RetroGameInfo game);

        [DllImport(corefile, EntryPoint = "retro_set_video_refresh")]
        public static extern void RetroSetVideoRefresh(DelegateDefinition.RetroVideoRefreshDelegate r);

        [DllImport(corefile, EntryPoint = "retro_set_audio_sample")]
        public static extern void RetroSetAudioSample(DelegateDefinition.RetroAudioSampleDelegate r);

        [DllImport(corefile, EntryPoint = "retro_set_audio_sample_batch")]
        public static extern void RetroSetAudioSampleBatch(DelegateDefinition.RetroAudioSampleBatchDelegate r);

        [DllImport(corefile, EntryPoint = "retro_set_input_poll")]
        public static extern void RetroSetInputPoll(DelegateDefinition.RetroInputPollDelegate r);

        [DllImport(corefile, EntryPoint = "retro_set_input_state")]
        public static extern void RetroSetInputState(DelegateDefinition.RetroInputStateDelegate r);

        [DllImport(corefile, EntryPoint = "retro_set_environment")]
        public static extern bool RetroSetEnvironment(DelegateDefinition.RetroEnvironmentDelegate r);

        [DllImport(corefile, EntryPoint = "retro_set_controller_port_device")]
        public static extern void RetroSetControllerPortDevice(uint port, uint device);

        [DllImport(corefile, EntryPoint = "retro_run")]
        public static extern void RetroRun();

        [DllImport(corefile, EntryPoint = "retro_deinit")]
        public static extern void RetroDeInit();
    }
}
